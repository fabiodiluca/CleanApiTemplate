using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.IoC
{
    public static class AddAutoMapperExtensions
    {
        public static void AddAutoMapperAndMaps(this IServiceCollection services)
        {

            services.AddAutoMapper(cfg =>
            {
                //Data
                cfg.AddMaps(Assemblies.Data);
                //Application
                cfg.AddMaps(Assemblies.Application);
            });
        }
    }
}
