namespace CleanTemplate.Api.Settings
{
    public class Api
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public string MajorVersion { 
            get 
            {
                return Version.Substring(0, Version.IndexOf('.'));
            } 
        }
    }
}
