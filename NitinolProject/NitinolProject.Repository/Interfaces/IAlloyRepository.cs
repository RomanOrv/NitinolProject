using NitinolProject.Entities;
using System.Collections.Generic;

namespace NitinolProject.Repository.Interfaces
{
    public interface IAlloyRepository
    {
        IList<NicelideTitanumSample> GetAllAlloySamples();
        NicelideTitanumQualityBaseValue GetNicelideTitanumQualityBaseValue();
    }
}