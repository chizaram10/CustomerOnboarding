using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    private static void GenerateKeyFromEmail(string email, out byte[] key, out byte[] iv)
    {
        using (var sha256 = SHA256.Create())
        {
            key = sha256.ComputeHash(Encoding.UTF8.GetBytes(email));
        }

        using (var md5 = MD5.Create())
        {
            iv = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
        }
    }

    public static string Encrypt(string password, string email)
    {
        GenerateKeyFromEmail(email, out byte[] key, out byte[] iv);

        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(password);
                        }
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }

    public static string Decrypt(string encryptedPassword, string email)
    {
        GenerateKeyFromEmail(email, out byte[] key, out byte[] iv);

        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedPassword)))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
