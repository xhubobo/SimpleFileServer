using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Nancy;
using Nancy.Responses.Negotiation;
using SimpleFileServer.Tools;

namespace SimpleFileServer.Modules
{
    public class UploadModule : NancyModule
    {
        public UploadModule()
        {
            Post["/FileList/Upload/{filePath}"] = OnUploadFile;
            Get["/FileList/Download/{fileName}"] = OnDownloadFile;
            Get["/FileList/Show"] = OnShow;
        }

        private Negotiator OnShow(dynamic o)
        {
            var folder = new DirectoryInfo(GlobalConfig.Instance.UploadDirectory);
            var fileList = new List<FileInfo>();
            foreach (var file in folder.GetFiles())
            {
                fileList.Add(new FileInfo()
                {
                    FileName = file.Name,
                    FilePath = file.FullName
                });
            }
            return View["Show", fileList];
        }

        private Response OnUploadFile(dynamic o)
        {
            if (!Directory.Exists(GlobalConfig.Instance.UploadDirectory))
            {
                try
                {
                    Directory.CreateDirectory(GlobalConfig.Instance.UploadDirectory);
                }
                catch (Exception e)
                {
                    LogHelper.AddLog(e);
                    return Response.AsText($"Internal error: {e.Message}");
                }
            }

            try
            {
                foreach (var file in Request.Files)
                {
                    var fileName = Path.Combine(GlobalConfig.Instance.UploadDirectory, file.Name);
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        file.Value.CopyTo(stream);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.AddLog(e);
            }

            return Response.AsRedirect("/FileList/GetInfo/{GlobalConfig.Instance.UploadDirectory}");
        }

        private Response OnDownloadFile(dynamic o)
        {
            var fileName = (string) o.fileName;
            var filePath = Path.Combine(GlobalConfig.Instance.UploadDirectory, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            return Response.AsFile(filePath);
        }
    }
}
