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
     * usage

            string s_original = "test string";
            string s_encryptedData;
            string s_output;            
            s_encryptedData = AESSecureUtility.Encrypt(s_original);
            s_output = AESSecureUtility.Decrypt(s_encryptedData);
    */

    public static class AESSecureUtility
    {
        private static int _iterations = 2;
        private static int _keySize = 256;

        private static string hash     = "SHA1";       
        private static string salt = "54jdiejdur95jfug";
        private static string vector = "2h3jd8rkcu4j3d8r";
        private static string BuiltInPassword = "9d8fneud7r3kws9c"; 
        
        public static string Encrypt(string originalData)
        {
            return Encrypt<AesManaged>(originalData, BuiltInPassword);
        }

        public static string Encrypt(string value, string password) {
            return Encrypt<AesManaged>(value, password);
        }
        public static string Encrypt<T>(string value, string password) 
                where T : SymmetricAlgorithm, new() {
                    ASCIIEncoding asc = new ASCIIEncoding();
                    byte[] vectorBytes = asc.GetBytes(vector);
                    byte[] saltBytes = asc.GetBytes(salt);
                    byte[] valueBytes = asc.GetBytes(value);

            byte[] encrypted;
            using (T cipher = new T()) {
                PasswordDeriveBytes _passwordBytes = 
                    new PasswordDeriveBytes(password, saltBytes, hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes)) {
                    using (MemoryStream to = new MemoryStream()) {
                        using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write)) {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }
                cipher.Clear();
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string value)
        {
            return Decrypt<AesManaged>(value, BuiltInPassword);
        }

        public static string Decrypt(string value, string password) {
            return Decrypt<AesManaged>(value, password);
        }
        public static string Decrypt<T>(string value, string password) where T : SymmetricAlgorithm, new() {

            ASCIIEncoding asc = new ASCIIEncoding();
            byte[] vectorBytes = asc.GetBytes(vector);
            byte[] saltBytes = asc.GetBytes(salt);
            byte[] valueBytes = Convert.FromBase64String(value);

            byte[] decrypted;
            int decryptedByteCount = 0;

            using (T cipher = new T()) {
                PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(password, saltBytes, hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                try {
                    using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes)) {
                        using (MemoryStream from = new MemoryStream(valueBytes)) {
                            using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read)) {
                                decrypted = new byte[valueBytes.Length];
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                } catch (Exception ex) {
                    return String.Empty;
                }

                cipher.Clear();
            }
            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }

    }
}
