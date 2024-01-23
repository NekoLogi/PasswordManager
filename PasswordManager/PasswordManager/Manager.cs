using PMUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PasswordManager
{
    internal class Manager
    {
        public static Account SelectedItem = null;
        static List<Account> ITEMS = new();

        public static bool Startup()
        {
            try
            {
                if (!File.Exists(Encryption.KEY_NAME))
                {
                    Encryption.GenerateAES();
                    Encryption.SaveAES(Encryption.AES);
                }
                if (!Directory.Exists("Accounts"))
                    Directory.CreateDirectory("Accounts");

                Encryption.LoadAES();

                ITEMS.Clear();
                ITEMS = Account.Load();
                if (ITEMS != null)
                    return true;

            } catch (Exception) { }

            return false;
        }

        public static Account[] Sort(string product)
        {
            if (product == null)
                return DisplayItems();
            else
                return SortByWebsite(product);
        }

        public static bool CreateItem(Account item)
        {
            try
            {
                if (ITEMS.Contains(item))
                    return false;

                ITEMS.Add(item);
                Account.Save(item);

                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public static bool EditItem(Account item)
        {
            try
            {
                int index = GetAccount();
                if (index != -1)
                {
                    ITEMS.RemoveAt(index);
                    ITEMS.Add(item);
                    Account.Delete(SelectedItem);
                    Account.Save(item);
                }
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public static int GetAccount()
        {
            for (int i = 0; i < ITEMS.Count; i++)
            {
                if (ITEMS[i].Website == SelectedItem.Website && ITEMS[i].Name == SelectedItem.Name && ITEMS[i].Password == SelectedItem.Password)
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool DeleteItem()
        {
            int index = GetAccount();
            try
            {
                if (index != -1)
                {
                    ITEMS.RemoveAt(index);
                    Account.Delete(SelectedItem);
                }
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public static string GeneratePassword(int length)
        {
            int specialChars = length * (100 - 70) / 100;
            int digits = length * (100 - 70) / 100;
            const string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string specialCharList = "!@#$%^&*()_+-=[]{}|;':\"<>,.?/\\";
            const string digitList = "0123456789";

            var chars = new char[length];
            var rnd = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < length; i++)
            {
                if (i < specialChars)
                    chars[i] = specialCharList[rnd.Next(0, specialCharList.Length)];
                else if (i < specialChars + digits)
                    chars[i] = digitList[rnd.Next(0, digitList.Length)];
                else if (i < specialChars + digits + 1)
                    chars[i] = upperCaseChars[rnd.Next(0, upperCaseChars.Length)];
                else
                    chars[i] = lowerCaseChars[rnd.Next(0, lowerCaseChars.Length)];
            }

            return new string(chars.OrderBy(s => rnd.Next()).ToArray());
        }

        private static Account[] SortByWebsite(string product)
        {
            var itemsByWebsite = SortName(product);
            var result = new List<Account>();

            foreach (var item in itemsByWebsite)
            {
                if (!result.Contains(item))
                    result.Add(item);
            }
            return result.ToArray();
        }

        private static Account[] SortName(string name)
        {
            List<Account> result = new List<Account>();
            foreach (var item in ITEMS)
            {
                if (item.Website.ToLower().Contains(name.ToLower()))
                    result.Add(item);

            }
            return result.ToArray();
        }

        private static Account[] DisplayItems()
        {
            return ITEMS.ToArray();
        }
    }
}
