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
    
    public partial class Available
    {
        public Nullable<System.DateTime> availableDate { get; set; }
        public Nullable<System.TimeSpan> timeStart { get; set; }
        public Nullable<System.TimeSpan> timeEnd { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string username { get; set; }
    
        public virtual Client Client { get; set; }
    }
}