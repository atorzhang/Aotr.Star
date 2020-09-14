using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Dictionary_Item")]
    [SqlSugar.SugarTable("Sys_Dictionary_Item")]
    public class SysDictionaryItem : EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true,Length = 32)]
        public string SysDictionaryItemId { get; set; } 

        [Display(Name = "字典ID")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysDictionaryId { get; set; }

        [Display(Name = "字典项值")]
        [StringLength(1000)]
        [SugarColumn(Length = 1000, IsNullable = true)]
        public string SysDictionaryItemValue { get; set; }

        [Display(Name = "字典项名称")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string SysDictionaryItemName { get; set; }

    }
}
