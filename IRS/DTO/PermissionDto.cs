namespace IRS.DTO
{
    public class PermissionDto
    {
        public int FunctionID { get; set; }

        public int RoleID { get; set; }

        public int ActionID { get; set; }
    }
    public class MenuDto
    {
        public string Module { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }
        public object Children { get; set; }
        public int Sequence { get; set; }
        public bool HasChildren { get; set; }
    }
}