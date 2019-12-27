using Ator.Model.Api.TimeLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.IService
{
    public interface ISysCmsInfoService
    {
        List<KeyValuePair<string, string>> GetInfoList();

        List<YearData> GetTimeLineData();
    }
}
