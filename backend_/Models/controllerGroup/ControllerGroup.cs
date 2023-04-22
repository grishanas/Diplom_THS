using backend_.Models.controller;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_.Models.UserModels;

namespace backend_.Models.controllerGroup
{
    [Table("microcontroller_group")]
    public class ControllerGroup
    {
        [Required]
        [Column("mc_g_id")]
        public int id { get; set; }
        [Column("mc_g_description")]
        [Required]
        public string  GroupDescription { get; set; }
        public List<UserRole> userRoles { get; set; } = new List<UserRole>();
        public List<m2mUserRoleControllerGroup> m2mUserRolesControllerGroups { get; set; } = new List<m2mUserRoleControllerGroup>();
    }

    [Table("m2m_mcg_mc")]
    public class m2mControllerControllerGroup
    {
        public ControllerGroup? ControllerGroup {get;set;}
        [Column("mc_g_id")]
        public int ControllerGroupID { get; set; }
        [Column("mc_addres")]
        public int ControllerID { get; set; }
        public Controller? Controller {get;set;}
    }

    [Table("microcontroller_output_group")]
    public class ControllerOutputGroup
    {
        [Required]
        [Column("mco_g_id")]
        public int id { get; set; }

        [Column("mco_g_description")]
        public string description { get; set; }

        public List<UserRole> userRoles { get; set; } = new List<UserRole>();
       
        public List<m2mUserRoleControllerOutputGroup> m2mUserRoleControllerOutputGroups = new List<m2mUserRoleControllerOutputGroup>();
        
    }

    [Table("m2m_mco_mcog")]
    public class m2mControllerOutputGroup
    {

        [Column("mco_g_id")]
        public int controllerOutputGroupID { get; set; }
        [Column("id")]
        public int controllerOutputID { get; set; }
        [Column("mc_address")]
        public int controllerID { get; set; }

        public ControllerOutputGroup? controllerOutputGroup { get; set; }
        public ControllerOutput? controllerOutput { get; set; }
    }




}
