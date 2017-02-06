using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace WeixinApiClass
{
    public class CodeMaker
    {
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径</param>
        /// <param name="thumbnailPath">缩略图路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式:HW指定高宽缩放(可能变形);W指定宽，高按比例 H指定高，宽按比例 Cut指定高宽裁减(不变形,没有空白或者黑色边线)</param>　　
        /// <param name="mode">要缩略图保存的格式(gif,jpg,bmp,png) 为空或未知类型都视为jpg</param>　　
        public void MakeCodeWithZhiwen(string originalImagePath, string thumbnailPath)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            Image zhiwen = Image.FromFile("D://LVWEIBA//WeixinSys//Media//codeZhiwen.jpg");
            Image dFont = Image.FromFile("D://LVWEIBA//WeixinSys//Media//codeFont.jpg");
            int towidth = originalImage.Width+zhiwen.Width;
            int toheight = originalImage.Height+dFont.Height;
            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(Color.White);
            //在指定位置并且按指定大小绘制原图片的指定部分
            //g.DrawImage(originalImage, 0, 0);
            g.DrawImage(originalImage, new Rectangle(0, 0, 430, 430), new Rectangle(0, 0, 430, 430), GraphicsUnit.Pixel);
            g.DrawImage(zhiwen, new Rectangle(430, 0, 290, 430), new Rectangle(430, 0, 290, 430), GraphicsUnit.Pixel);
            g.DrawImage(dFont, new Rectangle(0, 430, 720, 100), new Rectangle(0, 430, 720, 100), GraphicsUnit.Pixel);
            g.DrawImage(zhiwen, originalImage.Width, 0);
            g.DrawImage(dFont, 0, originalImage.Height);
            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                zhiwen.Dispose();
                dFont.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }


        public void MakeHaibaoWithOpenid(string openid)
        {
            Image Codeimg = Image.FromFile("D://LVWEIBA//WeixinSys//Media//code//" + openid + ".jpg");
            string scode = "D://LVWEIBA//WeixinSys//Media//xcode//" + openid + ".jpg";
            Image haibao = Image.FromFile("D://LVWEIBA//WeixinSys//Media//haibao.jpg");
            Image Touxiang = Image.FromFile("D://LVWEIBA//WeixinSys//Media//headimg//" + openid + ".jpg");
            string stx = "D://LVWEIBA//WeixinSys//Media///xtx//" + openid + ".jpg";
            string haibaoPath = "D://LVWEIBA//WeixinSys//Media//haibao//" + openid + ".jpg";
            //int towidth = originalImage.Width + haibao.Width;
            //int toheight = originalImage.Height + Touxiang.Height;
            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(720, 832);
            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(Color.White);
            //在指定位置并且按指定大小绘制原图片的指定部分
            //g.DrawImage(originalImage, 0, 0);
            g.DrawImage(haibao, new Rectangle(0, 0, 720, 830), new Rectangle(0, 0, 720, 832), GraphicsUnit.Pixel);//海报
          //  g.DrawImage(TouxiangBG, new Rectangle(122, 292, 120, 120), new Rectangle(0, 0, Touxiang.Width, Touxiang.Height), GraphicsUnit.Pixel);//头像
            g.DrawImage(Touxiang, new Rectangle(610, 79, 90, 90), new Rectangle(0, 0, Touxiang.Width, Touxiang.Height), GraphicsUnit.Pixel);//头像
            g.DrawImage(Codeimg, new Rectangle(427, 8, 160, 160), new Rectangle(0, 0, Codeimg.Width, Codeimg.Height), GraphicsUnit.Pixel);//二维码
            //g.DrawImage(haibao, originalImage.Width, 0);
            //g.DrawImage(Touxiang, 0, originalImage.Height);
            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(haibaoPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                Codeimg.Dispose();
                haibao.Dispose();
                Touxiang.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

    }
}
