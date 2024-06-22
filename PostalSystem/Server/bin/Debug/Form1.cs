using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Net.Sockets;

namespace Server
{
    public partial class Server : Form
    {
        IConfigurationRoot config;
        UdpClient server;
        int localPort = 1111;
        ClientHandler clientHandler;
        CancellationTokenSource tokenSource;
        CancellationToken token;

        public Server()
        {
            InitializeComponent();
            config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            server = new(localPort);
            cbSelect.Items.AddRange(new string[] { "Clients", "Parcels", "ParcelTypes", "Buildings" });
            clientHandler = new(server, config, this);
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
        }

        private async void bStart_Click(object sender, EventArgs e)
        {
            AppendToLogs("Server started.");
            bStop.Enabled = true;
            nudX.Enabled = true;
            nudY.Enabled = true;
            nudCostMultiplier.Enabled = true;
            bAddBuilding.Enabled = true;
            bRemoveBuilding.Enabled = true;
            tbParcelType.Enabled = true;
            bAddParcelType.Enabled = true;
            bRemoveParcelType.Enabled = true;
            lbLog.Enabled = true;
            cbSelect.Enabled = true;
            bSelect.Enabled = true;
            bStart.Enabled = false;
            if (token.IsCancellationRequested)
            {
                tokenSource.Dispose();
                tokenSource = new CancellationTokenSource();
                token = tokenSource.Token;
            }
            await clientHandler.HandleClientAsync(token);
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            AppendToLogs("Server stopped.");
            bStart.Enabled = true;
            bStop.Enabled = false;
            nudX.Enabled = false;
            nudY.Enabled = false;
            nudCostMultiplier.Enabled = false;
            bAddBuilding.Enabled = false;
            bRemoveBuilding.Enabled = false;
            tbParcelType.Enabled = false;
            bAddParcelType.Enabled = false;
            bRemoveParcelType.Enabled = false;
            lbLog.Enabled = false;
            cbSelect.Enabled = false;
            bSelect.Enabled = false;
            tokenSource.Cancel();
        }

        private async void bAddBuilding_Click(object sender, EventArgs e)
        {
            using SqlConnection connection = new(config.GetConnectionString("SqlClient"));
            await connection.OpenAsync();
            if (await connection.ExecuteScalarAsync<int>($"select count(*) from Buildings where X = @X and Y = @Y", new { X = nudX.Value, Y = nudY.Value }) > 0)
            {
                MessageBox.Show("The building with the following coordinates already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await connection.ExecuteAsync($"insert into Buildings values (@X, @Y)", new { X = nudX.Value, Y = nudY.Value });
            AppendToLogs("New building added.");
            MessageBox.Show("New building was successfully added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void bAddParcelType_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbParcelType.Text) || nudCostMultiplier.Value <= 1)
                return;

            using SqlConnection connection = new(config.GetConnectionString("SqlClient"));
            await connection.OpenAsync();
            if (await connection.ExecuteScalarAsync<int>($"select count(*) from ParcelTypes where Type = @Type and CostMultiplier = @CostMultiplier", new { Type = tbParcelType.Text, CostMultiplier = nudCostMultiplier.Value }) > 0)
            {
                MessageBox.Show("The parcel type with the following data already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await connection.ExecuteAsync($"insert into ParcelTypes values (@Type, @CostMultiplier)", new { Type = tbParcelType.Text, CostMultiplier = nudCostMultiplier.Value });
            AppendToLogs("New parcel added.");
            MessageBox.Show("New parcel type was successfully added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void bRemoveBuilding_Click(object sender, EventArgs e)
        {
            using SqlConnection connection = new(config.GetConnectionString("SqlClient"));
            await connection.OpenAsync();
            if (await connection.ExecuteScalarAsync<int>($"select count(*) from Buildings where X = @X and Y = @Y", new { X = nudX.Value, Y = nudY.Value }) == 0)
            {
                MessageBox.Show("The building with the following coordinates doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await connection.ExecuteAsync($"delete from Buildings where X = @X and Y = @Y", new { X = nudX.Value, Y = nudY.Value });
            AppendToLogs("Building removed.");
            MessageBox.Show("Building was successfully removed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void bRemoveParcelType_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbParcelType.Text) || nudCostMultiplier.Value <= 1)
                return;

            using SqlConnection connection = new(config.GetConnectionString("SqlClient"));
            await connection.OpenAsync();
            if (await connection.ExecuteScalarAsync<int>($"select count(*) from ParcelTypes where Type = @Type and CostMultiplier = @CostMultiplier", new { Type = tbParcelType.Text, CostMultiplier = nudCostMultiplier.Value }) == 0)
            {
                MessageBox.Show("The parcel type with the following data doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await connection.ExecuteAsync($"delete from ParcelTypes where Type = @Type and CostMultiplier = @CostMultiplier", new { Type = tbParcelType.Text, CostMultiplier = nudCostMultiplier.Value });
            AppendToLogs("Parcel removed.");
            MessageBox.Show("Parcel type was successfully removed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void bSelect_Click(object sender, EventArgs e)
        {
            if (cbSelect.SelectedIndex == -1) 
                return;

            using SqlConnection connection = new(config.GetConnectionString("SqlClient"));
            await connection.OpenAsync();
            int amountOfColumns = 0;
            switch (cbSelect.SelectedItem as string)
            {
                case "Clients": amountOfColumns = 3; break;
                case "Parcels": amountOfColumns = 9; break;
                case "ParcelTypes": amountOfColumns = 3; break;
                case "Buildings": amountOfColumns = 3; break;
            }
            IEnumerable<dynamic> data = await connection.QueryAsync($"select * from {cbSelect.SelectedItem as string}");
            if (data.IsNullOrEmpty())
            {
                MessageBox.Show("The table is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            AppendToLogs("Data selected.");
            SelectDataDisplayer dataDisplayer = new(data, amountOfColumns);
            dataDisplayer.ShowDialog();
        }
        public void AppendToLogs(string message)
        {
            lbLog.Items.Add($"{DateTime.Now}: {message}{Environment.NewLine}");
        }
    }
}