using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Page")]
    [SqlSugar.SugarTable("Sys_Page")]
    public class SysPage:EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true)]
        public string SysPageId { get; set; }

        [Display(Name ="页面名称")]
        [StringLength(50)]
        [SugarColumn(Length = 50, IsNullable = true)]
        public string SysPageName { get; set; }

        [Display(Name = "页面编号")]
        [StringLength(50)]
        [SugarColumn(Length = 50, IsNullable = true)]
        public string SysPageNum { get; set; }

        [Display(Name = "页面地址")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string SysPageUrl { get; set; }

        [Display(Name = "页面地址1")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string SysPageTargetUrl { get; set; }

        
        [Display(Name = "页面图标")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string SysPageImg { get; set; }

        [Display(Name = "父级页面")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysPageParent { get; set; }

    }
}
