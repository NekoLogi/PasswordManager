using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;

namespace PMUtils
{
    public class Encryption
    {
        public const string KEY_NAME = "Manager.dll";
        public static TMP_AES AES = new TMP_AES();

        public class TMP_AES
        {
            public byte[] KEY;
            public byte[] IV;
        }

        public static byte[] Encrypt(string text, TMP_AES aes)
        {
            if (text == null || text.Length <= 0)
                throw new ArgumentNullException("plainText");

            Aes aesAlg = Aes.Create();
            aesAlg.Key = aes.KEY;
            aesAlg.IV = aes.IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }
            return msEncrypt.ToArray();
        }

        public static string Decrypt(byte[] text, TMP_AES aes)
        {
            if (text == null || text.Length <= 0)
                throw new ArgumentNullException("cipherText");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = aes.KEY;
                aesAlg.IV = aes.IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var msDecrypt = new MemoryStream(text);
                var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    plaintext = srDecrypt.ReadToEnd();
                }
            }
            return plaintext;
        }

        public static void GenerateAES()
        {
            var aes = Aes.Create();
            aes.GenerateIV();
            aes.GenerateKey();

            AES.IV = aes.IV;
            AES.KEY = aes.Key;
        }

        public static void SaveAES(TMP_AES aes)
        {
            File.WriteAllText(KEY_NAME, JsonConvert.SerializeObject(aes, Formatting.Indented));
        }

        public static void LoadAES()
        {
            try
            {
                var jsonData = File.ReadAllText(KEY_NAME);
                AES = JsonConvert.DeserializeObject<TMP_AES>(jsonData);
            } catch (Exception) { }
        }
    }
}
