using CUAFunding.Common.Exceptions.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.Common.Exceptions
{
    public class AuthorizationException : BaseException
    {
        public AuthorizationException(string message) : base(message)
        {
        }

        public AuthorizationException(string message, IEnumerable<Exception> errors) : base(message, errors)
        {
        }
    }
}
