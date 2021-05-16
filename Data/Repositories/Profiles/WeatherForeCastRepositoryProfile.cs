using AutoMapper;
using CleanTemplate.Data.Model;
using CleanTemplate.Domain;

namespace CleanTemplate.Data.Repositories.Profiles
{
    public class WeatherForeCastRepositoryProfile: Profile
    {
        public WeatherForeCastRepositoryProfile()
        {
            CreateMap<WeatherForeCastDataModel, WeatherForeCast>();
            CreateMap<WeatherForeCast, WeatherForeCastDataModel>();
        }
    }
}
