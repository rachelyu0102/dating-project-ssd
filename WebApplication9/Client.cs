//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication9
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
        public string email { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> birthdate { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public Nullable<System.DateTime> availableDate { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string profile { get; set; }
    
        public virtual AspNetUserRole AspNetUserRole { get; set; }
        public virtual Available Available { get; set; }
    }
}
