
using DPKS.Common.Enum;
using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace DPKS.Data.Config
{
    public class PhuongThucThanhToanConfiguration : IEntityTypeConfiguration<PhuongThucThanhToan>
    {
        public void Configure(EntityTypeBuilder<PhuongThucThanhToan> builder)
        {
            builder.ToTable("PhuongThucThanhToan");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();


            // Make the Enum Registerd in the Database as its Value 
            builder.Property(x => x.loaiThanhToan)
                   .HasConversion(
                   x => x.ToString(),
                   x => (enLoaiThanhToan)Enum.Parse(typeof(enLoaiThanhToan), x)
                   );


        }
    }
}
