namespace PicPay.Api.Tasks;

public class PicPayTask
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public PicPayTaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }
    public Guid? EventId { get; set; }
    public int Duration { get; set; }
    public Guid? ParentId { get; set; }

    public PicPayTask() { }

    public PicPayTask(Guid? eventId, object data)
    {
        Id = Guid.NewGuid();
        Type = data.GetType().ToString();
        Data = data.Serialize();
        CreatedAt = DateTime.Now;
        EventId = eventId;
    }

    public void Processed(double duration)
    {
        ProcessedAt = DateTime.Now;
        Duration = Convert.ToInt32(duration);
        Status = Error.HasValue() ? PicPayTaskStatus.Error : PicPayTaskStatus.Success;
    }
}
