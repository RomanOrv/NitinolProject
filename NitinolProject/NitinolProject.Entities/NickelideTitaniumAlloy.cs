//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NitinolProject.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class NickelideTitaniumAlloy
    {
        public NickelideTitaniumAlloy()
        {
            this.NicelideTitanumSamples = new HashSet<NicelideTitanumSample>();
        }
    
        public int NickelideTitaniumAlloyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<NicelideTitanumSample> NicelideTitanumSamples { get; set; }
    }
}
