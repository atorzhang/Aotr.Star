
using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.Model.ViewModel.Sys;
using Ator.Utility.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ator.Repository.Sys
{
    public class SysCmsInfoRepository : Repository<DbFactory>, IRepository
    {
        public SysCmsInfoRepository(DbFactory factory) : base(factory)
        {

        }
        public List<SysCmsInfoSearchDto> GetInfoPage(string SysCmsColumnId,string InfoTitle, string InfoType, string Status,  string ordering, int Page,int Limit)
        {
            var data = GetInfoQuery(SysCmsColumnId, InfoTitle, InfoType, Status,   ordering, Page, Limit);
            return data;
        }
        public async Task<List<SysCmsInfoSearchDto>> GetInfoPageAsync(string SysCmsColumnId, string InfoTitle, string InfoType, string Status,  string ordering, int Page, int Limit)
        {
            return await Task.Run(() => 
                GetInfoQuery(SysCmsColumnId, InfoTitle, InfoType, Status, ordering, Page, Limit)
            );
        }

        private List<SysCmsInfoSearchDto> GetInfoQuery(string SysCmsColumnId, string InfoTitle, string InfoType, string Status,  string ordering, int Page, int Limit)
        {
            var queryable = DbContext.Queryable<SysCmsInfo>();
            #region 添加条件查询
            queryable.WhereIF(!string.IsNullOrEmpty(InfoTitle), i => i.InfoTitle.Contains(InfoTitle));
            queryable.WhereIF(!string.IsNullOrEmpty(SysCmsColumnId), i => i.SysCmsColumnId.Equals(SysCmsColumnId));
            queryable.WhereIF(!string.IsNullOrEmpty(InfoType), i => i.InfoType.Equals(InfoType));
            queryable.WhereIF(!string.IsNullOrEmpty(Status), i => i.Status.Equals(int.Parse(Status)));
            #endregion
            queryable.OrderBy(ordering).Select(o => new SysCmsInfoSearchDto
            {
                CreateTime = o.CreateTime,
                CreateUser = o.CreateUser,
                InfoTop = o.InfoTop,
                InfoAuthor = o.InfoAuthor,
                InfoClicks = o.InfoClicks,
                InfoComments = o.InfoComments,
                InfoGoods = o.InfoGoods,
                InfoLable = o.InfoLable,
                InfoPublishTime = o.InfoPublishTime,
                InfoSource = o.InfoSource,
                InfoTitle = o.InfoTitle,
                InfoType = o.InfoType,
                Remark = o.Remark,
                Sort = o.Sort,
                Status = o.Status,
                SysCmsInfoId = o.SysCmsInfoId,
                InfoAbstract = o.InfoAbstract,
                SysCmsColumnId = o.SysCmsColumnId,
                InfoImage = o.InfoImage
            });

            var queryData = queryable.ToPageList(Page, Limit).Map<SysCmsInfo, SysCmsInfoSearchDto>();
            return queryData;
        }
    }
}
