using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHttp;
using System.IO;

namespace DiagnosticServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Http.DiagnosticServer(8080);
            server.Listen();
            Console.WriteLine("Press <Enter> to shutdown the server...");
            Console.ReadLine();
            server.Shutdown();
        }
    }
}
