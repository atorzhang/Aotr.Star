using Ator.DbEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysPageSearchViewModel : PagingViewModel
    {
        [Display(Name = "页面名称")]
        [StringLength(50)]
        public string SysPageName { get; set; }

        [Display(Name = "页面编号")]
        [StringLength(50)]
        public string SysPageNum { get; set; }

        public dynamic ReturnData { get; set; }
    }

    public class SysPageSearchDto : EntityBase
    {
        [Key]
        [StringLength(32)]
        public string SysPageId { get; set; }

        [Display(Name = "页面名称")]
        [StringLength(50)]
        public string SysPageName { get; set; }

        [Display(Name = "页面编号")]
        [StringLength(50)]
        public string SysPageNum { get; set; }

        [Display(Name = "页面地址")]
        [StringLength(255)]
        public string SysPageUrl { get; set; }

        [Display(Name = "页面图标")]
        [StringLength(255)]
        public string SysPageImg { get; set; }

        [Display(Name = "父级页面")]
        [StringLength(32)]
        public string SysPageParent { get; set; }
    }
}
