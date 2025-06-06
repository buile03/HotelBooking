
using DPKS.Common.Enum;
using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Reflection.Emit;


namespace DPKS.Data.Config
{
    public class PhongConfiguration : IEntityTypeConfiguration<Phong>
    {
        public void Configure(EntityTypeBuilder<Phong> builder)
        {
            builder.ToTable("Phong");

            builder.HasKey(room => room.PhongId);

            builder.Property(room => room.PhongId).ValueGeneratedOnAdd();

            builder.HasIndex(room => room.SoPhong).IsUnique();

            builder.Property(x => x.loaiGiuong)
                  .HasConversion(
                  x => x.ToString(),
                  x => (enLoaiGiuong)Enum.Parse(typeof(enLoaiGiuong), x)
                  );

            builder.Property(x => x.loaiView)
                  .HasConversion(
                  x => x.ToString(),
                  x => (enLoaiView)Enum.Parse(typeof(enLoaiView), x)
            );


            builder.Property(p => p.Gia)
            .HasPrecision(18, 2); //

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
            //relation with RoomStatuses

            builder.HasOne(room => room.TrangThaiPhong)
                   .WithMany(status => status.Phongs)
                   .HasForeignKey(room => room.TrangThaiPhongId)
                   .IsRequired();

            // relation with RoomTypes 
            builder.HasOne(room => room.LoaiPhong)
                    .WithMany(type => type.phongs)
                    .HasForeignKey(room => room.LoaiPhongId)
                    .IsRequired();

            //relation with RoomPhotos  
            builder.HasMany(r => r.anhPhongs)
           .WithOne(p => p.Phong)
           .HasForeignKey(p => p.PhongId)
           .IsRequired()
           .OnDelete(DeleteBehavior.Cascade);





        }
    }
}
