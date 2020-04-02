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
        [SugarColumn(IsPrimaryKey = true)]
        public string SysCmsColumnId { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [Display(Name = "栏目名称")]
        [StringLength(50)]
        public string ColumnName { get; set; }

        /// <summary>
        /// 栏目logo
        /// </summary>
        [Display(Name = "栏目logo")]
        [StringLength(255)]
        public string ColumnLogo { get; set; }

        /// <summary>
        /// 栏目说明
        /// </summary>
        [Display(Name = "栏目说明")]
        [StringLength(1000)]
        public string ColumnDescript { get; set; }

        /// <summary>
        /// 父级栏目编码
        /// </summary>
        [Display(Name = "父级栏目编码")]
        [StringLength(32)]
        public string ColumnParent { get; set; } = "";

    }
}
