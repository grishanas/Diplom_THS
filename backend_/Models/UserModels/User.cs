using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_.Models.UserModels
{
    public class UserLogin
    {
        public string login { get; set; }
        public string password { get; set; }
    }
    [Table("users")]
    public class User
    {
        [Key]
        [Required]
        [Column("u_id")]
        public int id { get; set; }

        [Column("u_password")]
        public string password { get; set; }
        [Column("u_login")]
        public string login { get; set; }

        public List<m2mUserRole>? m2mUserRoles { get; set; } = new List<m2mUserRole>(); 
        public List<UserRole>? userRoles { get; set; } = new List<UserRole>(); 
    }
}
