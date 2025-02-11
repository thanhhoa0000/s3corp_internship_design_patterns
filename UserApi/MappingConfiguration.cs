using static Azure.Core.HttpHeader;

namespace UserManagementApp.UserApi
{
    public static class MappingConfiguration
    {
        public static MapperConfiguration ConfigureMapping()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<AppUser, AppUserDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
