using _80sModelCollector.Data;

namespace _80sModelCollector.Web
{
    public class AppSettings
    {
        public DatabaseSettings Database { get; set; }
        public class DatabaseSettings
        {
            public string ConnectionString { get; set; }
        }

    }
}