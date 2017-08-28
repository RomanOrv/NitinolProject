using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using NitinolProject.Entities;
using NitinolProject.Repository.Interfaces;
using NitinolProject.Web.Models.Alloy;

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