using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_UserRole")]
    [SqlSugar.SugarTable("Sys_UserRole")]
    public class SysUserRole:EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true)]
        public string SysUserRoleId { get; set; }
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysUserId { get; set; }
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysRoleId { get; set; }

    }
}
