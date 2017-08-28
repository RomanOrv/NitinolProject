using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitinolProject.Web.Models.Alloy
{
   public class CyclogramChartModel
    {
        public string name { get; set; }
        public List<decimal> data { get; set; } = new List<decimal>();

    }
}
