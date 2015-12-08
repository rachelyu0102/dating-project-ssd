using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9.Models
{
    public class ClientProfile
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Interest1 { get; set; }
        public string Interest2 { get; set; }
        public string Interest3 { get; set; }

        public ClientProfile()
        {

        }

        public ClientProfile(string email, string userName, int? age, string gender, string city, string province, string country, string interest1, string interest2,
            string interest3)
        {
            Email = email;
            UserName = userName;
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