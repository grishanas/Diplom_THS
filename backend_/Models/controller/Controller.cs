using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_.Models.network;

namespace backend_.Models.controller
{

    public class UserController
    {
        public int IpAddress { get; set; }
        public string? description { get; set; }

        public int IpPort { get; set; }
        public int? ControllerState { get; set; }
        public string? Name { get; set; }
        public int? controllerName { get; set; }
    }



    [Table("microcontroller")]
    public class Controller
    {
        public Controller(UserController userController)
        {
            IpAddress=(int)userController.IpAddress;
            if (userController.IpPort == null)
                IpPort = 9600;
            else
            {
                IpPort = (int)userController.IpPort;
            }
            description = userController.description;
            ControllerState = userController.ControllerState;
            Name = userController.Name;
            controllerName = userController.controllerName;
        }

        public Controller()
        {

        }



        [Column("mc_addres")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IpAddress { get; set; }

        [Column("mc_description")]
        public string? description { get; set; }

        [Column("mc_port")]
        public int IpPort { get; set; }

        [Column("mc_s_id")]
        public int? ControllerState { get; set; }

        [Column("mc_name")]
        public string? Name { get; set; }

        [Column("mcn_id")]
        public int? controllerName { get; set; }



    }

    [Table("microcontroller_state")]
    public class ControllerState
    {
        [Column("mc_s_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("mc_s_discription")]
        public string Description { get; set; }
    }

    [Table("microcontroller_name")]
    public class ControllerName
    {
        [Column("mcn_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public UInt32 id { get; set; }

        [Column("mcn_name")]
        public string name { get; set; }

        [Column("mcn_version")]
        public string version { get; set; }
    }
}
