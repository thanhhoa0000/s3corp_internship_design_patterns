namespace UserManagementApp.UserApi
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<AppUser, AppUserDto>().ReverseMap();
        }
    }
}
