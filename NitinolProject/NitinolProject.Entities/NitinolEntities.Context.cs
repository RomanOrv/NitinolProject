﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NitinolProject.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class assessment_quality_materialsEntities1 : DbContext
    {
        public assessment_quality_materialsEntities1()
            : base("name=assessment_quality_materialsEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CrystalLattice> CrystalLattices { get; set; }
        public DbSet<Metal> Metals { get; set; }
        public DbSet<MetalQualityBaseValue> MetalQualityBaseValues { get; set; }
        public DbSet<MetalSample> MetalSamples { get; set; }
        public DbSet<NicelideTitanumQualityBaseValue> NicelideTitanumQualityBaseValues { get; set; }
        public DbSet<NicelideTitanumSample> NicelideTitanumSamples { get; set; }
        public DbSet<NickelideTitaniumAlloy> NickelideTitaniumAlloys { get; set; }
    }
}