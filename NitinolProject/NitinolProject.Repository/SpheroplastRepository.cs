using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitinolProject.Repository.Interfaces;

namespace NitinolProject.Repository
{
    public class SpheroplastRepository : ISpheroplastRepository
    {
        private readonly string _connectionString;
        public SpheroplastRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }
    }
}
