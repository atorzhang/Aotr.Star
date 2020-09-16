using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Cms_Column")]
    [SugarTable("Sys_Cms_Column")]
    public class SysCmsColumn : EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true,Length = 32)]
        public string SysCmsColumnId { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [Display(Name = "栏目名称")]
        [StringLength(100)]
        [SugarColumn(Length = 100,IsNullable = true)]
        public string ColumnName { get; set; }

        /// <summary>
        /// 栏目logo
        /// </summary>
        [Display(Name = "栏目logo")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string ColumnLogo { get; set; }

        /// <summary>
        /// 栏目说明
        /// </summary>
        [Display(Name = "栏目说明")]
        [StringLength(1000)]
        [SugarColumn(Length = 1000, IsNullable = true)]
        public string ColumnDescript { get; set; }

        /// <summary>
        /// 父级栏目编码
        /// </summary>
        [Display(Name = "父级栏目编码")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string ColumnParent { get; set; } = "";


        [Display(Name = "是否不可删除")]
        public bool Unchangeable { get; set; } = false;
    }
}
