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

        [HttpPost]
        public ActionResult Index(Login clientLogin)
        {
            if (ModelState.IsValid)
            {
                ClientRepo clientRepo = new ClientRepo();
                if (clientRepo.LoginAuth(clientLogin))
                {
                    return RedirectToAction("MyAccount", new { email = clientLogin.email });
                }
                else
                {
                    ViewBag.ErrorMessage = "The email and password you entered did not match our records. Please double-check and try again.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "This entry is invalid.";
            }

            return View(clientLogin);
        }


        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUP(SignUp newClient)
        {
            ViewBag.ErrorMessage = "";
            if (ModelState.IsValid)
            {
                ClientRepo clientRepo = new ClientRepo();
                if (clientRepo.clientExist(newClient))
                {
                    ViewBag.ErrorMessage = "Welcome to Dating!";
                    clientRepo.Create(newClient);
                    return RedirectToAction("Welcome", new { email = newClient.Email });
                }
                else
                {
                    ViewBag.ErrorMessage = "This email is already registered. Want to login or recover your password?!";
                }
                return View(newClient);
            }
            return View(newClient);
        }

        //public ActionResult MyAccount(string email, string password)
        //{
        //    ClientRepo clientRepo = new ClientRepo();
        //    ViewBag.Email = email;
        //    ViewBag.Password = password;
        //    ViewBag.UserName = clientRepo.getClientProfile(email, password).UserName;
        //    return View();
        //}

        public ActionResult MyAccount(string email)
        {
            ClientRepo clientRepo = new ClientRepo();
            ViewBag.Email = email;
            ViewBag.UserName = clientRepo.getClientDetail(email).UserName;
            List<ClientLocation> LocationList = new List<ClientLocation>();
            LocationList = clientRepo.GetClientsByLocation(email);
            return View(LocationList);
        }

        public ActionResult Profile(string email)
        {
            ClientRepo clientRepo = new ClientRepo();
            ClientProfile clientProfile = new ClientProfile();
            ViewBag.Email = email;
            ViewBag.UserName = clientRepo.getClientProfile(email).UserName;
            clientProfile = clientRepo.getClientProfile(email);
            return View(clientProfile);
        }


        public ActionResult Update(string email)
        {
            ClientRepo clientRepo = new ClientRepo();
            ViewBag.Email = email;
            ViewBag.UserName = clientRepo.getClientProfile(email).UserName;
            ClientDetails clientDetails = new ClientDetails();
            clientDetails = clientRepo.getClientDetail(email);
            return View(clientDetails);
        }

        public ActionResult Welcome(string email)
        {
            ClientRepo clientRepo = new ClientRepo();
            ViewBag.Email = email;
            ViewBag.UserName = clientRepo.getClientProfile(email).UserName;
            return View();

        }
    }
}