using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_LinkItem")]
    [SqlSugar.SugarTable("Sys_LinkItem")]
    public class SysLinkItem : EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true,Length = 32)]
        public string SysLinkItemId { get; set; } 

        [Display(Name = "链接类型ID")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysLinkTypeId { get; set; }

        [Display(Name = "链接名称")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string SysLinkName { get; set; }

        [Display(Name = "链接地址")]
        [StringLength(500)]
        [SugarColumn(Length = 500, IsNullable = true)]
        public string SysLinkUrl { get; set; }

        [Display(Name = "链接分组")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string SysLinkGroup { get; set; }

        [Display(Name = "链接图片")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string SysLinkImg { get; set; }


    }
}
