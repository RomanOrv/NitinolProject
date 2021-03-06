﻿using NitinolProject.Entities;
using NitinolProject.Web.Models.Alloy;
using System.Collections.Generic;

namespace NitinolProject.Repository.Interfaces
{
    public interface IAlloyRepository
    {
        IList<NicelideTitanumSample> GetAllAlloySamples();
        NicelideTitanumQualityBaseValue GetNicelideTitanumQualityBaseValue();
        void AddAlloySample(NicelideTitanumSampleModel model);
        void EditAlloySample(NicelideTitanumSampleModel model);
        void DeleteAlloySample(int id);
        NicelideTitanumCoefficientWeighting GetNicelideTitanumCoefficientWeighting();
        NicelideTitanumSample GetAlloySample(int id);
    }
}