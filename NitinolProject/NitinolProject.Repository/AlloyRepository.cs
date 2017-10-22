using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitinolProject.Entities;
using NitinolProject.Repository.Interfaces;
using System.Data.Objects;
using NitinolProject.Web.Models.Alloy;

namespace NitinolProject.Repository
{
    public class AlloyRepository : IAlloyRepository
    {
        private readonly string _connectionString;
        public AlloyRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public void AddAlloySample(NicelideTitanumSampleModel model)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var alloys = context.CreateObjectSet<NickelideTitaniumAlloy>();
                if (!alloys.Select(x => x.Name).Contains(model.NickelideTitaniumAlloyName))
                {
                    var alloy = new NickelideTitaniumAlloy
                    {
                        Name = model.NickelideTitaniumAlloyName,
                        Description = null
                    };
                    alloys.AddObject(alloy);
                    context.SaveChanges();
                }
                var alloyArray = context.CreateObjectSet<NickelideTitaniumAlloy>().ToList();
                var sample = new NicelideTitanumSample
                {
                    HammerSpeed = model.HammerSpeed.Value,
                    SampleNumber = model.SampleNumber.Value,
                    HammerThickness = model.HammerThickness.Value,
                    SampleThickness = model.SampleThickness.Value,
                    SpallSpeed = model.SpallSpeed.Value,
                    SpallStrength = model.SpallStrength.Value,
                    NickelideTitaniumAlloyId = alloyArray.First(x => x.Name == model.NickelideTitaniumAlloyName)
                        .NickelideTitaniumAlloyId
                };
                var samples = context.CreateObjectSet<NicelideTitanumSample>();
                samples.AddObject(sample);
                context.SaveChanges();
            }
        }

        public void DeleteAlloySample(int id)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var sample = context.CreateObjectSet<NicelideTitanumSample>()
                    .First(x => x.NicelideTitanumSampleId == id);
                context.DeleteObject(sample);
                context.SaveChanges();
            }
        }

        public void EditAlloySample(NicelideTitanumSampleModel model)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                var sample = context.CreateObjectSet<NicelideTitanumSample>()
                    .Include("NickelideTitaniumAlloy")
                    .First(x => x.NicelideTitanumSampleId == model.NicelideTitanumSampleId);

                var alloys = context.CreateObjectSet<NickelideTitaniumAlloy>();
                if (!alloys.Select(x => x.Name).Contains(model.NickelideTitaniumAlloyName))
                {
                    var alloy = new NickelideTitaniumAlloy
                    {
                        Name = model.NickelideTitaniumAlloyName,
                        Description = null
                    };
                    alloys.AddObject(alloy);
                    context.SaveChanges();
                }
                var alloyArray = context.CreateObjectSet<NickelideTitaniumAlloy>().ToList();

                sample.HammerSpeed = model.HammerSpeed.Value;
                //sample.SampleNumber = model.SampleNumber.Value;
                sample.HammerThickness = model.HammerThickness.Value;
                sample.SampleThickness = model.SampleThickness.Value;
                sample.SpallSpeed = model.SpallSpeed.Value;
                sample.SpallStrength = model.SpallStrength.Value;
                sample.NickelideTitaniumAlloyId = alloyArray.First(x => x.Name == model.NickelideTitaniumAlloyName)
                    .NickelideTitaniumAlloyId;

                context.SaveChanges();
            }
        }

        public IList<NicelideTitanumSample> GetAllAlloySamples()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<NicelideTitanumSample>().Include("NickelideTitaniumAlloy").ToList();
            }
        }

        public NicelideTitanumSample GetAlloySample(int id)
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<NicelideTitanumSample>()
                    .Include("NickelideTitaniumAlloy")
                    .First(x => x.NicelideTitanumSampleId == id);
            }
        }

        public NicelideTitanumCoefficientWeighting GetNicelideTitanumCoefficientWeighting()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<NicelideTitanumCoefficientWeighting>().FirstOrDefault();
            }
        }

        public NicelideTitanumQualityBaseValue GetNicelideTitanumQualityBaseValue()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<NicelideTitanumQualityBaseValue>().FirstOrDefault();
            }
        }
    }
}
