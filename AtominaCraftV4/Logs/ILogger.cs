namespace AtominaCraftV4.Logs {
    public interface ILogger {
        void Info(string msg);
        void Warn(string msg);
        void Error(string msg);
        void Fatal(string msg);
    }
}