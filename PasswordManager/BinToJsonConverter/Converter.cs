using Newtonsoft.Json;
using PasswordManager;
using System;
using System.Collections.Generic;
using System.IO;

namespace BinToJsonConverter
{
    internal class Converter
    {
        public string path { get; private set; }


        public Converter(string fullPath)
        {
            path = fullPath;
        }

        public void Convert()
        {
            var data = SaveSystem.Load();
            if (data == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to convert {path}");
                Console.ResetColor();
                return;
            }
            SaveAsJson(data);
        }

        private void SaveAsJson(List<Item> data)
        {
            PMUtils.Encryption.GenerateAES();
            var key = PMUtils.Encryption.AES;
            PMUtils.Encryption.SaveAES(key);

            foreach (var acc in data)
            {
                try
                {
                    Account account = new(
                        acc.WEBSITE,
                        acc.NAME,
                        acc.PASSWORD == "" ? null : PMUtils.Encryption.Encrypt(acc.PASSWORD, key),
                        key);
                    string path = $"Accounts/{account.Website.Replace(':', '-').Replace('/', '_')}-{account.Name}.json";
                    string json = JsonConvert.SerializeObject(account, Formatting.Indented);
                    File.WriteAllText(path, json);
                } catch (Exception) { }
            }
            Console.WriteLine("Accounts converted.");
        }
    }

    public class Account
    {
        public string Website;
        public string Name;
        public byte[] Password;

        public Account(string website, string name, byte[] password, PMUtils.Encryption.TMP_AES aes)
        {
            Website = website;
            Name = name;
            Password = password;
        }
    }
}
