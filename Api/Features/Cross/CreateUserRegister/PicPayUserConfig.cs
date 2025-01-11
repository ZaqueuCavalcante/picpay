namespace PicPay.Api.Features.Cross.CreateUserRegister;

public class PicPayUserConfig : IEntityTypeConfiguration<PicPayUser>
{
    public void Configure(EntityTypeBuilder<PicPayUser> user)
    {
        user.ToTable("users");

        user.HasKey(u => u.Id);
        user.Property(u => u.Id).ValueGeneratedNever();
    }
}
