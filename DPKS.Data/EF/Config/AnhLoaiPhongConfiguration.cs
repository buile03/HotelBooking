using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.EF.Config
{
    public class AnhLoaiPhongConfiguration : IEntityTypeConfiguration<AnhLoaiPhong>
    {
        public void Configure(EntityTypeBuilder<AnhLoaiPhong> builder)
        {
            builder.ToTable("AnhLoaiPhong");

            builder.HasKey(photo => photo.PhotoId);
            builder.Property(photo => photo.PhotoId).ValueGeneratedOnAdd();
            builder.HasIndex(photo => photo.PhotoName).IsUnique();

            builder.HasOne(photo => photo.LoaiPhong)
                   .WithMany(lp => lp.anhLoaiPhongs) 
                   .HasForeignKey(photo => photo.LoaiPhongId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
