using System;
using Nancy;

namespace SimpleFileServer.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            //路由规则
            //             Get["/blog/{name}"] = r =>
            //             {
            //                 return "blog name " + r.name;
            //             };
            // 
            Get["/"] = r =>
            {
                var os = Environment.OSVersion;
                return "Hello Nancy<br/> System:" + os.VersionString;
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
