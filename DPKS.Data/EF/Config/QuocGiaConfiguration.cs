
using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DPKS.Data.Config
{
    public class QuocGiaConfiguration : IEntityTypeConfiguration<QuocGia>
    {
        public void Configure(EntityTypeBuilder<QuocGia> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.ToTable("QuocGia");
        }
    }
}
