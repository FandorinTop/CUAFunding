using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.Common.Exceptions.Base
{
    public class BaseException : Exception
    {
        public BaseException(string message) : base(message)
        {
        }
        public IEnumerable<Exception> Errors { get; }
        public BaseException(string message, IEnumerable<Exception> errors) : base(message)
        {
            Errors = errors;
        }
    }
}
