using WhirlpoolPromptWeb.Models;


public interface IAuthenticatorService
{
    Task<List<UserSession>> AuthenticateUserAPI(string email, string password);

}

