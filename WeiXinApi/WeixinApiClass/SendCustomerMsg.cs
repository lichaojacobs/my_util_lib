using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeixinApiClass
{
   public class SendCustomerMsg
    {


       public string SendMsg(string openid,string postData)
       {
           string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + GetWeiXinInf.Getaccess_token();

           return GetWeiXinInf.PostData2Page(url, postData);
       }
    }
}
