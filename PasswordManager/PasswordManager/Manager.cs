using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    internal class Manager
    {
        public static Item SelectedItem = null;
        static List<Item> ITEMS = new List<Item>();

        public static bool Startup()
        {
            try
            {
                ITEMS.Clear();
                if (SaveSystem.Load(ITEMS))
                    return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Item[] Sort(string product)
        {
            if (product == null)
                return DisplayItems();
            else
                return SortByWebsite(product);
        }

        public static bool CreateItem(Item item)
        {
            try
            {
                if (ITEMS.Contains(item))
                    return false;

                ITEMS.Add(item);
                SaveSystem.Save(ITEMS);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool EditItem(Item item)
        {
            try
            {
                ITEMS.RemoveAt(ITEMS.IndexOf(SelectedItem));
                ITEMS.Add(item);
                SaveSystem.Save(ITEMS);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteItem()
        {
            try
            {
                ITEMS.RemoveAt(ITEMS.IndexOf(SelectedItem));
                SaveSystem.Save(ITEMS);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GeneratePassword(int length)
        {
            string password = "";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                switch (random.Next(3))
                {
                    case 0:
                        password += random.Next(9);
                        break;
                    case 1:
                        password += AddChar();
                        break;
                    case 2:
                        password += AddSymbol();
                        break;
                }
            }
            return password;
        }

        private static char AddChar()
        {
            Random random = new Random();
            char[] letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            return letters[random.Next(26)];
        }
       
        private static char AddSymbol()
        {
            Random random = new Random();
            char[] symbols = new char[] { '!', '@', '$', '^', '&', '*', '?'};
            return symbols[random.Next(6)];
        }
        private static Item[] SortByWebsite(string product)
        {
            var itemsByWebsite = SortName(product);
            var result = new List<Item>();

            foreach (var item in itemsByWebsite)
            {
                if (!result.Contains(item))
                    result.Add(item);
            }
            return result.ToArray();
        }

        private static Item[] SortName(string name)
        {
            List<Item> result = new List<Item>();
            foreach (var item in ITEMS)
            {
                if (item.WEBSITE.ToLower().Contains(name.ToLower()))
                    result.Add(item);
            }
            return result.ToArray();
        }

        private static Item[] DisplayItems()
        {
            return ITEMS.ToArray();
        }
    }
}
