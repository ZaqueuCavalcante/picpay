using Bogus;

namespace PicPay.Tests.Data;

public static class Company
{
    public static string GetRandomName()
    {
        return new Faker().Company.CompanyName();
    }
}
