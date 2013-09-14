using System;

namespace IWorld.Model
{
    /// <summary>
    /// 活动参与条件
    /// </summary>
    public class ExchangeCondition : ModelBase, ICondition
    {
        #region 公开属性

        /// <summary>
        /// 类型
        /// </summary>
        public ConditionType Type { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        public double Limit { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        public double Upper { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的活动参与条件
        /// </summary>
        public ExchangeCondition()
        {
        }

        /// <summary>
        /// 实例化一个新的活动参与条件
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="limit">下限</param>
        /// <param name="upper">上限</param>
        public ExchangeCondition(ConditionType type, double limit, double upper)
        {
            this.Type = type;
            this.Limit = limit;
            this.Upper = upper;
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 审核用户是否符合资格
        /// </summary>
        /// <param name="user">目标用户</param>
        public void Audit(Author user)
        {
            switch (this.Type)
            {
                case ConditionType.消费量:
                    if (user.Consumption < this.Limit
                        || user.Consumption > this.Upper)
                    {
                        throw new Exception(string.Format("该用户的消费类[{0}]并不处于限制的范围内：{1} - {2}"
                            , user.Consumption, this.Limit, this.Upper));
                    }
                    break;
                case ConditionType.用户组等级:
                    if (user.Group.Grade < this.Limit
                        || user.Group.Grade > this.Upper)
                    {
                        throw new Exception(string.Format("该用户的等级[{0}级]并不处于限制的范围内：{1}级 - {2}级"
                            , user.Group.Grade, this.Limit, this.Upper));
                    }
                    break;
                case ConditionType.注册时间:
                    int days = (DateTime.Now - user.CreatedTime).Days + 1;
                    if (days < this.Limit
                        || days > this.Upper)
                    {
                        throw new Exception(string.Format("该用户的注册时间[{0}天]并不处于限制的范围内：{1}天 - {2}天"
                            , days, this.Limit, this.Upper));
                    }
                    break;
                case ConditionType.资金余额:
                    if (user.Money < this.Limit
                        || user.Money > this.Upper)
                    {
                        throw new Exception(string.Format("该用户的资金余额[{0}]并不处于限制的范围内：{1} - {2}"
                            , user.Money, this.Limit, this.Upper));
                    }
                    break;
            }
        }

        #endregion
    }
}
