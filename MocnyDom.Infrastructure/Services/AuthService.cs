using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MocnyDom.Application.DTOs;
using MocnyDom.Application.Exceptions;
using MocnyDom.Application.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MocnyDom.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;

        public AuthService(UserManager<IdentityUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Login(LoginRequestDto requestDto)
        {
            ArgumentNullException.ThrowIfNull(requestDto);
            var user = await _userManager.FindByNameAsync(requestDto.Username) ??
                throw new InvalidUsernameException(requestDto.Username);

            if (!await _userManager.CheckPasswordAsync(user, requestDto.Password))
                throw new InvalidPasswordException();

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = await _jwtService.GenerateJwt(claims);

            return new AuthResponseDto
            {
                Username = user.UserName!,
                Roles = roles,
                Token = token.Item1,
                Expires = token.Item2
            };
        }

        public async Task<AuthResponseDto> Register(RegisterRequestDto requestDto)
        {
            var user = new IdentityUser
            {
                UserName = requestDto.Username,
                Email = requestDto.Email
            };

            var result = await _userManager.CreateAsync(user, requestDto.Password);

            if (!result.Succeeded)
                throw new RegistrationFailedException("Registration failed: "+string.Join(",",result.Errors));

            await _userManager.AddToRoleAsync(user, "User");

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            Tuple<string, DateTime> jwtResult = await _jwtService.GenerateJwt(claims);

            return new AuthResponseDto
            {
                Username = user.UserName!,
                Roles = new List<string> { "User" },
                Token = jwtResult.Item1,
                Expires = jwtResult.Item2
            };
        }
    }
}
