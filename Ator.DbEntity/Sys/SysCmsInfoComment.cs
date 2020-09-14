using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Cms_InfoComment")]
    [SqlSugar.SugarTable("Sys_Cms_InfoComment")]
    public class SysCmsInfoComment : EntityDb
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true,Length = 32)]
        public string SysCmsInfoCommentId { get; set; }

        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string ToCommentId { get; set; }

        [Display(Name = "评论人")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysUserId { get; set; } = "";

        [Display(Name = "评论目标用户")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string ToUserId { get; set; }

        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysCmsInfoId { get; set; }

        [StringLength(2000)]
        [Display(Name = "评论内容")]
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Comment { get; set; }

        [Display(Name = "评论时间")]
        public DateTime? CommentTime { get; set; }

        [Display(Name = "用户标签")]//比如博主1,其他暂定
        [SugarColumn(Length = 20, IsNullable = true)]
        public string  UserLable { get; set; }

        [StringLength(100)]
        [Display(Name = "Ip")]
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Ip { get; set; }

        [StringLength(100)]
        [Display(Name = "地址")]
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Address { get; set; }

        [StringLength(50)]
        [Display(Name = "栏目编码")]
        [SugarColumn(Length = 50, IsNullable = true)]
        public string SysCmsColumnId { get; set; }
        
        /// <summary>
        /// 状态0-删除，1-正常，2-禁用，3-待审核
        /// </summary>
        [Display(Name = "状态0-逻辑删除，1-正常，2-禁用,...")]
        [SugarColumn(IsNullable = true)]
        public virtual int? Status { get; set; } = 2;

    }
}
