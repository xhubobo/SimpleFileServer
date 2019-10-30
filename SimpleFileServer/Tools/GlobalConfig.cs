using System;
using System.IO;

namespace SimpleFileServer.Tools
{
    internal sealed class GlobalConfig
    {
        public string UploadDirectory { get; set; }

        public void Init()
        {
            var rootDir = Path.Combine(Environment.CurrentDirectory, "Content");
            if (!Directory.Exists(rootDir))
            {
                Directory.CreateDirectory(rootDir);
            }
            
            UploadDirectory = Path.Combine(rootDir, "uploads");
            if (!Directory.Exists(UploadDirectory))
            {
                Directory.CreateDirectory(UploadDirectory);
            }
        }

        #region 单例模式

        private static GlobalConfig _instance;

        private static readonly object LockInstanceHelper = new object();

        private GlobalConfig()
        {

        }

        public static GlobalConfig Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (LockInstanceHelper)
                {
                    _instance = _instance ?? new GlobalConfig();
                }

                return _instance;
            }
        }

        #endregion
    }
}
