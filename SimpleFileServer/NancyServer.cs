using System;
using Nancy.Hosting.Self;

namespace SimpleFileServer
{
    public class NancyServer
    {
        private NancyHost _host;

        public bool Start()
        {
            var ret = true;
            try
            {
                var hostConfigs = new HostConfiguration
                {
                    UrlReservations = new UrlReservations() { CreateAutomatically = true }
                };

                _host = new NancyHost(hostConfigs, new Uri("http://localhost:9000"));
                _host.Start();

                //The Nancy self host was unable to start, as no namespace reservation existed for the provided url(s).
                //    Please either enable UrlReservations.CreateAutomatically on the HostConfiguration provided to
                //the NancyHost, or create the reservations manually with the (elevated) command(s):
                //netsh http add urlacl url="http://+:9000/" user="Everyone"
            }
            catch (Exception exception)
            {
                ret = false;
                Console.WriteLine(exception);
            }

            return ret;
        }

        public void Stop()
        {
            _host?.Dispose();
            _host = null;
        }
    }
}
