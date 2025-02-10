namespace UserManagementApp.UserApi.Models
{
    public class AppUser
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MinLength(2), MaxLength(30)]
        public required string Name { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public int Age { get; set; }

    }

    public enum Gender : byte
    {
        Male = 0,
        Female = 1
    }
}
