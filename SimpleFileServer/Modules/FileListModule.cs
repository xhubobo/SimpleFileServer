using System;
using Nancy;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SimpleFileServer.Modules
{
    public class FileListModule : NancyModule
    {
        public FileListModule()
        { 
            Get["/FileList"] = OnGetDrivers;
            Get["/FileList/Fixed"] = OnGetFixedDrivers;
            Get["/FileList/Removable"] = OnGetRemovableDrivers;
            Get["/FileList/GetInfo/{folderPath}"] = OnGetFileList;
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

        private string OnGetRemovableDrivers(dynamic o)
        {
            return GetFixedOrRemovableDrivers(DriveType.Removable);
        }

        private string OnGetFixedDrivers(dynamic o)
        {
            return GetFixedOrRemovableDrivers(DriveType.Fixed);
        }

        private string OnGetFileList(dynamic o)
        {
            var jObject = new JObject();
            var jArrayFolder = new JArray();
            var jArrayFile = new JArray();

            var folderPath = o.folderPath;
            if (!Directory.Exists(folderPath))
            {
                jObject["FolderList"] = jArrayFolder.ToString();
                jObject["FileList"] = jArrayFile.ToString();
                return jObject.ToString();
            }

            var di = new DirectoryInfo(folderPath);
            foreach (var directoryInfo in di.GetDirectories())
            {
                jArrayFolder.Add(new JObject
                {
                    ["IsFolder"] = true,
                    ["Name"] = directoryInfo.Name,
                    ["FullName"] = directoryInfo.FullName,
                    ["Extension"] = directoryInfo.Extension,
                    ["CreationTime"] = directoryInfo.CreationTime,
                    ["LastWriteTime"] = directoryInfo.LastWriteTime,
                    ["LastAccessTime"] = directoryInfo.LastAccessTime,
                    ["Hidden"] = (directoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden
                });
            }

            foreach (var fileInfo in di.GetFiles())
            {
                jArrayFile.Add(new JObject
                {
                    ["IsFolder"] = false,
                    ["Name"] = fileInfo.Name,
                    ["FullName"] = fileInfo.FullName,
                    ["Extension"] = fileInfo.Extension,
                    ["CreationTime"] = fileInfo.CreationTime,
                    ["LastWriteTime"] = fileInfo.LastWriteTime,
                    ["LastAccessTime"] = fileInfo.LastAccessTime,
                    ["Hidden"] = (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden,
                    ["ReadOnly"] = (fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly
                });
            }

            jObject["FolderList"] = jArrayFolder.ToString();
            jObject["FileList"] = jArrayFile.ToString();
            return jObject.ToString();
        }

        private string GetFixedOrRemovableDrivers(DriveType driveType)
        {
            var jArray = new JArray();

            var drivers = DriveInfo.GetDrives();
            foreach (var driver in drivers)
            {
                if (driver.DriveType != driveType)
                {
                    continue;
                }

                jArray.Add(new JObject
                {
                    ["Name"] = driver.Name,
                    ["DriveType"] = (int)driver.DriveType,
                    ["DriveFormat"] = driver.DriveFormat,
                    ["VolumeLabel"] = driver.VolumeLabel,
                    ["RootDirectory"] = driver.RootDirectory.FullName
                });
            }

            return jArray.ToString();
        }
    }
}
