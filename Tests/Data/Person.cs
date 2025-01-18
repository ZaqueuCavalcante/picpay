using Bogus;

namespace PicPay.Tests.Data;

public static class Person
{
    public static string GetRandomEmail()
    {
        return new Faker().Internet.Email();
    }

    public static string GetRandomName()
    {
        return new Faker("pt_BR").Person.FullName;
    }
}
