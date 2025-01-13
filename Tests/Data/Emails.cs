namespace PicPay.Tests.Data;

public static class Emails
{
    public static string New => $"{Guid.NewGuid().ToString().OnlyNumbers()}@picpay.com";

    public static IEnumerable<object[]> Invalid()
    {
        List<string> emails = [
            "",
            " ",
            "zaqueugmail",
            "majuasp.net",
            "#@%^%#$@#$@#.com",
            "@example.com",
            "Joe Smith <email@example.com>",
            "email.example.com",
            "email@example@example.com",
            ".email@example.com",
            "email.@example.com",
            "email..email@example.com",
            "email@example.com (Joe Smith)",
            "email@example",
            "email@-example.com",
            "email@example..com",
            "Abc..123@example.com",
        ];

        foreach (var email in emails)
        {
            yield return [email];
        }
    }
}
