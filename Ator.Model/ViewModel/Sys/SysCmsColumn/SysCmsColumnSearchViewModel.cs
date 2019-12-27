using System.ComponentModel.DataAnnotations;

namespace Ator.Model.ViewModel.Sys
{
    public class SysCmsColumnSearchViewModel : PagingViewModel
    {
        [Key]
        [StringLength(32)]
        public string SysCmsColumnId { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [Display(Name = "栏目名称")]
        [StringLength(50)]
        public string ColumnName { get; set; }


        public dynamic ReturnData { get; set; }
    }

}
