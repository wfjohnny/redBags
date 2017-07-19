using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Logger.Implements
{
    /// <summary>
    /// 以普通的文字流的方式写日志
    /// </summary>
    internal class NormalLogger : LoggerBase
    {
        static readonly object objLock = new object();
        protected override void InputLogger(string message)
        {
            string filePath = Path.Combine(FileUrl, DateTime.Now.ToString("yyyyMMdd") + ".log");

            if (!System.IO.Directory.Exists(FileUrl))
                System.IO.Directory.CreateDirectory(FileUrl);

            //写日志委托
            Action<string> write = (fileName) =>
            {
                lock (objLock)//防治多线程读写冲突
                {
                    using (FileStream srFile = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        var info = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}{1}{2}"
                            , DateTime.Now.ToString().PadRight(20)
                            , ("[ThreadID]").PadRight(14)
                            , message));
                        srFile.Write(info, 0, info.Length);
                    }
                }
            };

            try
            {
                write(filePath);
            }
            catch (Exception)
            {
                write(Path.Combine(FileUrl, DateTime.Now.ToUniversalTime() + Process.GetCurrentProcess().Id.ToString() + ".log"));
            }

        }

    }
}
