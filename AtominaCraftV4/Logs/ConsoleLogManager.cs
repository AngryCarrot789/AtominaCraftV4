using System;
using System.Collections.Generic;

namespace AtominaCraftV4.Logs {
    public static class ConsoleLogManager {
        private static Dictionary<string, ILogger> loggers = new Dictionary<string, ILogger>();

        public static ILogger GetLogger(string name = "AtominaCraft") {
            if (!loggers.TryGetValue(name, out ILogger logger)) {
                loggers[name] = logger = new ConsoleLogger();
            }

            return logger;
        }

        private class ConsoleLogger : ILogger {
            public void Info(string msg) {
                Console.WriteLine($"[{DateTime.Now.ToString("T")}] [INFO] {msg}\n");
            }

            public void Warn(string msg) {
                Console.WriteLine($"[{DateTime.Now.ToString("T")}] [WARNING] {msg}\n");
            }

            public void Error(string msg) {
                Console.WriteLine($"[{DateTime.Now.ToString("T")}] [ERROR] {msg}\n");
            }

            public void Fatal(string msg) {
                Console.WriteLine($"[{DateTime.Now.ToString("T")}] [FATAL] {msg}\n");
            }
        }
    }
}