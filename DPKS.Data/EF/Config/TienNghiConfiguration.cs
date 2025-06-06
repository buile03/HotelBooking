using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DPKS.Data.Config
{
    public class TienNghiConfiguration : IEntityTypeConfiguration<TienNghi>
    {
        public void Configure(EntityTypeBuilder<TienNghi> builder)
        {
            builder.ToTable("TienNghi");
            builder.HasKey(x => x.Id);
          

            builder.Property(x => x.Id).ValueGeneratedOnAdd();


        }
    }
}
