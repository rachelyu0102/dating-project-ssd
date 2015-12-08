using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication9.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Email required.")]
        [RegularExpression(@"^(([^<>()[\]\\.,;:\s@\""]+"
                           + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                           + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                           + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                           + @"[a-zA-Z]{2,}))$", ErrorMessage = "Not a valid email address")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password required.")]
        public string password { get; set; }
    }
}