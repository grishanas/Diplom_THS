using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_.Models.controllerGroup;

namespace backend_.Models.UserModels
{


    [Table("users_role")]
    public class UserRole
    {
        [Required]
        [Key]
        [Column("ut_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column("ut_description")]
        public string description { get; set; }

        public List<User>? users { get; set; } = new List<User>();
        public List<m2mUserRole>? m2mUserRoles { get; set; } = new List<m2mUserRole>();

    }

    [Table("m2m_users_role")]
    public class m2mUserRole
    {
        public int idUserRole { get; set; }
        public UserRole userRole { get; set; }

        public int idUser { get; set; }
        public User user { get; set; }
    }
}
