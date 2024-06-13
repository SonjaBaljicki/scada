using ScadaCore.model;
using ScadaCore.processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web.UI.WebControls;

namespace ScadaCore
{
    public class DatabaseManagerService : IDatabaseManagerService
    {

        private static Dictionary<string, User> authenticatedUsers = new Dictionary<string, User>();
        public static Dictionary<string, Tag> inputTags = new Dictionary<string, Tag>();
        public static Dictionary<string, Tag> outputTags = new Dictionary<string, Tag>();

        public bool Registration(string username, string password)
        {
            string encryptedPassword = EncryptData(password);
            User user = new User(username, encryptedPassword);
            return UserProcessing.AddUser(user);
        }
        public string Login(string username, string password)
        {
            List<User> users = UserProcessing.GetAllUsers();
            foreach (var user in users)
            {
                if (username == user.Username && ValidateEncryptedData(password, user.EncryptedPassword))
                {
                    string token = GenerateToken(username);
                    authenticatedUsers.Add(token, user);
                    return token;
                }

            }
            return "Login failed";
        }

        public bool LogOut(string token)
        {
            return authenticatedUsers.Remove(token);
        }

        string GenerateToken(string username)
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] randVal = new byte[32];
            crypto.GetBytes(randVal);
            string randStr = Convert.ToBase64String(randVal);
            return username + randStr;
        }

        bool ValidateEncryptedData(string valueToValidate, string valueFromDatabase)
        {
            string[] arrValues = valueFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + valueToValidate);
            using (var sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(saltedValue);
                string enteredValueToValidate = Convert.ToBase64String(hash);
                return encryptedDbValue.Equals(enteredValueToValidate);
            }
        }

        string EncryptData(string valueToEncrypt)
        {
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
            string EncryptValue(string strValue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strValue);
                using (SHA256Managed sha = new SHA256Managed())
                {
                    byte[] hash = sha.ComputeHash(saltedPassword);
                    return $"{Convert.ToBase64String(hash)}:{saltValue}";
                }
            }
            return EncryptValue(valueToEncrypt);
        }
        public bool DatabaseEmpty()
        {
            using (var db = new DatabaseContext())
            {
                if (db.users.Count() == 0)
                {
                    return true;
                }
                return false;
            }

        }

        public static bool ContainsTag(string name)
        {
            if (inputTags.ContainsKey(name) || outputTags.ContainsKey(name))
            {
                return true;
            }
            return false;
        }

        public bool AddDigitalInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn)
        {
            if (ContainsTag(name))
            {
                return false;
            }
            DigitalInput tag = new DigitalInput(name, description, driver, address, scanTime, scanOn);
            inputTags[name] = tag;
            return true;
        }

        public bool AddAnalogInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn, int lowLimit, int hightLimit, string units)
        {
            if (ContainsTag(name))
            {
                return false;
            }
            AnalogInput tag = new AnalogInput(name, description, address, driver, scanTime, scanOn,new List<Alarm>(),lowLimit,hightLimit,units);
            inputTags[name] = tag;
            return true;
        }

        public bool AddDigitalOutputTag(string name, string description, string address, int initialValue)
        {
            if (ContainsTag(name))
            {
                return false;
            }
            DigitalOutput tag = new DigitalOutput(name, description, address,initialValue);
            outputTags[name] = tag;
            TagProcessing.AddDigitalOutputTag(tag,initialValue);
            return true;
        }

        public bool AddAnalogOutputTag(string name, string description, string address, int initialValue, int lowLimit, int hightLimit, string units)
        {
            if (ContainsTag(name))
            {
                return false;
            }
            AnalogOutput tag = new AnalogOutput(name, description, address, initialValue,lowLimit,hightLimit,units);
            outputTags[name] = tag;
            TagProcessing.AddAnalogOutputTag(tag, initialValue);
            return true;
        }

        public bool TurnOnScan(string name)
        {
            if (!ContainsTag(name))
            {
                return false;
            }
            Tag tag = inputTags[name];
            if(tag is DigitalInput)
            {
                DigitalInput digitalInput = (DigitalInput)tag;
                digitalInput.ScanOn = true;
            }
            else
            {
                AnalogInput analogInput = (AnalogInput)tag;
                analogInput.ScanOn = true;
            }
            return true;
            
        }

        public bool TurnOffScan(string name)
        {
            if (!ContainsTag(name))
            {
                return false;
            }
            Tag tag = inputTags[name];
            if (tag is DigitalInput)
            {
                DigitalInput digitalInput = (DigitalInput)tag;
                digitalInput.ScanOn = false;
            }
            else
            {
                AnalogInput analogInput = (AnalogInput)tag;
                analogInput.ScanOn = false;
            }
            return true;
        }

        public bool RemoveInputTag(string name)
        {
            if (!ContainsTag(name))
            {
                return false;
            }
            inputTags.Remove(name);
            return true;
        }

        public bool RemoveOutputTag(string name)
        {
            if (!ContainsTag(name))
            {
                return false;
            }
            outputTags.Remove(name);
            return true;
        }

        public Dictionary<string, double> GetDigitalOutputTags()
        {
            Dictionary<string, double>digitalTags=new Dictionary<string, double>();
            foreach(Tag tag in outputTags.Values)
            {
                if(tag is DigitalOutput)
                {
                    DigitalOutput digitalOutput = (DigitalOutput)tag;
                    digitalTags[digitalOutput.TagName] = digitalOutput.Value;
                }
            }
            return digitalTags;
        }

        public Dictionary<string, double> GetAnalogOutputTags()
        {
            Dictionary<string, double> analoglTags = new Dictionary<string, double>();
            foreach (Tag tag in outputTags.Values)
            {
                if (tag is AnalogOutput)
                {
                    AnalogOutput analogOutput = (AnalogOutput)tag;
                    analoglTags[analogOutput.TagName] = analogOutput.Value;
                }
            }
            return analoglTags;
        }

        public bool ChangeValueDigitalOutputTag(string name, int newValue)
        {
            if (!ContainsTag(name))
            {
                return false;
            }
           
            Tag tag=outputTags[name];
            DigitalOutput digitalOutput = (DigitalOutput)tag;
            digitalOutput.Value = newValue;
            TagProcessing.AddDigitalOutputTag(digitalOutput, newValue);
            return true;
        }

        public bool ChangeValueAnalogOutputTag(string name, int newValue)
        {
            if (!ContainsTag(name))
            {
                return false;
            }

            Tag tag = outputTags[name];
            AnalogOutput analogOutput = (AnalogOutput)tag;
            analogOutput.Value = newValue;
            TagProcessing.AddAnalogOutputTag(analogOutput, newValue);
            return true;
        }
    }
}
