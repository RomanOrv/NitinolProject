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
    
    public partial class NicelideTitanumSample
    {
        public int NicelideTitanumSampleId { get; set; }
        public int NickelideTitaniumAlloyId { get; set; }
        public int SampleNumber { get; set; }
        public decimal SampleThickness { get; set; }
        public decimal HammerThickness { get; set; }
        public int HammerSpeed { get; set; }
        public int SpallSpeed { get; set; }
        public decimal SpallStrength { get; set; }
    
        public virtual NickelideTitaniumAlloy NickelideTitaniumAlloy { get; set; }
    }
}
