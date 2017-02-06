using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinApiClass
{
    public class JS_SDK_Class
    {
        private string Timestamp;
        private string Noncestr;
        private string jsapi_ticket;
        private string signature;
//        private string jsApiList;
        public string getTimestamp()
        {

            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string res = Convert.ToInt64(ts.TotalSeconds).ToString();
            Timestamp = res;
            return res;
        }


        public string getNoncestr()
        {
            Random random = new Random();
            string res = BaseClass.Common.Common.GetMD5(random.Next(1000).ToString()).ToLower().Replace("s", "S");
            Noncestr = res;
            return res;
        }
        public string Getjsapi_ticket()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + GetWeiXinInf.Getaccess_token() + "&type=jsapi";
            string json = GetWeiXinInf.GetHttpWebRequestResponseTXT(url);

            JObject jo = JObject.Parse(json);
            jsapi_ticket = jo["ticket"].ToString();
            return jo["ticket"].ToString();
        }
        public string Creat_signature(string dqUrl)
        {
            string restr = "";
            string url = dqUrl;
            string str1 = "jsapi_ticket=";
            str1 += Getjsapi_ticket();
            str1 += "&noncestr=";
            str1 += Noncestr;
            str1 += "&timestamp=";
            str1 += Timestamp;
            str1 += "&url=";
            str1 += dqUrl;
            restr = ClassForString.EncryptToSHA1(str1);
            signature = restr;
            return signature;

        }

    }
}
