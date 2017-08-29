using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitinolProject.Web.Models.Alloy
{
    public class NicelideTitanumSampleQualityModel
    {
        public int NicelideTitanumSampleId { get; set; }
        public string NickelideTitaniumAlloyName { get; set; }
        public int SampleNumber { get; set; }
        public decimal SampleThickness { get; set; }
        public decimal HammerThickness { get; set; }
        public decimal HammerSpeed { get; set; }
        public decimal SpallSpeed { get; set; }
        public decimal SpallStrength { get; set; }
        public decimal QualityRate { get; set; }
    }
}
