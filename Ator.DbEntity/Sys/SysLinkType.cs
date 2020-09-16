using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_LinkType")]
    [SqlSugar.SugarTable("Sys_LinkType")]
    public class SysLinkType : EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true,Length = 32)]
        public string SysLinkTypeId { get; set; } 

        [Display(Name = "链接类型名称")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysLinkTypeName { get; set; }

        [Display(Name = "分组")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string Group { get; set; }

        [Display(Name = "Logo")]
        [StringLength(200)]
        [SugarColumn(Length = 200,IsNullable = true)]
        public string SysLinkTypeLogo { get; set; }


        [Display(Name = "是否不可删除")]
        public bool Unchangeable { get; set; } = false;
    }
}
