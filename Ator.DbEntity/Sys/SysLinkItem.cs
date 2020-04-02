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
        [SugarColumn(IsPrimaryKey = true)]
        public string SysLinkItemId { get; set; } 

        [Display(Name = "链接类型ID")]
        [StringLength(32)]
        public string SysLinkTypeId { get; set; }

        [Display(Name = "链接名称")]
        [StringLength(32)]
        public string SysLinkName { get; set; }

        [Display(Name = "链接地址")]
        [StringLength(255)]
        public string SysLinkUrl { get; set; }

        [Display(Name = "链接分组")]
        [StringLength(255)]
        public string SysLinkGroup { get; set; }

        [Display(Name = "链接图片")]
        [StringLength(255)]
        public string SysLinkImg { get; set; }


    }
}
