namespace PicPay.Api.Features.Cross.CreateUser;

public class WalletConfig : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> wallet)
    {
        wallet.ToTable("wallets");

        wallet.HasKey(w => w.Id);
        wallet.Property(w => w.Id).ValueGeneratedNever();
    }
}
