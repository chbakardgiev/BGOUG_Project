//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class @event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public @event()
        {
            this.members_events = new HashSet<members_events>();
        }
    
        public int ID { get; set; }
        public string name { get; set; }
        public string place { get; set; }
        public System.DateTime date { get; set; }
        public string description { get; set; }
        public int attendance { get; set; }
        public Nullable<decimal> costs { get; set; }
        public int comments { get; set; }
    
        public virtual comment comment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<members_events> members_events { get; set; }
    }
}