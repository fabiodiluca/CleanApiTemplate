namespace CleanTemplate.Api.Settings.Persistence
{
    public class PersistenceSettings
    {
        public string Database { get; set; }
        public string ConnectionString { get; set; }
        public bool UpdateDatabaseOnStartup { get; set; }
        public bool ShowSql { get; set; }
    }
}
