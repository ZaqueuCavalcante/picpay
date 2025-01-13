namespace PicPay.Tests.Data;

public static class Documents
{
    public static IEnumerable<object[]> InvalidCpfs()
    {
        List<string> cpfs = [
            "",
            " ",
            "159",
            "000.000.000-00",
            "111.111.111-11",
            "123.456.789.00",
            "119.245.284-ab",
            "789.456.123-88",
        ];

        foreach (var cpf in cpfs)
        {
            yield return [cpf];
        }
    }

    public static IEnumerable<object[]> ValidCpfs()
    {
        List<string> cpfs = [
            "50859603865",
            "395.474.830-12",
            "012.345.678-96",
            "123.456.789-09",
            "987.654.321-00",
            "745.892.310-44",
            "301.458.962-17",
            "654.789.321-98",
            "852.147.963-00",
            "123.789.456-01",
            "789.456.123-07",
        ];

        foreach (var cpf in cpfs)
        {
            yield return [cpf];
        }
    }

    public static IEnumerable<object[]> InvalidCnpjs()
    {
        List<string> cnpjs = [
            "12.345.678/0000-00",
            "11.111.111/1111-11",
            "22.333.444/0001-XX",
            "33.444.555/000",
            "44.555.666/",
            "55.666.777/abcd-12",
            "66.777.888/0001-999",
            "77.888.999/0000-0A",
            "88.999.000/0001-00",
            "99.000.111/1234-56",
        ];

        foreach (var cnpj in cnpjs)
        {
            yield return [cnpj];
        }
    }

    public static IEnumerable<object[]> ValidCnpjs()
    {
        List<string> cnpjs = [
            "52543666000190",
            "12.345.678/0001-95",
            "98.765.432/0001-60",
            "45.678.912/0001-37",
            "11.222.333/0001-81",
            "22.333.444/0001-02",
            "33.444.555/0001-19",
            "44.555.666/0001-54",
            "55.666.777/0001-83",
            "66.777.888/0001-70",
            "77.888.999/0001-01",
        ];

        foreach (var cnpj in cnpjs)
        {
            yield return [cnpj];
        }
    }

    public static string GetRandomCpf()
    {
        int sum = 0;
        int[] mult1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] mult2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        var rnd = new Random();
        string seed = rnd.Next(100000000, 999999999).ToString();

        for (int i = 0; i < 9; i++)
            sum += int.Parse(seed[i].ToString()) * mult1[i];

        var rest = sum % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        seed += rest;
        sum = 0;

        for (int i = 0; i < 10; i++)
            sum += int.Parse(seed[i].ToString()) * mult2[i];

        rest = sum % 11;

        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        return seed + rest;
    }

    public static string GetRandomCnpj()
    {
        int sum = 0;
        int[] mult1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] mult2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        var rnd = new Random();
        string seed = rnd.Next(10000000, 99999999) + "0001";

        for (int i = 0; i < 12; i++)
            sum += int.Parse(seed[i].ToString()) * mult1[i];

        int rest = sum % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        seed += rest;
        sum = 0;

        for (int i = 0; i < 13; i++)
            sum += int.Parse(seed[i].ToString()) * mult2[i];

        rest = sum % 11;

        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        return seed + rest;
    }

    public static string GetRandomKey()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }
}
