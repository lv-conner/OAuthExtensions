using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Coco.Exntesion.FileLogger
{
    public class FileLoggerOption
    {
        public string LogPath { get; set; }
        public string LogFileExtension { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
