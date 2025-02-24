using System.ComponentModel.DataAnnotations.Schema;

namespace IkariDoTrainingBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [Column("password_hash")]
        public string PasswordHash { get; set; }
    }
}
