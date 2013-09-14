using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 用户绑定信息
    /// </summary>
    public class UserBinding : ModelBase
    {
        #region 属性

        /// <summary>
        /// 安全邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 一个布尔值 标识用户是否已经绑定安全邮箱
        /// </summary>
        public bool AlreadyBoundTheEmail { get; set; }

        /// <summary>
        /// 银行卡
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 开户人
        /// </summary>
        public string HolderOfTheCard { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        public Bank BankOfTheCard { get; set; }

        /// <summary>
        /// 一个布尔值 标识用户已经绑定银行卡
        /// </summary>
        public bool AlreadyBoundTheCard { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户绑定信息
        /// </summary>
        public UserBinding()
        {
            this.Email = "";
            this.AlreadyBoundTheEmail = false;
            this.Card = "";
            this.HolderOfTheCard = "";
            this.BankOfTheCard = Bank.无;
            this.AlreadyBoundTheCard = false;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 设置安全邮箱
        /// </summary>
        /// <param name="email">邮箱地址</param>
        public void SetEmail(string email)
        {
            if (this.AlreadyBoundTheEmail)
            {
                throw new Exception("安全邮箱已经被绑定 操作无效");
            }

            this.Email = email;
        }

        /// <summary>
        /// 重置安全邮箱
        /// </summary>
        public void ResetEmail()
        {
            this.Email = "";
            this.AlreadyBoundTheEmail = false;
        }

        /// <summary>
        /// 设置银行账户
        /// </summary>
        /// <param name="card">卡号</param>
        /// <param name="holderOfTheCard">开户人</param>
        /// <param name="bankOfTheCard">开户银行</param>
        public void SerCard(string card, string holderOfTheCard, Bank bankOfTheCard)
        {
            if (this.AlreadyBoundTheCard)
            {
                throw new Exception("银行账户已经被绑定 操作无效");
            }

            this.Card = card;
            this.HolderOfTheCard = holderOfTheCard;
            this.BankOfTheCard = bankOfTheCard;
        }

        /// <summary>
        /// 充值银行账户
        /// </summary>
        public void ResetCard()
        {
            this.Card = "";
            this.HolderOfTheCard = "";
            this.BankOfTheCard = Bank.无;
            this.AlreadyBoundTheCard = false;
        }

        #endregion
    }
}
