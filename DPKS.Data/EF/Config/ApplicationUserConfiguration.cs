using DPKS.Data.Entites;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DPKS.Data.Config
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");



            builder.HasOne(user => user.QuocGia)
                   .WithMany(QuocGia => QuocGia.Users)
                   .HasForeignKey(user => user.QuocGiaId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.NoAction);


            // relation for state 

            builder.HasOne(user => user.Tinh)
                   .WithMany(state => state.Users)
                   .HasForeignKey(user => user.TinhId)
                   .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction); ;

            // relation for Reservation 
            builder.HasMany(user => user.datPhongs)
                    .WithOne(reservation => reservation.User)
                    .HasForeignKey(reservation => reservation.UserId)
                    .IsRequired();


            // relation for feedbacks 
            builder.HasMany(user => user.FeedBacks)
                   .WithOne(feedback => feedback.User)
                   .HasForeignKey(feedback => feedback.UserId)
                   .IsRequired();

        }
    }
}
