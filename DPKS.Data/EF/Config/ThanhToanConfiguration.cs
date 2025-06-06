
using DPKS.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;


namespace DPKS.Data.Config
{
    public class ThanhToanConfiguration : IEntityTypeConfiguration<ThanhToan>
    {
        public void Configure(EntityTypeBuilder<ThanhToan> builder)
        {
            builder.ToTable("ThanhToan");

            builder.HasKey(pay => pay.Id);

            builder.Property(pay => pay.Id).ValueGeneratedOnAdd();


            builder
            .Property(p => p.Gia).HasColumnName("Gia")
            .HasPrecision(18, 2); //

            // realtion with PaymentMethod 
            builder.HasOne(pay => pay.PhuongThucThanhToan)
                   .WithMany(method => method.thanhToans)
                   .HasForeignKey(pay => pay.PhuongThucThanhToanId)
            .IsRequired();

      
        

        }
    }
}
