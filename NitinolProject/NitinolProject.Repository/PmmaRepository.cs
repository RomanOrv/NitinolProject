using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NitinolProject.Repository.Interfaces;

namespace NitinolProject.Repository
{
    public class PmmaRepository : IPmmaRepository
    {
        private readonly string _connectionString;
        public PmmaRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }
    }
}
