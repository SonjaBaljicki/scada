using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealTimeUnit
{
    internal class Program
    {

        static ServiceReference1.IRealTimeUnitService service = new ServiceReference1.RealTimeUnitServiceClient();

        public static string ContainerName { get; private set; } 
        const string EXPORT_FOLDER = @"C:\public_key\";
        const string PUBLIC_KEY_FILE = @"rsaPublicKey";
        const string KEY_STORE_NAME = "MyKeyStore";
        public static CspParameters csp = new CspParameters();
        public static RSACryptoServiceProvider rsa;

        static void Main(string[] args)
        {
            //ContainerName = "KeyContainer";
            ContainerName = CreateAsmKeys(true);

            string address = "";
            int id = 0;
            bool check = false;

            while (!check)
            {
                Console.WriteLine("Enter new real time unit id: ");
                string idString = Console.ReadLine();
                check = int.TryParse(idString, out id);
                Console.WriteLine("Enter new real time unit address: ");
                address = Console.ReadLine();
                service.CreateRealTimeUnit(id, address);
            }

            check = false;
            double lowLimit = 0;
            double highLimit = 0;
            while (!check)
            {
                Console.WriteLine("Enter low limit: ");
                string low = Console.ReadLine();
                check = double.TryParse(low, out lowLimit);
            }

            check = false;
            while (!check) {
                Console.WriteLine("Enter high limit: ");
                string hight = Console.ReadLine();
                check = double.TryParse(hight, out highLimit);

                if (lowLimit > highLimit) check = false; 
            }

            GenerateValues(lowLimit, highLimit, address, id);

        }

        private static void GenerateValues(double lowLimit, double highLimit, string address, int id)
        {
            while (true)
            {
                Random random = new Random();
                double randomValue = random.NextDouble() * (highLimit - lowLimit) + lowLimit;
                ExportPublicKey(id.ToString());
                string message = address + "," + randomValue.ToString();
                byte[] signedMessage;
                SignMessage(message, out signedMessage);
                Console.WriteLine(randomValue);
                service.SetValue(message, id.ToString(), signedMessage);
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }

        private static byte[] SignMessage(string message, out byte[] hashValue)
        {
            using (SHA256 sha = SHA256Managed.Create())
            {
                hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
                CspParameters csp = new CspParameters();
                csp.KeyContainerName = ContainerName;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA256");
                return formatter.CreateSignature(hashValue);
            }
        }

        private static void ExportPublicKey(string id)
        {
            if (!Directory.Exists(EXPORT_FOLDER))
            {
                Directory.CreateDirectory(EXPORT_FOLDER);
            }
            using (StreamWriter writer = new StreamWriter(Path.Combine(EXPORT_FOLDER,
            PUBLIC_KEY_FILE + id + ".txt")))
            {
                writer.WriteLine(rsa.ToXmlString(false));
            }
        }

        private static string CreateAsmKeys(bool useMachineKeyStore)
        {
            csp.KeyContainerName = KEY_STORE_NAME;
            if (useMachineKeyStore)
                csp.Flags = CspProviderFlags.UseMachineKeyStore;
            rsa = new RSACryptoServiceProvider(csp);
            rsa.PersistKeyInCsp = true;
            CspKeyContainerInfo info = new CspKeyContainerInfo(csp);
           return info.KeyContainerName;
        }
       
    }

}
