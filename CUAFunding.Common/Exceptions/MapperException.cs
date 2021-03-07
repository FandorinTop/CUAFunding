using CUAFunding.Common.Exceptions.Base;
using System;
using System.Collections.Generic;

namespace CUAFunding.Common.Exceptions
{
    public class MapperException : BaseException
    {
        protected List<KeyValuePair<string, string>> ExceptionsList = new List<KeyValuePair<string, string>>();

        public void AddValidationMistake(KeyValuePair<string, string> pair) {
            ExceptionsList.Add(pair);
        }

        public string ValidationMessage
        {
            get
            {
                var errorMessage = "";
                if (ExceptionsList.Count >= 1)
                {
                    foreach (var error in ExceptionsList)
                    {
                        errorMessage += $"Exception in {error.Key}, Error message {error.Value}\n";
                    }
                }

                return errorMessage;
            }
        }

        public int ValidationMistakeCount { get => ExceptionsList.Count;}

        public MapperException(string message) : base(message)
        {
        }

        public MapperException(string message, IEnumerable<Exception> errors) : base(message, errors)
        {
        }
    }
}
