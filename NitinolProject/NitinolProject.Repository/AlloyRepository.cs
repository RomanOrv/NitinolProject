using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitinolProject.Entities;
using NitinolProject.Repository.Interfaces;
using System.Data.Objects;

namespace NitinolProject.Repository
{
    public class AlloyRepository : IAlloyRepository
    {
        private readonly string _connectionString;
        public AlloyRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public IList<NicelideTitanumSample> GetAllAlloySamples()
        {
            using (ObjectContext context = new ObjectContext(_connectionString))
            {
                return context.CreateObjectSet<NicelideTitanumSample>().Include("NickelideTitaniumAlloy").ToList();               
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
