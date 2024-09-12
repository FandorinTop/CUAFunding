using CUAFunding.Common.Exceptions.Base;
using System;
using System.Collections.Generic;

namespace CUAFunding.Common.Exceptions
{
    public class UploadingException : BaseException
    {
        public UploadingException(string message) : base(message)
        {
        }

        public UploadingException(string message, IEnumerable<Exception> errors) : base(message, errors)
        {
        }
    }
}
