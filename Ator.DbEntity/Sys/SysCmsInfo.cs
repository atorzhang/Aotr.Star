using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.DbEntity.Sys
{
    [Table("Sys_Cms_Info")]
    [SqlSugar.SugarTable("Sys_Cms_Info")]
    public class SysCmsInfo:EntityBase
    {
        [Key]
        [StringLength(32)]
        [SugarColumn(IsPrimaryKey = true,Length = 32)]
        public string SysCmsInfoId { get; set; }

        [Display(Name = "标题")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string InfoTitle { get; set; } = "";

        [Display(Name = "副标题")]
        [StringLength(255)]
        [SugarColumn(Length = 255, IsNullable = true)]
        public string InfoSecTitle { get; set; } = "";

        [Display(Name = "摘要")]
        [StringLength(500)]
        [SugarColumn(Length = 500, IsNullable = true)]
        public string InfoAbstract { get; set; } = "";

        [Display(Name = "信息类型")]
        [StringLength(20)]
        [SugarColumn(Length = 20, IsNullable = true)]
        public string InfoType { get; set; } = "1";//2-为前台显示文章信息，1为其他信息

        [Display(Name = "信息封面图")]
        [StringLength(10000)]
        [SugarColumn(Length = 10000, IsNullable = true)]
        public string InfoImage { get; set; } = "";

        [Display(Name = "信息来源")]
        [StringLength(50)]
        [SugarColumn(Length = 50, IsNullable = true)]
        public string InfoSource { get; set; } = "";

        [Display(Name = "信息作者")]
        [StringLength(50)]
        [SugarColumn(Length = 50, IsNullable = true)]
        public string InfoAuthor { get; set; } = "";

        [Display(Name = "信息内容")]
        [Column(TypeName = "longtext")]
        [SugarColumn(ColumnDataType = "longtext", IsNullable = true)]
        public string InfoContent { get; set; } = "";

        [SugarColumn(IsNullable = true)]
        public int? InfoTop { get; set; } = 0;

        [Display(Name = "信息标签")]
        [StringLength(200)]
        [SugarColumn(Length = 200, IsNullable = true)]
        public string InfoLable { get; set; } 

        [Display(Name = "点击数")]
        [SugarColumn(IsNullable = true)]
        public int? InfoClicks { get; set; } = 0;

        [Display(Name = "点赞数")]
        [SugarColumn(IsNullable = true)]
        public int? InfoGoods { get; set; } = 0;

        [Display(Name = "评论数")]
        [SugarColumn(IsNullable = true)]
        public int? InfoComments { get; set; } = 0;

        [Display(Name = "发布时间")]
        public DateTime? InfoPublishTime { get; set; } = DateTime.Now;

        [Display(Name = "编辑人")]
        [StringLength(50)]
        [SugarColumn(Length = 50, IsNullable = true)]
        public string InfoEditUser { get; set; }

        [Display(Name = "编辑时间")]
        public DateTime? InfoEditTime { get; set; }

        [Display(Name = "审核人")]
        [StringLength(50)]
        [SugarColumn(Length = 50, IsNullable = true)]
        public string InfoCheckUser { get; set; }

        [Display(Name = "审核时间")]
        public DateTime? InfoCheckTime { get; set; }

        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string SysCmsColumnId { get; set; }


        [Display(Name = "是否不可删除")]
        public bool Unchangeable { get; set; } = false;
    }
}
