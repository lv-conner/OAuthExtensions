using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using System.IO;

namespace Coco.Exntesion.FileLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ILogger> _cache;
        private FileLoggerOption _fileLoggerOption;
        private LogLevel _logLevel;
        public FileLoggerProvider(IOptionsMonitor<FileLoggerOption> options)
        {
            _cache = new ConcurrentDictionary<string, ILogger>();
            _fileLoggerOption = options.CurrentValue;
            options.OnChange(OnOptionsChange);
            _logLevel = options.CurrentValue.LogLevel;
        }

        private void OnOptionsChange(FileLoggerOption fileLoggerOption)
        {
            _fileLoggerOption = fileLoggerOption;
            _cache.Clear();
        }
        public ILogger CreateLogger(string categoryName)
        {
            return _cache.GetOrAdd(categoryName, LoggerFactory);
        }

        public void Dispose()
        {
            
        }
        private ILogger LoggerFactory(string categoryName)
        {
            return new FileLogger(_fileLoggerOption,categoryName);
        }

    }
}
