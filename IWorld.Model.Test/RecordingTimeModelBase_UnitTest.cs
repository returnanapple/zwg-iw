using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IWorld.Model.Test
{
    /// <summary>
    /// 记录创建时间和最新修改时间的数据模型的基类的单元测试
    /// </summary>
    [TestClass]
    public class RecordingTimeModelBase_UnitTest
    {
        #region 构造方法

        /// <summary>
        /// 测试构造方法
        /// </summary>
        [TestMethod]
        public void Model_RecordingTimeModelBase_TestConstructor()
        {
            RecordingTimeModelBase model = new RecordingTimeModelBase();
            DateTime d = new DateTime();
            Assert.AreNotEqual(model.CreatedTime, d);
            Assert.AreNotEqual(model.ModifiedTime, d);
            Assert.AreEqual(model.CreatedTime, model.ModifiedTime);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 测试方法：声明数据模型已经被修改
        /// </summary>
        [TestMethod]
        public void Model_RecordingTimeModelBase_TestOnModify()
        {
            RecordingTimeModelBase model = new RecordingTimeModelBase();
            DateTime d = new DateTime();
            System.Threading.Thread.Sleep(1);
            model.OnModify();
            Assert.AreNotEqual(model.CreatedTime, d);
            Assert.AreNotEqual(model.ModifiedTime, d);
            Assert.AreNotEqual(model.CreatedTime, model.ModifiedTime);
        }

        #endregion
    }
}
