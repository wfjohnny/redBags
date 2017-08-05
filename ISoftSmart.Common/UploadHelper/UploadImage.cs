using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Net.Http;
using System.Web;
using System.Drawing.Imaging;

namespace ISoftSmart.Common.UploadHelper
{
   public class UploadImage
    {
        /// <summary> 
        /// 字节流转换成图片 
        /// </summary> 
        /// <param name="byt">要转换的字节流</param> 
        /// <returns>转换得到的Image对象</returns> 
        public Image BytToImg(byte[] byt)
        {
            MemoryStream ms = new MemoryStream(byt);
            Image img = Image.FromStream(ms);
            return img;
        }
        public System.Drawing.Image UploadImg(string openid,string byt)
        {
            byte[] arr = Convert.FromBase64CharArray(byt.ToCharArray(), 0, byt.Length);
            MemoryStream ms = new MemoryStream(arr);
            try
            {
                var bmp = Image.FromStream(ms);
                var bmpfileName = openid == null ? "olDlVsy5vYjAhbWIDMYaj5PSVp04.jpg" : openid + ".jpg";
                string root = HttpContext.Current.Server.MapPath("~/QRFile/");
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/QRFile/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/QRFile/"));
                }
                var path = root + bmpfileName;
                bmp.Save(path + ".jpg", ImageFormat.Png);

                ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                ms.Close();
                return null;
            }
        }
    }
}
