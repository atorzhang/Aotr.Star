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
        [SugarColumn(IsPrimaryKey = true)]
        public string SysDictionaryItemId { get; set; } 

        [Display(Name = "字典ID")]
        [StringLength(32)]
        public string SysDictionaryId { get; set; }

        [Display(Name = "字典项值")]
        [StringLength(255)]
        public string SysDictionaryItemValue { get; set; }

        [Display(Name = "字典项名称")]
        [StringLength(32)]
        public string SysDictionaryItemName { get; set; }

    }
}
