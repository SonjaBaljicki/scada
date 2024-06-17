using ScadaCore.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ScadaCore.processing
{
    public static class UserProcessing
    {
        private static Dictionary<string, User> authenticatedUsers = new Dictionary<string, User>();
        
        private static object lockDatabase = new object();


        public static bool Registration(string username, string password)
        {
            string encryptedPassword = EncryptData(password);
            User user = new User(username, encryptedPassword);
            return AddUser(user);
        }

        public static string LogIn(string username, string password)
        {
            List<User> users = GetAllUsers();
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

        public static bool LogOut(string token)
        {
            return authenticatedUsers.Remove(token);
        }

        public static bool AddUser(User user)
        {
            lock (lockDatabase)
            {
                using (var db = new DatabaseContext())
                {
                    try
                    {
                        db.users.Add(user);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                return true;
            }
            
        }

        public static List<User> GetAllUsers()
        {
            lock (lockDatabase)
            {
                using (var db = new DatabaseContext())
                {
                    return db.users.ToList();
                }
            }
        }

        public static string GenerateToken(string username)
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] randVal = new byte[32];
            crypto.GetBytes(randVal);
            string randStr = Convert.ToBase64String(randVal);
            return username + randStr;
        }

        public static bool ValidateEncryptedData(string valueToValidate, string valueFromDatabase)
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

        public static string EncryptData(string valueToEncrypt)
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
    }
}