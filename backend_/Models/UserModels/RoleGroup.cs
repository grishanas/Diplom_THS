using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_.Models.controllerGroup;


namespace backend_.Models.UserModels
{
    public class m2mUserRoleControllerGroup
    {
        public UserRole userRole { get; set; }
        public int userRoleId { get; set; }

        public ControllerGroup controllerGroup { get; set; }
        public int controllerGroupId { get; set; }

    }

    public class m2mUserRoleControllerOutputGroup
    {
        public UserRole userRole { get; set; }
        public int userRoleId { get; set; }
        public ControllerOutputGroup controllerOutputGroup { get; set; }
        public int controllerOutputGroupID { get; set; }


    }
}
