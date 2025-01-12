namespace PicPay.Api.Validators;

public static class DocumentValidator
{
    private static readonly int[] _firstKeyValidators = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    private static readonly int[] _secondKeyValidators = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

    public static bool IsValidDocument(this string value)
    {
        value = value.OnlyNumbers();

        if (value.Length == 11) return ValidCpf(value);

        if (value.Length == 14) return ValidCnpj(value);

        return false;
    }

    private static bool ValidCpf(string value)
    {
        var numbers = value.Select(c => c - '0').ToArray();

        if (RepeatedCpfNumbers(numbers))
            return false;

        if (InvalidCpfKeyNumber(numbers, 9) || InvalidCpfKeyNumber(numbers, 10))
            return false;

        return true;
    }

    private static bool RepeatedCpfNumbers(int[] numbers)
    {
        return numbers.All(d => d == numbers[0]);
    }

    private static bool InvalidCpfKeyNumber(int[] numbers, int keySlot)
    {
        var sum = numbers
            .Take(keySlot)
            .Select((d, i) => d * (keySlot + 1 - i))
            .Sum() % 11;

        var keyNumber = sum < 2 ? 0 : 11 - sum;

        return numbers[keySlot] != keyNumber;
    }

    private static bool ValidCnpj(string value)
    {
        var numbers = value.Select(c => c - '0').ToArray();

        if (InvalidCnpjKeyNumber(numbers, 12) || InvalidCnpjKeyNumber(numbers, 13))
            return false;

        return true;
    }

    private static bool InvalidCnpjKeyNumber(int[] numbers, int keySlot)
    {
        var validators = keySlot == 12 ? _firstKeyValidators : _secondKeyValidators;

        int calculatedCheckDigit = numbers
            .Take(keySlot)
            .Select((n, i) => n * validators[i])
            .Sum() % 11;

        calculatedCheckDigit = calculatedCheckDigit < 2 ? 0 : 11 - calculatedCheckDigit;

        return numbers[keySlot] != calculatedCheckDigit;
    }
}
