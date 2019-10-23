using System;
using System.Runtime.InteropServices;
using Nancy;

namespace SimpleFileServer.Modules
{
    public class FileListModule : NancyModule
    {
        public FileListModule()
        {
            Get["/FileList"] = r =>
            {
                var os = Environment.OSVersion;
                return "Hello FileListModule<br/> System:" + os.VersionString;
            };

            Get["/FileList/{folderPath}"] = OnGetFileList;
        }

        private string OnGetFileList(dynamic o)
        {
            var folderPath = o.folderPath;
            return $"Get FileListModule: {folderPath}";
        }
    }
}
