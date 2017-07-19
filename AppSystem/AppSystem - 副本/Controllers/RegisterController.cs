using AppSystem.Common.Enum;
using AppSystem.Common.Model;
using AppSystem.Common.Ocr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AppSystem.Controllers
{
    public class RegisterController : Controller
    {
        private string IMG_BASE64;
        private string APPKEY = System.Configuration.ConfigurationManager.AppSettings["AppKey"].ToString();
        private string APPSECRET = System.Configuration.ConfigurationManager.AppSettings["AppSecret"].ToString();
        private int INDEX;
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UploadCardInfo()
        {
            return View();
        }
        public ActionResult test()
        {
            return View();
        }
        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase files,int type)
        {
            IMG_BASE64 = Request.Form["data"];
            INDEX = type;
            UserInfoModel model=CardRun();
            

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        private UserInfoModel CardRun()
        {
            try
            {
                string error = string.Empty;
                 OcrHelp help = new OcrHelp(APPKEY, APPSECRET);
                UserInfoModel model = help.GetCard(IMG_BASE64, INDEX == 0 ? Side.face : Side.back, out error);
                return model;
            }
            catch(Exception ex)
            { //MessageBox.Show("未知错误!");
            }
            return null;
        }
        private string face(UserInfoModel info)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("姓名:" + info.name);
            sb.AppendLine("性别:" + info.sex);
            sb.AppendLine("身份证号码:" + info.num);
            sb.AppendLine("出生年月:" + info.birth);
            sb.AppendLine("地址:" + info.address);
            return sb.ToString();
        }

        private string back(UserInfoModel info)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("有效期");
            sb.AppendLine("开始:" + info.start_date);
            sb.AppendLine("结束:" + info.end_date);
            return sb.ToString();
        }
        private byte[] CompressionImage(Stream fileStream, long quality)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromStream(fileStream))
            {
                using (Bitmap bitmap = new Bitmap(img))
                {
                    ImageCodecInfo CodecInfo = GetEncoder(img.RawFormat);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, CodecInfo, myEncoderParameters);
                        myEncoderParameters.Dispose();
                        myEncoderParameter.Dispose();
                        return ms.ToArray();
                    }
                }
            }
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                { return codec; }
            }
            return null;
        }
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
    }
}