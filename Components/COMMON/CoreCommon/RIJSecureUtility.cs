using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ropal.CoreCommon
{
    /*
     * Usage

            string s_original = "test string";
            string s_encryptedData;
            string s_output;
            s_encryptedData = RIJSecureUtility.Encrypt(s_original);
            s_output = RIJSecureUtility.Decrypt(s_encryptedData);
     * 
     * */

    /*
    * another Usage

           string s_original = "test string";
           string s_encryptedData;
           string s_output;
           using (RijndaelManaged myRijndael = new RijndaelManaged())
           {
                myRijndael.GenerateKey();
                myRijndael.GenerateIV();
                s_encryptedData = RIJSecureUtility.Encrypt(s_original, myRijndael.Key, myRijndael.IV);
                s_output = RIJSecureUtility.Decrypt(s_encryptedData, myRijndael.Key, myRijndael.IV);
           }
    * 
    * */


    public static class RIJSecureUtility
    {
        private static readonly byte[] SALT = new byte[] { 0x66, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x9d, 0x08, 0x66, 0x3c };
        private static readonly byte[] KEY = new byte[] { 0x68, 0xfc, 0x5f, 0x30, 0xdd, 0xf6, 0x6a, 0xff, 0xd5, 0xff, 0x03, 0xaf, 0x9d, 0x08, 0x46, 0x6a };
        private static readonly byte[] IV = new byte[] { 0x66, 0xdc, 0xff, 0x00, 0xad, 0x8d, 0x3a, 0x3f, 0x35, 0xff, 0x53, 0x3f, 0x3d, 0x58, 0x56, 0xdc };


        public static string Encrypt(string originalData)
        {
            return Convert.ToBase64String(EncryptStringToBytes(originalData, KEY, IV));
        }

        public static string Encrypt(string originalData, byte[] Key, byte[] IV)
        {
            return Convert.ToBase64String(EncryptStringToBytes(originalData, Key, IV));
        }

        public static string Decrypt(string encryptedData)
        {
            byte[] valueBytes = Convert.FromBase64String(encryptedData);
            return DecryptStringFromBytes(valueBytes, KEY, IV);
        }

        public static string Decrypt(string encryptedData, byte[] Key, byte[] IV)
        {
            byte[] valueBytes = Convert.FromBase64String(encryptedData);
            return DecryptStringFromBytes(valueBytes, Key, IV);
        }

        public static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        { 
            string plaintext = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
