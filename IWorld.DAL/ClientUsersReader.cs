using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Contract.Client;
using IWorld.Helper;
using IWorld.Setting;

namespace IWorld.DAL
{
    /// <summary>
    /// 用户信息的阅读者对象（前台）
    /// </summary>
    public class ClientUsersReader : ReaderBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户信息的阅读者对象（前台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ClientUsersReader(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 读取用户信息列表
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="keyword">关键字</param>
        /// <param name="onlyImmediate">一个布尔值 标记是否只看直属下级</param>
        /// <param name="page">页码</param>
        /// <returns>返回基础用户信息的分页列表</returns>
        public PaginationList<BasicUserInfoResult> ReadUsers(int userId, string keyword, bool onlyImmediate, int page)
        {
            var aSet = db.Set<Author>();
            Author user = aSet.Find(userId);

            Expression<Func<Author, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<Author, bool>> predicate2 = x => x.Id > 0;
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate1 = x => kws.All(kw => x.Username.Contains(kw));
            }
            if (onlyImmediate == true)
            {
                predicate2 = x => x.LeftKey >= user.LeftKey && x.RightKey <= user.RightKey && x.Layer == user.Layer + 1;
            }
            else
            {
                predicate2 = x => x.LeftKey >= user.LeftKey && x.RightKey <= user.RightKey;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForClient);

            int tCount = aSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(x => x.Tree == user.Tree)
                .Count();
            List<BasicUserInfoResult> tList = aSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(x => x.Tree == user.Tree)
                .OrderBy(x => x.Layer)
                .OrderBy(x => x.Username)
                .Skip(startRow)
                .Take(webSetting.PageSizeForClient)
                .ToList()
                .ConvertAll(x => new BasicUserInfoResult(x));

            return new PaginationList<BasicUserInfoResult>(page, webSetting.PageSizeForClient, tCount, tList);
        }

        #endregion
    }
}
