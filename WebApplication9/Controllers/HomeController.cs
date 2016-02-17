using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication9.BusinessLogic;
using PagedList;
using System.Net;



using WebApplication9.ViewModels;
using System.Data;

namespace WebApplication9.Controllers
{
    public class HomeController : Controller
    {

        Boolean UserNoFound;
        Boolean PasswordIncorrent;
        Boolean Locked;

        Repository repo = new Repository();
        SSDDatingEntities11 context = new SSDDatingEntities11();

        // GET: Home
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(Login login)
        {
            // UserStore and UserManager manages data retreival.
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.Find(login.UserName,
                                                             login.Password);

            if (ModelState.IsValid)
            {
                if (ValidLogin(login))
                {
                    IAuthenticationManager authenticationManager
                                           = HttpContext.GetOwinContext().Authentication;
                    authenticationManager
                   .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = new ClaimsIdentity(new[] {
                                            new Claim(ClaimTypes.Name, login.UserName),
                                        },
                                        DefaultAuthenticationTypes.ApplicationCookie,
                                        ClaimTypes.Name, ClaimTypes.Role);

                    // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);

                    return RedirectToAction("Square", "Home", new {UserName=login.UserName});
                }
                else
                {
                    if (UserNoFound || PasswordIncorrent)
                    {
                        ViewBag.Message = "The username and password you entered did not match our records. Please double-check and try again.";
                    }
                    else if (Locked)
                    {
                        ViewBag.Message = "You have been locked. Please login in 10 minutes.";
                    }

                }

            }
            return View();
        }

        //show all the clients in square page
        [Authorize]
        public ActionResult Square(string UserName, string searchString, string interestString, string genderString, string sortOrder, int? page)
        {            
            IEnumerable<ClientDetailInfo> AllClients = repo.getAllClientsInOneLocation(UserName, searchString, interestString, genderString, sortOrder);

            ViewBag.Province = context.Clients.Find(UserName).province;
            ViewBag.CurrentSearchString = searchString;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentGenderString = genderString;
            ViewBag.CurrentInterestString = interestString;
            ViewBag.UserName = UserName;

            if (sortOrder == null || sortOrder == "Name")
            {
                ViewBag.CurrentSortOrder = "Name";

            }else if (sortOrder == "Age"){

                ViewBag.CurrentSortOrder = "Age";
            }
            else {
                ViewBag.CurrentSortOrder = "Age_Desc";
            }

            const int PAGE_SIZE = 8;
            int pageNumber = (page ?? 1);

            AllClients = AllClients.ToPagedList(pageNumber, PAGE_SIZE);


            ViewBag.interests = repo.getAllInterests();
            return View(AllClients);
          
        }

        [Authorize]
        public ActionResult foundDates(String UserName, string searchString, string interestringString, string genderString, string sortOrder)
        {
           IEnumerable <ClientDetailInfo> clients= repo.getAllClientsInOneLocation(UserName, searchString, interestringString, genderString, sortOrder);

            List<ClientDetailInfo> clientsHasAvail = new List<ClientDetailInfo> ();
            Client getClient = context.Clients.Find(UserName);


            foreach(ClientDetailInfo client in clients)
            {

                if (client.client.availableDate!=null)
                {
                    clientsHasAvail.Add(client);
                }
            }

            return View(clientsHasAvail);
        }

        //UserProfile page
        public ActionResult UserProfile(string userName)
        {
            ClientDetailInfo clientDetailInfo = repo.getOneUserDetailInfo(userName);

            
            ViewBag.interests = repo.getAllInterests();
            return View(clientDetailInfo);
        }

        [HttpPost]
        public ActionResult UserProfile(Client client,  HttpPostedFileBase photo, string[] interests, string country, string state)
        {
            if(photo!=null)
            {
                updateUserProfile(photo, client.UserName);
                if(!updateUserProfile(photo, client.UserName).Contains("successful"))
                {
                    ViewBag.uploadPhotoError = updateUserProfile(photo, client.UserName);

                    ClientDetailInfo clientDetailInfo = repo.getOneUserDetailInfo(client.UserName);
                    ViewBag.interests = repo.getAllInterests();
                    return View(clientDetailInfo);

                }
            }
                         
            repo.updatgeProfile(client, interests, country, state);

            return RedirectToAction("UserProfile", new { userName= client.UserName});
        }

        [Authorize]
        [HttpGet]
        public ActionResult findADate(string username)
        {
            AspNetUser user = repo.GetUser(username);
            ViewBag.UserName = user.UserName;
            ViewBag.Id = user.Id;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult findADate(String userName, DateTime availableDate, DateTime timepicker1, String gender, String location )
        {
            repo.saveAvailableDate(userName, availableDate, timepicker1);

            return RedirectToAction("foundDates", new { UserName = userName, Gender = gender, Location = location });
            
        }

        
        public ActionResult About()
        {

            return View();
        }
      

        public ActionResult MyAccount()
        {
          
            return View();
        }

       
      

        public ActionResult Welcome()
        {
           
            return View();

        }

       
        
        public ActionResult prototype()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(RegisteredUser newUser)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };


            var user = manager.FindByName(newUser.UserName);
            if (user != null)
            {
                ViewBag.Message = "Username '" + newUser.UserName + "' already exists! Please login or try another username!";
                return View();

            }

            var findUserByEmail = manager.FindByEmail(newUser.Email);

            if (findUserByEmail != null)
            {
                ViewBag.Message = "Email '" + newUser.Email + "' already exists! Please login or try another email!";
                return View();
            }


            var identityUser = new IdentityUser()
            {
                UserName = newUser.UserName,
                Email = newUser.Email
            };

            IdentityResult result = manager.Create(identityUser, newUser.Password);

            if (result.Succeeded)
            {
                
            return RedirectToAction("CompleteInfo", new { userName= identityUser.UserName, email= identityUser.Email, id=identityUser.Id });

        }
            return View();
        }

        [HttpGet]
        public ActionResult CompleteInfo(string userName, string email, string id)
        {
            ViewBag.email = email;
            ViewBag.username = userName;
            ViewBag.id = id;
            ClientInterestViewModel user = repo.getClientInterest(id);

            return View(user);
        }

        [HttpPost]
        public ActionResult CompleteInfo(ClientInterestViewModel client)
        {
            repo.saveClientInfo(client);

            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);

            CreateTokenProvider(manager, EMAIL_CONFIRMATION);

          var user = manager.FindByName(client.userName);
            var code = manager.GenerateEmailConfirmationToken(user.Id);

            var callbackUrl = Url.Action("ConfirmEmail", "Home",
                                            new { userId = user.Id, code = code },
                                                protocol: Request.Url.Scheme);

            string emailBody = "Please confirm your account by clicking this link: <a href=\""
                            + callbackUrl + "\">Confirm Registration</a>";

            MailHelper mailer = new MailHelper();

            string Subject = "Confirm registration";
            string response = mailer.EmailFromArvixe(
                                       new Message(client.email, Subject, emailBody));

            if (response.IndexOf("Success") >= 0)
            {
             //   ViewBag.Message = "A confirm email has been sent. Please check your email.";
                TempData["Message"] = "A confirm email has been sent. Please check your email.";
                return RedirectToAction("CompleteRegistration");
            }
            else {
                ViewBag.Message = response;
            }

            ClientInterestViewModel newClient = repo.getClientInterest(client.userId);
            return View(newClient);
           // return RedirectToAction("UserProfile", new { userName = client.userName});
        }


        public ActionResult CompleteRegistration()
        {
            return View();

        }

        //update profile images
        [HttpPost]
        public ActionResult UploadProfileImage(HttpPostedFileBase photo, ClientDetailInfo updateClientInfo)
        {
            string message = updateUserProfile(photo, updateClientInfo.client.UserName);
            ViewBag.Message = message;
            return RedirectToAction("UserProfile", new { userName = updateClientInfo.client
                .UserName });
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
            public ActionResult AddRole()
            {
                return View();
            }

        [Authorize(Roles = "Admin")]
        [HttpPost]
            public ActionResult AddRole(AspNetRole role)
            {
          
                context.AspNetRoles.Add(role);
                context.SaveChanges();
                return View();
            }

        [Authorize(Roles = "Admin")]
        [HttpGet]
            public ActionResult AddUserToRole()
            {
                return View();
            }

        [Authorize(Roles = "Admin")]
        [HttpPost]
            public ActionResult AddUserToRole(string userName, string roleName)
            {
              
                AspNetUser user = context.AspNetUsers
                                 .Where(u => u.UserName == userName).FirstOrDefault();
                AspNetRole role = context.AspNetRoles
                                 .Where(r => r.Name == roleName).FirstOrDefault();
            AspNetUserRole userRole = new AspNetUserRole();
            userRole.UserId = user.Id;
            userRole.RoleId = role.Id;
            context.AspNetUserRoles.Add(userRole);

                //user.AspNetRoles.Add(role);
                context.SaveChanges();
                return View();
            }
        // To allow more than one role access use syntax like the following:
        // [Authorize(Roles="Admin, Staff")]



  

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        bool ValidLogin(Login login)
            {
                UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(userStore)
                {
                    UserLockoutEnabledByDefault = true,
                    DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                    MaxFailedAccessAttemptsBeforeLockout = 5
                };
                var user = userManager.FindByName(login.UserName);

                if (user == null)
                {
                    UserNoFound = true;
                    return false;
                }
                    

                // User is locked out.
                if (userManager.SupportsUserLockout && userManager.IsLockedOut(user.Id))
                {
                    Locked = true;
                    return false;
                }
                   

                // Validated user was locked out but now can be reset.
                if (userManager.CheckPassword(user, login.Password) && userManager.IsEmailConfirmed(user.Id))

                {
                    if (userManager.SupportsUserLockout
                     && userManager.GetAccessFailedCount(user.Id) > 0)
                    {
                        userManager.ResetAccessFailedCount(user.Id);
                    }
                }
                // Login is invalid so increment failed attempts.
                else {
                    bool lockoutEnabled = userManager.GetLockoutEnabled(user.Id);
                    PasswordIncorrent = true;
                    if (userManager.SupportsUserLockout && userManager.GetLockoutEnabled(user.Id))
                    {
                        userManager.AccessFailed(user.Id);
                        return false;
                    }
                }
                return true;
            }
        

            const string EMAIL_CONFIRMATION = "EmailConfirmation";
            const string PASSWORD_RESET = "ResetPassword";

            void CreateTokenProvider(UserManager<IdentityUser> manager, string tokenType)
            {
                manager.UserTokenProvider = new EmailTokenProvider<IdentityUser>();
            }


            string updateUserProfile(HttpPostedFileBase photo, string UserName)
            {
                string extension = Path.GetExtension(photo.FileName).ToLower();
                string message = "";
                switch (extension)
                {
                    case ".png":
                    case ".jpg":
                    case ".gif":
                        break;
                    default:             
                    message = "We only accept .png, .jpg, and .gif!";
                    return message;
                }

                try
                {

                    if (photo != null && photo.ContentLength > 0)
                    {
                        var fileName = UserName + extension;
                        var path = Path.Combine(Server.MapPath("~/Images/Uploads/UserProfile"), fileName);

                        photo.SaveAs(path);
                        Client client = context.Clients.Find(UserName);
                        client.profile = fileName;
                        context.SaveChanges();

                        message = "Upload successful";
                    }
                }
                catch
                {
                    message = "Upload failed";
                }

            return message;
             

            }
        public ActionResult ConfirmEmail(string userId, string code)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindById(userId);


            CreateTokenProvider(manager, EMAIL_CONFIRMATION);
            try
            {
                IdentityResult result = manager.ConfirmEmail(userId, code);
                if (result.Succeeded)
                {
                    IAuthenticationManager authenticationManager
                                          = HttpContext.GetOwinContext().Authentication;
                    authenticationManager
                   .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = new ClaimsIdentity(new[] {
                                            new Claim(ClaimTypes.Name, user.UserName),
                                        },
                                        DefaultAuthenticationTypes.ApplicationCookie,
                                        ClaimTypes.Name, ClaimTypes.Role);

                    // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);

                    ViewBag.UserName = user.UserName;
                    ViewBag.Message = "You are now registered!";

                }
               
            }
            catch
            {
                ViewBag.Message = "Validation attempt failed!";
            }
            return View();
        }






        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
          
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindByEmail(email);

            if (user == null)
            {
                ViewBag.Message = "Email not found. Please double-check and try again.";
                return View();
            }
            CreateTokenProvider(manager, PASSWORD_RESET);

            var code = manager.GeneratePasswordResetToken(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Home",
                                         new { userId = user.Id, code = code },
                                         protocol: Request.Url.Scheme);
        
            string emailBody = "Please reset your password by clicking <a href=\""
                                     + callbackUrl + "\">here</a>";

            MailHelper mailer = new MailHelper();

            string Subject = "Confirm password reset";
            string response = mailer.EmailFromArvixe(
                                       new Message(user.Email, Subject, emailBody));

            if (response.IndexOf("Success") >= 0)
            {
                ViewBag.Message = "A confirm email has been sent. Please check your your email.";
                ViewBag.Email = user.Email;
            }
            else
            {
                ViewBag.Message = response;
            }
            return View();
        }


        [HttpGet]
        public ActionResult ResetPassword(string userID, string code)
        {
            ViewBag.PasswordToken = code;
            ViewBag.UserID = userID;
            return View();
        }


        [HttpPost]
        public ActionResult ResetPassword(ResetPassword resetPassword,
                                          string passwordToken, string userID)
        {
            if (ModelState.IsValid)
            {
                var userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
                var user = manager.FindById(userID);
                CreateTokenProvider(manager, PASSWORD_RESET);

                IdentityResult result = manager.ResetPassword(userID, passwordToken, resetPassword.Password);
                if (result.Succeeded)
                {
                    TempData["Message"] = "The password has been reset. Please login in";
                    return RedirectToAction("Index");
                }
                else {
                    ViewBag.Message = "The password has not been reset.";
                }
            }
            else
            {
                ViewBag.Message = "Passworld reset failed. Please make sure password combine alphabet letter and numbers.";
            }

            return View();

        }



    }

}