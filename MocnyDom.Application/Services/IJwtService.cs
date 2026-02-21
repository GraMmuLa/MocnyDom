using MocnyDom.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MocnyDom.Application.Services
{
    public interface IJwtService
    {
        public abstract Task<Tuple<string, DateTime>> GenerateJwt(IEnumerable<Claim> claims);
    }
}
