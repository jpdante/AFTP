using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using AFTPLib;
using AFTPLib.Configuration;

namespace AFTP.Testing {
    public class Program {
        public static void Main(string[] args) {
            Console.WriteLine("Starting...");
            var aftpServer = new AftpServer(new ServerConfig() {
/*#if (LINUX)
                ServerCertificate = new X509Certificate2(@"/home/jpdante/Desktop/cert.pfx", "12345"),
#else
                ServerCertificate = new X509Certificate2(@"C:\Users\jpdante\Desktop\cert.pfx", "12345"),
#endif*/
                IpEndPoint = new IPEndPoint(IPAddress.Any, 49535),
                BackLog = 10,
                FirewallMaxConnections = 10,
                FirewallMaxFailedAttempts = 10,
                FirewallHistoryTimeout = 3600,
                FirewallBanTimeout = 5
            });
            aftpServer.Start();
            Console.WriteLine("AFTP Server started!");
            var aftpClient = new AftpClient(new ClientConfig() {
                DisableCertificateCheck = true
            });
            Console.WriteLine("Connecting to 'localhost:49535'...");
            aftpClient.Connect("localhost", 49535, "user", "1234");
            Console.WriteLine("CONTINUE");
            var data = aftpClient.GetDirectoryList(@"C:\Users\jpdante\Desktop").GetAwaiter().GetResult();
            Console.WriteLine(data.Guid);
            foreach (var i in data.DirectoryEntries) {
                Console.WriteLine($"{i.FileName} - {i.IsFile} - {i.Size} - {i.Modified} - {i.Permissions} - {i.Owner}");
            }
            Console.ReadKey();
        }
    }
}
