using System;
using System.Collections.Generic;
using System.IO;
using Nancy;
using Nancy.Responses.Negotiation;
using SimpleFileServer.Tools;

namespace SimpleFileServer.Modules
{
    public class UploadModule : NancyModule
    {
        public UploadModule()
        {
            Post["/FileList/Upload"] = OnUploadFile;
            Get["/FileList/Upload"] = OnUploadView;
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

        private Negotiator OnUploadView(dynamic o)
        {
            return View["FileUpload", new FileInfo()];
        }

        private Negotiator OnUploadFile(dynamic o)
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
                    //return Response.AsText($"Internal error: {e.Message}");
                    return View["FileUpload", new FileInfo()];
                }
            }

            var fileInfo = new FileInfo();

            try
            {
                foreach (var file in Request.Files)
                {
                    var filePath = Path.Combine(GlobalConfig.Instance.UploadDirectory, file.Name);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    if (string.IsNullOrEmpty(fileInfo.FileName))
                    {
                        fileInfo.FileName = file.Name;
                        fileInfo.FilePath = filePath;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.Value.CopyTo(stream);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.AddLog(e);
            }

            

            return View["FileUpload", fileInfo];

            //return Response.AsRedirect("/FileList/GetInfo/{GlobalConfig.Instance.UploadDirectory}");
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
