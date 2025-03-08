using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto1040.Core.Entities
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PermissionName { get; set; }

        public string Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
