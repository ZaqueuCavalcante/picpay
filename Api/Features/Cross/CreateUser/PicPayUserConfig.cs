namespace PicPay.Api.Features.Cross.CreateUser;

public class PicPayUserConfig : IEntityTypeConfiguration<PicPayUser>
{
    public void Configure(EntityTypeBuilder<PicPayUser> user)
    {
        user.ToTable("users");

        user.HasKey(u => u.Id);
        user.Property(u => u.Id).ValueGeneratedNever();

        user.HasIndex(u => u.Document).IsUnique();
        user.HasIndex(u => u.Email).IsUnique();

        user.HasOne(u => u.Wallet)
            .WithOne()
            .HasPrincipalKey<PicPayUser>(u => u.Id)
            .HasForeignKey<Wallet>(w => w.UserId);
    }
}
