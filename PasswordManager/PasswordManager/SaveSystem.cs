using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PasswordManager
{
    public class SaveSystem
    {
        static string path = "storage.save";

        public static bool Save(List<Item> items)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Create);


                formatter.Serialize(stream, items.ToArray());
                stream.Close();

                return true;

            } catch (Exception)
            {
                return false;
            }
        }

        public static List<Item> Load()
        {
            List<Item> items = new List<Item>();
            if (File.Exists(path))
            {
                foreach (var item in GetData())
                    items.Add(item);

                return items;
            }
            else
                return null;
        }

        static Item[] GetData()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Item[] data = formatter.Deserialize(stream) as Item[];
            stream.Close();

            return data;
        }
    }
}
