using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PMUtils
{
    public class Account
    {
        public string Website { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public Account(string website, string name, string password)
        {
            Website = website;
            Name = name;
            Password = password;
        }

        public static bool Save(Account account)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new Data(account.Website, account.Name, Encryption.Encrypt(account.Password, Encryption.AES)), Formatting.Indented);
                File.WriteAllText(GeneratePath(account), json);
                return true;
            } catch (Exception) { }
            return false;
        }

        public static bool Delete(Account account)
        {
            try
            {
                File.Delete(GeneratePath(account));
                return true;
            } catch (Exception) { }
            return false;
        }

        private static string GeneratePath(Account account)
        {
            return $"Accounts/{account.Website.Replace(':', '-').Replace('/', '_')}-{account.Name}.json";
        }

        public static List<Account> Load()
        {
            if (!Directory.Exists("Accounts"))
                Directory.CreateDirectory("Accounts");
            if (!File.Exists("Manager.dll"))
            {
                Encryption.GenerateAES();
                Encryption.SaveAES(Encryption.AES);
            }

            string[] files = Directory.GetFiles("Accounts");
            List<Account> items = new List<Account>();
            foreach (string file in files)
            {
                try
                {
                    string data = File.ReadAllText(file);
                    var account = JsonConvert.DeserializeObject<Data>(data);
                    items.Add(new Account(account.Website, account.Name, account.Password != null ? Encryption.Decrypt(account.Password, Encryption.AES) : null));
                } catch (Exception) { return null; }
            }
            return items;

        }
    }
}
