using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApplication9.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            return View();
        }

    

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
      

        public ActionResult MyAccount()
        {
          
            return View();
        }

        public ActionResult Profile()
        {
          return View();
        }


        public ActionResult Update()
        {
          
            return View();
        }

        public ActionResult Welcome()
        {
           
            return View();

        }
        public ActionResult findADate()
        {

            return View();

        }
        public ActionResult blindDateProfiles()
        {

            return View();

        }
        public ActionResult dateSummary()
        {

            return View();

        }
        public ActionResult prototype()
        {
            return View();
        }
    }
}