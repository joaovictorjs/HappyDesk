namespace HappyDesk.Domain.Constants;

public static class EndPoints
{
    public const string Domain = "helpdesk.zeusautomacao.com.br";
    public const string Base = $"https://{Domain}";
    public const string Home = $"{Base}/Home";
    public const string Login = $"{Base}/Account/Login?ReturnUrl=%2F";
}
