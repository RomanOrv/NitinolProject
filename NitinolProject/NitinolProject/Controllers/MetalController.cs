using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using NitinolProject.Entities;
using NitinolProject.Repository.Interfaces;
using NitinolProject.Web.Models.Alloy;
using Kendo.Mvc.UI;
using NitinolProject.Web.Models.Metal;

namespace NitinolProject.Web.Controllers
{
    public class MetalController : Controller
    {
        private readonly IMetalRepository _metalRepository;

        public MetalController(IMetalRepository metalRepository)
        {
            this._metalRepository = metalRepository;
        }

        // GET: Metal
        public ActionResult Index()
        {
            var metalSamples = this._metalRepository.GetAllMetalSamples();
            var baseValues = _metalRepository.GetMetalQualityBaseValues();
            var chartsData = new List<CyclogramChartModel>();
            foreach (var group in metalSamples.GroupBy(x => x.MetalId))
            {
                foreach (var sample in group)
                {
                    var chart = new CyclogramChartModel();
                    chart.name = $"{sample.SampleNumber}. {sample.Name}";
                    var pairsData = new Dictionary<string, decimal>();
                    pairsData.Add(nameof(sample.SpallStrength), sample.SpallStrength);
                    pairsData.Add(nameof(sample.LoadingSpeed), sample.LoadingSpeed);
                    pairsData.Add(nameof(sample.ShearStrainRate), sample.ShearStrainRate);
                    pairsData.Add(nameof(sample.LongitudinalShearRate), sample.LongitudinalShearRate);
                    pairsData.Add(nameof(sample.LateralShearRate), sample.LateralShearRate);
                    var convertedData = ConvertCyclogramChartModel(pairsData, baseValues.First(x => x.MetalId == sample.MetalId));
                    chart.data.AddRange(convertedData);
                    chartsData.Add(chart);
                }
            }
            ViewBag.ChartsData = chartsData.OrderBy(x => x.name);
            return View();
        }


        public ActionResult GetMetalSamples([DataSourceRequest] DataSourceRequest request)
        {
            var metalSamples = this._metalRepository.GetAllMetalSamples();
            var models = metalSamples.Select(x => new MetalSampleGridModel
            {
                MetalSampleId = x.MetalSampleId,
                CrystalLatticeName = x.CrystalLattice.Name,
                SampleNumber = x.SampleNumber,
                Name = x.Name,
                SpallStrength = x.SpallStrength,
                LateralShearRate = x.LateralShearRate,
                LongitudinalShearRate = x.LongitudinalShearRate,
                ShearStrainRate = x.ShearStrainRate,
                LoadingSpeed = x.LoadingSpeed
            });
            return Json(models.ToDataSourceResult(request));
        }


        private IList<decimal> ConvertCyclogramChartModel(Dictionary<string, decimal> dataDictionary, MetalQualityBaseValue baseValues)
        {
            IList<decimal> convertedData = new List<decimal>();
            foreach (var item in dataDictionary)
            {
                var property = baseValues.GetType().GetProperties().FirstOrDefault(x => x.Name == item.Key);
                var baseValue = Convert.ToDecimal(property.GetValue(baseValues));
                var result = Math.Round(item.Value / baseValue, 2);
                convertedData.Add(result);
            }
            return convertedData;
        }
    }
}