using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace Translate.Translate.TencentEngine
{
    public class TranslateEngineTencent : TranslateEngineBase
    {
        public TranslateEngineTencent()
        {
            this.Name = "腾讯引擎";
            this.ReadIdAndKey();
        }
        public override string GetResult(string translateValue)
        {
            try
            {
                // 实例化一个认证对象，入参需要传入腾讯云账户secretId，secretKey,此处还需注意密钥对的保密
                // 密钥可前往https://console.cloud.tencent.com/cam/capi网站进行获取
                Credential cred = new Credential
                {
                    SecretId = this.sourceId,
                    SecretKey = this.sourceKey
                };
                // 实例化一个client选项，可选的，没有特殊需求可以跳过
                ClientProfile clientProfile = new ClientProfile();
                // 实例化一个http选项，可选的，没有特殊需求可以跳过
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("tmt.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                // 实例化要请求产品的client对象,clientProfile是可选的
                TmtClient client = new TmtClient(cred, "ap-shanghai", clientProfile);
                // 实例化一个请求对象,每个接口都会对应一个request对象
                TextTranslateRequest req = new TextTranslateRequest();
                req.SourceText = translateValue;
                req.Source = "auto";
                req.Target = "en";
                req.ProjectId = 0;
                // 返回的resp是一个TextTranslateResponse的实例，与请求对象对应
                TextTranslateResponse resp = client.TextTranslateSync(req);
                // 输出json格式的字符串回包
                //Console.WriteLine(AbstractModel.ToJsonString(resp));
                TargerClass tc = AbstractModel.FromJsonString<TargerClass>(AbstractModel.ToJsonString(resp));
                return base.GetResult(tc.TargetText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return string.Empty;
        }
        public class TargerClass
        {
            public string TargetText;
            public string Source;
            public string Target;
            public string RequestId;
        }
    }
}
