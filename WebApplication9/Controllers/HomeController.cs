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


using WebApplication9.ViewModels;

namespace WebApplication9.Controllers
{
    public class HomeController : Controller
    {

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

                    return RedirectToAction("Square", "Home");
                }

            }
            return View();
        }

        //show all the clients in square page
        [Authorize]
        public ActionResult Square(Login login, string searchString, string interestString, string genderString, string sortOrder, int? page)
        {
            IEnumerable<ClientDetailInfo> AllClients = repo.getAllClientsDetailsInfo(searchString, interestString, genderString, sortOrder);
           
            ViewBag.CurrentSearchString = searchString;
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentGenderString = genderString;
            ViewBag.CurrentInterestString = interestString;

            if (sortOrder == null || sortOrder == "Name")
            {
                ViewBag.CurrentSortOrder = "Name";

            }else if (sortOrder == "Age"){

                ViewBag.CurrentSortOrder = "Age";
            }
            else {
                ViewBag.CurrentSortOrder = "Age_Desc";
            }

            const int PAGE_SIZE = 4;
            int pageNumber = (page ?? 1);

            AllClients = AllClients.ToPagedList(pageNumber, PAGE_SIZE);


            ViewBag.interests = repo.getAllInterests();
            return View(AllClients);
          
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
                string message = updateUserProfile(photo, client.UserName);
            }
         
            repo.updatgeProfile(client, interests[0], interests[1], interests[2]);
           

            
            return RedirectToAction("UserProfile", new { userName= client.UserName});
        }


        [HttpGet]
        public ActionResult findADate(string username)
        {
            AspNetUser user = repo.GetUser(username);
            ViewBag.UserName = user.UserName;
            ViewBag.Id = user.Id;
            return View();
        }

        [HttpPost]
        public ActionResult findADate(String userName, DateTime availableDate, DateTime timepicker1 )
        {

            repo.saveAvailableDate(userName, availableDate, timepicker1);
            return View();
        }
        public ActionResult foundDates()
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

       


        public ActionResult Update()
        {
          
            return View();
        }

        public ActionResult Welcome()
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


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisteredUser newUser)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };

            var identityUser = new IdentityUser()
            {
                UserName = newUser.UserName,
                Email = newUser.Email
            };
            IdentityResult result = manager.Create(identityUser, newUser.Password);

            if (result.Succeeded)
            {
                CreateTokenProvider(manager, EMAIL_CONFIRMATION);

                var code = manager.GenerateEmailConfirmationToken(identityUser.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Home",
                                                new { userId = identityUser.Id, code = code },
                                                    protocol: Request.Url.Scheme);

                string email = "Please confirm your account by clicking this link: <a href=\""
                                + callbackUrl + "\">Confirm Registration</a>";
                ViewBag.FakeConfirmation = email;

            return RedirectToAction("CompleteInfo", new { userName= identityUser.UserName, email= identityUser.Email, id=identityUser.Id });

            // return RedirectToAction("SecureArea", "Home");
        }
            return View();
        }

        public ActionResult CompleteInfo(string userName, string email, string id)
        {
            ViewBag.email = email;
            ViewBag.username = userName;
            ViewBag.id = id;       
            return View();
          
        }

        [HttpPost]
        public ActionResult CompleteInfo(Client clientInfo, string interest1, string interest2, string interest3)
        {
            repo.saveClientInfo(clientInfo, interest1, interest2, interest3);
            return RedirectToAction("UserProfile", new { userName = clientInfo.UserName});
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

       


        [HttpGet]
            public ActionResult AddRole()
            {
                return View();
            }

            [HttpPost]
            public ActionResult AddRole(AspNetRole role)
            {
          
                context.AspNetRoles.Add(role);
                context.SaveChanges();
                return View();
            }

            [HttpGet]
            public ActionResult AddUserToRole()
            {
                return View();
            }
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
            public ActionResult PaidUserOnly()
            {
                return View();
            }


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
                    return false;

                // User is locked out.
                if (userManager.SupportsUserLockout && userManager.IsLockedOut(user.Id))
                    return false;

                // Validated user was locked out but now can be reset.
                if (userManager.CheckPassword(user, login.Password))

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
        /*
            public ActionResult ConfirmEmail(string userID, string code)
            {
                var userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
                var user = manager.FindById(userID);
                CreateTokenProvider(manager, EMAIL_CONFIRMATION);
                try
                {
                    IdentityResult result = manager.ConfirmEmail(userID, code);
                    if (result.Succeeded)
                        ViewBag.Message = "You are now registered!";
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
                CreateTokenProvider(manager, PASSWORD_RESET);

                var code = manager.GeneratePasswordResetToken(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Home",
                                             new { userId = user.Id, code = code },
                                             protocol: Request.Url.Scheme);
                ViewBag.FakeEmailMessage = "Please reset your password by clicking <a href=\""
                                         + callbackUrl + "\">here</a>";
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
            public ActionResult ResetPassword(string password, string passwordConfirm,
                                              string passwordToken, string userID)
            {

                var userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
                var user = manager.FindById(userID);
                CreateTokenProvider(manager, PASSWORD_RESET);

                IdentityResult result = manager.ResetPassword(userID, passwordToken, password);
                if (result.Succeeded)
                    ViewBag.Result = "The password has been reset.";
                else
                    ViewBag.Result = "The password has not been reset.";
                return View();
            }
            */
        }
        
    }