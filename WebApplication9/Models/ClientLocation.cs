using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9.Models
{
    public class ClientLocation
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public ClientLocation(string email, string userName)
        {

            Email = email;
            UserName = userName;

        }
    }
}