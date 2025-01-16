namespace PicPay.Api.Tasks;

public class PicPayTaskConfig : IEntityTypeConfiguration<PicPayTask>
{
    public void Configure(EntityTypeBuilder<PicPayTask> task)
    {
        task.ToTable("tasks");

        task.HasKey(t => t.Id);
        task.Property(t => t.Id).ValueGeneratedNever();
    }
}
