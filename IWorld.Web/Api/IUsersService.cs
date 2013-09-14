using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using IWorld.Contract.Client;
using IWorld.Model;

namespace IWorld.Web.Api
{
    /// <summary>
    /// 定义用户信息相关的数据服务
    /// </summary>
    [ServiceContract]
    public interface IUsersService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回登录操作的结果</returns>
        [OperationContract]
        LoginResult Login(string username, string password);

        /// <summary>
        /// 获取用户个人信息
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回用户信息</returns>
        [OperationContract]
        UserInfoResult GetUserInfo(string token);

        /// <summary>
        /// 绑定用户初始信息
        /// </summary>
        /// <param name="userInfoBindingImport">用于绑定用户初始信息的数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Binding(UserInfoBindingImport userInfoBindingImport, string token);

        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="onlyImmediate">一个布尔值 标记是否只看直属下级</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回基础用户信息的分页列表</returns>
        [OperationContract]
        PaginationList<BasicUserInfoResult> GetUsers(string keyword, bool onlyImmediate, int page, string token);

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="createUserImport">用于创建新用户的数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回创建新用户结果</returns>
        [OperationContract]
        CreateUserResult CreateUser(CreateUserImport createUserImport, string token);

        /// <summary>
        /// 升点
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="normalReturnPoints">普通返点</param>
        /// <param name="uncertainReturnPoints">不定位返点</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult UpgradePorn(int userId, double normalReturnPoints, double uncertainReturnPoints, string token);

        /// <summary>
        /// 编辑银行卡绑定信息
        /// </summary>
        /// <param name="card">银行卡卡号</param>
        /// <param name="holder">开户人</param>
        /// <param name="bank">开户银行</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditBank(string card, string holder, Bank bank, string token);

        /// <summary>
        /// 编辑安全邮箱
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditEmail(string email, string token);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditPassword(string oldPassword, string newPassword, string token);

        /// <summary>
        /// 修改安全码
        /// </summary>
        /// <param name="oldSafeWord">原安全码</param>
        /// <param name="newSafeWord">新安全码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditSafeWord(string oldSafeWord, string newSafeWord, string token);
    }
}
