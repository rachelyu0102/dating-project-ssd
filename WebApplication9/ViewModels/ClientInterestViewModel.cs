using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplication9.ViewModels
{
    public class ClientInterestViewModel
    {
    SSDDatingEntities11 db = new SSDDatingEntities11();
        public string userId { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public List<string> interests { get; set; }
    }
}