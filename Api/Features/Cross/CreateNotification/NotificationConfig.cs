using PicPay.Api.Features.Cross.CreateUser;
using PicPay.Api.Features.Cross.CreateTransaction;

namespace PicPay.Api.Features.Cross.CreateNotification;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> notification)
    {
        notification.ToTable("notifications");

        notification.HasKey(n => n.Id);
        notification.Property(n => n.Id).ValueGeneratedNever();

        notification.HasOne<PicPayUser>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(n => n.UserId);

        notification.HasOne<Transaction>()
            .WithOne()
            .HasPrincipalKey<Transaction>(t => t.Id)
            .HasForeignKey<Notification>(n => n.TransactionId);
    }
}
