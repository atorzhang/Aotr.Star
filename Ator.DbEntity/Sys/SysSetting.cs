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
        [StringLength(50)]
        public string SysSettingName { get; set; }

        [Display(Name = "设置分组")]
        [StringLength(50)]
        public string SysSettingGroup { get; set; }

        [Display(Name = "设置类型")]
        [StringLength(50)]
        public string SysSettingType { get; set; }

        [Display(Name = "设置值")]
        [StringLength(500)]
        public string SetValue { get; set; }

    }
}
