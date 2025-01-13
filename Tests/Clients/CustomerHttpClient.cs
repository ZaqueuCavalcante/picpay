namespace PicPay.Tests.Clients;

public class CustomerHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    // public async Task<List<EnrollmentClassOut>> GetStudentEnrollmentClasses()
    // {
    //     var client = new GetStudentEnrollmentClassesClient(Http);
    //     return await client.Get();
    // }
}
