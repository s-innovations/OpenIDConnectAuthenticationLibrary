using OpenIDConnectAuthenticationLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Thinktecture.Samples;

namespace TestConsole_for_Authentication_Library
{
    class Program
    {
        static void Main(string[] args)
        {
            //,"data.read data.write sub.read", "token"

            var authContext = new OpenIDConnectContext("https://identity-staging.cloudapp.net/connect/authorize", "6994A4A8-0E65-4FED-A82B-C684A0DD1758");

            var result = authContext.RequestTokenAsync("data.read data.write sub.read", "token");

            var test = result.Result;
           
        }

      
    }
}
