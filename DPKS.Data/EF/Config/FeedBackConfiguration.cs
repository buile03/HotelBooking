
using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DPKS.Data.Config
{
    public class FeedBackConfiguration : IEntityTypeConfiguration<FeedBack>
    {
        public void Configure(EntityTypeBuilder<FeedBack> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.ToTable("FeedBacks");

            builder.ToTable(t => t.HasCheckConstraint("CK_FeedBacks_DanhGia", "DanhGia >=1 AND DanhGia<=5"));
        }
    }
}
