using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Cms_InfoGood")]
    [SqlSugar.SugarTable("Sys_Cms_InfoGood")]
    public class SysCmsInfoGood : EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true)]
        public string SysCmsInfoGoodId { get; set; } 

        [Display(Name = "点赞人")]
        [StringLength(32)]
        public string SysUserId { get; set; } = "";

        [StringLength(32)]
        public string SysCmsInfoId { get; set; }="";

        [StringLength(100)]
        public string Ip { get; set; }


    }
}
