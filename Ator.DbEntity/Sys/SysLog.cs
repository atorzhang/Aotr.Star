using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Log")]
    [SqlSugar.SugarTable("Sys_Log")]
    public class SysLog : EntityDb
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [SugarColumn(IsPrimaryKey = true,IsIdentity = true)]
        public int SysLogId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Required]
        public DateTime LogDate { get; set; }

        /// <summary>
        /// 操作等级
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string LogLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 400, IsNullable = true)]
        public string LogType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 4000, IsNullable = true)]
        public string Logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 5000, IsNullable = true)]
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string MachineName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string MachineIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string NetRequestMethod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string NetRequestUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string NetUserIsauthenticated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string NetUserAuthtype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string NetUserIdentity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 5000, IsNullable = true)]
        public string Exception { get; set; }
    }
}
