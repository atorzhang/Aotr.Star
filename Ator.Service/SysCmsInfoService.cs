using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.IService;
using Ator.Model;
using Ator.Model.Api.TimeLine;
using Ator.Model.ViewModel.Sys;
using Ator.Repository;
using Ator.Repository.Sys;
using Ator.Utility.Ext;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ator.Service
{
    public class SysCmsInfoService : Repository<DbFactory, SysCmsInfoRepository>, ISysCmsInfoService
    {
        public SysCmsInfoService(DbFactory factory) : base(factory)
        {
        }

        public List<KeyValuePair<string, string>> GetInfoList()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            var all = DbContext.GetList<SysCmsInfo>($"{nameof(SysCmsInfo.InfoTop)} desc,{nameof(SysCmsInfo.InfoPublishTime)} asc,{nameof(SysCmsInfo.InfoPublishTime)} desc");
            foreach (var item in all)
            {
                data.Add(new KeyValuePair<string, string>(item.SysCmsInfoId, item.InfoTitle));
            }
            return data;
        }

        public PageData<SysCmsInfoSearchDto> GetInfoPage(string SysCmsColumnId, string InfoTitle, string InfoType, string Status, string ordering, int Page, int Limit)
        {
            var predicate = PredicateBuilder.New<SysCmsInfo>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(InfoTitle))
            {
                predicate = predicate.And(i => i.InfoTitle.Contains(InfoTitle));
            }
            if (!string.IsNullOrEmpty(SysCmsColumnId))
            {
                predicate = predicate.And(i => i.SysCmsColumnId.Equals(SysCmsColumnId));
            }
            if (!string.IsNullOrEmpty(InfoType))
            {
                predicate = predicate.And(i => i.InfoType.Equals(InfoType));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                predicate = predicate.And(i => i.Status.Equals(int.Parse(Status)));
            }
            #endregion
            var pageData = DbContext.GetPageList<SysCmsInfo, SysCmsInfoSearchDto>(predicate.And(o => true), ordering, Page, Limit);
            return pageData;
        }

    

        public async Task<PageData<SysCmsInfoSearchDto>> GetInfoPageAsync(string SysCmsColumnId, string InfoTitle, string InfoType, string Status, string ordering, int Page, int Limit)
        {
            var predicate = PredicateBuilder.New<SysCmsInfo>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(InfoTitle))
            {
                predicate = predicate.And(i => i.InfoTitle.Contains(InfoTitle));
            }
            if (!string.IsNullOrEmpty(SysCmsColumnId))
            {
                predicate = predicate.And(i => i.SysCmsColumnId.Equals(SysCmsColumnId));
            }
            if (!string.IsNullOrEmpty(InfoType))
            {
                predicate = predicate.And(i => i.InfoType.Equals(InfoType));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                predicate = predicate.And(i => i.Status.Equals(int.Parse(Status)));
            }
            #endregion
            var pageData = await DbContext.GetPageListAsync<SysCmsInfo, SysCmsInfoSearchDto>(predicate.And(o => true), ordering, Page, Limit);
            return pageData;
        }



        /// <summary>
        /// 获取时间轴数据
        /// </summary>
        /// <returns></returns>
        public List<YearData> GetTimeLineData()
        {
            var data = DbContext.GetList<SysCmsInfo>(o => o.SysCmsColumnId == "a1962d915dde48ebaaa1b590a51cde75" && o.Status == 1, $"{nameof(SysCmsInfo.InfoPublishTime)} desc");
            List<YearData> returnData = new List<YearData>();
            foreach (var item in data)
            {
                var thisyear = ((DateTime)item.InfoPublishTime).Year.ToString();//年
                var thismonth = ((DateTime)item.InfoPublishTime).Month.ToString().PadLeft(2, '0');//月
                var thisday = ((DateTime)item.InfoPublishTime).ToString("MM月dd日 HH:mm");//日数据
                //不存在年份
                var thisYearData = returnData.FirstOrDefault(o => o.year == thisyear);
                if(thisYearData == null)
                {
                    thisYearData = new YearData() { year = thisyear};
                    returnData.Add(thisYearData);
                }
                var thisMonthData = thisYearData.months.FirstOrDefault(o => o.month == thismonth);
                //不存在月份数据
                if(thisMonthData == null)
                {
                    thisMonthData = new MonthData() { month = thismonth };
                    thisYearData.months.Add(thisMonthData);
                }
                //添加日数据
                thisMonthData.data.Add(new DayData
                {
                    content = item.InfoContent,
                    create_time = thisday
                });
            }
            return returnData;
        }
    }
}
