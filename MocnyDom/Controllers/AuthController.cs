using Microsoft.AspNetCore.Mvc;
using MocnyDom.Application.DTOs;
using MocnyDom.Application.Exceptions;
using MocnyDom.Application.Services;

namespace MocnyDom.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            
            try
            {
                return Ok(await _authService.Login(request));
            } catch(InvalidUsernameException ex)
            {
                return Unauthorized(ex.Message);
            } catch(InvalidPasswordException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                return Ok(await _authService.Register(request));
            } catch(RegistrationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
