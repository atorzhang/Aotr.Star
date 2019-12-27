using Ator.DbEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysRoleSearchViewModel : PagingViewModel
    {
        [StringLength(32)]
        [Display(Name = "权限名称")]
        public string RoleName { get; set; }

        public dynamic ReturnData { get; set; }
    }

    public class SysRoleSearchDto : EntityBase
    {
        [Key]
        [StringLength(32)]
        public string SysRoleId { get; set; }

        [StringLength(32)]
        [Display(Name = "权限名称")]
        public string RoleName { get; set; }
    }
}
