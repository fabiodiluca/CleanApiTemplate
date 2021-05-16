using CleanTemplate.Api.Settings.Persistence;
using CleanTemplate.Api.Swagger;

namespace CleanTemplate.Api.Settings
{
    public class AppSettings
    {
        public Api Api { get; set; }
        public PersistenceSettings Persistence { get; set; }
        public SwaggerUIConfiguration SwaggerUI { get; set; }
    }
}
