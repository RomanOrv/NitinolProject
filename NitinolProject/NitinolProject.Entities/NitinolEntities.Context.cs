﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NitinolProject.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class assessment_quality_materialsEntities : DbContext
    {
        public assessment_quality_materialsEntities()
            : base("name=assessment_quality_materialsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CrystalLattice> CrystalLattices { get; set; }
        public virtual DbSet<Metal> Metals { get; set; }
        public virtual DbSet<MetalQualityBaseValue> MetalQualityBaseValues { get; set; }
        public virtual DbSet<MetalSample> MetalSamples { get; set; }
        public virtual DbSet<NicelideTitanumQualityBaseValue> NicelideTitanumQualityBaseValues { get; set; }
        public virtual DbSet<NicelideTitanumSample> NicelideTitanumSamples { get; set; }
        public virtual DbSet<NickelideTitaniumAlloy> NickelideTitaniumAlloys { get; set; }
    }
}
