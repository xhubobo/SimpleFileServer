using System;
using System.Runtime.InteropServices;
using Nancy;
using System.IO;
using System.Text;

namespace SimpleFileServer.Modules
{
    public class FileListModule : NancyModule
    {
        public FileListModule()
        {
            Get["/FileList"] = OnGetDrivers;
            Get["/FileList/{folderPath}"] = OnGetFileList;
        }

        private string OnGetDrivers(dynamic o)
        {
            var info = new StringBuilder();

            var drivers = DriveInfo.GetDrives();
            foreach (var driver in drivers)
            {
                if (driver.DriveType == DriveType.Fixed || driver.DriveType == DriveType.Removable)
                {
                    info.Append($"{driver.Name}, {driver.DriveType}, " +
                        $"{driver.DriveFormat}, {driver.VolumeLabel}, {driver.RootDirectory}");
                }
                else
                {
                    info.Append($"{driver.Name}, {driver.DriveType}, {driver.RootDirectory}");
                }

                info.Append(Environment.NewLine);
            }

            var body = $"Driver list:{Environment.NewLine}{info.ToString()}";
            body = body.Replace(Environment.NewLine, "<br>");
            var charset = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";
            var html = "<!DOCTYPE html>\r\n" +
                "<html>\r\n" +
                $"<head>\r\n{charset}\r\n</head>\r\n" +
                $"<body>\r\n{body}\r\n</body>\r\n" +
                "</html>";
            html = Encoding.GetEncoding("GBK").GetString(Encoding.Default.GetBytes(html));
            return html;
        }

        private string OnGetFileList(dynamic o)
        {
            var folderPath = o.folderPath;
            return $"Get FileListModule: {folderPath}";
        }
    }
}
