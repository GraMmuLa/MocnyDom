using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace MocnyDom.Application.Exceptions
{
    public class InvalidPasswordException : AuthenticationFailureException
    {
        public InvalidPasswordException()
            : base("Invalid password")
        {
        }
    }
}
