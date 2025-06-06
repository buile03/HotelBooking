using DPKS.Data.Entites;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;


namespace DPKS.Data.Config
{
    public class LoaiPhongConfiguration : IEntityTypeConfiguration<LoaiPhong>
    {
        public void Configure(EntityTypeBuilder<LoaiPhong> builder)
        {
            builder.ToTable("LoaiPhong");

            builder.HasKey(roomType => roomType.Id);

            builder.Property(roomType => roomType.Id).ValueGeneratedOnAdd();


            builder
             .HasMany(rt => rt.tienNghis)
             .WithMany(a => a.loaiPhongs)
             .UsingEntity<TienNghiTheoLoaiPhong>(
                 j => j
                     .HasOne(rta => rta.TienNghi)
                     .WithMany(a => a.tienNghiTheoLoaiPhongs)
                     .HasForeignKey(rta => rta.TienNghiId),
                 j => j
                     .HasOne(rta => rta.LoaiPhong)
                     .WithMany(rt => rt.tienNghiTheoLoaiPhongs)
                     .HasForeignKey(rta => rta.LoaiPhongId),
                 j =>
                 {
                     j.HasKey(rta => new { rta.LoaiPhongId, rta.TienNghiId });
                 });

            



          
        }
    }
}
