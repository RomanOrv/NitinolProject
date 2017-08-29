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

namespace NitinolProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAlloyRepository _alloyRepository;

        public HomeController(IAlloyRepository alloyRepository)
        {
            this._alloyRepository = alloyRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            var alloySamples = this._alloyRepository.GetAllAlloySamples();
            var baseValues = _alloyRepository.GetNicelideTitanumQualityBaseValue();
            var chartsData = new List<CyclogramChartModel>();
            foreach (var sample in alloySamples)
            {
                var chart = new CyclogramChartModel();
                chart.name = $"{sample.SampleNumber}. {sample.NickelideTitaniumAlloy.Name}";
                var pairsData = new Dictionary<string, decimal>();
                pairsData.Add(nameof(sample.SpallStrength), sample.SpallStrength);
                pairsData.Add(nameof(sample.HammerSpeed), sample.HammerSpeed);
                pairsData.Add(nameof(sample.HammerThickness), sample.HammerThickness);
                pairsData.Add(nameof(sample.SpallSpeed), sample.SpallSpeed);
                pairsData.Add(nameof(sample.SampleThickness), sample.SampleThickness);
                var convertedData = ConvertCyclogramChartModel(pairsData, baseValues);
                chart.data.AddRange(convertedData);
                chartsData.Add(chart);
            }
            ViewBag.ChartsData = chartsData;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetAlloySamples([DataSourceRequest] DataSourceRequest request)
        {
            var alloySamples = this._alloyRepository.GetAllAlloySamples();
            var models = alloySamples.Select(x => new NicelideTitanumSampleModel
            {
                NicelideTitanumSampleId = x.NicelideTitanumSampleId,
                NickelideTitaniumAlloyName = x.NickelideTitaniumAlloy.Name,
                HammerSpeed = x.HammerSpeed,
                SampleNumber = x.SampleNumber,
                SampleThickness = x.SampleThickness,
                SpallStrength = x.SpallStrength,
                HammerThickness = x.HammerThickness,
                SpallSpeed = x.SpallSpeed
            });
            return Json(models.ToDataSourceResult(request));
        }

        public ActionResult GetAlloySampleQualities([DataSourceRequest] DataSourceRequest request)
        {
            var alloySamples = this._alloyRepository.GetAllAlloySamples();
            var baseValues = _alloyRepository.GetNicelideTitanumQualityBaseValue();
            var models = new List<NicelideTitanumSampleQualityModel>();
            foreach (var sample in alloySamples)
            {
                var item = new NicelideTitanumSampleQualityModel
                {
                    NicelideTitanumSampleId = sample.NicelideTitanumSampleId,
                    NickelideTitaniumAlloyName = $"{sample.SampleNumber}. {sample.NickelideTitaniumAlloy.Name}",
                    HammerSpeed = Math.Round((decimal)sample.HammerSpeed / baseValues.HammerSpeed, 2),
                    SampleThickness = Math.Round((decimal)sample.SampleThickness / baseValues.SampleThickness, 2),
                    SpallStrength = Math.Round((decimal)sample.SpallStrength / baseValues.SpallStrength, 2),
                    HammerThickness = Math.Round((decimal)sample.HammerThickness / baseValues.HammerThickness, 2),
                    SpallSpeed = Math.Round((decimal)sample.SpallSpeed / baseValues.SpallSpeed, 2),                    
                };
                item.QualityRate = Math.Round(
                    (item.HammerSpeed + item.SpallSpeed + item.SpallStrength + item.HammerThickness +
                     item.SampleThickness) / 5, 2);
                models.Add(item);
            }
            return Json(models.ToDataSourceResult(request));
        }

        [HttpGet]
        public ActionResult AddAlloySample()
        {
            return View(new NicelideTitanumSampleModel());
        }

        [HttpPost]
        public ActionResult AddAlloySample(NicelideTitanumSampleModel model)
        {
            _alloyRepository.AddAlloySample(model);
            return RedirectToAction("About");
        }

        private IList<decimal> ConvertCyclogramChartModel(Dictionary<string, decimal> dataDictionary, NicelideTitanumQualityBaseValue baseValues)
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