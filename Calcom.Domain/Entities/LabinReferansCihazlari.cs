//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Calcom.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class LabinReferansCihazlari
    {
        public LabinReferansCihazlari()
        {
            this.OlcumdekiReferansCihaz = new HashSet<OlcumdekiReferansCihaz>();
        }
    
        public int Id { get; set; }
        public int LabId { get; set; }
        public int RefId { get; set; }
    
        public virtual ReferansCihazlar ReferansCihazlar { get; set; }
        public virtual ICollection<OlcumdekiReferansCihaz> OlcumdekiReferansCihaz { get; set; }
        public virtual Laboratuvarlar Laboratuvarlar { get; set; }
    }
}
