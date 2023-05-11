using backend_.Models.controller;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_.Models.UserModels;

namespace backend_.Models.controllerGroup
{



    [Table("m2m_mcg_ut")]
    public class m2mUserRoleControllerGroup
    {
        public UserRole1 userRole { get; set; }
        public int userRoleId { get; set; }

        public ControllerGroupUser controllerGroup { get; set; }
        public int controllerGroupId { get; set; }

    }

    [Table("m2m_mcog_ut")]
    public class m2mUserRoleControllerOutputGroup
    {
        public UserRole1 userRole { get; set; }
        public int userRoleId { get; set; }
        public ControllerOutputGroupUser controllerOutputGroup { get; set; }
        public int controllerOutputGroupID { get; set; }


    }

    [Table("users_role")]
    public class UserRole1
    {
        [Required]
        [Key]
        [Column("ut_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column("ut_description")]
        public string description { get; set; }

        public List<ControllerOutputGroupUser>? controllerOutputGroups { get; set; } = new List<ControllerOutputGroupUser>();
        public List<m2mUserRoleControllerOutputGroup>? m2mUserRoleControllerOutputGroups { get; set; } = new List<m2mUserRoleControllerOutputGroup>();
        public List<ControllerGroupUser>? controllerGroups { get; set; } = new List<ControllerGroupUser>();
        public List<m2mUserRoleControllerGroup>? m2mUserRoleControllerGroups { get; set; } = new List<m2mUserRoleControllerGroup>();

    }

    public class userRole
    {
        public int id { get; set; }
        public string description { get; set; }

        public userRole(UserRole1 role)
        {
            id = role.id;
            description = role.description;
        }
    }
    public class UserControllerGroup
    {
        public int id { get; set; }
        public string GroupDescription { get; set; }
    }

    public class UserControllerGroupWithRoles
    {
        public int id { get; set; }
        public string GroupDescription { get; set; }

        public List<userRole> userRoles { get; set; } = new List<userRole>();
    }

    [Table("microcontroller_output_group")]
    public class ControllerOutputGroupUser
    {
        [Required]
        [Column("mco_g_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("mco_g_description")]
        public string description { get; set; }

        public List<UserRole1> userRoles { get; set; } = new List<UserRole1>();

        public List<m2mUserRoleControllerOutputGroup> m2mUserRoleControllerOutputGroups = new List<m2mUserRoleControllerOutputGroup>();
    }

    [Table("microcontroller_group")]
    public class ControllerGroupUser
    {
        [Required]
        [Column("mc_g_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column("mc_g_description")]
        [Required]
        public string GroupDescription { get; set; }
        public List<UserRole1>? userRoles { get; set; } = new List<UserRole1>();
        public List<m2mUserRoleControllerGroup>? m2mUserRolesControllerGroups { get; set; } = new List<m2mUserRoleControllerGroup>();
    }


    [Table("microcontroller_group")]
    public class ControllerGroup
    {
        [Required]
        [Column("mc_g_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column("mc_g_description")]
        [Required]
        public string  GroupDescription { get; set; }
        public List<Controller>? controllers { get; set; } = new List<Controller>();
        public List<m2mControllerControllerGroup>? m2mControllers { get; set; } = new List<m2mControllerControllerGroup>();

    }

    [Table("m2m_mcg_mc")]
    public class m2mControllerControllerGroup
    {
        public ControllerGroup? ControllerGroup {get;set;}
        [Column("mc_g_id")]
        public int ControllerGroupID { get; set; }
        [Column("mc_addres")]
        public UInt32 ControllerID { get; set; }
        public Controller? Controller {get;set;}
    }

    [Table("microcontroller_output_group")]
    public class ControllerOutputGroup
    {
        [Required]
        [Column("mco_g_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("mco_g_description")]
        public string description { get; set; }
        
        public List<ControllerOutput> outputs { get; set; } = new List<ControllerOutput> ();
        public List<m2mControllerOutputGroup> m2mControllers { get; set; } = new List<m2mControllerOutputGroup>();
    }


    [Table("m2m_mco_mcog")]
    public class m2mControllerOutputGroup
    {

        [Column("mco_g_id")]
        public int controllerOutputGroupID { get; set; }
        [Column("id")]
        public int controllerOutputID { get; set; }
        [Column("mc_address")]
        public UInt32 controllerID { get; set; }

        public ControllerOutputGroup? controllerOutputGroup { get; set; }
        public ControllerOutput? controllerOutput { get; set; }
    }




}
