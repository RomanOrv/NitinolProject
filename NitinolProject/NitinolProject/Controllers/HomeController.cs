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
using NitinolProject.Web.Models.Shared;
using static System.Math;

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
            }).OrderBy(x => x.SampleNumber);
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
                    SampleNumber = sample.SampleNumber
                };
                item.QualityRate = Math.Round(
                    (item.HammerSpeed + item.SpallSpeed + item.SpallStrength + item.HammerThickness +
                     item.SampleThickness) / 5, 2);
                models.Add(item);
            }
            return Json(models.OrderBy(x => x.SampleNumber).ToDataSourceResult(request));
        }

        [HttpGet]
        public ActionResult AddAlloySample()
        {
            var alloySamples = this._alloyRepository.GetAllAlloySamples();
            ViewBag.MaxSampleNumber = alloySamples.Select(x => x.SampleNumber).Max();
            var baseValues = _alloyRepository.GetNicelideTitanumQualityBaseValue();
            ViewBag.MaxSampleThickness = baseValues.SampleThickness;
            ViewBag.MaxSpallStrength = baseValues.SpallStrength;
            ViewBag.MaxHammerSpeed = baseValues.HammerSpeed;
            ViewBag.MaxHammerThickness = baseValues.HammerThickness;
            ViewBag.MaxSpallSpeed = baseValues.SpallSpeed;
            return View(new NicelideTitanumSampleModel());
        }

        [HttpPost]
        public ActionResult AddAlloySample(NicelideTitanumSampleModel model)
        {
            _alloyRepository.AddAlloySample(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditAlloySample(int id)
        {
            var sample = _alloyRepository.GetAlloySample(id);
            var model = new NicelideTitanumSampleModel
            {
                NicelideTitanumSampleId = sample.NicelideTitanumSampleId,
                NickelideTitaniumAlloyName = sample.NickelideTitaniumAlloy.Name,
                HammerSpeed = sample.HammerSpeed,
                SampleNumber = sample.SampleNumber,
                SampleThickness = sample.SampleThickness,
                SpallStrength = sample.SpallStrength,
                HammerThickness = sample.HammerThickness,
                SpallSpeed = sample.SpallSpeed
            };
            var alloySamples = this._alloyRepository.GetAllAlloySamples();
            ViewBag.MaxSampleNumber = alloySamples.Select(x => x.SampleNumber).Max();
            var baseValues = _alloyRepository.GetNicelideTitanumQualityBaseValue();
            ViewBag.MaxSampleThickness = baseValues.SampleThickness;
            ViewBag.MaxSpallStrength = baseValues.SpallStrength;
            ViewBag.MaxHammerSpeed = baseValues.HammerSpeed;
            ViewBag.MaxHammerThickness = baseValues.HammerThickness;
            ViewBag.MaxSpallSpeed = baseValues.SpallSpeed;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditAlloySample(NicelideTitanumSampleModel model)
        {
            _alloyRepository.EditAlloySample(model);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAlloySample(int id)
        {
            _alloyRepository.DeleteAlloySample(id);
            return null;
        }

        [HttpGet]
        public ActionResult AlloySampleDetails(int id)
        {
            var alloySamples = this._alloyRepository.GetAllAlloySamples();
            var sample = alloySamples.First(x => x.NicelideTitanumSampleId == id);
            var baseValues = _alloyRepository.GetNicelideTitanumQualityBaseValue();
            var coeficientValues = _alloyRepository.GetNicelideTitanumCoefficientWeighting();

            // var propertyList = SetAlloySamplePropertyList(baseValues, sample, coeficientValues);
            var coeficientK = GetCoeficientK(coeficientValues, sample, baseValues);

            ViewBag.CoeficientK = coeficientK;

            return View(new NicelideTitanumSampleModel { NicelideTitanumSampleId = sample.NicelideTitanumSampleId, SampleNumber = sample.SampleNumber });
        }

        private decimal GetCoeficientK(NicelideTitanumCoefficientWeighting coeficientValues, NicelideTitanumSample sample, NicelideTitanumQualityBaseValue baseValues)
        {
            return (decimal)Round(Sqrt(
                (double)(coeficientValues.SampleThickness * Round(sample.SampleThickness / baseValues.SampleThickness, 2) * Round(sample.SampleThickness / baseValues.SampleThickness, 2)
                         + coeficientValues.HammerThickness * Round(sample.HammerThickness / baseValues.HammerThickness, 2) * Round(sample.HammerThickness / baseValues.HammerThickness, 2)
                         + coeficientValues.HammerSpeed * Round((decimal)sample.HammerSpeed / baseValues.HammerSpeed, 2) * Round((decimal)sample.HammerSpeed / baseValues.HammerSpeed, 2)
                         + coeficientValues.SpallSpeed * Round((decimal)sample.SpallSpeed / baseValues.SpallSpeed, 2) * Round((decimal)sample.SpallSpeed / baseValues.SpallSpeed, 2)
                         + coeficientValues.SpallStrength * Round(sample.SpallStrength / baseValues.SpallStrength, 2) * Round(sample.SpallStrength / baseValues.SpallStrength, 2)
                )), 2);
        }


        public ActionResult GetAlloySampleProperties([DataSourceRequest] DataSourceRequest request, int sampleId)
        {
            var alloySamples = this._alloyRepository.GetAllAlloySamples();
            var sample = alloySamples.First(x => x.NicelideTitanumSampleId == sampleId);
            var baseValues = _alloyRepository.GetNicelideTitanumQualityBaseValue();
            var coeficientValues = _alloyRepository.GetNicelideTitanumCoefficientWeighting();

            var propertyList = SetAlloySamplePropertyList(baseValues, sample, coeficientValues);
            return Json(propertyList.ToDataSourceResult(request));
        }


        public ActionResult GetAlloyPieChartData(int sampleId)
        {
            var alloySamples = this._alloyRepository.GetAllAlloySamples();
            var baseValues = _alloyRepository.GetNicelideTitanumQualityBaseValue();
            var sample = alloySamples.First(x => x.NicelideTitanumSampleId == sampleId);
            var labels = new string[] { "У(H)", "У(h)", "У(V)", "У(W)", "У(σ)" };
            var data = new decimal[]
            {
                Round(sample.SampleThickness / baseValues.SampleThickness, 2),
                Round(sample.HammerThickness / baseValues.HammerThickness, 2),
                Round((decimal) sample.HammerSpeed / baseValues.HammerSpeed, 2),
                Round((decimal) sample.SpallSpeed / baseValues.SpallSpeed, 2),
                Round(sample.SpallStrength / baseValues.SpallStrength, 2),
            };

            return Json(new PieChartModel { Labels = labels, Data = data }, JsonRequestBehavior.AllowGet);
        }

        private IList<AlloySampleQualityRateModel> SetAlloySamplePropertyList(NicelideTitanumQualityBaseValue baseValues, NicelideTitanumSample sample,
            NicelideTitanumCoefficientWeighting coeficientValues)
        {
            var propertyList = new List<AlloySampleQualityRateModel>();
            var model = new AlloySampleQualityRateModel
            {
                SampleProperty = "Товщина зразку H, мм",
                BaseValue = baseValues.SampleThickness,
                SampleValue = sample.SampleThickness,
                RelativeValue = Math.Round((decimal)sample.SampleThickness / baseValues.SampleThickness, 2),
                CoefficientWeighting = coeficientValues.SampleThickness,
                Angle = Round(360 * coeficientValues.SampleThickness, 0)
            };
            propertyList.Add(model);

            model = new AlloySampleQualityRateModel
            {
                SampleProperty = "Товщина ударника h, мм",
                BaseValue = baseValues.HammerThickness,
                SampleValue = sample.HammerThickness,
                RelativeValue = Math.Round((decimal)sample.HammerThickness / baseValues.HammerThickness, 2),
                CoefficientWeighting = coeficientValues.HammerThickness,
                Angle = Round(360 * coeficientValues.HammerThickness, 0)
            };
            propertyList.Add(model);

            model = new AlloySampleQualityRateModel
            {
                SampleProperty = "Швидкість ударника V, м/c",
                BaseValue = baseValues.HammerSpeed,
                SampleValue = sample.HammerSpeed,
                RelativeValue = Math.Round((decimal)sample.HammerSpeed / baseValues.HammerSpeed, 2),
                CoefficientWeighting = coeficientValues.HammerSpeed,
                Angle = Round(360 * coeficientValues.HammerSpeed, 0)
            };
            propertyList.Add(model);

            model = new AlloySampleQualityRateModel
            {
                SampleProperty = "Швидкість відколу W, м/с",
                BaseValue = baseValues.SpallSpeed,
                SampleValue = sample.SpallSpeed,
                RelativeValue = Math.Round((decimal)sample.SpallSpeed / baseValues.SpallSpeed, 2),
                CoefficientWeighting = coeficientValues.SpallSpeed,
                Angle = Round(360 * coeficientValues.SpallSpeed, 0)
            };
            propertyList.Add(model);

            model = new AlloySampleQualityRateModel
            {
                SampleProperty = "Міцність відколу σ, ГПа",
                BaseValue = baseValues.SpallStrength,
                SampleValue = sample.SpallStrength,
                RelativeValue = Math.Round((decimal)sample.SpallStrength / baseValues.SpallStrength, 2),
                CoefficientWeighting = coeficientValues.SpallStrength,
                Angle = Round(360 * coeficientValues.SpallStrength, 0)
            };
            propertyList.Add(model);
            return propertyList;
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