using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorManager.Models
{
    public class UserModel
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }

        [NotMapped]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        public string FullName()
        {
            return this.FirstName + " " + this.LastName;
        }
    }
}
