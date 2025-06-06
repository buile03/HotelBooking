
using DPKS.Common.Enum;
using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace DPKS.Data.Config
{
    public class TrangThaiDatPhongConfiguration : IEntityTypeConfiguration<TrangThaiDatPhong>
    {
        public void Configure(EntityTypeBuilder<TrangThaiDatPhong> builder)
        {
            builder.ToTable("TrangThaiDatPhong");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.trangThaiDatPhong)
                  .HasConversion(
                  x => x.ToString(),
                  x => (enTrangThaiDatPhong)Enum.Parse(typeof(enTrangThaiDatPhong), x)
                  );
        }
    }
}
