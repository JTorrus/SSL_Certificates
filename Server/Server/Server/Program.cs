using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;

namespace Server
{
    class Program
    {
        private static IPAddress ServerIP;
        private static int PortIP;
        private static IPEndPoint ServerEndPoint;

        static void Main(string[] args)
        {
            PortIP = 11000;
            IPAddress[] ips;

            ips = Dns.GetHostAddresses("127.0.0.1");
            ServerIP = ips[0];
            ServerEndPoint = new IPEndPoint(ServerIP, PortIP);

            TcpListener Server = new TcpListener(ServerEndPoint);
            Console.WriteLine("Servidor creat");

            Server.Start();
            Console.WriteLine("Servidor iniciat");

            TcpClient Client = Server.AcceptTcpClient();
            Console.WriteLine("Client connectat");

            //Rebem dades del client
            NetworkStream ServerNS = Client.GetStream();
            SslStream ReceivedSslStream = new SslStream(ServerNS, false);

            byte[] BufferLocal = new byte[256];
            string s = "";

            int BytesRebuts = ReceivedSslStream.Read(BufferLocal, 0, BufferLocal.Length);
            //Passem de bytes a string
            s = Encoding.UTF8.GetString(BufferLocal, 0, BytesRebuts);

            string reverseString = ReverseString(s);

            //Passem de string a bytes
            byte[] fraseBytes = Encoding.UTF8.GetBytes(reverseString);

            //Enviem al servidor
            ServerNS.Write(fraseBytes, 0, fraseBytes.Length);


            Console.WriteLine("Server finalitzat");

            ServerNS.Close();
            Client.Close();

            Server.Stop();

            Console.ReadLine();
        }

        static string ReverseString (string intString)
        {
            char[] charArray = intString.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
