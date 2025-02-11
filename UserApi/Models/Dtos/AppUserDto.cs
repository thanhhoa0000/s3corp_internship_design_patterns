namespace UserManagementApp.UserApi.Models.Dtos
{
    public class AppUserDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
    }
}
