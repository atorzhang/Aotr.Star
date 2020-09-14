using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_RolePage")]
    [SqlSugar.SugarTable("Sys_RolePage")]
    public class SysRolePage:EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true)]
        public string SysRolePageId { get; set; }

        [Display(Name = "页面ID")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysPageId { get; set; }

        [Display(Name = "角色ID")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysRoleId { get; set; }

    }
}
