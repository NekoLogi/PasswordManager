using System;
using System.IO;

namespace BinToJsonConverter
{
    internal class Program
    {

        static void Main(string[] args)
        {
            string path = "Accounts";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }

            path = "Storage.save";
            if (!File.Exists(path))
            {
                Console.WriteLine("Could not find Storage.save");
                return;
            }

            var converter = new Converter(path);
            converter.Convert();

            Console.WriteLine("Press 'Enter' to close this window...");
            Console.ReadLine();
        }
    }
}
