using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitinolProject.Web.Models.Alloy
{
    public class AlloySampleQualityRateModel
    {
        public string SampleProperty { get; set; }
        public decimal BaseValue { get; set; }
        public decimal SampleValue { get; set; }
        public decimal RelativeValue { get; set; }
        public decimal CoefficientWeighting { get; set; }
        public decimal Angle { get; set; }
    }
}
