using Ator.DbEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysSettingSearchViewModel : PagingViewModel
    {
        [Display(Name = "设置名称")]
        [StringLength(50)]
        public string SysSettingName { get; set; }

        [Display(Name = "设置分组")]
        [StringLength(50)]
        public string SysSettingGroup { get; set; }

        public dynamic ReturnData { get; set; }
    }

    public class SysSettingSearchDto : EntityBase
    {
        [Key]
        [StringLength(32)]
        public string SysSettingId { get; set; }

        [Display(Name = "设置名称")]
        [StringLength(50)]
        public string SysSettingName { get; set; }

        [Display(Name = "设置分组")]
        [StringLength(50)]
        public string SysSettingGroup { get; set; }

        [Display(Name = "设置类型")]
        [StringLength(50)]
        public string SysSettingType { get; set; }

        [Display(Name = "设置值")]
        [StringLength(500)]
        public string SetValue { get; set; }
    }
}
