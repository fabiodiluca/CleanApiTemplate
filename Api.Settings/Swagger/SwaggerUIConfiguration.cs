namespace CleanTemplate.Api.Swagger
{
    public class SwaggerUIConfiguration
    {
        public const string EndPointVersionTag = "{version}";
        public string Endpoint { get; set; }

        /// <summary>
        /// Expecting: x.x.x = major.minor.patch. Example: 1.2.22
        /// </summary>
        /// <param name="version"></param>
        /// <returns>v1 vMajor</returns>
        public string FormatEndpoint(string version)
        {
            return Endpoint.Replace(
                EndPointVersionTag,
                "v" + version.Substring(0, version.IndexOf('.'))
            );
        }
    }
}
