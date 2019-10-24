using System;
using Nancy;

namespace SimpleFileServer.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = r =>
            {
                var os = Environment.OSVersion;
                return "SimpleFileServer implemented by Nancy<br/> System:" + os.VersionString;
            };
            // 
            //             Get["/mvc/{controller}/{action}/{id}"] = r =>
            //             {
            //                 var mvc = new StringBuilder();
            //                 mvc.AppendLine("controller :" + r.controller + "<br/>");
            //                 mvc.AppendLine("action :" + r.action + "<br/>");
            //                 mvc.AppendLine("id :" + r.id + "<br/>");
            //                 return mvc.ToString();
            //             };
        }
    }
}
