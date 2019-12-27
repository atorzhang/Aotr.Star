using Ator.DbEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysCmsInfoSearchViewModel : PagingViewModel
    {
        [Key]
        [StringLength(32)]
        public string SysCmsInfoId { get; set; }

        /// <summary>
        /// 信息标题
        /// </summary>
        [Display(Name = "信息标题")]
        [StringLength(50)]
        public string InfoTitle { get; set; }

        /// <summary>
        /// 栏目编码
        /// </summary>
        [Display(Name = "栏目编码")]
        [StringLength(50)]
        public string SysCmsColumnId { get; set; }

        public dynamic ReturnData { get; set; }

    }
    public class SysCmsInfoSearchDto : EntityBase
    {
        public string SysCmsInfoId { get; set; }

        [Display(Name = "标题")]
        [StringLength(255)]
        public string InfoTitle { get; set; } = "";

        [Display(Name = "封面图")]
        [StringLength(255)]
        public string InfoImage { get; set; } = "";

        [Display(Name = "栏目id")]
        public string SysCmsColumnId { get; set; } = "";

        [Display(Name = "摘要")]
        [StringLength(200)]
        public string InfoAbstract { get; set; } = "";

        [Display(Name = "标签")]
        [StringLength(255)]
        public string InfoLable { get; set; } = "";

        [Display(Name = "信息类型")]
        [StringLength(20)]
        public string InfoType { get; set; } = "1";

        [Display(Name = "信息来源")]
        [StringLength(50)]
        public string InfoSource { get; set; } = "";

        [Display(Name = "信息作者")]
        [StringLength(50)]
        public string InfoAuthor { get; set; } = "";

        [Display(Name = "点击数")]
        public int? InfoClicks { get; set; } = 0;

        [Display(Name = "点赞数")]
        public int? InfoGoods { get; set; } = 0;

        [Display(Name = "评论数")]
        public int? InfoComments { get; set; } = 0;

        [Display(Name = "置顶级别")]
        public int? InfoTop { get; set; } = 0;

        [Display(Name = "发布时间")]
        public DateTime? InfoPublishTime { get; set; } = DateTime.Now;
    }
}
