using Shared.Dtos.IdentityModule;

namespace Services.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        //Login ==> UserResultDto [DisplayName, Token, Email] ==> [Email, Password]
        Task<UserResultDto> LoginAsync(LoginDto loginDto);

        //Register ==> UserResultDto [DisplayName, Token, Email] ==> [Email, Password, PhoneNumber, UserName, DisplayName]
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
    }
}
