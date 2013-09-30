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
        /// 一个布尔值 表示是否已经绑定安全邮箱
        /// </summary>
        public bool AlreadyBindingEmail { get; set; }

        /// <summary>
        /// 银行卡
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 银行卡的开户人
        /// </summary>
        public string HolderOfTheCard { get; set; }

        /// <summary>
        /// 银行卡的开户银行
        /// </summary>
        public Bank BankOfTheCard { get; set; }

        /// <summary>
        /// 一个布尔值 表示是否已经绑定银行卡
        /// </summary>
        public bool AlreadyBindingCard { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户绑定信息
        /// </summary>
        public UserBinding()
        {
            this.Email = "";
            this.AlreadyBindingEmail = false;
            this.Card = "";
            this.HolderOfTheCard = "";
            this.BankOfTheCard = Bank.无;
            this.AlreadyBindingCard = false;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 绑定安全邮箱
        /// </summary>
        /// <param name="email">所要绑定的邮箱地址</param>
        public void BindingEmail(string email)
        {
            if (this.AlreadyBindingEmail)
            {
                throw new Exception("不允许重复绑定安全邮箱 操作无效");
            }
            this.Email = email;
            this.AlreadyBindingEmail = true;
        }

        /// <summary>
        /// 重置安全邮箱
        /// </summary>
        public void ResetEmail()
        {
            this.Email = "";
            this.AlreadyBindingEmail = false;
        }

        /// <summary>
        /// 绑定银行卡
        /// </summary>
        /// <param name="card">银行卡</param>
        /// <param name="holderOfTheCard">银行卡的开户人</param>
        /// <param name="bankOfTheCard">银行卡的开户银行</param>
        public void BindingCard(string card, string holderOfTheCard, Bank bankOfTheCard)
        {
            if (this.AlreadyBindingCard)
            {
                throw new Exception("不允许重复绑定银行卡 操作无效");
            }
            this.Card = card;
            this.HolderOfTheCard = holderOfTheCard;
            this.BankOfTheCard = bankOfTheCard;
        }

        /// <summary>
        /// 重置银行卡
        /// </summary>
        public void ResetCard()
        {
            this.Card = "";
            this.HolderOfTheCard = "";
            this.BankOfTheCard = Bank.无;
            this.AlreadyBindingCard = false;
        }

        #endregion
    }
}
