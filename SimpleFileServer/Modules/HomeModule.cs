using System;
using Nancy;

namespace SimpleFileServer.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = OnMainPage;
        }

        private Response OnMainPage(dynamic o)
        {
            var info = $"SimpleFileServer implemented by Nancy.{Environment.NewLine}" +
                       $"MachineName: {Environment.MachineName}.{Environment.NewLine}" +
                       $"System: {Environment.OSVersion.VersionString}.";
            return Response.AsText(info);
        }
    }
}
