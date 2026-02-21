using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MocnyDom.Application.Exceptions
{
    public class RegistrationFailedException : AuthenticationFailureException
    {
        public RegistrationFailedException(string message) : base(message)
        {
        }
    }
}
