using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mono.Unix;
using Mono.Unix.Native;
using Nancy.Hosting.Self;

namespace Xena.Micro.ZipCodeService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var uri = "http://localhost:8900";
            using (var host = new NancyHost(new HostConfiguration {UrlReservations = new UrlReservations {CreateAutomatically = true}}, new Uri(uri)))
            {
                IEnumerable<string> errors;
                if (!new ZipcodeImporter().TryImport(out errors))
                {
                    Console.WriteLine("Import failed for the following reasons:");
                    Console.Write(string.Join(Environment.NewLine, errors));
                    Console.ReadKey();
                    return;
                }
                host.Start();
                if (Type.GetType("Mono.Runtime") != null)
                {
                    UnixSignal.WaitAny(new[]
                    {
                        new UnixSignal(Signum.SIGINT),
                        new UnixSignal(Signum.SIGTERM),
                        new UnixSignal(Signum.SIGQUIT),
                        new UnixSignal(Signum.SIGHUP)
                    });
                }
                else
                {
                    Console.WriteLine(string.Format("I'm a runnin' on {0}", uri));
                    Console.ReadKey();
                }
                host.Stop();
            }
        }
    }
}