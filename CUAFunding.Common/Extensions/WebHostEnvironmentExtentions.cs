using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace CUAFunding.Common.Extensions
{
    public static class WebHostEnvironmentExtentions
    {
        public static bool IsTesting(this IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Testing")
            {
                return true;
            }
            return false;
        }
    }
}
