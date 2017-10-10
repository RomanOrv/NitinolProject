using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitinolProject.Entities;
using NitinolProject.Repository.Interfaces;
using NitinolProject.Web.Models.Metal;
using System.Data.Objects;

namespace NitinolProject.Repository
{
    public class MetalRepository : IMetalRepository
    {
        private readonly string _connectionString;
        public MetalRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public void AddMetalSample(MetalSampleModel model)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var sample = new MetalSample
                {
                    Name = model.Name,
                    SampleNumber = model.SampleNumber.Value,
                    MetalId = model.MetalId.Value,
                    CrystalLatticeId = model.CrystalLatticeId.Value,
                    LateralShearRate = model.LateralShearRate.Value,
                    LoadingSpeed = model.LoadingSpeed.Value,
                    LongitudinalShearRate = model.LongitudinalShearRate.Value,
                    ShearStrainRate = model.ShearStrainRate.Value,
                    SpallStrength = model.SpallStrength.Value
                };

                var samples = context.CreateObjectSet<MetalSample>();
                samples.AddObject(sample);
                context.SaveChanges();
            }
        }

        public void AddMetalType(MetalTypeModel model)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var type = new Metal
                {
                    Name = model.MetalName
                };
                var types = context.CreateObjectSet<Metal>();
                types.AddObject(type);

                var baseValues = context.CreateObjectSet<MetalQualityBaseValue>();
                var baseValue = new MetalQualityBaseValue
                {
                    MetalId = type.MetalId,
                    LateralShearRate = model.LateralShearRate,
                    LoadingSpeed = model.LoadingSpeed,
                    LongitudinalShearRate = model.LongitudinalShearRate,
                    ShearStrainRate = model.ShearStrainRate,
                    SpallStrength = model.SpallStrength
                };
                baseValues.AddObject(baseValue);


                var coeficients = context.CreateObjectSet<MetalCoefficientWeighting>();
                var coeficient = new MetalCoefficientWeighting
                {
                    MetalId = type.MetalId,
                    LateralShearRate = model.LateralShearRateC,
                    LoadingSpeed = model.LoadingSpeedC,
                    LongitudinalShearRate = model.LongitudinalShearRateC,
                    ShearStrainRate = model.ShearStrainRateC,
                    SpallStrength = model.SpallStrengthC
                };
                coeficients.AddObject(coeficient);

                context.SaveChanges();
            }
        }

        public void DeleteMetalSample(int id)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var sample = context.CreateObjectSet<MetalSample>().First(x => x.MetalSampleId == id);
                context.DeleteObject(sample);
                context.SaveChanges();
            }
        }

        public void DeleteMetalType(int id)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var type = context.CreateObjectSet<Metal>().First(x => x.MetalId == id);
                context.DeleteObject(type);
                context.SaveChanges();
            }
        }

        public void EditMetalSample(MetalSampleModel model)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var sample = context.CreateObjectSet<MetalSample>().First(x => x.MetalSampleId == model.MetalSampleId);
                sample.Name = model.Name;
                sample.SampleNumber = model.SampleNumber.Value;
                sample.MetalId = model.MetalId.Value;
                sample.CrystalLatticeId = model.CrystalLatticeId.Value;
                sample.LateralShearRate = model.LateralShearRate.Value;
                sample.LoadingSpeed = model.LoadingSpeed.Value;
                sample.LongitudinalShearRate = model.LongitudinalShearRate.Value;
                sample.ShearStrainRate = model.ShearStrainRate.Value;
                sample.SpallStrength = model.SpallStrength.Value;
                context.SaveChanges();
            }
        }

        public void EditMetalType(MetalTypeModel model)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var type = context.CreateObjectSet<Metal>().FirstOrDefault(x => x.MetalId == model.MetalTypeId);
                type.Name = model.MetalName;

                var baseValue = context.CreateObjectSet<MetalQualityBaseValue>().FirstOrDefault(x => x.MetalId == model.MetalTypeId);
                baseValue.LateralShearRate = model.LateralShearRate;
                baseValue.LoadingSpeed = model.LoadingSpeed;
                baseValue.LongitudinalShearRate = model.LongitudinalShearRate;
                baseValue.ShearStrainRate = model.ShearStrainRate;
                baseValue.SpallStrength = model.SpallStrength;



                var coeficient = context.CreateObjectSet<MetalCoefficientWeighting>().FirstOrDefault(x => x.MetalId == model.MetalTypeId);

                coeficient.LateralShearRate = model.LateralShearRateC;
                coeficient.LoadingSpeed = model.LoadingSpeedC;
                coeficient.LongitudinalShearRate = model.LongitudinalShearRateC;
                coeficient.ShearStrainRate = model.ShearStrainRateC;
                coeficient.SpallStrength = model.SpallStrengthC;

                context.SaveChanges();
            }
        }

        public IList<CrystalLattice> GetAllCrystalLattices()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<CrystalLattice>().ToList();
            }
        }

        public IList<MetalCoefficientWeighting> GetAllMetalCoefficientsWeighting()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<MetalCoefficientWeighting>().ToList();
            }
        }

        public IList<MetalSample> GetAllMetalSamples()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<MetalSample>().Include("Metal").Include("CrystalLattice").ToList();
            }
        }

        public IList<Metal> GetAllMetalTypes()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<Metal>().Include("MetalQualityBaseValues").Include("MetalCoefficientWeightings").ToList();
            }
        }

        public IList<MetalQualityBaseValue> GetMetalQualityBaseValues()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<MetalQualityBaseValue>().Include("Metal").ToList();
            }
        }

        public MetalSample GetMetalSample(int id)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<MetalSample>().Include("Metal").Include("CrystalLattice").First(x => x.MetalSampleId == id);
            }
        }

        public Metal GetMetalType(int id)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<Metal>().Include("MetalQualityBaseValues").Include("MetalCoefficientWeightings").First(x => x.MetalId == id);
            }
        }
    }
}
