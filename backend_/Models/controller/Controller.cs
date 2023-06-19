using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_.Models.network;
using backend_.Models.controllerGroup;

namespace backend_.Models.controller
{

    public class UserControllerName
    {
        public string Name { get; set; }
        public string version { get; set; }
    }
    public class UserController
    {
        public int IpAddress { get; set; }
        public string? description { get; set; }

        public int IpPort { get; set; }
        public string ControllerState { get; set; }
        public string? Name { get; set; }
        public UserControllerName controllerName { get; set; }
    }



    [Table("microcontroller")]
    public class Controller
    {
        public Controller(UserController userController)
        {
            IpAddress = (UInt32)userController.IpAddress;
            if (userController.IpPort == null)
                IpPort = 9600;
            else
            {
                IpPort = (int)userController.IpPort;
            }
            description = userController.description;
            Name = userController.Name;
        }

        public Controller()
        {

        }



        [Column("mc_addres")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public UInt32 IpAddress { get; set; }

        [Column("mc_description")]
        public string? description { get; set; }

        [Column("mc_port")]
        public int IpPort { get; set; }
        [Column("mc_s_id")]
        public int ControllerStateId { get; set; }

        public ControllerState ControllerState { get; set; }

        [Column("mc_name")]
        public string? Name { get; set; }

        [Column("mcn_id")]
        public int? controllerNameId { get; set; }
        public ControllerName? controllerName { get; set; }

        public List<ControllerGroup>? controllerGroups { get; set; } = new List<ControllerGroup>();
        public List<m2mControllerControllerGroup> m2mControllerGroups { get; set; } = new List<m2mControllerControllerGroup>();

        public List<ControllerOutput> outputs { get; set; } = new List<ControllerOutput>();
    }

    [Table("microcontroller_state")]
    public class ControllerState
    {
        [Column("mc_s_id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("mc_s_discription")]
        public string Description { get; set; }

        public List<Controller>? controllers { get; set; }
    }

    [Table("microcontroller_name")]
    public class ControllerName
    {
        [Column("mcn_id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("mcn_name")]
        public string name { get; set; }

        [Column("mcn_version")]
        public string version { get; set; }

        public List<Controller>? controllers { get; set; }
    }
}
