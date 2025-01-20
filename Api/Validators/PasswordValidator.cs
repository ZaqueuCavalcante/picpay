namespace PicPay.Api.Validators;

public static class PasswordValidator
{
    public static bool IsStrongPassword(this string password)
    {
        if (password.IsEmpty()) return false;

        if (password.Length < 8) return false;

        var hasNumbers = password.IndexOfAny(_numbers.ToCharArray()) >= 0;
        var hasLower = password.IndexOfAny(_lowers.ToCharArray()) >= 0;
        var hasUpper = password.IndexOfAny(_uppers.ToCharArray()) >= 0;
        var hasNonAlphanumeric = password.IndexOfAny(_nonAlphanumeric.ToCharArray()) >= 0;

        if (!hasNumbers || !hasLower || !hasUpper || !hasNonAlphanumeric) return false;

        return true;
    }

    private static string _numbers = "0123456789";
    private static string _lowers = "abcdefghijklmnopqrstuvwxyz";
    private static string _uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string _nonAlphanumeric = "()~!@#$%^&*-+=|{}[]:;<>,.?/_";
}
