using Microsoft.AspNetCore.Mvc;
using MocnyDom.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MocnyDom.Application.Services
{
    public interface IAuthService
    {
        public Task<AuthResponseDto> Login(LoginRequestDto requestDto);
        public Task<AuthResponseDto> Register(RegisterRequestDto requestDto);
    }
}
