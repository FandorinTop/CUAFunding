using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.Configurations
{
    public static class StringConfiguration
    {
        private static IConfiguration AppCofiguration { get;}

        public static string ConnectionString {  get; private set; }

        static StringConfiguration()
        {
            AppCofiguration = new ConfigurationBuilder().AddJsonFile("../CUAFunding/appsettings.json").Build();
            ConnectionString = AppCofiguration.GetConnectionString("Testing");
        }
    }
}
