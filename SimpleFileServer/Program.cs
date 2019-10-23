using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var nancyServer = new NancyServer();
            if (!nancyServer.Start())
            {
                Console.WriteLine("SimpleFileServer start failed.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("SimpleFileServer start.");
            Console.WriteLine("Press key 'q' to stop.");

            while (Console.ReadKey().KeyChar.ToString().ToUpper() != "Q")
            {
                Console.WriteLine();
            }

            nancyServer.Stop();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
