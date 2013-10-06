using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IWorld.Model.Aid;

namespace IWorld.Model.Aid.Test
{
    /// <summary>
    /// 数据合法性的检查者对象的单元测试
    /// </summary>
    [TestClass]
    public class ValidityChecker_UnitTest
    {
        #region 检测边际条件

        #region 下限

        /// <summary>
        /// 测试静态方法：检查下限边际（输入参数为double类型）
        /// </summary>
        [TestMethod]
        public void TestCheckLower_double()
        {
            List<double> beau = new List<double> { 0.1, 0, -0.1, 0.1, 0, -0.1 };
            List<double> lower = new List<double> { 0, 0, 0, 0, 0, 0 };
            List<bool> canEquals = new List<bool> { false, false, false, true, true, true };
            List<bool> willThrowError = new List<bool> { false, true, true, false, false, true };
            List<string> error = new List<string>();
            string pd = "测试数据";

            for (int i = 0; i < beau.Count; i++)
            {
                bool hadThrowError = false;

                try
                {
                    ValidityChecker.CheckLower(beau[i], lower[i], pd, canEquals[i]);
                }
                catch (Exception)
                {
                    hadThrowError = true;
                }
                string message = string.Format("\r\n输入数据：{0} 输入边际：{1} 允许相等：{2} 预期结果：{3} 实际结果：{4}"
                    , beau[i]
                    , lower[i]
                    , canEquals[i] ? "允许" : "不允许"
                    , willThrowError[i] ? "抛出异常" : "不抛出异常"
                    , hadThrowError ? "抛出异常" : "未抛出异常");
                if (hadThrowError != willThrowError[i])
                {
                    error.Add(message);
                }
            }
            Assert.IsTrue(error.Count == 0, string.Join("", error));
        }

        /// <summary>
        /// 测试静态方法：检查下限边际（输入参数为int类型）
        /// </summary>
        [TestMethod]
        public void TestCheckLower_int()
        {
            List<int> beau = new List<int> { 1, 0, -1, 1, 0, -1 };
            List<int> lower = new List<int> { 0, 0, 0, 0, 0, 0 };
            List<bool> canEquals = new List<bool> { false, false, false, true, true, true };
            List<bool> willThrowError = new List<bool> { false, true, true, false, false, true };
            List<string> error = new List<string>();
            string pd = "测试数据";

            for (int i = 0; i < beau.Count; i++)
            {
                bool hadThrowError = false;

                try
                {
                    ValidityChecker.CheckLower(beau[i], lower[i], pd, canEquals[i]);
                }
                catch (Exception)
                {
                    hadThrowError = true;
                }
                string message = string.Format("\r\n输入数据：{0} 输入边际：{1} 允许相等：{2} 预期结果：{3} 实际结果：{4}"
                    , beau[i]
                    , lower[i]
                    , canEquals[i] ? "允许" : "不允许"
                    , willThrowError[i] ? "抛出异常" : "不抛出异常"
                    , hadThrowError ? "抛出异常" : "未抛出异常");
                if (hadThrowError != willThrowError[i])
                {
                    error.Add(message);
                }
            }
            Assert.IsTrue(error.Count == 0, string.Join("", error));
        }

        #endregion

        #region 上限

        /// <summary>
        /// 测试静态方法：检查上限边际（输入参数为double类型）
        /// </summary>
        [TestMethod]
        public void TestCheckCheckCaps_double()
        {
            List<double> beau = new List<double> { 0.1, 0, -0.1, 0.1, 0, -0.1 };
            List<double> lower = new List<double> { 0, 0, 0, 0, 0, 0 };
            List<bool> canEquals = new List<bool> { false, false, false, true, true, true };
            List<bool> willThrowError = new List<bool> { true, true, false, true, false, false };
            List<string> error = new List<string>();
            string pd = "测试数据";

            for (int i = 0; i < beau.Count; i++)
            {
                bool hadThrowError = false;

                try
                {
                    ValidityChecker.CheckCaps(beau[i], lower[i], pd, canEquals[i]);
                }
                catch (Exception)
                {
                    hadThrowError = true;
                }
                string message = string.Format("\r\n输入数据：{0} 输入边际：{1} 允许相等：{2} 预期结果：{3} 实际结果：{4}"
                    , beau[i]
                    , lower[i]
                    , canEquals[i] ? "允许" : "不允许"
                    , willThrowError[i] ? "抛出异常" : "不抛出异常"
                    , hadThrowError ? "抛出异常" : "未抛出异常");
                if (hadThrowError != willThrowError[i])
                {
                    error.Add(message);
                }
            }
            Assert.IsTrue(error.Count == 0, string.Join("", error));
        }

        /// <summary>
        /// 测试静态方法：检查上限边际（输入参数为int类型）
        /// </summary>
        [TestMethod]
        public void TestCheckCheckCaps_int()
        {
            List<int> beau = new List<int> { 1, 0, -1, 1, 0, -1 };
            List<int> lower = new List<int> { 0, 0, 0, 0, 0, 0 };
            List<bool> canEquals = new List<bool> { false, false, false, true, true, true };
            List<bool> willThrowError = new List<bool> { true, true, false, true, false, false };
            List<string> error = new List<string>();
            string pd = "测试数据";

            for (int i = 0; i < beau.Count; i++)
            {
                bool hadThrowError = false;

                try
                {
                    ValidityChecker.CheckCaps(beau[i], lower[i], pd, canEquals[i]);
                }
                catch (Exception)
                {
                    hadThrowError = true;
                }
                string message = string.Format("\r\n输入数据：{0} 输入边际：{1} 允许相等：{2} 预期结果：{3} 实际结果：{4}"
                    , beau[i]
                    , lower[i]
                    , canEquals[i] ? "允许" : "不允许"
                    , willThrowError[i] ? "抛出异常" : "不抛出异常"
                    , hadThrowError ? "抛出异常" : "未抛出异常");
                if (hadThrowError != willThrowError[i])
                {
                    error.Add(message);
                }
            }
            Assert.IsTrue(error.Count == 0, string.Join("", error));
        }

        #endregion

        #endregion
    }
}
