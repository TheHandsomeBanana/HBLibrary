using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Exceptions {
    public class LoggerException : Exception {
        public LoggerException(string message) : base(message) {
        }

        public static void ThrowLoggerNotRegistered(string name)
            => throw new LoggerException("Logger " + name + " not registered");

        public static void ThrowLoggerRegistered(string name)
            => throw new LoggerException("Logger " + name + " already registered");

        public static LoggerException ConfigurationNotFound(string name)
            => new LoggerException("No configuration found for logger " + name);
        
        public static LoggerException LoggerNotAsync(string name)
            => new LoggerException("Logger " + name + " not async");
    }
}
