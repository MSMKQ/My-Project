using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Classes
{
    public class ClsValidations
    {
        private static string _Key = "0123456789123456";







        public static string HashPasswords(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                return BitConverter.ToString(hashPassword).Replace("-", "").ToLower();
            }
        }

        public static string EncryptPassword(string password)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(_Key);
                    aesAlg.IV = new byte[aesAlg.BlockSize / 8];

                    ICryptoTransform Encrypto = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, Encrypto, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(password);
                        }

                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            catch (FormatException fx)
            {
                MessageBox.Show($"Error: {fx.Message}.", "Error Formatting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            catch (CryptographicException cx)
            {
                MessageBox.Show($"Error: {cx.Message}.", "Error Cryptographic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        public static string DecryptPassword(string password)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(_Key);
                    aesAlg.IV = new byte[aesAlg.BlockSize / 8];

                    ICryptoTransform decrypt = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (var msDecrypt = new MemoryStream(Convert.FromBase64String(password)))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decrypt, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (FormatException fx)
            {
                MessageBox.Show($"Error: {fx.Message}.", "Error Formatting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            catch (CryptographicException cx)
            {
                MessageBox.Show($"Error: {cx.Message}.", "Error Cryptographic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        
    }
}
