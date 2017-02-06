using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinApiClass
{
    public static class PostDataToUrl
    {
        /// <summary>
        /// post数据到指定接口并返回数据
        /// </summary>
        public static string PostXmlToUrl(string url, string postData)
        {
            string returnmsg = "";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                returnmsg = wc.UploadString(url, "POST", postData);
            }
            return returnmsg;
        }
    }
}
