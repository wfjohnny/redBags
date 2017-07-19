using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ISoftSmart.API.Providers
{
    /// <summary>
    /// Create xulong.ji
    /// Date:20170516
    /// DES:提供工具帮助
    /// </summary>
    public class ToolsProvider
    {
        private static readonly ILog _log = LogManager.GetLogger("TypeConvert");
        public readonly static string AudioToolsPath = ConfigurationManager.AppSettings["AudioToolsPath"];
        /// <summary>
        /// 音频转换
        /// </summary>
        /// <param name="sourceFullPath">原文件绝对路径</param>
        /// <param name="targetFullPath">目标文件绝对路径</param>
        /// <returns>状态标识</returns>
        public static bool AudioConvert(string sourceFullPath, string targetFullPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(AudioToolsPath))
                {
                    string cmd = string.Format("{0} -i {1}  {2}", HttpContext.Current.Server.MapPath(AudioToolsPath), sourceFullPath, targetFullPath);

                    ProcessStartInfo info = new ProcessStartInfo("cmd.exe");
                    info.RedirectStandardOutput = false;
                    info.UseShellExecute = false;
                    Process p = Process.Start(info);
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.Start();
                    p.StandardInput.WriteLine(cmd);
                    p.StandardInput.AutoFlush = true;
                    p.StandardInput.WriteLine("exit");
                    p.WaitForExit();

                    if (!string.IsNullOrEmpty(p.StandardOutput.ReadToEnd()))
                    {
                        p.Close();
                        return true;
                    }
                }
                else
                {
                    _log.Info("没有找到工具的配置信息");
                }

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
            return false;
        }
    }
}