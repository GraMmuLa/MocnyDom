using System;
using System.Collections.Generic;
using System.Text;

namespace MocnyDom.Application.DTOs
{
    public class RegisterRequestDto
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
