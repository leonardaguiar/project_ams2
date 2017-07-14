using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://127.0.0.1:7070";

            using (WebApp.Start<Startup>(url: baseAddress)) {
                Console.WriteLine("Servico iniciado. Ouvindo a porta: 7070");
                System.Threading.Thread.Sleep(-1);
            }
        }
    }
}
