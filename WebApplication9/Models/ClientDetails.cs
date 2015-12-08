using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication9.Models
{
    public class ClientDetails
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Username required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "UserName must between 4-20 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must between 6-20 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Age required.")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Gener required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "City required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Province required.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Country required.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Interest1 required.")]
        public string Interest1 { get; set; }

        [Required(ErrorMessage = "Interest2 required.")]
        public string Interest2 { get; set; }

        [Required(ErrorMessage = "Interest3 required.")]
        public string Interest3 { get; set; }

        public ClientDetails()
        {

        }


        public ClientDetails(string email, string userName, string password, int? age, string gender, string city, string province, string country, string interest1, string interest2,
            string interest3)
        {
            Email = email;
            UserName = userName;
            Password = password;
            Age = age;
            Gender = gender;
            City = city;
            Province = province;
            Country = country;
            Interest1 = interest1;
            Interest2 = interest2;
            Interest3 = interest3;
        }
    }
}