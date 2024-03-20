namespace TutorManager.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public required string? Password { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
