using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DelegateService.Utility
{
    public static class Encryption
    {
        private static byte[] Temp1 = { 0x81, 0xab, 0x7c, 0x9d, 0x9f, 0xa7, 0xc8, 0xf7, 
                                          0xe8, 0x7d, 0x87, 0xf8, 0xb7, 0xd0, 0x98, 0x09};

        private static byte[] initVectorBytes = { 0x25, 0xbc, 0x73, 0x7d, 0x6e, 0x7f, 0x67, 0xcc,
                                                   0xa7, 0x6c, 0x72, 0xd8, 0xe6, 0xa5, 0x9c, 0x8d };

        private static byte[] Temp = { 0x9d, 0x8b, 0x1a, 0x26, 0xc3, 0xd0, 0xe5, 0xf7, 
                                        0xd6, 0xdf, 0x2e, 0x3f, 0x1d, 0xc0, 0x8b, 0x5a };


        public static string Encrypt(string strPlainText)
        {
            try
            {
                byte[] bytKey = Encoding.UTF8.GetBytes(strPlainText);
                return Encrypt(bytKey);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private static string Encrypt(byte[] plainTextBytes)
        {
            try
            {
                byte[] TempBytes = EncryptEx(Temp, Temp1);
                byte[] cipherTextBytes = EncryptEx(plainTextBytes, TempBytes);

                // Convert encrypted data into a base64-encoded string.
                //string cipherText = Convert.ToBase64String(cipherTextBytes);
                string cipherText = HexAsciiConvertByte(ref cipherTextBytes);

                // Return encrypted string.
                return cipherText;
            }
            catch (System.Exception exp)
            {
                return null;
            }

        }

        private static byte[] EncryptEx(byte[] plainTextBytes, byte[] keyBytes)
        {
            try
            {

                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Padding = PaddingMode.Zeros;

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate encryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                                 keyBytes,
                                                                 initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream();

                // Define cryptographic stream (always use Write mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             encryptor,
                                                             CryptoStreamMode.Write);
                // Start encrypting.
                //memoryStream.SetLength(16);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                // Finish encrypting.
                cryptoStream.FlushFinalBlock();

                // Convert our encrypted data from a memory stream into a byte array.
                byte[] cipherTextBytes = memoryStream.ToArray();

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                return cipherTextBytes;

            }
            catch (System.Exception exp)
            {
                return null;
            }
        }

        public static string Decrypt(string strPlainText)
        {
            try
            {
                byte[] TempBytes = EncryptEx(Temp, Temp1);
                byte[] strDecoded = DecryptByt(strPlainText, TempBytes);
                return Encoding.UTF8.GetString(strDecoded);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private static byte[] DecryptByt(string cipherText, byte[] keyBytes)
        {
            try
            {
                // Convert our ciphertext into a byte array.
                //byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                byte[] cipherTextBytes = ConvertToHexByte(cipherText);
                if (cipherTextBytes == null)
                    return null;
                // First, we must create a password, from which the key will be 
                // derived. This password will be generated from the specified 
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                //PasswordDeriveBytes password = new PasswordDeriveBytes(
                //                                                passPhrase,
                //                                                saltValueBytes,
                //                                                hashAlgorithm,
                //                                                passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                //byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Padding = PaddingMode.Zeros;

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                                 keyBytes,
                                                                 initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                              decryptor,
                                                              CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. 
                // Let us assume that the original plaintext string was UTF8-encoded.
                //string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.   
                return plainTextBytes;
            }
            catch (System.Exception exp)
            {
                return null;
            }
        }

        private static string HexAsciiConvertByte(ref byte[] hex)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string temp;
                for (int i = 0; i <= hex.Length - 1; i++)
                {

                    temp = Convert.ToString(hex[i], 16);
                    if (temp.Length < 2)
                        temp = "0" + temp;

                    sb.Append(temp);

                }

                return sb.ToString();
            }
            catch (System.Exception exp)
            {
                return null;
            }
        }

        private static byte[] ConvertToHexByte(string asciiString)
        {
            try
            {
                byte[] bytTemp = new byte[asciiString.Length / 2];
                int n = 0;
                for (int i = 0; i < asciiString.Length - 1; i++)
                {
                    bytTemp[n] = (byte)((System.Convert.ToInt16(asciiString.Substring(i, 1), 16) * 0x10)
                                    + System.Convert.ToInt16(asciiString.Substring(i + 1, 1), 16));
                    i++;
                    n++;
                }
                return bytTemp;
            }
            catch (System.Exception exp)
            {
                return null;
            }

        }
    }
}
