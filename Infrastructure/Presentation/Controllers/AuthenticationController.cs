using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManger _serviceManger):ApiController
    {
        //Post ==> Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> RegisterAsync(RegisterDto registerDto)
            => Ok(await _serviceManger.AuthenticationService.RegisterAsync(registerDto));

        //Post ==> Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> LoginAsync(LoginDto loginDto)
            => Ok(await _serviceManger.AuthenticationService.LoginAsync(loginDto));

        //Check Email Exist
        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExistAsync(string email)
            =>  Ok(await _serviceManger.AuthenticationService.CheckEmailExistAsync(email));

        //Get Current User
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentUserAsync()
        { 
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _serviceManger.AuthenticationService.GetCurrentUserAsync(email);
            return Ok(user);
        }

        //Get Address
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddressAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManger.AuthenticationService.GetUserAddressAsync(email);
            return Ok(address);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManger.AuthenticationService.UpdateUserAddressAsync(email, addressDto);
            return Ok(address);
        }





    }
}
