using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Translate.Translate.BaiduEngine
{
    public class TranslateEngineBaidu : TranslateEngineBase
    {
        public TranslateEngineBaidu()
        {
            this.Name = "百度引擎";
            this.ReadIdAndKey();
        }
        TranClass tranClass;
        public override string GetResult(string translateValue)
        {
            tranClass = new TranClass();
            tranClass.From = "zh";
            tranClass.To = "en";
            return GetTransReslut(translateValue);
        }
        public string GetTransReslut(string BeforeStr)
        {
            string resultStr = BeforeStr;
            try
            {
                WebClient client = new WebClient();  //引用System.Net
                string appid = this.sourceId;//改成自己的APP ID
                string rand = DateTime.Now.ToString("yyyyMMddhhmmss"); //这个是随机数,不用改
                string key = this.sourceKey; //改成自己的密钥
                string CmdStr = $"{appid}{BeforeStr}{rand}{key}";
                byte[] result = Encoding.UTF8.GetBytes(CmdStr);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                string md5Str = BitConverter.ToString(output).Replace("-", "").ToLower();//转换成MD5,且一定要小写
                string url = string.Format("http://api.fanyi.baidu.com/api/trans/vip/translate?q={0}&from={1}&to={2}&appid={3}&salt={4}&sign={5}", UrlEncode(BeforeStr), tranClass.From, tranClass.To, appid, rand, md5Str);
                var buffer = client.DownloadData(url);
                string result2 = Encoding.UTF8.GetString(buffer);
                StringReader sr = new StringReader(result2);
                JsonTextReader jsonReader = new JsonTextReader(sr);  //引用Newtonsoft.Json 自带
                JsonSerializer serializer = new JsonSerializer();
                var r = serializer.Deserialize<TranClass>(jsonReader); //Json转换成结果类
                resultStr = base.GetResult(r.Trans_result[0].dst);
            }
            catch(Exception ex)
            {

            }
            
            return resultStr;  //返回结果
        }
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        }
    }
    public class TranClass
    {
        public string From { get; set; }
        public string To { get; set; }
        public List<Trans_result> Trans_result { get; set; }
    }
    public class Trans_result
    {
        public string src { get; set; }
        public string dst { get; set; }
    }
}
