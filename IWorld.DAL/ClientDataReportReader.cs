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
        /// <param name="selectType">筛选类型</param>
        /// <param name="type">类型</param>
        /// <param name="page">页码</param>
        /// <returns>返回报表数据的分页列表</returns>
        public PaginationList<DataReportsResult> ReadReports(int userId, ReportsSelectType selectType, ReportsType type, int page)
        {
            List<DataReportsResult> tList = new List<DataReportsResult>();
            var aSet = db.Set<Author>();
            Author user = aSet.Find(userId);
            DateTime now = DateTime.Now;
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForClient);
            int tCount = aSet.Count(x => x.LeftKey >= user.LeftKey && x.RightKey <= user.RightKey
                && x.Tree == user.Tree);

            switch (selectType)
            {
                case ReportsSelectType.当日:
                    #region 日报表

                    var drForDaySet = db.Set<PersonalDataAtDay>();
                    aSet.Where(x => x.LeftKey >= user.LeftKey && x.RightKey <= user.RightKey
                         && x.Tree == user.Tree)
                        .OrderBy(x => x.Layer)
                        .Skip(startRow)
                        .Take(webSetting.PageSizeForClient)
                        .ToList().ForEach(_user =>
                            {
                                Expression<Func<PersonalDataAtDay, bool>> predicate = x => x.Id > 0;
                                switch (type)
                                {
                                    case ReportsType.个人:
                                        predicate = x => x.Owner.Id == _user.Id;
                                        break;
                                    case ReportsType.团队:
                                        predicate = x => x.Owner.LeftKey >= _user.LeftKey
                                            && x.Owner.RightKey <= _user.RightKey;
                                        break;
                                }

                                List<PersonalDataAtDay> _tList = drForDaySet.Where(predicate)
                                    .Where(x => x.Year == now.Year
                                        && x.Month == now.Month
                                        && x.Day == now.Day)
                                    .OrderBy(x => x.Owner.Layer)
                                    .ToList();
                                if (_tList.Count == 0)
                                {
                                    _tList.Add(new PersonalDataAtDay(_user));
                                }

                                tList.Add(new DataReportsResult(_tList));
                            });

                    #endregion
                    break;
                case ReportsSelectType.当月:
                case ReportsSelectType.全部:
                    #region 月报表

                    var drForMonthSet = db.Set<PersonalDataAtMonth>();
                    aSet.Where(x => x.LeftKey >= user.LeftKey && x.RightKey <= user.RightKey
                         && x.Tree == user.Tree)
                        .OrderBy(x => x.Layer)
                        .Skip(startRow)
                        .Take(webSetting.PageSizeForClient)
                        .ToList().ForEach(_user =>
                            {
                                Expression<Func<PersonalDataAtMonth, bool>> predicate1 = x => x.Id > 0;
                                Expression<Func<PersonalDataAtMonth, bool>> predicate2 = x => x.Id > 0;

                                switch (selectType)
                                {
                                    case ReportsSelectType.当月:
                                        predicate1 = x => x.Year == now.Year && x.Month == now.Month;
                                        break;
                                    case ReportsSelectType.全部:
                                        break;
                                    default:
                                        throw new Exception("这个选项不应该在这里出现，请严肃认真地检查代码");
                                }

                                switch (type)
                                {
                                    case ReportsType.个人:
                                        predicate2 = x => x.Owner.Id == _user.Id;
                                        break;
                                    case ReportsType.团队:
                                        predicate2 = x => x.Owner.LeftKey >= _user.LeftKey
                                            && x.Owner.RightKey <= _user.RightKey;
                                        break;
                                }

                                List<PersonalDataAtMonth> _tList = drForMonthSet
                                    .Where(predicate1)
                                    .Where(predicate2)
                                    .OrderBy(x => x.Owner.Layer)
                                    .ToList();
                                if (_tList.Count == 0)
                                {
                                    _tList.Add(new PersonalDataAtMonth(_user));
                                }

                                tList.Add(new DataReportsResult(_tList));
                            });

                    #endregion
                    break;
            }

            return new PaginationList<DataReportsResult>(page, webSetting.PageSizeForClient, tCount, tList);
        }

        #endregion
    }
}
