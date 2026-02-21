using System;
using System.Collections.Generic;
using System.Text;

namespace MocnyDom.Application.DTOs
{
    public class AuthResponseDto
    {
        public string Username { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
    }
}
