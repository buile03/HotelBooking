
using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Config
{
    public class AnhPhongConfiguration : IEntityTypeConfiguration<AnhPhong>
    {
        public void Configure(EntityTypeBuilder<AnhPhong> builder)
        {
            builder.ToTable("AnhPhong");

            builder.HasKey(roomPhoto => roomPhoto.PhotoId);

            builder.Property(roomPhoto => roomPhoto.PhotoId).ValueGeneratedOnAdd();

            builder.HasIndex(roomPhoto => roomPhoto.PhotoName).IsUnique();

           


        }
    }
}
