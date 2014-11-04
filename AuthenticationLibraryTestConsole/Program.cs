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

            var authContext = new OpenIDConnectContext("*", "*", "oob://localhost/wpfclient");

            var result = authContext.RequestTokenAsync("data.read data.write sub.read", "token");

            var test = result.Result;
           
        }

      
    }
}
