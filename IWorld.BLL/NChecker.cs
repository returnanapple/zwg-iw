using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Setting;

namespace IWorld.BLL
{
    /// <summary>
    /// 通用的检查者对象
    /// </summary>
    public class NChecker
    {
        /// <summary>
        /// 检查实例是否存在
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="id">存储指针</param>
        /// <param name="name">实例名称</param>
        /// <param name="db">数据库连接对象</param>
        public static void CheckEntity<T>(int id, string name, DbContext db)
            where T : ModelBase
        {
            bool exist = db.Set<T>().Any(x => x.Id == id);
            if (!exist) { throw new Exception("指定的" + name + "不存在"); }
        }

        /// <summary>
        /// 检查制定的返点数是否处于正确范围
        /// </summary>
        /// <param name="normalReturnPoints">普通返点数</param>
        /// <param name="uncertainReturnPoints">不定位返点数</param>
        /// <param name="parentId">上级用户的存储指针</param>
        /// <param name="db">数据库连接对象</param>
        public static void CheckerReturnPoints(double normalReturnPoints, double uncertainReturnPoints, int parentId, DbContext db)
        {
            WebSetting webSetting = new WebSetting();
            if (normalReturnPoints > webSetting.MaximumReturnPoints
                    || normalReturnPoints < webSetting.MinimumReturnPoints)
            {
                throw new Exception(string.Format("普通返点数不得超过系统设限（{0}-{1}）", webSetting.MinimumReturnPoints
                    , webSetting.MaximumReturnPoints));
            }
            if (uncertainReturnPoints > webSetting.MaximumReturnPoints
                || uncertainReturnPoints < webSetting.MinimumReturnPoints)
            {
                throw new Exception(string.Format("不定位返点数不得超过系统设限（{0}-{1}）", webSetting.MinimumReturnPoints
                    , webSetting.MaximumReturnPoints));
            }
            var rp = (from c in db.Set<Author>()
                      where c.Id == parentId
                      select new
                      {
                          c.NormalReturnPoints,
                          c.UncertainReturnPoints
                      })
                      .FirstOrDefault();
            double nrp = rp.NormalReturnPoints - webSetting.ReturnPointsDifference;
            if (normalReturnPoints > nrp)
            {
                throw new Exception(string.Format("上下级用户的普通返点差不得小于{0}", webSetting.ReturnPointsDifference));
            }
            double urp = rp.UncertainReturnPoints - webSetting.ReturnPointsDifference;
            if (uncertainReturnPoints > urp)
            {
                throw new Exception(string.Format("上下级用户的不定位返点差不得小于{0}", webSetting.ReturnPointsDifference));
            }
        }
    }
}
