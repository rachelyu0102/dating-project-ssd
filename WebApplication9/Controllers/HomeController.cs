using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication9.Models;

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
    }
}