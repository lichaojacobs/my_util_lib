using BaseClass.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinApiClass
{
    public class CheckSignature
    {
        public bool Check(string signature, string timestamp, string nonce, string token)
        {
            string[] tmpArr = { token, timestamp, nonce };
            List<string> tmpArrsignList = tmpArr.ToList();
            //对signList按照ASCII码从小到大的顺序排序
            tmpArrsignList.Sort();

            string signOld = tmpArrsignList[0] + tmpArrsignList[1] + tmpArrsignList[2];
            signOld = SHA1(signOld).ToLower();
            TXT_Help th = new TXT_Help();

           // th.ReFreshTXT(signature + "/n/r" + signOld, "D:\\msg\\" + DateTime.Now.ToString("mddhhmmssffff") + ".txt");

            if (signOld == signature.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public string GetSignature(string timestamp, string nonce, string token)
        {
            return "";
        }

        private static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
    }
}
