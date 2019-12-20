using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Operate_Record")]
    [SqlSugar.SugarTable("Sys_Operate_Record")]
    public class SysOperateRecord: EntityDb
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string SysOperateRecordId { get; set; } 

        /// <summary>
        /// 操作修改表
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string TableName { get; set; }

        /// <summary>
        /// 操作方法所在类
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ClassName { get; set; }

        /// <summary>
        /// 操作方法名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MethodName { get; set; }

        [Display(Name = "操作人Id")]
        [StringLength(32)]
        public string SysUserId { get; set; } = "";

        /// <summary>
        /// 操作人姓名
        /// </summary>
        [Display(Name = "操作人用户名")]
        [StringLength(50)]
        public string UserName { get; set; } = "";

        /// <summary>
        /// 操作时间
        /// </summary>
        [Display(Name = "操作时间")]
        public DateTime? CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 操作类型
        /// 1：添加；2：删除，3-修改，4-查询,5-登录，6-注册
        /// </summary>
        [Required]
        public short Type { get; set; }

        /// <summary>
        /// 操作结果
        /// 1：成功；2：失败
        /// </summary>
        [Required]
        [Display(Name = "操作结果")]
        public short Result { get; set; } = 1;

        /// <summary>
        /// 所做操作
        /// </summary>
        [Required]
        [Display(Name = "操作内容")]
        [MaxLength(255)]
        public string Operate { get; set; }

    }
}
