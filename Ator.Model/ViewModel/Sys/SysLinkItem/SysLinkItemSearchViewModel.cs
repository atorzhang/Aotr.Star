
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysLinkItemSearchViewModel : PagingViewModel
    {
        [Key]
        [StringLength(32)]
        public string SysLinkItemId { get; set; }

        [Display(Name = "链接类型ID")]
        [StringLength(32)]
        public string SysLinkTypeId { get; set; }

        [Display(Name = "链接名称")]
        [StringLength(32)]
        public string SysLinkName { get; set; }

        [Display(Name = "链接分组")]
        [StringLength(255)]
        public string SysLinkGroup { get; set; }

        public dynamic ReturnData { get; set; }
    }

}
