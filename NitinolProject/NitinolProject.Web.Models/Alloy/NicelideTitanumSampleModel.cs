using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitinolProject.Web.Models.Alloy
{
   public class NicelideTitanumSampleModel
    {
        public int NicelideTitanumSampleId { get; set; }
        public int NickelideTitaniumAlloyId { get; set; }
        public int SampleNumber { get; set; }
        public decimal SampleThickness { get; set; }
        public decimal HammerThickness { get; set; }
        public int HammerSpeed { get; set; }
        public int SpallSpeed { get; set; }
        public decimal SpallStrength { get; set; }
    }
}
