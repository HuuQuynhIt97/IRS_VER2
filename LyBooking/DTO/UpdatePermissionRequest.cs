using System.Collections.Generic;


namespace IRS.DTO
{
    public class UpdatePermissionRequest
    {
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
}
