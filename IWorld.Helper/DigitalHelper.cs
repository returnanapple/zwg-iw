using System;
using System.Collections.Generic;

namespace IWorld.Helper
{
    /// <summary>
    /// 数字相关的帮助着对象
    /// </summary>
    public class DigitalHelper
    {
        /// <summary>
        /// 根据指定的开始和结束数字获取不重复的随机数列表
        /// </summary>
        /// <param name="start">开始数字</param>
        /// <param name="end">结束数字</param>
        /// <param name="length">所要获取的长度</param>
        /// <returns>返回不重复的随机数列表</returns>
        public static List<int> RandomList(int start, int end, int length)
        {
            List<int> result = new List<int>();
            bool goRanmdom = true;
            if (end - start + 1 < length)
            {
                if (start > end)
                {
                    goRanmdom = false;
                }
                else
                {
                    length = end - start + 1;
                }
            }

            if (goRanmdom == true)
            {
                List<int> tList = new List<int>();
                for (int i = start; i <= end; i++)
                {
                    tList.Add(i);
                }
                Random r = new Random();
                int surplus = end - start + 1;
                for (int i = 0; i < length; i++)
                {
                    int t = r.Next(0, surplus);
                    result.Add(tList[t]);
                    tList.RemoveAt(t);
                    surplus -= 1;
                }
            }

            return result;
        }

        /// <summary>
        /// 阶乘（0 - 12）
        /// </summary>
        /// <param name="input">指定的数字</param>
        /// <returns>制定数字的阶乘</returns>
        public static int GetFactorialIn0To12(int input)
        {
            if (input < 0)
            {
                throw new Exception("所要求阶乘的数字不能小于0");
            }
            else if (input == 0)
            {
                return 1;
            }
            else if (input > 12)
            {
                throw new Exception("所要求阶乘的数字不能大于12");
            }
            else
            {
                int result = 1;
                for (int i = 1; i <= input; i++)
                {
                    result *= i;
                }
                return result;
            }
        }
    }
}
