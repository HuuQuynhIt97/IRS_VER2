
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.Models
{
    [Table("Permissions")]
    public class Permission
    {
        public Permission()
        {
        }

        public Permission(int roleID, int actionID, int functionSystemID)
        {
            RoleID = roleID;
            ActionID = actionID;
            FunctionSystemID = functionSystemID;
        }

        public int RoleID { get; set; }
        public int ActionID { get; set; }
        public int FunctionSystemID { get; set; }
        public FunctionSystem Functions { get; set; }
        public Role Role { get; set; }
        public Action Action { get; set; }
    }
}
