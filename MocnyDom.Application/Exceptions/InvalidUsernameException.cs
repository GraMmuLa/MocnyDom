using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace MocnyDom.Application.Exceptions
{
    public class InvalidUsernameException : AuthenticationFailureException
    {
        public InvalidUsernameException(string username)
            : base($"Username {username} doesn't exist")
        {
        }
    }
}
