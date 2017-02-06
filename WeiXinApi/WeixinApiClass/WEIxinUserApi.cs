using BaseClass.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinApiClass
{
    public class WEIxinUserApi
    {
        /// <summary>
        /// 获取关注会员数量
        /// </summary>
        /// <returns></returns>
        public int GetsubscribeUserCount()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + GetWeiXinInf.Getaccess_token();
            string json = GetWeiXinInf.GetHttpWebRequestResponseTXT(url);
            if (json.Contains("errcode"))
            {
                return 0;
            }
            else
            {
                JObject jo = JObject.Parse(json);
                string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();

                return int.Parse(values[0].ToString());
            }

        }
        public string GetUserOpenid(string code)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + GetWeiXinInf.appid + "&secret=" + GetWeiXinInf.AppSecret + "&code="+code+"&grant_type=authorization_code ";
            string json = GetWeiXinInf.GetHttpWebRequestResponseTXT(url);
            if (json.Contains("errcode"))
            {
                return "";
            }
            else
            {
                JObject jo = JObject.Parse(json);
                return jo["openid"].ToString();
            }

        }
        public DBCLASSFORWEIXIN.Model.LocalWeixinUser GetSingleUserInf(string openid)
        {
            DBCLASSFORWEIXIN.Model.LocalWeixinUser lum = new DBCLASSFORWEIXIN.Model.LocalWeixinUser();
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + GetWeiXinInf.Getaccess_token() + "&openid=" + openid + "&lang=zh_CN";
            string json = GetWeiXinInf.GetHttpWebRequestResponseTXT(url);
           // TXT_Help th = new TXT_Help();
            if (json.Contains("errcode"))
            {
                return null;
            }
            else
            {
                JObject jo = JObject.Parse(json);

                //th.ReFreshTXT(jo["nicknamewww"].ToString(), "D:\\msg\\" + DateTime.Now.ToString("yyyymddhhmmssffff") + ".txt");

                lum.city = jo["city"].ToString();
                lum.country = jo["country"].ToString();
                lum.groupid = int.Parse(jo["groupid"].ToString());
                lum.headimgurl = jo["headimgurl"].ToString();
                lum.nickname = jo["nickname"].ToString();
                lum.openid = jo["openid"].ToString();
               // lum.pid = jo["headimgurl"].ToString();
                lum.province = jo["province"].ToString();
               // lum.regtime = jo["headimgurl"].ToString();
                lum.remark = jo["remark"].ToString();
                lum.sex = int.Parse(jo["sex"].ToString());
                lum.subscribe = int.Parse(jo["subscribe"].ToString());
                lum.subscribe_time = jo["subscribe_time"].ToString();
                lum.refresh_token = "";
                if (jo["subscribe_time"] != null)
                {
                    lum.unionid = jo["subscribe_time"].ToString();
                }
                
            }
            return lum;
        }
        /// <summary>
        /// 获取关注会员openid数组集合
        /// </summary>
        /// <returns></returns>
        public string[] GetTop10000UserList()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + GetWeiXinInf.Getaccess_token();
            string json = GetWeiXinInf.GetHttpWebRequestResponseTXT(url);
            //TXT_Help th = new TXT_Help();
            if (json.Contains("errcode"))
            {
                // th.ReFreshTXT(json, "D:\\msg\\" + DateTime.Now.ToString("yyyymddhhmmssffff") + ".txt");
                return null;
            }
            else
            {
                JObject jo = JObject.Parse(json);
                string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();
                int len = int.Parse(values[0].ToString());
                string valueslist = values[2].ToString();
                valueslist = valueslist.Replace("\"", string.Empty);
                valueslist = valueslist.Replace("{", string.Empty);
                valueslist = valueslist.Replace("}", string.Empty);
                valueslist = valueslist.Replace("\n", string.Empty);
                valueslist = valueslist.Replace("[", string.Empty);
                valueslist = valueslist.Replace("]", string.Empty);
                valueslist = valueslist.Replace("\r", "");
                valueslist = valueslist.Replace("\n", "");
                valueslist = valueslist.Replace("\t", "");
                valueslist = valueslist.Replace(" ", "");
                valueslist = valueslist.Replace("openid:", string.Empty);
                return valueslist.Split(',');
                //th.ReFreshTXT(valueslist.ToString(), "D:\\msg\\" + DateTime.Now.ToString("yyyymddhhmmssffff") + ".txt");

            }

        }
    }
}
