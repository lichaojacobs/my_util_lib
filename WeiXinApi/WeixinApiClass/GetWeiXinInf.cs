using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeixinApiClass
{
    public static class GetWeiXinInf
    {
        public static string appid = ConfigurationManager.AppSettings["AppId"].ToString();
        public static string AppSecret = ConfigurationManager.AppSettings["AppSecret"].ToString();
        public static string Getaccess_token()
        {
            DBCLASSFORWEIXIN.Model.Wx_access_token wt = new DBCLASSFORWEIXIN.Model.Wx_access_token();
            DBCLASSFORWEIXIN.DAL.Wx_access_token wdd = new DBCLASSFORWEIXIN.DAL.Wx_access_token();
            wt = wdd.GetModel();
            if (wt == null)
            {

                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + AppSecret;
                JObject jo = JObject.Parse(GetHttpWebRequestResponseTXT(url));
                // string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();
                wt.create_time = DateTime.Now;
                wt.accessToken = jo["access_token"].ToString();
                wt.expires_in = int.Parse(jo["expires_in"].ToString());
                wdd.Update(wt);
            }
            else if (TokenExpired(wt.accessToken))
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + AppSecret;
                JObject jo = JObject.Parse(GetHttpWebRequestResponseTXT(url));
                // string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();
                wt.create_time = DateTime.Now;
                wt.accessToken = jo["access_token"].ToString();
                wt.expires_in = int.Parse(jo["expires_in"].ToString());
                wdd.Update(wt);
            }
            return wt.accessToken;
        }
        #region 验证Token是否过期
        /// <summary>
        /// 验证Token是否过期
        /// </summary>
        public static bool TokenExpired(string access_token)
        {
            string jsonStr = GetHttpWebRequestResponseTXT(string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", access_token));
            // JObject jo = JObject.Parse(jsonStr);
            if (jsonStr.Contains("errcode"))
            {
                return true;//过期
            }
            return false;//未过期
        }
        #endregion
        /// <summary>
        /// 获取返http回值
        /// </summary>
        /// <param name="strURL"></param>
        /// <returns></returns>
        public static string GetHttpWebRequestResponseTXT(string strURL)
        {
            System.Net.HttpWebRequest request;
            // 创建一个HTTP请求
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);

            //request.Method="get";
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            return responseText;
        }
        public static string PostData2Page(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...  
            try
            {

                // 设置参数  
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据  
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

        public static string getCodeForOpenId(string url)
        {
            try
            {
                System.Net.HttpWebRequest request;
                // 创建一个HTTP请求
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);

                //request.Method="get";
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string responseText = myreader.ReadToEnd();
                Regex reg = new Regex(@"code=([\w]+)");
                Match match = reg.Match(response.ResponseUri.PathAndQuery);
                return match.Groups[1].Value;
            }
            catch (Exception)
            {
                BaseClass.Common.LoggerUtil.printLog("code 无效");
                return null;
            }
        }
    }
}
