using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IWorld.Model;
using System.Collections.Generic;

namespace IWorld.Model.Test
{
    /// <summary>
    /// 类目相关的数据模型的基类的单元测试
    /// </summary>
    [TestClass]
    public class CategoryModelBase_UnitTest
    {
        #region 构造方法

        /// <summary>
        /// 测试构造方法：父祖节点为空
        /// </summary>
        [TestMethod]
        public void Model_CategoryModelBase_TestConstructor_WhenRelativesIsNull()
        {
            CategoryModelBase model = new CategoryModelBase(null);
            Assert.IsNotNull(model.Relatives, "输入父祖节点为null时，模型的父祖节点不应为null，而应该为空List");
            Assert.AreEqual(model.Relatives.Count, 0, "输入父祖节点为null时，模型的父祖节点应为空的List（Count为0）");
            Regex reg = new Regex(@"^[a-zA-Z0-9]{32}$");
            Assert.IsTrue(reg.IsMatch(model.Tree), "treeName应为32位的guid");
        }

        /// <summary>
        /// 测试构造方法：父祖节点不为空
        /// </summary>
        [TestMethod]
        public void Model_CategoryModelBase_TestConstructor_WhenRelativesIsNotNull()
        {
            List<Relative> relatives = new List<Relative>();
            string tree = Guid.NewGuid().ToString("N");
            bool hadThrowError = false;
            try
            {
                CategoryModelBase _model = new CategoryModelBase(relatives, tree);
            }
            catch (Exception)
            {
                hadThrowError = true;
            }
            Assert.IsTrue(hadThrowError, "当输入的父祖节点为空列表时，构造方法没有报错。请检查构造方法。");

            relatives.Add(new Relative(new CategoryModelBase(null)));
            CategoryModelBase model = new CategoryModelBase(relatives, tree);
            Assert.AreEqual(model.Relatives, relatives, "输入父祖节点不为null时，模型的父祖节点应等于这个对象");
            Assert.AreEqual(model.Tree, tree, "treeName应为32位的guid");
        }

        #endregion

        #region 方法

        /// <summary>
        /// 测试方法：判断目标对象是否与当前对象位于同一个树状结构
        /// </summary>
        [TestMethod]
        public void Model_CategoryModelBase_TestIsOnSameTree()
        {
            CategoryModelBase c1 = new CategoryModelBase(null);
            CategoryModelBase c2 = new CategoryModelBase(null);
            List<Relative> rs = new List<Relative>();
            rs.Add(new Relative(c1));
            CategoryModelBase c3 = new CategoryModelBase(rs, c1.Tree);
            Assert.IsFalse(c1.IsOnSameTree(c2), "c1和c2并不在同一个tree中！如果断言为true，请检查方法：IsOnSameTree。");
            Assert.IsTrue(c1.IsOnSameTree(c3), "c1和c3处于同一个tree中！如果断言为false，请检查方法：IsOnSameTree。");
        }

        /// <summary>
        /// 测试方法：判断当前对象是否是目标对象的父节点
        /// </summary>
        [TestMethod]
        public void Model_CategoryModelBase_TestIsAncestry()
        {
            CategoryModelBase c1 = new CategoryModelBase(null) { Id = 1 };
            CategoryModelBase c2 = new CategoryModelBase(null) { Id = 2 };
            List<Relative> rs = new List<Relative>();
            rs.Add(new Relative(c1));
            CategoryModelBase c3 = new CategoryModelBase(rs, c1.Tree) { Id = 3 };
            Assert.IsFalse(c1.IsAncestry(c2), "c1不是c2的父祖节点！如果断言为true，请检查方法：IsAncestry。");
            Assert.IsTrue(c1.IsAncestry(c3), "c1是c3的父祖节点！如果断言为false，请检查方法：IsAncestry。");
            Assert.IsFalse(c3.IsAncestry(c1), "c3不是c1的父祖节点！如果断言为true，请检查方法：IsAncestry。");
        }



        /// <summary>
        /// 测试方法：判断当前对象是否是目标对象的父节点
        /// </summary>
        [TestMethod]
        public void Model_CategoryModelBase_TestIsParent()
        {
            CategoryModelBase c1 = new CategoryModelBase(null) { Id = 1 };
            CategoryModelBase c2 = new CategoryModelBase(null) { Id = 2 };
            List<Relative> rs1 = new List<Relative>();
            rs1.Add(new Relative(c1));
            CategoryModelBase c3 = new CategoryModelBase(rs1, c1.Tree) { Id = 3 };
            List<Relative> rs2 = new List<Relative>();
            rs2.Add(new Relative(c1));
            rs2.Add(new Relative(c3));
            CategoryModelBase c4 = new CategoryModelBase(rs2, c1.Tree) { Id = 4 };
            Assert.IsFalse(c1.IsParent(c2), "c1不是c2的父节点！如果断言为true，请检查方法：IsParent。");
            Assert.IsTrue(c1.IsParent(c3), "c1是c3的父节点！如果断言为false，请检查方法：IsParent。");
            Assert.IsFalse(c3.IsParent(c1), "c3不是c1的父节点！如果断言为true，请检查方法：IsParent。");
            Assert.IsFalse(c1.IsParent(c4), "c1不是c4的父节点！如果断言为true，请检查方法：IsParent。");
            Assert.IsTrue(c3.IsParent(c4), "c3是c4的父节点！如果断言为false，请检查方法：IsParent。");
            Assert.IsFalse(c4.IsParent(c3), "c4不是c3的父节点！如果断言为true，请检查方法：IsParent。");
        }

        /// <summary>
        /// 测试方法：判断当前对象是否是目标对象的的子孙节点
        /// </summary>
        [TestMethod]
        public void Model_CategoryModelBase_TestIsOffspring()
        {
            CategoryModelBase c1 = new CategoryModelBase(null) { Id = 1 };
            CategoryModelBase c2 = new CategoryModelBase(null) { Id = 2 };
            List<Relative> rs = new List<Relative>();
            rs.Add(new Relative(c1));
            CategoryModelBase c3 = new CategoryModelBase(rs, c1.Tree) { Id = 3 };
            Assert.IsFalse(c2.IsOffspring(c1), "c2不是c1的子孙节点！如果断言为true，请检查方法：IsOffspring。");
            Assert.IsTrue(c3.IsOffspring(c1), "c3是c1的子孙节点！如果断言为false，请检查方法：IsOffspring。");
            Assert.IsFalse(c1.IsOffspring(c3), "c1不是c3的子孙节点！如果断言为true，请检查方法：IsOffspring。");
        }

        /// <summary>
        /// 测试方法：判断当前对象是否是目标对象的的子节点
        /// </summary>
        [TestMethod]
        public void Model_CategoryModelBase_TestIsChild()
        {
            CategoryModelBase c1 = new CategoryModelBase(null) { Id = 1 };
            CategoryModelBase c2 = new CategoryModelBase(null) { Id = 2 };
            List<Relative> rs1 = new List<Relative>();
            rs1.Add(new Relative(c1));
            CategoryModelBase c3 = new CategoryModelBase(rs1, c1.Tree) { Id = 3 };
            List<Relative> rs2 = new List<Relative>();
            rs2.Add(new Relative(c1));
            rs2.Add(new Relative(c3));
            CategoryModelBase c4 = new CategoryModelBase(rs2, c1.Tree) { Id = 4 };
            Assert.IsFalse(c2.IsChild(c1), "c2不是c1的子节点！如果断言为true，请检查方法：IsChild。");
            Assert.IsTrue(c3.IsChild(c1), "c3是c1的子节点！如果断言为false，请检查方法：IsChild。");
            Assert.IsFalse(c1.IsChild(c3), "c1不是c3的子节点！如果断言为true，请检查方法：IsChild。");
            Assert.IsFalse(c4.IsChild(c1), "c4不是c1的子节点！如果断言为true，请检查方法：IsChild。");
            Assert.IsTrue(c4.IsChild(c3), "c4是c3的子节点！如果断言为false，请检查方法：IsChild。");
            Assert.IsFalse(c3.IsChild(c4), "c3不是c4的子节点！如果断言为true，请检查方法：IsChild。");
        }

        #endregion
    }
}
