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
            => throw new LoggerException("Logger " + name + " not registered.");

        public static void ThrowLoggerRegistered(string name)
            => throw new LoggerException("Logger " + name + " already registered.");
        
        public static LoggerException LoggerNotAsync(string name)
            => new LoggerException("Logger " + name + " not async.");

        public static void ThrowAsyncTargetsNotAllowed(string name)
            => throw new LoggerException($"{name} is synchronous and should not use async targets.");

        public static void ThrowRegistryConfigured()
            => throw new LoggerException("Registry is already configured.");

        public static LoggerException LevelThresholdNotSet()
            => new LoggerException("Level threshold is not set.");
    }
}
