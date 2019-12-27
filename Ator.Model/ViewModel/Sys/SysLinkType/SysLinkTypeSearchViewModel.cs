using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysLinkTypeSearchViewModel : PagingViewModel
    {
        [Key]
        [StringLength(32)]
        public string SysLinkTypeId { get; set; }

        [Display(Name = "链接类型名称")]
        [StringLength(32)]
        public string SysLinkTypeName { get; set; }

        [Display(Name = "分组")]
        [StringLength(32)]
        public string Group { get; set; }

        public dynamic ReturnData { get; set; }
    }

}
