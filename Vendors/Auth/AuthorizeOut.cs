namespace PicPay.Vendors.Auth;

public class AuthorizeOut
{
    public string Status { get; set; }
    public AuthorizeDataOut Data { get; set; }

    public AuthorizeOut(bool authorize)
    {
        Status = authorize ? "success" : "fail";
        Data = new() { Authorization = authorize };
    }
}

public class AuthorizeDataOut
{
    public bool Authorization { get; set; }
}
