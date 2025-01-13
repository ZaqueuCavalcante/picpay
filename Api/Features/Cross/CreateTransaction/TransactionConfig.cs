namespace PicPay.Api.Features.Cross.CreateTransaction;

public class TransactionConfig : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> transaction)
    {
        transaction.ToTable("transactions");

        transaction.HasKey(t => t.Id);
        transaction.Property(t => t.Id).ValueGeneratedNever();
    }
}
