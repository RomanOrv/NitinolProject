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

        public void DeleteMetalSample(int id)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var sample = context.CreateObjectSet<MetalSample>().First(x => x.MetalSampleId == id);
                context.DeleteObject(sample);
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
                return context.CreateObjectSet<Metal>().ToList();
            }
        }

        public IList<MetalQualityBaseValue> GetMetalQualityBaseValues()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<MetalQualityBaseValue>().ToList();
            }
        }

        public MetalSample GetMetalSample(int id)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<MetalSample>().Include("Metal").Include("CrystalLattice").First(x => x.MetalSampleId == id);
            }
        }
    }
}
