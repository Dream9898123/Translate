using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translate.Translate
{
    public class TranslateEngineBase
    {
        private string name;
        public virtual string GetResult(string translateValue)
        {
            string changeValue = translateValue;
            if(!string.IsNullOrEmpty(translateValue))
            {
                changeValue = "";
                string[] dealValue = translateValue.Split(' ');
                for(int i = 0; i < dealValue.Length; i++)
                {
                    if(i == 0)
                    {
                        changeValue += dealValue[i].ToLower();
                    }
                    else
                    {
                        changeValue += FirstCharToUpper(dealValue[i]);
                    }
                }
            }
            return changeValue;
        }
        private string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
        public string sourceId;
        public string sourceKey;
        public bool needLine;
        private string Apppath = Directory.GetCurrentDirectory();

        public string Name { get => name; set => name = value; }

        public virtual void ReadIdAndKey()
        {
            //文件路径
            string filePath = Path.Combine(Apppath, this.GetType().Name + ".txt");
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
                    {
                        string s = File.ReadAllText(filePath);
                        if (!string.IsNullOrEmpty(s))
                        {
                            byte[] mybyte = Encoding.UTF8.GetBytes(s);
                            string idAndkey = Encoding.UTF8.GetString(mybyte);
                            if (!string.IsNullOrEmpty(idAndkey))
                            {
                                idAndkey = AESLock.GetInstance().Decrypt(idAndkey);
                                string[] IdKey = idAndkey.Split(';');
                                sourceId = IdKey[0];
                                sourceKey = IdKey[1];
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                sourceId = sourceKey = string.Empty;
            }
        }
        public virtual void SaveIDAndKey()
        {
            //文件路径
            string filePath = Path.Combine(Apppath, this.GetType().Name + ".txt");
            try
            {
                if(!string.IsNullOrEmpty(sourceId) && !string.IsNullOrEmpty(sourceKey))
                {
                    string idAndkey = sourceId + ";" + sourceKey;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    File.WriteAllText(filePath, AESLock.GetInstance().Encrypt(idAndkey));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public virtual void Init(string text1, string text2)
        {
            this.sourceId = text1;
            this.sourceKey = text2;
            this.SaveIDAndKey();
        }
    }
}
