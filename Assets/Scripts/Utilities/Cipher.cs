
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Utility class for encrypt / decrypt values
    /// </summary>
    public static class Cipher
    {
        private static readonly string password = "vAP0Rbr34ker-M0b1l3@";

        /// <summary>
        /// Encrypt a content based on password
        /// </summary>
        /// <param name="content"> Content to be encrypted </param>
        /// <returns> Encrypted content </returns>
        public static string Encrypt(string content)
        {
            try
            {
                if (string.IsNullOrEmpty(content)) return string.Empty;

                byte[] contentBytes = Encoding.UTF8.GetBytes(content);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                byte[] encryptedText = Cipher.Encrypt(contentBytes, passwordBytes);
                return Convert.ToBase64String(encryptedText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Decrypt a content based on password
        /// </summary>
        /// <param name="encrypted"> Encrypted content </param>
        /// <returns> Decrypted content </returns>
        public static string Decrypt(string encrypted)
        {
            try
            {
                if (string.IsNullOrEmpty(encrypted)) return string.Empty;

                byte[] bytesToBeDecrypted = Convert.FromBase64String(encrypted);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                byte[] decryptedBytes = Cipher.Decrypt(bytesToBeDecrypted, passwordBytes);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Encrypt content bytes based on password bytes
        /// </summary>
        /// <param name="content"> Content bytes </param>
        /// <param name="password"> Password bytes </param>
        /// <returns> Encrypted content bytes </returns>
        private static byte[] Encrypt(byte[] content, byte[] password)
        {
            try
            {
                byte[] salt = { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        var key = new Rfc2898DeriveBytes(password, salt, 1000);

                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                        AES.Mode = CipherMode.CBC;

                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(content, 0, content.Length);
                            cryptoStream.Close();
                        }

                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Decrypt content bytes based on password bytes
        /// </summary>
        /// <param name="content"> Content bytes </param>
        /// <param name="password"> Password bytes </param>
        /// <returns> Decrypted content bytes </returns>
        private static byte[] Decrypt(byte[] content, byte[] password)
        {
            try
            {
                byte[] salt = { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        var key = new Rfc2898DeriveBytes(password, salt, 1000);

                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                        AES.Mode = CipherMode.CBC;

                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(content, 0, content.Length);
                            cryptoStream.Close();
                        }

                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
