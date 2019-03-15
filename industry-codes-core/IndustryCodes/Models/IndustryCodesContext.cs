//
//  IndustryCodesContext.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2019
//
using Microsoft.EntityFrameworkCore;

namespace IndustryCodes.Models
{
    public partial class IndustryCodesContext : DbContext
    {
        public IndustryCodesContext()
        {
        }

        public IndustryCodesContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<ClassificationCodes> ClassificationCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassificationCodes>(entity =>
            {
                entity.Property(e => e.Id)
                      .HasColumnName("ID");

                entity.Property(e => e.IndustrySector)
                      .HasColumnName("INDUSTRY_SECTOR")
                      .HasMaxLength(255);

                entity.Property(e => e.IndustrySubsector)
                      .HasColumnName("INDUSTRY_SUBSECTOR")
                      .HasMaxLength(255);

                entity.Property(e => e.NorthAmericanCode)
                      .HasColumnName("NORTH_AMERICAN_CODE");

                entity.Property(e => e.NorthAmericanDescription)
                      .HasColumnName("NORTH_AMERICAN_DESCRIPTION")
                      .HasMaxLength(255);

                entity.Property(e => e.StandardCode)
                      .HasColumnName("STANDARD_CODE");

                entity.Property(e => e.StandardDescription)
                      .HasColumnName("STANDARD_DESCRIPTION")
                      .HasMaxLength(255);


                entity.Property(e => e.KindCode)
                      .HasColumnName("KIND_CODE");

                entity.Property(e => e.KindCodeDescription)
                      .HasColumnName("KIND_CODE_DESCRIPTION")
                      .HasMaxLength(255);

                entity.Property(e => e.NorthAmericanCode2002)
                      .HasColumnName("NORTH_AMERICAN_CODE_2002");

                entity.Property(e => e.NorthAmericanDescription2002)
                      .HasColumnName("NORTH_AMERICAN_DESCRIPTION_2002")
                      .HasMaxLength(255);
            });
        }
    }
}