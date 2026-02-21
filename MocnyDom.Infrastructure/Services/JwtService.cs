using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MocnyDom.Application.Services;
using MocnyDom.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MocnyDom.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public JwtService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<Tuple<string, DateTime>> GenerateJwt(IEnumerable<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            DateTime expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["ExpiresHours"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new Tuple<string, DateTime>(new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
