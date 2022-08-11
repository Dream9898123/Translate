using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Translate
{
    public class AESLock
    {
        #region single

        private static object syncObj = new object();
        private static AESLock instance = null;
        private AESLock()
        { }
        public static AESLock GetInstance()
        {
            lock (syncObj)
            {
                if (instance == null)
                {
                    instance = new AESLock();
                }
            }
            return instance;
        }

        #endregion

        #region body

        //测试密钥向量    
        private readonly static byte[] MY_IV = {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07
        };
        //测试密钥
        private const string publicKey = "whttranslatelock";

        #endregion

        #region IXmlEncrypt 成员

        /// <summary>   
        /// AES加密算法   
        /// </summary>   
        /// <param name="plainText">明文字符串</param>   
        /// <returns>返回加密后的密文字节数组</returns>   
        public string Encrypt(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            bytes = Encrypt(bytes);
            string res = Convert.ToBase64String(bytes);
            return res;
        }

        /// <summary>   
        /// AES解密   
        /// </summary>   
        /// <param name="cipherText">密文字节数组</param>   
        /// <returns>返回解密后的字符串</returns>   
        public string Decrypt(string text)
        {
            byte[] bytes = Convert.FromBase64String(text);
            bytes = Decrypt(bytes);
            string res = Encoding.UTF8.GetString(bytes);
            //去除尾部自动填充的无效字符
            res = res.TrimEnd('\0');
            return res;
        }

        #endregion

        #region  成员

        public byte[] Encrypt(byte[] data)
        {
            //分组加密算法   
            SymmetricAlgorithm des = Rijndael.Create();

            //设置密钥及密钥向量   
            des.Key = Encoding.UTF8.GetBytes(publicKey);
            des.IV = MY_IV;

            byte[] cipherBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();//得到加密后的字节数组   
                    //cs.Close();
                }
                //ms.Close();
            }
            return cipherBytes;
        }

        public byte[] Decrypt(byte[] data)
        {
            //分组加密算法   
            SymmetricAlgorithm des = Rijndael.Create();

            //设置密钥及密钥向量   
            des.Key = Encoding.UTF8.GetBytes(publicKey);
            des.IV = MY_IV;

            byte[] decryptBytes = new byte[data.Length];
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    //cs.Close();
                }
                //ms.Close();
            }
            return decryptBytes;
        }

        #endregion

    }
}
