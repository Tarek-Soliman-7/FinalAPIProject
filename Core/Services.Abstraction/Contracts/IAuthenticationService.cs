using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;

namespace Services.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        //Login ==> UserResultDto [DisplayName, Token, Email] ==> [Email, Password]
        Task<UserResultDto> LoginAsync(LoginDto loginDto);

        //Register ==> UserResultDto [DisplayName, Token, Email] ==> [Email, Password, PhoneNumber, UserName, DisplayName]
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);

        //Get Current User
        Task<UserResultDto> GetCurrentUserAsync(string userEmail);

        //Check If Email Exist
        Task<bool> CheckEmailExistAsync(string userEmail);

        //Get Address
        Task<AddressDto> GetUserAddressAsync(string userEmail);

        //Update Address
        Task<AddressDto> UpdateUserAddressAsync(string userEmail, AddressDto addressDto);
    }
}
