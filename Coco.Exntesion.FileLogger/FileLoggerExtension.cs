using System;
using System.Collections.Generic;
using System.Text;
using Coco.Exntesion.FileLogger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FileLoggerExtension
    {
        public static IServiceCollection AddFileLogger(this IServiceCollection services,Action<FileLoggerOption> configure)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());
            services.Configure(configure);
            return services;
        }
        public static IServiceCollection AddFileLogger(this IServiceCollection services,IConfiguration configuration)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());
            services.Configure<FileLoggerOption>(configuration);
            return services;
        }
    }
}
