using System.Net.Sockets;
using System.Text;

namespace Client
{
    public partial class Login : Form
    {
        string? login, password;
        int localPort, serverPort = 1111;

        public Login()
        {
            InitializeComponent();
        }

        private async void button_signIn_Click(object sender, EventArgs e)
        {
            login = textBox_login.Text.Trim();
            password = textBox_password.Text.Trim();

            if (!int.TryParse(textBox_port.Text.Trim(), out localPort) || localPort == serverPort || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all the fields with valid data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using UdpClient udpClient = new(localPort);
                string requestData = "L;" + login + ";" + password;
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestData);
                await udpClient.SendAsync(requestBytes, requestBytes.Length, "127.0.0.1", serverPort);

                UdpReceiveResult res = await udpClient.ReceiveAsync();
                byte[] buf = res.Buffer;
                string message = Encoding.UTF8.GetString(buf);
                switch (message)
                {
                    case "User exists":
                        {
                            MessageBox.Show("Data is correct", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        }
                        break;
                    case "User does not exist":
                        {
                            MessageBox.Show("User does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case "Invalid login request format":
                        {
                            MessageBox.Show("Invalid login request format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button_signUp_Click(object sender, EventArgs e)
        {
            login = textBox_login.Text.Trim();
            password = textBox_password.Text.Trim();

            if (!int.TryParse(textBox_port.Text.Trim(), out localPort) || localPort == serverPort || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all the fields with valid data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using UdpClient udpClient = new(localPort);
                string requestData = "R;" + login + ";" + password;
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestData);

                await udpClient.SendAsync(requestBytes, requestBytes.Length, "127.0.0.1", serverPort);

                UdpReceiveResult res = await udpClient.ReceiveAsync();
                byte[] buf = res.Buffer;
                string message = Encoding.UTF8.GetString(buf);
                switch (message)
                {
                    case "Registration successful":
                        {
                            MessageBox.Show("Registration successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        }
                        break;
                    case "User with this login already exists":
                        {
                            MessageBox.Show("User with this login already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case "Invalid registration request format":
                        {
                            MessageBox.Show("Invalid registration request format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public int GetLocalPort()
        {
            return localPort;
        }

        public string GetLogin()
        {
            return login!;
        }
    }
}