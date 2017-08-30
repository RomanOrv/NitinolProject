using System.Collections.Generic;
using NitinolProject.Entities;
using NitinolProject.Web.Models.Metal;

namespace NitinolProject.Repository.Interfaces
{
    public interface IMetalRepository
    {
        IList<MetalSample> GetAllMetalSamples();
        IList<MetalQualityBaseValue> GetMetalQualityBaseValues();
        void AddMetalSample(MetalSampleModel model);
        IList<Metal> GetAllMetalTypes();
        IList<CrystalLattice> GetAllCrystalLattices();
    }
}