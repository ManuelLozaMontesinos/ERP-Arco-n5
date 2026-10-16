using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ESD.Core.Cryptography
{
   public class Cryptor
   {

      private readonly byte[] salt = new byte[] { 0x26, 0x19, 0x81, 0x4E, 0x7F, 0x6D, 0x95, 0xFF, 0x26, 0x75, 0x64, 0x05, 0x26 };

      public byte[] GetEncodeBytes(string plainText)
      {
         return Encoding.UTF8.GetBytes(plainText);
      }

      public string GetDecodeBytes(byte[] encodeBytes)
      {
         return Encoding.UTF8.GetString(encodeBytes);
      }

      public string Encrypt(string plainText, byte[] publicKey)
      {
         Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(GetDecodeBytes(publicKey), salt);
         using (RijndaelManaged rijndaelManaged = new RijndaelManaged { Key = rfc2898DeriveBytes.GetBytes(32), IV = rfc2898DeriveBytes.GetBytes(16) })
         {
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
            {
               byte[] data = Encoding.UTF8.GetBytes(plainText);
               cryptoStream.Write(data, 0, data.Length);
               cryptoStream.FlushFinalBlock();

               return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
         }
      }
      public string Decrypt(string plainText, byte[] publicKey)
      {
         Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(GetDecodeBytes(publicKey), salt);

         using (RijndaelManaged rijndaelManaged = new RijndaelManaged { Key = rfc2898DeriveBytes.GetBytes(32), IV = rfc2898DeriveBytes.GetBytes(16) })
         {
            using (MemoryStream memoryStream = new MemoryStream())
            {
               using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
               {
                  byte[] data = Convert.FromBase64String(plainText);
                  cryptoStream.Write(data, 0, data.Length);
                  cryptoStream.FlushFinalBlock();

                  string value = Encoding.UTF8.GetString(memoryStream.ToArray());
                  return value;
               }
            }
         }
      }
   }
}
