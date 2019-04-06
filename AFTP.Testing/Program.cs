using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using AFTPLib;
using AFTPLib.Configuration;

namespace AFTP.Testing {
    public class Program {
        public static void Main(string[] args) {
            Console.WriteLine("Starting...");
            AftpServer aftpServer = new AftpServer(new ServerConfig() {
                //ServerCertificate = new X509Certificate2(@"C:\Users\jpdante\Desktop\cert.pfx", "12345"),
                ServerCertificate = new X509Certificate2(@"/home/jpdante/Desktop/cert.pfx", "12345"),
                IpEndPoint = new IPEndPoint(IPAddress.Any, 49535),
                BackLog = 10,
                FirewallMaxConnections = 10,
                FirewallMaxFailedAttempts = 10,
                FirewallHistoryTimeout = 3600,
                FirewallBanTimeout = 5
            });
            aftpServer.Start();
            Console.WriteLine("AFTP Server started!");
            AftpClient aftpClient = new AftpClient(new ClientConfig() {
                DisableCertificateCheck = true
            });
            Console.WriteLine("Connecting to 'localhost:49535'...");
            aftpClient.Connect("localhost", 49535, "user", "1234");
            Console.ReadKey();
        }
    }
}
