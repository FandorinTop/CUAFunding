using CUAFunding.Common.Exceptions.Base;
using System;
using System.Collections.Generic;

namespace CUAFunding.Common.Exceptions
{
    public class DbException : BaseException
    {
        public DbException(string message) : base(message)
        {
        }

        public DbException(string message, IEnumerable<Exception> errors) : base(message, errors)
        {
        }
    }
}
