using DPKS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPKS.Data.Config
{
    public class TrackingConfiguration : IEntityTypeConfiguration<Tracking>
    {
        public void Configure(EntityTypeBuilder<Tracking> builder)
        {
            builder.ToTable("Tracking");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Time)
                   .IsRequired();

            builder.Property(t => t.Action)
                   .HasMaxLength(256);

            builder.Property(t => t.ErorMessage)
                   .HasMaxLength(1000);

            builder.Property(t => t.IsError)
                   .HasDefaultValue(false);

            // 👇 Foreign key rõ ràng cho User
            builder.HasOne(t => t.User)
                   .WithMany() // hoặc .WithMany(u => u.Trackings) nếu có trong ApplicationUser
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Restrict); // hoặc khác nếu bạn muốn cascade
        }
    }
}
