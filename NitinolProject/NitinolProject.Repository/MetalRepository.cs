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
            throw new NotImplementedException();
        }

        public IList<MetalSample> GetAllMetalSamples()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<MetalSample>().Include("Metal").Include("CrystalLattice").ToList();
            }
        }

        public IList<MetalQualityBaseValue> GetMetalQualityBaseValues()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<MetalQualityBaseValue>().ToList();
            }
        }
    }
}
