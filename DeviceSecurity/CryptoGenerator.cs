using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Utilities;

namespace DeviceSecurity
{
    public static class CryptoGenerator
    {
        public static string Encrypt(string hashedAccessKey)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Config.EncryptionKey);
                aes.IV = iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(hashedAccessKey);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string Decrypt(string ecryptedKey)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(ecryptedKey);

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Config.EncryptionKey);
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
