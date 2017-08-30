using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitinolProject.Web.Models.Metal
{
    public class MetalSampleModel
    {
        public int MetalSampleId { get; set; }
        public string Name { get; set; }
        public int SampleNumber { get; set; }
        public int MetalId { get; set; }
        public int CrystalLatticeId { get; set; }
        public int LoadingSpeed { get; set; }
        public int LateralShearRate { get; set; }
        public int LongitudinalShearRate { get; set; }
        public decimal ShearStrainRate { get; set; }
        public decimal SpallStrength { get; set; }
    }
}
