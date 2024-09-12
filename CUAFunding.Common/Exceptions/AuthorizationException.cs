using CUAFunding.Common.Exceptions.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.Common.Exceptions
{
    public class AuthorizationException : BaseException
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();

        public void AddMessage(IEnumerable<string> value) {
            ErrorMessages.AddRange(value);
        }

        public AuthorizationException(string message) : base(message)
        {
        }

        public AuthorizationException(string message, IEnumerable<Exception> errors) : base(message, errors)
        {
        }
    }
}
