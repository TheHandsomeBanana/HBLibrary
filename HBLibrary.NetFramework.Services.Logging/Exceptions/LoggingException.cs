using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.Logging.Exceptions {
    public class LoggingException : Exception {
        public LoggingException(string message) : base(message) {
        }

        public static void ThrowLoggerNotRegistered(string name)
            => throw new LoggingException("Logger " + name + " not registered.");

        public static void ThrowLoggerRegistered(string name)
            => throw new LoggingException("Logger " + name + " already registered.");
        
        public static LoggingException LoggerNotAsync(string name)
            => new LoggingException("Logger " + name + " not async.");

        public static void ThrowAsyncTargetsNotAllowed(string name)
            => throw new LoggingException($"{name} is synchronous and should not use async targets.");

        public static void ThrowRegistryConfigured()
            => throw new LoggingException("Registry is already configured.");

        public static LoggingException LevelThresholdNotSet()
            => new LoggingException("Level threshold is not set.");
    }
}
