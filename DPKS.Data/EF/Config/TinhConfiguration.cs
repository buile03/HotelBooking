
using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DPKS.Data.Config
{
    public class TinhConfiguration : IEntityTypeConfiguration<Tinh>
    {
        public void Configure(EntityTypeBuilder<Tinh> builder)
        {
            builder.ToTable("Tinh");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();


        }
    }
}
