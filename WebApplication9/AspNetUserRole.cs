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
    
    public partial class AspNetUserRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetUserRole()
        {
            this.Clients = new HashSet<Client>();
        }
    
        public string UserId { get; set; }
        public string RoleId { get; set; }
    
        public virtual AspNetRole AspNetRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client> Clients { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
