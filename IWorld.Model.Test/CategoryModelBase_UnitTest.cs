using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IWorld.Model;
using System.Collections.Generic;

namespace IWorld.Model.Test
{
    [TestClass]
    public class CategoryModelBase_UnitTest
    {
        #region 构造方法

        [TestMethod]
        public void Model_CategoryModelBase_TestConstructor_WhenRelativesIsNull()
        {
            CategoryModelBase model = new CategoryModelBase(null);
            Assert.IsNotNull(model.Relatives, "输入父祖节点为null时，模型的父祖节点不应为null，而应该为空List");
            Assert.AreEqual(model.Relatives.Count, 0, "输入父祖节点为null时，模型的父祖节点应为空的List（Count为0）");
            Regex reg = new Regex(@"^[a-zA-Z0-9]{32}$");
            Assert.IsTrue(reg.IsMatch(model.Tree), "treeName应为32位的guid");
        }



        public void Model_CategoryModelBase_TestConstructor_WhenRelativesIsNotNull()
        {
            List<Relative> relatives = new List<Relative>();
            string tree = Guid.NewGuid().ToString("N");
            CategoryModelBase model = new CategoryModelBase(relatives, tree);
            Assert.AreEqual(model.Relatives, relatives, "输入父祖节点不为null时，模型的父祖节点应等于这个对象");
            Assert.AreEqual(model.Tree, tree, "treeName应为32位的guid");
        }

        #endregion
    }
}
