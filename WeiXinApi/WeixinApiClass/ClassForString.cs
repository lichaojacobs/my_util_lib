using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WeixinApiClass
{
    public static class ClassForString
    {
        #region 获取由SHA1加密的字符串
        public static string EncryptToSHA1(string str)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(str);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
        #endregion

        #region 获取由MD5加密的字符串
        public static string EncryptToMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(str);
            byte[] str2 = md5.ComputeHash(str1, 0, str1.Length);
            md5.Clear();
            (md5 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }
        #endregion
    }
}
