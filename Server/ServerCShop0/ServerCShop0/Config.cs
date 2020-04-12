using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

//Install-Package System.Text.Json -Version 4.7.1
//NuGet StackExchange.Redis
//NuGet MySql.Data

namespace Lib
{
    class Config
    {
        public static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public static readonly ConfigurationOptions _optionsRedis = new ConfigurationOptions()
        {
            EndPoints = { { "localhost", 6379 } },
            AllowAdmin = true,
            ConnectTimeout = 60 * 1000,
        };

        public static readonly string _optionsMySql = string.Format(
                "server={0};uid={1};pwd={2};database={3};charset=utf8 ;",
                "lsu3.cafe24.com", //url
                "lsu3",            //id
                "ghtmxld1",        //pw
                "lsu3"              //db
        );
    }
}
