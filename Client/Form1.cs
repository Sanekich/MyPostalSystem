using Models;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client
{
    public partial class Client : Form
    {
        private UdpClient client;
        private readonly int serverPort = 1111;
        private readonly string clientLogin;
        ParcelModel[] parcels;

        public Client()
        {
            InitializeComponent();
            Login login = new();
            login.ShowDialog();
            if (login.GetLocalPort() == 0)
                throw new Exception("User is not logged in");
            else
            {
                client = new UdpClient(login.GetLocalPort());
                clientLogin = login.GetLogin();
            }
            login.Dispose();
            LoadBuildings();
            LoadTypes();
            LoadParcels();
        }

        async void LoadBuildings()
        {
            try
            {
                byte[] buf = Encoding.UTF8.GetBytes("B");
                await client.SendAsync(buf, buf.Length, "127.0.0.1", serverPort);
                UdpReceiveResult res = await client.ReceiveAsync();
                byte[] responseData = res.Buffer;
                string[] buildings = JsonSerializer.Deserialize<string[]>(Encoding.UTF8.GetString(responseData))!;
                comboBox_from.Invoke(() => { comboBox_from.Items.AddRange(buildings!); });
                comboBox_to.Invoke(() => { comboBox_to.Items.AddRange(buildings!); });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async void LoadTypes()
        {
            try
            {
                byte[] buf = Encoding.UTF8.GetBytes("T");
                await client.SendAsync(buf, buf.Length, "127.0.0.1", serverPort);
                UdpReceiveResult res = await client.ReceiveAsync();
                byte[] responseData = res.Buffer;
                string[] types = JsonSerializer.Deserialize<string[]>(Encoding.UTF8.GetString(responseData))!;
                cbParcelTypes.Invoke(() => { cbParcelTypes.Items.AddRange(types!); });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async void LoadParcels()
        {
            try
            {
                byte[] buf = Encoding.UTF8.GetBytes($"P;{clientLogin}");
                await client.SendAsync(buf, buf.Length, "127.0.0.1", serverPort);
                UdpReceiveResult res = await client.ReceiveAsync();
                byte[] responseData = res.Buffer;
                parcels = JsonSerializer.Deserialize<ParcelModel[]>(Encoding.UTF8.GetString(responseData))!;
                foreach (ParcelModel parcel in parcels)
                {
                    listBox_listParcel.Invoke(() => { listBox_listParcel.Items.Add(parcel.ToString()); });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void listBox_listParcel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ParcelModel parcel = parcels.FirstOrDefault(p => p.Description == listBox_listParcel.SelectedItem!.ToString())!;
                byte[] buf = JsonSerializer.SerializeToUtf8Bytes(parcel);
                await client.SendAsync(buf, buf.Length, "127.0.0.1", serverPort);
                UdpReceiveResult res = await client.ReceiveAsync();
                string data = Encoding.UTF8.GetString(res.Buffer);
                MessageBox.Show(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button_createParcel_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedFromBuilding = comboBox_from.SelectedItem?.ToString();
                string selectedToBuilding = comboBox_to.SelectedItem?.ToString();
                string selectedParcelType = cbParcelTypes.SelectedItem?.ToString();
                string description = textBox_description.Text;
                string receiverLogin = textBox_receiverLogin.Text;
                if (selectedFromBuilding == null || selectedToBuilding == null || selectedParcelType == null || description == "" || receiverLogin == "")
                {
                    MessageBox.Show("Enter all data correctly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int typeId = 0;
                int fromBuildingId = int.Parse(selectedFromBuilding.Substring(selectedFromBuilding.IndexOf('r') + 2, 1));
                int toBuildingId = int.Parse(selectedToBuilding.Substring(selectedToBuilding.IndexOf('r') + 2, 1));
                string request = $"C;{clientLogin};{receiverLogin};{selectedParcelType};{description};{fromBuildingId};{toBuildingId}";
                SendRequest(request);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SendRequest(string request)
        {
            try
            {
                byte[] requestData = Encoding.UTF8.GetBytes(request);
                await client.SendAsync(requestData, requestData.Length, "127.0.0.1", serverPort);

                UdpReceiveResult response = await client.ReceiveAsync();
                string responseData = Encoding.UTF8.GetString(response.Buffer);

                MessageBox.Show(responseData, "Server Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadParcels();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

