using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;

namespace Coco.Exntesion.FileLogger
{
    public class FileLogger : ILogger
    {

        private readonly string _filePath;
        private readonly LogLevel _logLevel;
        public FileLogger(FileLoggerOption options, string categoryName,LogLevel logLevel)
        {

            _filePath = GetFilePath(options, categoryName);
            _logLevel = logLevel;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        private string GetFilePath(FileLoggerOption fileLoggerOption, string categoryName)
        {
            if (!Directory.Exists(fileLoggerOption.LogPath))
            {
                Directory.CreateDirectory(fileLoggerOption.LogPath);
            }
            return Path.Combine(fileLoggerOption.LogPath, categoryName + fileLoggerOption.LogFileExtension);
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                var str = formatter(state, exception);
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    File.AppendAllLines(_filePath, new string[] { str, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Environment.NewLine }, Encoding.UTF8);
                    stopwatch.Stop();
                    Debug.WriteLine(stopwatch.ElapsedMilliseconds);
                }
                catch(IOException ioEx)
                {

                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
