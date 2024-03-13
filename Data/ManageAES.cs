using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
namespace BuyBikeShop.Data
{
    public static class ManageAES
    {
        /*public static string Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;    
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs)) 
                        { 
                            sw.Write(plainText);
                            encrypted = ms.ToArray();
                        }
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }*/
        public static string Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
        public static string Decrypt(string cipher, byte[] key, byte[] iv)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipher);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        /*public static string Decrypt(string CipherText, byte[] Key, byte[] IV)
        {
            byte[] cipher = Convert.FromBase64String(CipherText);
            string plainText = "";
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            plainText = sr.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        } */
    }
}

public static class KeyManager
{
    private static byte[] GenerateKey()
    {
        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            return aes.Key;
        }
    }

    // Method to generate a random initialization vector (IV)
    private static byte[] GenerateIV()
    {
        using (Aes aes = Aes.Create())
        {
            aes.GenerateIV();
            return aes.IV;
        }
    }
    public static byte[] GetGenerateKey()
    {
        return GenerateKey();
    }
    public static byte[] GetGenerateIV()
    {
        return GenerateIV();
    }
    // Method to save the encryption key and IV to a secure configuration file
    public static void SaveKeyAndIV(byte[] key, byte[] iv)
    {
        // Ensure that the directory exists
        Directory.CreateDirectory("secure");

        // Write the key and IV to a file
        File.WriteAllBytes("secure/key.bin", key);
        File.WriteAllBytes("secure/iv.bin", iv);
    }

    // Method to load the encryption key from the secure configuration file
    public static byte[] LoadKey()
    {
        return File.ReadAllBytes("secure/key.bin");
    }

    // Method to load the IV from the secure configuration file
    public static byte[] LoadIV()
    {
        return File.ReadAllBytes("secure/iv.bin");
    }
}
