using PicPay.Api.Features.Cross.CreateUser;

namespace PicPay.Api.Features.Cross.CreateTransaction;

public class TransactionConfig : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> transaction)
    {
        transaction.ToTable("transactions");

        transaction.HasKey(t => t.Id);
        transaction.Property(t => t.Id).ValueGeneratedNever();

        transaction.HasOne<Wallet>()
            .WithMany()
            .HasPrincipalKey(w => w.Id)
            .HasForeignKey(t => t.SourceWalletId);

        transaction.HasOne<Wallet>()
            .WithMany()
            .HasPrincipalKey(w => w.Id)
            .HasForeignKey(t => t.TargetWalletId);
    }
}
