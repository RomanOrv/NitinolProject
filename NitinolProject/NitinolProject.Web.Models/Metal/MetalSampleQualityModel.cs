using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitinolProject.Web.Models.Metal
{
    public class MetalSampleQualityModel
    {
        public int MetalSampleId { get; set; }
        public string Name { get; set; }
        public int SampleNumber { get; set; }
        public decimal LoadingSpeed { get; set; }
        public decimal LateralShearRate { get; set; }
        public decimal LongitudinalShearRate { get; set; }
        public decimal ShearStrainRate { get; set; }
        public decimal SpallStrength { get; set; }
        public decimal QualityRate { get; set; }
    }
}
