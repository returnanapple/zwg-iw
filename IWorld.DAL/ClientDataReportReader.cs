using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Contract.Client;
using IWorld.Helper;
using IWorld.Setting;

namespace IWorld.DAL
{
    /// <summary>
    /// 数据报表的阅读者对象（前台）
    /// </summary>
    public class ClientDataReportReader : ReaderBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的数据报表的阅读者对象（前台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ClientDataReportReader(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 读取数据报表
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="type">类型</param>
        /// <param name="page">页码</param>
        /// <returns>返回报表数据的分页列表</returns>
        public PaginationList<DataReportsResult> ReadReports(int userId, string beginTime, string endTime, ReportsType type
            , int page)
        {
            List<DataReportsResult> tList = new List<DataReportsResult>();
            var aSet = db.Set<Author>();
            Author user = aSet.Find(userId);
            DateTime now = DateTime.Now;
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForClient);
            int tCount = aSet.Count(x => x.LeftKey >= user.LeftKey && x.RightKey <= user.RightKey
                && x.Tree == user.Tree);

            aSet.Where(x => x.LeftKey >= user.LeftKey && x.RightKey <= user.RightKey
                 && x.Tree == user.Tree)
                .OrderBy(x => x.Layer)
                .Skip(startRow)
                .Take(webSetting.PageSizeForClient)
                .ToList().ForEach(_user =>
                    {
                        Expression<Func<PersonalDataAtDay, bool>> predicate1 = x => x.Id > 0;
                        Expression<Func<PersonalDataAtDay, bool>> predicate2 = x => x.Id > 0;
                        Expression<Func<PersonalDataAtDay, bool>> predicate3 = x => x.Id > 0;
                        switch (type)
                        {
                            case ReportsType.个人:
                                predicate1 = x => x.Owner.Id == _user.Id;
                                break;
                            case ReportsType.团队:
                                predicate1 = x => x.Owner.LeftKey >= _user.LeftKey
                                    && x.Owner.RightKey <= _user.RightKey;
                                break;
                        }
                        if (beginTime != "")
                        {
                            string[] tTime = beginTime.Split(new char[] { '-' });
                            DateTime _time = new DateTime(Convert.ToInt32(tTime[0]), Convert.ToInt32(tTime[1])
                                , Convert.ToInt32(tTime[2]));
                            predicate2 = x => x.CreatedTime >= _time;
                        }
                        if (endTime != "")
                        {
                            string[] tTime = endTime.Split(new char[] { '-' });
                            DateTime _time = new DateTime(Convert.ToInt32(tTime[0]), Convert.ToInt32(tTime[1])
                                , Convert.ToInt32(tTime[2]))
                                .AddDays(1);
                            predicate3 = x => x.CreatedTime < _time;
                        }

                        List<PersonalDataAtDay> _tList = db.Set<PersonalDataAtDay>()
                            .Where(predicate1)
                            .Where(predicate2)
                            .Where(predicate3)
                            .OrderBy(x => x.Owner.Layer)
                            .ToList();
                        if (_tList.Count == 0)
                        {
                            _tList.Add(new PersonalDataAtDay(_user));
                        }

                        tList.Add(new DataReportsResult(_tList));
                    });

            return new PaginationList<DataReportsResult>(page, webSetting.PageSizeForClient, tCount, tList);
        }

        #endregion
    }
}
