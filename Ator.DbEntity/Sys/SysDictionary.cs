using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Dictionary")]
    [SqlSugar.SugarTable("Sys_Dictionary")]
    public class SysDictionary : EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true,Length = 32)]
        public string SysDictionaryId { get; set; } 

        [Display(Name = "字典名称")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysDictionaryName { get; set; }

        [Display(Name = "字典分组")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysDictionaryGroup { get; set; }


        [Display(Name = "是否不可删除")]
        public bool Unchangeable { get; set; } = false;
    }
}
