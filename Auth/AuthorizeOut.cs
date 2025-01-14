namespace PicPay.Auth;

public class AuthorizeOut
{
    public string Status { get; set; }
    public AuthorizeDataOut Data { get; set; }
}

public class AuthorizeDataOut
{
    public bool Authorization { get; set; }
}
