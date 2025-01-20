namespace PicPay.Tests.Data;

public static class Passwords
{
    public static IEnumerable<object[]> Weak()
    {
        foreach (var password in new List<string>()
        {
            "",
            " ",
            "syki",
            "syki123",
            "Syki123",
            "lalal.com",
            "12@3lalala",
            "5816811681816",
        })
        {
            yield return [password];
        }
    }
}
