using System;
using System.Threading;
using SimpleFileServer.Tools;
using SimpleLogHelper;

namespace SimpleFileServer
{
    class Program
    {
        private static Mutex _instanceMutex;

        static void Main(string[] args)
        {
            LogHelper.InitLogPath();
            LogHelper.AddLog("Instance is starting.");

            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                _instanceMutex?.ReleaseMutex();
                _instanceMutex = null;
                LogHelper.AddLog("Instance is shutting down.");
            };

            CheckSingleInstance();
            AddThreadExceptionHandler();

            Run();
        }

        /// <summary>
        /// 单实例检测
        /// </summary>
        private static void CheckSingleInstance()
        {
            bool mutexWasCreated;
            _instanceMutex = new Mutex(true, Constants.ProductName, out mutexWasCreated);
            if (!mutexWasCreated)
            {
                LogHelper.AddLog("Another instance is running!", MsgType.Error);
                _instanceMutex = null;
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 异常捕获
        /// </summary>
        private static void AddThreadExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                LogHelper.AddLog($"未处理的UI异常：{(Exception) args.ExceptionObject}");
            };
        }

        /// <summary>
        /// 主线程消息循环
        /// </summary>
        private static void Run()
        {
            var nancyServer = new NancyServer();
            if (!nancyServer.Start())
            {
                LogHelper.AddLog("NancyServer start failed.");
                Console.WriteLine("SimpleFileServer start failed.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                return;
            }

            LogHelper.AddLog("NancyServer start.");
            Console.WriteLine("SimpleFileServer start.");
            Console.WriteLine("Press key 'q' to stop.");

            while (Console.ReadKey().KeyChar.ToString().ToUpper() != "Q")
            {
                Console.WriteLine();
            }

            nancyServer.Stop();

            LogHelper.AddLog("NancyServer stop.");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
