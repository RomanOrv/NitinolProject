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
using NitinolProject.Web.Models.Shared;
using static System.Math;

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
                    var convertedData = 
                        ConvertCyclogramChartModel(pairsData, baseValues.First(x => x.MetalId == group.Key));
                    chart.data.AddRange(convertedData);
                    chartsData.Add(chart);
                }
            }
            ViewBag.ChartsData = chartsData.OrderBy(x => x.name);
            return View();
        }

        public ActionResult MetalTypeSamples(int metalType)
        {
            var type = this._metalRepository.GetAllMetalTypes().FirstOrDefault(x => x.MetalId == metalType);
            var metalSamples = this._metalRepository.GetAllMetalSamples().Where(x => x.MetalId == metalType);
            var baseValues = _metalRepository.GetMetalQualityBaseValues().FirstOrDefault(x => x.MetalId == metalType);
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
                    var convertedData = ConvertCyclogramChartModel(pairsData, baseValues);
                    chart.data.AddRange(convertedData);
                    chartsData.Add(chart);
                }
            }
            ViewBag.ChartsData = chartsData.OrderBy(x => x.name);
            return View(new MetalTypeModel { MetalTypeId = type.MetalId, MetalName = type.Name });
        }


        public ActionResult GetMetalSamples([DataSourceRequest] DataSourceRequest request, int metalType)
        {
            var metalSamples = this._metalRepository.GetAllMetalSamples().Where(x => x.MetalId == metalType);
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
                LoadingSpeed = x.LoadingSpeed,
                MetalId = x.MetalId
            }).OrderBy(x => x.SampleNumber);
            return Json(models.ToDataSourceResult(request));
        }

        public ActionResult GetMetalSampleQualities([DataSourceRequest] DataSourceRequest request, int metalType)
        {
            var metalSamples = this._metalRepository.GetAllMetalSamples().Where(x => x.MetalId == metalType);
            var baseValues = _metalRepository.GetMetalQualityBaseValues().FirstOrDefault(x => x.MetalId == metalType);
            var models = new List<MetalSampleQualityModel>();
            foreach (var group in metalSamples.GroupBy(x => x.MetalId))
            {
                var baseValue = baseValues/*.First(x => x.MetalId == group.Key)*/;
                foreach (var sample in group)
                {
                    var item = new MetalSampleQualityModel
                    {
                        MetalSampleId = sample.MetalSampleId,
                        Name = $"{sample.SampleNumber}. {sample.Name}",
                        LateralShearRate = Math.Round((decimal)sample.LateralShearRate / baseValue.LateralShearRate, 2),
                        LoadingSpeed = Math.Round((decimal)sample.LoadingSpeed / baseValue.LoadingSpeed, 2),
                        SpallStrength = Math.Round((decimal)sample.SpallStrength / baseValue.SpallStrength, 2),
                        LongitudinalShearRate = Math.Round((decimal)sample.LongitudinalShearRate / baseValue.LongitudinalShearRate, 2),
                        ShearStrainRate = Math.Round((decimal)sample.ShearStrainRate / baseValue.ShearStrainRate, 2),
                        SampleNumber = sample.SampleNumber
                    };
                    item.QualityRate = Math.Round(
                        (item.LateralShearRate + item.LoadingSpeed + item.SpallStrength + item.LongitudinalShearRate +
                         item.ShearStrainRate) / 5, 2);
                    models.Add(item);
                }
            }

            models = models.OrderBy(x => x.SampleNumber).ToList();
            return Json(models.ToDataSourceResult(request));
        }

        public ActionResult MetalSampleDetails(int id)
        {
            var metalSamples = this._metalRepository.GetAllMetalSamples();
            var sample = metalSamples.First(x => x.MetalSampleId == id);
            var baseValues = _metalRepository.GetMetalQualityBaseValues();
            var coeficientValues = _metalRepository.GetAllMetalCoefficientsWeighting()
                .FirstOrDefault(x => x.MetalId == sample.MetalId);
            var baseMetalValues = baseValues.First(x => x.MetalId == sample.MetalId);
            var coeficientK = GetCoeficientK(coeficientValues, sample, baseMetalValues);
            ViewBag.MetalTypeName = baseMetalValues.Metal.Name;
            ViewBag.CoeficientK = coeficientK;
            return View(new MetalSampleModel { MetalSampleId = sample.MetalSampleId, SampleNumber = sample.SampleNumber });
        }


        [HttpGet]
        public ActionResult AddMetalSample(int metalType)
        {
            ViewBag.MetalTypes = _metalRepository.GetAllMetalTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.MetalId.ToString() });
            ViewBag.CrystalLattices = _metalRepository.GetAllCrystalLattices().Select(x => new SelectListItem { Text = x.Name, Value = x.CrystalLatticeId.ToString() });
            var metalSamples = this._metalRepository.GetAllMetalSamples().Where(y => y.MetalId == metalType);
            ViewBag.MaxSampleNumber = metalSamples.Any() ? metalSamples.Select(x => x.SampleNumber).Max() : 0;
            var baseValues = _metalRepository.GetMetalQualityBaseValues().First(x => x.MetalId == metalType);
            ViewBag.MetalTypeName = baseValues.Metal.Name;
            ViewBag.MaxLoadingSpeed = baseValues.LoadingSpeed;
            ViewBag.MaxLateralShearRate = baseValues.LateralShearRate;
            ViewBag.MaxLongitudinalShearRate = baseValues.LongitudinalShearRate;
            ViewBag.MaxShearStrainRate = baseValues.ShearStrainRate;
            ViewBag.MaxSpallStrength = baseValues.SpallStrength;
            return View(new MetalSampleModel { MetalId = metalType, MetalTypeName = baseValues.Metal.Name });
        }

        [HttpPost]
        public ActionResult AddMetalSample(MetalSampleModel model)
        {
            _metalRepository.AddMetalSample(model);
            return RedirectToAction("MetalTypeSamples", new { metalType = model.MetalId });
        }

        [HttpGet]
        public ActionResult EditMetalSample(int id)
        {
            var sample = _metalRepository.GetMetalSample(id);
            var model = new MetalSampleModel
            {
                MetalSampleId = sample.MetalSampleId,
                Name = sample.Name,
                MetalId = sample.MetalId,
                SampleNumber = sample.SampleNumber,
                CrystalLatticeId = sample.CrystalLatticeId,
                SpallStrength = sample.SpallStrength,
                LongitudinalShearRate = sample.LongitudinalShearRate,
                LateralShearRate = sample.LateralShearRate,
                ShearStrainRate = sample.ShearStrainRate,
                LoadingSpeed = sample.LoadingSpeed,
                MetalTypeName = sample.Metal.Name
            };
            var metalSamples = this._metalRepository.GetAllMetalSamples();
            ViewBag.MaxSampleNumber = metalSamples.Where(y => y.MetalId == sample.MetalId).Select(x => x.SampleNumber).Max();
            var baseValues = _metalRepository.GetMetalQualityBaseValues().First(x => x.MetalId == sample.MetalId);
            ViewBag.MetalTypeName = baseValues.Metal.Name;
            ViewBag.MaxLoadingSpeed = baseValues.LoadingSpeed;
            ViewBag.MaxLateralShearRate = baseValues.LateralShearRate;
            ViewBag.MaxLongitudinalShearRate = baseValues.LongitudinalShearRate;
            ViewBag.MaxShearStrainRate = baseValues.ShearStrainRate;
            ViewBag.MaxSpallStrength = baseValues.SpallStrength;
            //ViewBag.MetalTypes = _metalRepository.GetAllMetalTypes().Select(x => new SelectListItem { Text = x.Name, Value = x.MetalId.ToString() });
            ViewBag.CrystalLattices = _metalRepository.GetAllCrystalLattices().Select(x => new SelectListItem { Text = x.Name, Value = x.CrystalLatticeId.ToString() });
            return View(model);
        }

        [HttpPost]
        public ActionResult EditMetalSample(MetalSampleModel model)
        {
            _metalRepository.EditMetalSample(model);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteMetalSample(int id)
        {
            _metalRepository.DeleteMetalSample(id);
            return null;
        }

        public ActionResult GetMetalPieChartData(int sampleId)
        {
            var metalSamples = this._metalRepository.GetAllMetalSamples();
            var baseValues = _metalRepository.GetMetalQualityBaseValues();
            var sample = metalSamples.First(x => x.MetalSampleId == sampleId);

            var baseMetalValues = baseValues.First(x => x.MetalId == sample.MetalId);
            var labels = new string[] { "У(V)", "У(Vt)", "У(VL)", "У(γ)", "У(σ)" };
            var data = new decimal[]
            {
                Round((decimal) sample.LoadingSpeed / baseMetalValues.LoadingSpeed, 2),
                Round((decimal)sample.LateralShearRate / baseMetalValues.LateralShearRate, 2),
                Round((decimal)sample.LongitudinalShearRate / baseMetalValues.LongitudinalShearRate, 2),
                Round((decimal) sample.ShearStrainRate / baseMetalValues.ShearStrainRate, 2),
                Round(sample.SpallStrength / baseMetalValues.SpallStrength, 2),
            };
            return Json(new PieChartModel { Labels = labels, Data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMetalSampleProperties([DataSourceRequest] DataSourceRequest request, int sampleId)
        {
            var metalSamples = this._metalRepository.GetAllMetalSamples();
            var sample = metalSamples.First(x => x.MetalSampleId == sampleId);
            var baseValues = _metalRepository.GetMetalQualityBaseValues();
            var baseMetalValues = baseValues.First(x => x.MetalId == sample.MetalId);
            var coeficientValues = _metalRepository.GetAllMetalCoefficientsWeighting()
                .FirstOrDefault(x => x.MetalId == sample.MetalId);

            var propertyList = SetMetalSamplePropertyList(baseMetalValues, sample, coeficientValues);
            return Json(propertyList.ToDataSourceResult(request));
        }



        private decimal GetCoeficientK(MetalCoefficientWeighting coeficientValues, MetalSample sample, MetalQualityBaseValue baseValues)
        {
            return (decimal)Round(Sqrt(
                (double)(coeficientValues.LateralShearRate * Round((decimal)sample.LateralShearRate / baseValues.LateralShearRate, 2) * Round((decimal)sample.LateralShearRate / baseValues.LateralShearRate, 2)
                         + coeficientValues.LongitudinalShearRate * Round((decimal)sample.LongitudinalShearRate / baseValues.LongitudinalShearRate, 2) * Round((decimal)sample.LongitudinalShearRate / baseValues.LongitudinalShearRate, 2)
                         + coeficientValues.ShearStrainRate * Round((decimal)sample.ShearStrainRate / baseValues.ShearStrainRate, 2) * Round((decimal)sample.ShearStrainRate / baseValues.ShearStrainRate, 2)
                         + coeficientValues.LoadingSpeed * Round((decimal)sample.LoadingSpeed / baseValues.LoadingSpeed, 2) * Round((decimal)sample.LoadingSpeed / baseValues.LoadingSpeed, 2)
                         + coeficientValues.SpallStrength * Round(sample.SpallStrength / baseValues.SpallStrength, 2) * Round(sample.SpallStrength / baseValues.SpallStrength, 2)
                )), 2);
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

        private IList<MetalSampleQualityRateModel> SetMetalSamplePropertyList(MetalQualityBaseValue baseValues, MetalSample sample,
            MetalCoefficientWeighting coeficientValues)
        {
            var propertyList = new List<MetalSampleQualityRateModel>();
            var model = new MetalSampleQualityRateModel
            {
                SampleProperty = "Швидкість навантаження V, м/с",
                BaseValue = baseValues.LoadingSpeed,
                SampleValue = sample.LoadingSpeed,
                RelativeValue = Math.Round((decimal)sample.LoadingSpeed / baseValues.LoadingSpeed, 2),
                CoefficientWeighting = coeficientValues.LoadingSpeed,
                Angle = Round(360 * coeficientValues.LoadingSpeed, 0)
            };
            propertyList.Add(model);

            model = new MetalSampleQualityRateModel
            {
                SampleProperty = "Поперечна швидкість зсуву Vt, м/с",
                BaseValue = baseValues.LateralShearRate,
                SampleValue = sample.LateralShearRate,
                RelativeValue = Math.Round((decimal)sample.LateralShearRate / baseValues.LateralShearRate, 2),
                CoefficientWeighting = coeficientValues.LateralShearRate,
                Angle = Round(360 * coeficientValues.LateralShearRate, 0)
            };
            propertyList.Add(model);

            model = new MetalSampleQualityRateModel
            {
                SampleProperty = "Поздовжня швидкість зсуву VL, м/с",
                BaseValue = baseValues.LongitudinalShearRate,
                SampleValue = sample.LongitudinalShearRate,
                RelativeValue = Math.Round((decimal)sample.LongitudinalShearRate / baseValues.LongitudinalShearRate, 2),
                CoefficientWeighting = coeficientValues.LongitudinalShearRate,
                Angle = Round(360 * coeficientValues.LongitudinalShearRate, 0)
            };
            propertyList.Add(model);

            model = new MetalSampleQualityRateModel
            {
                SampleProperty = "Швидкість деформації зсуву γ",
                BaseValue = baseValues.ShearStrainRate,
                SampleValue = sample.ShearStrainRate,
                RelativeValue = Math.Round((decimal)sample.ShearStrainRate / baseValues.ShearStrainRate, 2),
                CoefficientWeighting = coeficientValues.ShearStrainRate,
                Angle = Round(360 * coeficientValues.ShearStrainRate, 0)
            };
            propertyList.Add(model);

            model = new MetalSampleQualityRateModel
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

        public ActionResult MetalTypes()
        {
            return View();
        }

        public ActionResult GetMetalTypes([DataSourceRequest] DataSourceRequest request)
        {
            var baseValues = _metalRepository.GetMetalQualityBaseValues();
            var coeficientValues = _metalRepository.GetAllMetalCoefficientsWeighting();
            var metalTypes = _metalRepository.GetAllMetalTypes();
            var models = new List<MetalTypeModel>();
            foreach (var type in metalTypes)
            {
                var metalQualityBaseValue = type.MetalQualityBaseValues.FirstOrDefault();
                var metalCoefficientWeighting = type.MetalCoefficientWeightings.FirstOrDefault();
                var model = new MetalTypeModel
                {
                    MetalTypeId = type.MetalId,
                    MetalName = type.Name,
                    ShearStrainRate = metalQualityBaseValue.ShearStrainRate,
                    LoadingSpeed = metalQualityBaseValue.LoadingSpeed,
                    LateralShearRate = metalQualityBaseValue.LateralShearRate,
                    LongitudinalShearRate = metalQualityBaseValue.LongitudinalShearRate,
                    SpallStrength = metalQualityBaseValue.SpallStrength,
                    LateralShearRateC = metalCoefficientWeighting.LateralShearRate,
                    LoadingSpeedC = metalCoefficientWeighting.LoadingSpeed,
                    LongitudinalShearRateC = metalCoefficientWeighting.LongitudinalShearRate,
                    SpallStrengthC = metalCoefficientWeighting.SpallStrength,
                    ShearStrainRateC = metalCoefficientWeighting.SpallStrength
                };
                models.Add(model);
            }

            return Json(models.ToDataSourceResult(request));
        }

        [HttpGet]
        public ActionResult AddMetalType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMetalType(MetalTypeModel model)
        {
            _metalRepository.AddMetalType(model);
            return RedirectToAction("MetalTypes");
        }

        [HttpGet]
        public ActionResult EditMetalType(int id)
        {
            var type = _metalRepository.GetMetalType(id);
            var metalQualityBaseValue = type.MetalQualityBaseValues.FirstOrDefault();
            var metalCoefficientWeighting = type.MetalCoefficientWeightings.FirstOrDefault();
            var model = new MetalTypeModel
            {
                MetalTypeId = type.MetalId,
                MetalName = type.Name,
                ShearStrainRate = metalQualityBaseValue.ShearStrainRate,
                LoadingSpeed = metalQualityBaseValue.LoadingSpeed,
                LateralShearRate = metalQualityBaseValue.LateralShearRate,
                LongitudinalShearRate = metalQualityBaseValue.LongitudinalShearRate,
                SpallStrength = metalQualityBaseValue.SpallStrength,
                LateralShearRateC = metalCoefficientWeighting.LateralShearRate,
                LoadingSpeedC = metalCoefficientWeighting.LoadingSpeed,
                LongitudinalShearRateC = metalCoefficientWeighting.LongitudinalShearRate,
                SpallStrengthC = metalCoefficientWeighting.SpallStrength,
                ShearStrainRateC = metalCoefficientWeighting.SpallStrength
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditMetalType(MetalTypeModel model)
        {
            _metalRepository.EditMetalType(model);
            return RedirectToAction("MetalTypes");
        }




    }
}
