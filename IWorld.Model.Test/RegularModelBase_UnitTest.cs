using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IWorld.Model.Test
{
    /// <summary>
    /// 定期任务相关的数据模型的基类的单元测试
    /// </summary>
    [TestClass]
    public class RegularModelBase_UnitTest
    {
        #region 构造方法

        /// <summary>
        /// 测试构造方法
        /// </summary>
        [TestMethod]
        public void Model_RegularModelBase_TestConstructor()
        {
            #region 结束时间小于开始时间

            DateTime bt = DateTime.Now;
            DateTime et = DateTime.Now.AddDays(-1);
            bool hadThrowError = false;
            try
            {
                RegularModelBase _model = new RegularModelBase(bt, et);
            }
            catch (Exception)
            {
                hadThrowError = true;
            }
            Assert.IsTrue(hadThrowError, "定期活动的结束时间必须大约开始时间 如果断言为未报错 请检查对应的构造方法");

            #endregion

            #region 结束时间等于开始时间

            hadThrowError = false;
            et = bt;
            try
            {
                RegularModelBase _model2 = new RegularModelBase(bt, et);
            }
            catch (Exception)
            {
                hadThrowError = true;
            }
            Assert.IsTrue(hadThrowError, "定期活动的结束时间必须大约开始时间 如果断言为未报错 请检查对应的构造方法");

            #endregion

            #region 输入的声明初始状态是否为暂停

            et = bt.AddDays(1);
            RegularModelBase model = new RegularModelBase(bt, et);
            Assert.AreEqual(model.BeginTime, bt);
            Assert.AreEqual(model.EndTime, et);
            Assert.AreEqual(model.Hide, false);

            RegularModelBase model2 = new RegularModelBase(bt, et, true);
            Assert.AreEqual(model2.BeginTime, bt);
            Assert.AreEqual(model2.EndTime, et);
            Assert.AreEqual(model2.Hide, true);

            #endregion
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 测试私有方法：获取定时活动的当前状态
        /// </summary>
        [TestMethod]
        public void Model_RegularModelBase_TestGetStatus()
        {
            #region 参数

            List<DateTime> beginTime = new List<DateTime>
            {
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(-2),
                DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(-1)

            };
            List<DateTime> endTime = new List<DateTime>
            {
                DateTime.Now.AddDays(2),
                DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1)
            };
            List<bool> hide = new List<bool>
            {
                true,
                true,
                true,
                false
            };
            List<RegularStatus> status = new List<RegularStatus>
            {
                RegularStatus.未开始,
                RegularStatus.已过期,
                RegularStatus.暂停,
                RegularStatus.正常
            };

            #endregion

            for (int i = 0; i < beginTime.Count; i++)
            {
                RegularModelBase model = new RegularModelBase(beginTime[i], endTime[i], hide[i]);
                string message = string.Format("理论状态：{0}，目标状态：{1}，请检查私有方法 GetStatus"
                    , status[i]
                    , model.Status);
                Assert.AreEqual(model.Status, status[i], message);
            }
        }

        #endregion
    }
}
