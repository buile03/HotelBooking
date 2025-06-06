
using DPKS.Common.Enum;
using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace DPKS.Data.Config
{
    public class TrangThaiPhongConfiguration : IEntityTypeConfiguration<TrangThaiPhong>
    {
        public void Configure(EntityTypeBuilder<TrangThaiPhong> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.ToTable("TrangThaiPhong");


            builder.Property(x => x.trangThaiPhong)
                .HasConversion(
                x => x.ToString(),
                x => (enTrangThaiPhong)Enum.Parse(typeof(enTrangThaiPhong), x)
                );

        }
    }
}
