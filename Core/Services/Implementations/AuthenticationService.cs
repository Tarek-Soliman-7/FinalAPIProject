namespace Services.Implementations
{
    internal class AuthenticationService(UserManager<User> _userManager,IOptions<JwtOptions> _options, IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailExistAsync(string userEmail)
        {
            var user =await _userManager.FindByEmailAsync(userEmail);
            return user != null;
        }

        public async Task<UserResultDto> GetCurrentUserAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail)
                ?? throw new UserNotFoundException(userEmail);
            return new UserResultDto(user.DisplayName,await CreateTokenAsync(user),user.Email);

        }

        public async Task<AddressDto> GetUserAddressAsync(string userEmail)
        {
            //var user = await _userManager.FindByEmailAsync(userEmail);XXX
            var user = await _userManager.Users.Include(user=>user.Address)
                .FirstOrDefaultAsync(u=>u.Email==userEmail)
                ?? throw new UserNotFoundException(userEmail);
            return _mapper.Map<AddressDto>(user.Address);
        }
        public async Task<AddressDto> UpdateUserAddressAsync(string userEmail, AddressDto addressDto)
        {
            var user = await _userManager.Users.Include(user => user.Address)
                .FirstOrDefaultAsync(u => u.Email == userEmail)
                ?? throw new UserNotFoundException(userEmail);
            if (user.Address != null) //Update
            {
                user.Address.FirstName= addressDto.FirstName;
                user.Address.LastName= addressDto.LastName;
                user.Address.Country= addressDto.Country;
                user.Address.City= addressDto.City;
                user.Address.Street= addressDto.Street;
            }
            else //Create
            {
                var address= _mapper.Map<Address>(addressDto);
                user.Address = address;
            }
            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            //Email already exist [exist==acc]
            var user= await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnauthorizedException();
            //Check password 
            var result=await _userManager.CheckPasswordAsync(user,loginDto.Password);
            if(!result) throw new UnauthorizedException();
            return new UserResultDto(user.DisplayName,await CreateTokenAsync(user),user.Email);
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

            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
        }


        //Token ==> Encrypted String ==> function return string
        //Helper Method
        private async Task<string> CreateTokenAsync(User user)
        {
            var jwtOptions = _options.Value;   
            //Claims
            //Name , Email , Roles [m - m]
            var claims=new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.DisplayName),
                new Claim (ClaimTypes.Email,user.Email),
            };
            var roles=await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
            //Secret Key ==> SymmetricSecurityKey
            //UkRuJskDFDEEBjCZZySD1_b4nndLyIpSOAnEf8uh0fscN1XPXdmmrBPLgJay7J-M-ouqALJMmTMpr3KCDHulsw
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            //Algorithm [Algorithm + key] 
            var signInCreds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: jwtOptions.Issure,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(jwtOptions.ExpirationInDays),
                signingCredentials: signInCreds);
            //WriteToken [Obj Member Method] ==> JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
