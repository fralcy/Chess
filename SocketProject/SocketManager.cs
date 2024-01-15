using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace SocketProject
{
    public class SocketManager
    {
        #region Client

        Socket client;
        public bool ConnectServer()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(iep);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        #endregion

        #region Server

        Socket server;
        public void CreateServer()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(iep);
            server.Listen(10000);
            Thread acceptThread = new Thread(() =>
            {
                client = server.Accept();
            });

            acceptThread.IsBackground = true;
            acceptThread.Start();
        }

        public bool IsConected()
        {
            bool part1 = server.Poll(1000, SelectMode.SelectRead);
            bool part2 = (server.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        #endregion

        #region Both
        public string IP = "127.0.0.1";
        public int PORT = 9999;

        public void CloseSocket()
        {
            try
            {
                client.Shutdown(SocketShutdown.Both);
                
            }

            finally
            {
                client.Close();
                
            }
            /*
            try
            {
                server.Shutdown(SocketShutdown.Both);
            }
            finally 
            {
                server.Close();
            }
            */
        }

        public byte[] SerializeData(Object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();
            bf1.Serialize(ms, o);
            return ms.ToArray();
        }


        public SocketData DeserializeData(byte[] theByteArray)
        {
            MemoryStream ms = new MemoryStream(theByteArray);
            BinaryFormatter bf1 = new BinaryFormatter();
            ms.Position = 0;
            return (SocketData)bf1.Deserialize(ms);
        }


        public string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }

        public bool Send(object data)
        {
            byte[] sendData = SerializeData(data);
            return SendData(client, sendData);
        }

        public SocketData Receive()
        {
            byte[] receiveData = new byte[1024 * 8000];
            bool isOk = ReceiveData(client, receiveData);
            return (SocketData)DeserializeData(receiveData);
        }

        private bool SendData(Socket target, byte[] data)
        {
            return target.Send(data) == 1;
        }

        private bool ReceiveData(Socket target, byte[] data)
        {
            return target.Receive(data) == 1;
        }

        

        #endregion
    }

}
