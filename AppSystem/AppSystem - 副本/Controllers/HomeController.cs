using AppSystem.Admin.Models;
using AppSystem.BLL;
using Maticsoft.Common.DEncrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppSystem.Controllers
{
    public class HomeController : Controller
    {
        Doctor doc = new Doctor();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public JsonResult SignIn(string username, string password)
        {
            var den = DEncrypt.AesEncode(password);
            try
            {
                string str = " Mobile = '" + username + "' and pass = '" + den + "'";
                //var res = doc.GetModelList(str);
                var res = doc.GetModelList(str).FirstOrDefault();
                if (res != null)
                {
                    Response.Cookies["user"].Value = res.ID + "," + res.Name;
                    Response.Cookies["user"].Expires = DateTime.Now.AddDays(1);
                    //HttpCookie aCookie = new HttpCookie("lastVisit");
                    //aCookie.Value = DateTime.Now.ToString();
                    //aCookie.Expires = DateTime.Now.AddDays(1);
                    //Response.Cookies.Add(aCookie);
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }
    }
}