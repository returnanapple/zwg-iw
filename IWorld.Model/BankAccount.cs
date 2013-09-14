
namespace IWorld.Model
{
    /// <summary>
    /// 银行帐号
    /// </summary>
    public class BankAccount : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 索引字
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 开户人
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        public Bank Bank { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排列系数
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 一个布尔值 标识该对象是否为默认展示对象
        /// </summary>
        public bool IsDefault { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的银行帐号
        /// </summary>
        public BankAccount()
        {
        }

        /// <summary>
        /// 实例化一个新的银行帐号
        /// </summary>
        /// <param name="key">索引字</param>
        /// <param name="name">开户人</param>
        /// <param name="card">卡号</param>
        /// <param name="bank">银行</param>
        /// <param name="remark">备注</param>
        /// <param name="order">排列系数</param>
        /// <param name="isDefault">一个布尔值 标识该对象是否为默认展示对象</param>
        public BankAccount(string key, string name, string card, Bank bank, string remark, int order, bool isDefault)
        {
            this.Key = key;
            this.Name = name;
            this.Card = card;
            this.Bank = bank;
            this.Remark = remark;
            this.Order = order;
            this.IsDefault = isDefault;
        }

        #endregion
    }
}
