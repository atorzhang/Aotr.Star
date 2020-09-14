using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_User")]
    [SqlSugar.SugarTable("Sys_User")]
    public class SysUser: EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true)]
        public string SysUserId { get; set; } 

        [Display(Name = "用户名")]
        [Required]
        [StringLength(100)]
        [SugarColumn(Length = 100, IsNullable = false)]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = false)]
        public string Password { get; set; }

        [Display(Name = "真实姓名")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string TrueName { get; set; }

        [Display(Name = "昵称")]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string NikeName { get; set; }

        [Display(Name = "手机号")]
        [StringLength(20)]
        [SugarColumn(Length = 20, IsNullable = true)]

        public string Mobile { get; set; }

        [Display(Name = "邮箱")]
        [EmailAddress]
        [StringLength(100)]
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Email { get; set; }

        [Display(Name = "QQ号")]
        [StringLength(200)]
        [SugarColumn(Length = 200, IsNullable = true)]
        public string QQ { get; set; }

        [Display(Name = "微信号")]
        [StringLength(200)]
        [SugarColumn(Length = 200, IsNullable = true)]
        public string WX { get; set; }

        [Display(Name = "头像")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string Avatar { get; set; }

        [Display(Name = "性别")]
        [StringLength(1)]
        [SugarColumn(Length = 1, IsNullable = true)]
        public string Sex { get; set; }

        [Display(Name = "用户类型")]
        [StringLength(1)]
        [SugarColumn(Length = 1, IsNullable = true)]
        public string UserType { get; set; }//0-前台用户，1-管理用户

    }
}
