
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysDictionaryItemSearchViewModel : PagingViewModel
    {
        [Key]
        [StringLength(32)]
        public string SysDictionaryItemId { get; set; }

        [Display(Name = "字典ID")]
        [StringLength(32)]
        public string SysDictionaryId { get; set; }

        [Display(Name = "字典项名称")]
        [StringLength(32)]
        public string SysDictionaryItemName { get; set; }

        public dynamic ReturnData { get; set; }
    }

}
