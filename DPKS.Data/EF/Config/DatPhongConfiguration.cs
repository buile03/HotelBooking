using DPKS.Data.Entites;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DPKS.Data.Config
{
    public class DatPhongConfiguration : IEntityTypeConfiguration<DatPhong>
    {
        public void Configure(EntityTypeBuilder<DatPhong> builder)
        {
            builder.ToTable("DatPhong");
            builder.ToTable(t => t.HasCheckConstraint("CK_Reservation_SoLuongKhach", "SoLuongKhach >=1 AND SoLuongKhach<=4"));
            builder.ToTable(t => t.HasCheckConstraint("CK_Reservation_SoDem", "SoDem >=1 AND SoDem<=31"));

            builder.HasKey(res => res.Id);

            builder.Property(res => res.Id).ValueGeneratedOnAdd();
            builder.Property(x=> x.IsActive).HasDefaultValue(true);

            builder.Property(p => p.TongTien).HasColumnName("TongTien")
            .HasPrecision(18, 2); //


            // relation with FeedBacks 
            builder.HasMany(res => res.feedBacks)
                   .WithOne(feed => feed.DatPhong)
                   .HasForeignKey(feed => feed.DatPhongId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            //relation for ReservationStatuses 

            builder.HasOne(res => res.TrangThaiDatPhong)
                   .WithMany(resStat => resStat.datPhongs)
                   .HasForeignKey(res => res.TrangThaiDatPhongId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.NoAction);

            // relation for Rooms 
            builder.HasOne(res => res.Phong)
                   .WithMany(room => room.datPhongs)
                   .HasForeignKey(res => res.PhongId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.NoAction);

            // relation for payments 
            builder.HasOne(res => res.ThanhToan)
                   .WithOne(payment => payment.DatPhong)
                   .HasForeignKey<ThanhToan>(payment => payment.DatPhongId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.NoAction);

            

       

            




        }
    }
}
