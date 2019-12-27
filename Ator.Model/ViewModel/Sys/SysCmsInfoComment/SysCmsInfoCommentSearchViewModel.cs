using Ator.DbEntity.Sys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysCmsInfoCommentSearchViewModel : PagingViewModel
    {
        [StringLength(32)]
        public string SysCmsInfoId { get; set; }

        [StringLength(50)]
        public string SysUserId { get; set; }

        /// <summary>
        /// 栏目编码
        /// </summary>
        [Display(Name = "栏目编码")]
        [StringLength(50)]
        public string SysCmsColumnId { get; set; }

        public dynamic ReturnData { get; set; }

    }
}
