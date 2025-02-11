namespace UserManagementApp.UserApi.Models.Dtos
{
    public class AppUserDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public int Age { get; set; }
    }
}
