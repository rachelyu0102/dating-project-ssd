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
    
    public partial class UserActivityImage
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string title { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public Nullable<System.DateTime> uploadDate { get; set; }
    
        public virtual Client Client { get; set; }
    }
}
