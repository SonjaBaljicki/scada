using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    public class RealTimeUnitService : IRealTimeUnitService
    {
        public static string ContainerName { get; private set; }
        private static CspParameters csp = new CspParameters();
        private static RSACryptoServiceProvider rsa;
        const string EXPORT_FOLDER = @"C:\public_key\";
        const string PUBLIC_KEY_FILE = @"rsaPublicKey";
        const string KEY_STORE_NAME = "MyKeyStore";

        public bool CreateRealTimeUnit(int id, string address)
        {
            return RealTimeDriver.AddAddress(id, address);
        }

        public bool SetValue(string message, string id, byte[] signature)
        {
            ImportPublicKey(id);
            byte[] hash = ComputeMessageHash(message);
            VerifySignedMessage(hash, signature);
            string[] stringValue = message.Split(',');
            Console.WriteLine("adresa " + stringValue[0]);
            Console.WriteLine(stringValue[1]);
            return RealTimeDriver.SetValue(stringValue[0], double.Parse(stringValue[1]));
        }

        private static void ImportPublicKey(string id)
        {
            string path = Path.Combine(EXPORT_FOLDER, PUBLIC_KEY_FILE + id + ".txt");
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    csp.KeyContainerName = KEY_STORE_NAME;
                    rsa = new RSACryptoServiceProvider(csp);
                    string publicKeyText = reader.ReadToEnd();
                    rsa.FromXmlString(publicKeyText);
                    rsa.PersistKeyInCsp = true;
                }
            }
        }

        private static bool VerifySignedMessage(byte[] hash, byte[] signature)
        {
            csp.KeyContainerName = ContainerName;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp))
            {
                var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
                deformatter.SetHashAlgorithm("SHA256");
                return deformatter.VerifySignature(hash, signature);
            }
        }

        public static byte[] ComputeMessageHash(string value)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }
    }
}