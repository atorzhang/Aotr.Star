using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Setting")]
    [SqlSugar.SugarTable("Sys_Setting")]
    public class SysSetting : EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true)]
        public string SysSettingId { get; set; } 

        [Display(Name = "设置名称")]
        [StringLength(100)]
        [SugarColumn(Length = 100, IsNullable = true)]
        public string SysSettingName { get; set; }

        [Display(Name = "设置分组")]
        [StringLength(100)]
        [SugarColumn(Length = 100, IsNullable = true)]
        public string SysSettingGroup { get; set; }

        [Display(Name = "设置类型")]
        [StringLength(100)]
        [SugarColumn(Length = 100, IsNullable = true)]
        public string SysSettingType { get; set; }

        [Display(Name = "设置值")]
        [StringLength(1000)]
        [SugarColumn(Length = 1000, IsNullable = true)]
        public string SetValue { get; set; }


        [Display(Name = "是否不可删除")]
        public bool Unchangeable { get; set; } = false;
    }
}
