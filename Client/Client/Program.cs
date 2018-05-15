using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Client
{
    class Program
    {
        private static IPAddress ServerIP;
        private static int PortIP;
        private static IPEndPoint ServerEndPoint;

        static void Main(string[] args)
        {
            PortIP = 11000;
            ServerIP = IPAddress.Parse("127.0.0.1");
            ServerEndPoint = new IPEndPoint(ServerIP, PortIP);

            TcpClient Client = new TcpClient();

            Client.Connect(ServerEndPoint);

            if (Client.Connected)
                Console.WriteLine("Connectat al servidor");

            NetworkStream ClientNS = Client.GetStream();
            SslStream SslStreamToWrite = new SslStream(ClientNS, false, new RemoteCertificateValidationCallback(ValidationCertificate));

            SslStreamToWrite.AuthenticateAsClient("rtd.certificate2018.test");

            Console.WriteLine("Escriu una frase:");
            string frase = Console.ReadLine();

            //Passem de string a bytes
            byte[] fraseBytes = Encoding.UTF8.GetBytes(frase);
            
            //Enviem al servidor
            SslStreamToWrite.Write(fraseBytes, 0, fraseBytes.Length);

            /*byte[] BufferLocal = new byte[256];
            string s = "";

            int BytesRebuts = ClientNS.Read(BufferLocal, 0, BufferLocal.Length);
            
            //Passem de bytes a string
            s = Encoding.UTF8.GetString(BufferLocal, 0, BytesRebuts);

            Console.WriteLine("{0}", s);*/

            Console.WriteLine("final");

            ClientNS.Close();
            Client.Close();

            Console.ReadLine();
        }

        private static bool ValidationCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }
    }
}
