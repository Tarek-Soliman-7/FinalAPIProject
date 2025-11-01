using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    internal class AuthenticationService(UserManager<User> _userManager) : IAuthenticationService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            //Email already exist [exist==acc]
            var user= await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnauthorizedException();
            //Check password 
            var result=await _userManager.CheckPasswordAsync(user,loginDto.Password);
            if(!result) throw new UnauthorizedException();
            return new UserResultDto(user.DisplayName,"Token",user.Email);
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user= new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
           var result= await _userManager.CreateAsync(user,registerDto.Password);
            //Vaidate
            if(!result.Succeeded)
            {
                var errors=result.Errors.Select(e=>e.Description).ToList();
                throw new ValidtionException(errors);
            }

            return new UserResultDto(user.DisplayName, "Token", user.Email);
        }
    }
}
