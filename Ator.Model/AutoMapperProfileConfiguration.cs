using Ator.DbEntity.Sys;
using Ator.Model.ViewModel.Sys;
using AutoMapper;

namespace Ator.Model
{
    public class AutoMapperProfileConfiguration : Profile
    {
        //构造函数里完成初始化绑定
        public AutoMapperProfileConfiguration()
        {
            //添加配置
            CreateMap<string, int>().ReverseMap();//ReverseMap反转映射
            CreateMap<SysUserSearchDto, SysUser>().ReverseMap();//ReverseMap反转映射
            CreateMap<SysRoleSearchDto, SysRole>().ReverseMap();//ReverseMap反转映射
            CreateMap<SysPageSearchDto, SysPage>().ReverseMap();//ReverseMap反转映射
            CreateMap<SysSettingSearchDto, SysSetting>().ReverseMap();//ReverseMap反转映射
            CreateMap<SysDictionarySearchDto, SysDictionary>().ReverseMap();//ReverseMap反转映射
            CreateMap<SysCmsInfoSearchDto, SysCmsInfo>().ReverseMap();//ReverseMap反转映射
        }
    }
}
