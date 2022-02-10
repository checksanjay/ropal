using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ropal.CoreCommon
{
    public static class SecureDataUtility
    {
        private const ushort ITERATIONS = 300;
        private static readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

        private static byte[] CreateKey(string password, int keySize)
        {
            DeriveBytes derivedKey = new Rfc2898DeriveBytes(password, SALT, ITERATIONS);
            return derivedKey.GetBytes(keySize >> 3);
        }

        public static byte[] Encrypt(byte[] data, string password)
        {
            byte[] encryptedData = null;
            using (AesCryptoServiceProvider provider = new AesCryptoServiceProvider())
            {
                provider.GenerateIV();
                provider.Key = CreateKey(password, provider.KeySize);
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;

                using (MemoryStream memStream = new MemoryStream(data.Length))
                {
                    memStream.Write(provider.IV, 0, 16);
                    using (ICryptoTransform encryptor = provider.CreateEncryptor(provider.Key, provider.IV))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.FlushFinalBlock();
                        }
                    }
                    encryptedData = memStream.ToArray();
                }
            }
            return encryptedData;
        }

        public static byte[] Decrypt(byte[] data, string password)
        {
            byte[] decryptedData = new byte[data.Length];
            using (AesCryptoServiceProvider provider = new AesCryptoServiceProvider())
            {
                provider.Key = CreateKey(password, provider.KeySize);
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                using (MemoryStream memStream = new MemoryStream(data))
                {
                    byte[] iv = new byte[16];
                    memStream.Read(iv, 0, 16);
                    using (ICryptoTransform decryptor = provider.CreateDecryptor(provider.Key, iv))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                        {
                            cryptoStream.Read(decryptedData, 0, decryptedData.Length);
                        }
                    }
                }
            }
            return decryptedData;
        }
    }
}
