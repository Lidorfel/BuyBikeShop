using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
namespace BuyBikeShop.Data
{
    public static class ManageAES
    {
        
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

    //generate a random initialization vector (IV)
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
    //save the encryption key and IV to a secure configuration file
    public static void SaveKeyAndIV(byte[] key, byte[] iv)
    {
        Directory.CreateDirectory("secure");


        File.WriteAllBytes("secure/key.bin", key);
        File.WriteAllBytes("secure/iv.bin", iv);
    }

    //load the encryption key 
    public static byte[] LoadKey()
    {
        return File.ReadAllBytes("secure/key.bin");
    }

    //load the IV from the secure configuration file
    public static byte[] LoadIV()
    {
        return File.ReadAllBytes("secure/iv.bin");
    }
}
