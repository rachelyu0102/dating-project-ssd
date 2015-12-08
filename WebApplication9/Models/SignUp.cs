using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication9.Models
{
    public class SignUp
    {
        [Required(ErrorMessage = "Username required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "UserName must between 4-20 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email required.")]
        [RegularExpression(@"^(([^<>()[\]\\.,;:\s@\""]+"
                    + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                    + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                    + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                    + @"[a-zA-Z]{2,}))$", ErrorMessage = "Not a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must between 6-20 characters")]
        public string Password { get; set; }
    }
}