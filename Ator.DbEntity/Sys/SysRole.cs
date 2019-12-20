using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Role")]
    [SqlSugar.SugarTable("Sys_Role")]
    public class SysRole: EntityBase
    {
        [Key]
        [StringLength(32)]
        public string SysRoleId { get; set; } 

        [StringLength(32)]
        [Display(Name = "权限名称")]
        public string RoleName { get; set; }

    }
}
