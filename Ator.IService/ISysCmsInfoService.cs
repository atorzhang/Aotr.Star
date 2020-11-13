using Ator.DbEntity.Sys;
using Ator.Model;
using Ator.Model.Api.TimeLine;
using Ator.Model.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ator.IService
{
    public interface ISysCmsInfoService
    {
        List<KeyValuePair<string, string>> GetInfoList();

        PageData<SysCmsInfoSearchDto> GetInfoPage(string SysCmsColumnId, string InfoTitle, string InfoType, string Status, string ordering, int Page, int Limit);

        Task<PageData<SysCmsInfoSearchDto>> GetInfoPageAsync(string SysCmsColumnId, string InfoTitle, string InfoType, string Status, string ordering, int Page, int Limit);

        List<YearData> GetTimeLineData();
    }
}
