using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeixinApiClass
{
    public enum MediaType
    {
        /// <summary>
        /// 图片（image）: 1M，支持JPG格式
        /// </summary>
        image,
        /// <summary>
        /// 语音（voice）：2M，播放长度不超过60s，支持AMR\MP3格式
        /// </summary>
        voice,
        /// <summary>
        /// 视频（video）：10MB，支持MP4格式
        /// </summary>
        video,
        /// <summary>
        /// 缩略图（thumb）：64KB，支持JPG格式
        /// </summary>
        thumb
    }
    public class MeaidWxUpLoad
    {
        /// <summary>
        /// 微信上传多媒体文件 图片
        /// </summary>
        /// <param name="filepath">文件绝对路径</param>
        public string WxUpLoad(string filepath, string mt)
        {
            using (WebClient client = new WebClient())
            {
                byte[] b = client.UploadFile(string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", GetWeiXinInf.Getaccess_token(), mt.ToString()), filepath);//调用接口上传文件
                string retdata = Encoding.Default.GetString(b);//获取返回值
                JObject jo = JObject.Parse(retdata);
                string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();

                if (retdata.Contains("media_id"))//判断返回值是否包含media_id，包含则说明上传成功，然后将返回的json字符串转换成json
                {
                    return values[1].ToString();
                }
                else
                {//否则，写错误日志
                  //  WriteBug(retdata);//写错误日志
                    return "";
                }
            }
        }
    }
}
