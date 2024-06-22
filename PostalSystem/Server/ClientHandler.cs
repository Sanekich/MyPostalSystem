using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server
{
    public class ClientHandler
    {
        private readonly UdpClient udpServer;
        private readonly IConfigurationRoot config;
        private Server server;

        public ClientHandler(UdpClient server, IConfigurationRoot config, Server serv)
        {
            udpServer = server;
            this.config = config;
            this.server = serv;
        }

        public async Task HandleClientAsync(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    UdpReceiveResult res = await udpServer.ReceiveAsync(token);
                    byte[] buf = res.Buffer;
                    string message = Encoding.UTF8.GetString(buf);
                    using SqlConnection connection = new(config.GetConnectionString("SqlClient"));
                    switch (message[0])
                    {
                        case 'L':
                            {
                                string[] loginParams = message.Split(';');
                                if (loginParams.Length == 3)
                                {
                                    string login = loginParams[1];
                                    string password = loginParams[2];
                                    int count = await CountClientsWithLoginAndPassword(connection, login, password);
                                    byte[] loginResponseBytes;
                                    if (count == 1)
                                    {
                                        loginResponseBytes = Encoding.UTF8.GetBytes("User exists");
                                        AddToLog($"User {login} logged in");
                                    }
                                    else
                                    {
                                        loginResponseBytes = Encoding.UTF8.GetBytes("User does not exist");
                                    }
                                    await udpServer.SendAsync(loginResponseBytes, loginResponseBytes.Length, res.RemoteEndPoint);
                                }
                                else
                                {
                                    string invalidFormatMessage = "Invalid login request format";
                                    byte[] invalidFormatBytes = Encoding.UTF8.GetBytes(invalidFormatMessage);
                                    await udpServer.SendAsync(invalidFormatBytes, invalidFormatBytes.Length, res.RemoteEndPoint);
                                }
                            }
                            break;
                        case 'R':
                            {
                                string[] registerParams = message.Split(';');
                                if (registerParams.Length == 3)
                                {
                                    string login = registerParams[1];
                                    string password = registerParams[2];
                                    int count = await CountClientsWithLoginAndPassword(connection, login, password);
                                    if (count == 0)
                                    {
                                        await RegisterNewUserAsync(connection, login, password);
                                        AddToLog($"User {login} registered.");
                                        string registrationSuccessMessage = "Registration successful";
                                        byte[] registrationSuccessBytes = Encoding.UTF8.GetBytes(registrationSuccessMessage);
                                        await udpServer.SendAsync(registrationSuccessBytes, registrationSuccessBytes.Length, res.RemoteEndPoint);
                                    }
                                    else
                                    {
                                        string userExistsMessage = "User with this login already exists";
                                        byte[] userExistsBytes = Encoding.UTF8.GetBytes(userExistsMessage);
                                        await udpServer.SendAsync(userExistsBytes, userExistsBytes.Length, res.RemoteEndPoint);
                                    }
                                }
                                else
                                {
                                    string invalidFormatMessage = "Invalid registration request format";
                                    byte[] invalidFormatBytes = Encoding.UTF8.GetBytes(invalidFormatMessage);
                                    await udpServer.SendAsync(invalidFormatBytes, invalidFormatBytes.Length, res.RemoteEndPoint);
                                }
                            }
                            break;
                        case 'B':
                            {
                                IEnumerable<dynamic> data = await connection.QueryAsync("select Id, X, Y from Buildings");
                                List<string> buildings = [];
                                int count = 1;
                                foreach (dynamic b in data)
                                {
                                    buildings.Add($"Post office number {count}   ({b.X}, {b.Y})");
                                    count++;
                                }
                                buf = JsonSerializer.SerializeToUtf8Bytes(buildings);
                                await udpServer.SendAsync(buf, buf.Length, res.RemoteEndPoint);
                            }
                            break;
                        case 'T':
                            {
                                IEnumerable<string> data = await connection.QueryAsync<string>("select Type from ParcelTypes");
                                List<string> types = [];
                                types.AddRange(data);
                                buf = JsonSerializer.SerializeToUtf8Bytes(types);
                                await udpServer.SendAsync(buf, buf.Length, res.RemoteEndPoint);
                            }
                            break;
                        case 'P':
                            {
                                string login = message.Split(";")[1];
                                IEnumerable<ParcelModel> data = await connection.QueryAsync<ParcelModel>("select * from Parcels as p join Clients as c on (p.SenderId = c.Id or p.ReceiverId = c.Id) and c.Login = @clientLogin", new { clientLogin = login });
                                buf = JsonSerializer.SerializeToUtf8Bytes(data);                               
                                await udpServer.SendAsync(buf, buf.Length, res.RemoteEndPoint);
                            }
                            break;
                        case 'C':
                            {
                                string[] parcelParams = message.Split(';');
                                if (parcelParams.Length == 7)
                                {
                                    try
                                    {
                                        string clientLogin = parcelParams[1];
                                        string receiverLogin = parcelParams[2];
                                        string type = parcelParams[3];
                                        string description = parcelParams[4];
                                        int fromBuildingId = int.Parse(parcelParams[5]);
                                        int toBuildingId = int.Parse(parcelParams[6]);



                                        int senderId = await GetClientIdByLogin(connection, clientLogin);
                                        int receiverId = await GetClientIdByLogin(connection, receiverLogin);

                                        if (senderId != 0 && receiverId != 0)
                                        {
                                            await InsertParcelAsync(connection, senderId, receiverId, fromBuildingId, toBuildingId, type, description);

                                            string successMessage = "Parcel created successfully";
                                            byte[] successBytes = Encoding.UTF8.GetBytes(successMessage);
                                            await udpServer.SendAsync(successBytes, successBytes.Length, res.RemoteEndPoint);
                                        }
                                        else
                                        {
                                            string errorMessage = "Sender or receiver not found";
                                            byte[] errorBytes = Encoding.UTF8.GetBytes(errorMessage);
                                            await udpServer.SendAsync(errorBytes, errorBytes.Length, res.RemoteEndPoint);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string errorMessage = "Error creating parcel: " + ex.Message;
                                        byte[] errorBytes = Encoding.UTF8.GetBytes(errorMessage);
                                        await udpServer.SendAsync(errorBytes, errorBytes.Length, res.RemoteEndPoint);
                                    }
                                }
                                else
                                {
                                    string invalidFormatMessage = "Invalid parcel creation request format";
                                    byte[] invalidFormatBytes = Encoding.UTF8.GetBytes(invalidFormatMessage);
                                    await udpServer.SendAsync(invalidFormatBytes, invalidFormatBytes.Length, res.RemoteEndPoint);
                                }                                
                            }
                            break;
                            default:
                            {
                                ParcelModel parcel = JsonSerializer.Deserialize<ParcelModel>(message)!;
                                string data = "";
                                data += "Sender - " + await connection.ExecuteScalarAsync<string>("select Login from Clients where Id = @id", new { id = parcel.SenderId }) + "\n";
                                data += "Receiver - " + await connection.ExecuteScalarAsync<string>("select Login from Clients where Id = @id", new { id = parcel.ReceiverId }) + "\n";
                                data += "From post office number " + await connection.ExecuteScalarAsync<string>("select Id from Buildings where Id = @id", new { id = parcel.FromBuildingId }) + "\n";
                                data += "To post office number " + await connection.ExecuteScalarAsync<string>("select Id from Buildings where Id = @id", new { id = parcel.ToBuildingId }) + "\n";
                                data += "Type - " + await connection.ExecuteScalarAsync<string>("select Type from ParcelTypes where Id = @id", new { id = parcel.TypeId }) + "\n";
                                data += "Distance - " + parcel.Distance + "\n";
                                data += "Cost - " + parcel.Cost + "\n";
                                byte[] tmp = Encoding.UTF8.GetBytes(data);
                                await udpServer.SendAsync(tmp, tmp.Length, res.RemoteEndPoint);
                            }
                            break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("The operation was cancelled", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        async Task<int> CountClientsWithLoginAndPassword(SqlConnection connection, string login, string password)
        {
            string sql = "SELECT COUNT(*) FROM Clients WHERE Login = @Login AND Password = @Password";
            return await connection.ExecuteScalarAsync<int>(sql, new { Login = login, Password = password });
        }

        async Task RegisterNewUserAsync(SqlConnection connection, string login, string password)
        {
            string sql = "INSERT INTO Clients (Login, Password) VALUES (@Login, @Password)";
            await connection.ExecuteAsync(sql, new { Login = login, Password = password });
        }

        async Task<int> GetClientIdByLogin(SqlConnection connection, string login)
        {
            string sql = "SELECT Id FROM Clients WHERE Login = @Login";
            return await connection.ExecuteScalarAsync<int>(sql, new { Login = login });
        }

        async Task InsertParcelAsync(SqlConnection connection, int senderId, int receiverId, int fromBuildingId, int toBuildingId, string type, string description)
        {
            try
            {
                int typeId = await GetParcelTypeId(connection, type);

                string sql = @"INSERT INTO Parcels (SenderId, ReceiverId, FromBuildingId, ToBuildingId, TypeId, Description) VALUES (@SenderId, @ReceiverId, @FromBuildingId, @ToBuildingId, @TypeId, @Description)";

                await connection.ExecuteAsync(sql, new
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    FromBuildingId = fromBuildingId,
                    ToBuildingId = toBuildingId,
                    TypeId = typeId,
                    Description = description
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting parcel: {ex.Message}");
                throw;
            }
        }

        async Task<int> GetParcelTypeId(SqlConnection connection, string type)
        {
            string sql = "SELECT Id FROM ParcelTypes WHERE Type = @Type";
            return await connection.ExecuteScalarAsync<int>(sql, new { Type = type });
        }

        public void AddToLog(string msg)
        {
            server.AppendToLogs(msg);
        }
    }
}