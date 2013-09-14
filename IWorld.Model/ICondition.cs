
namespace IWorld.Model
{
    /// <summary>
    /// 定于限制条件的相关方法
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// 审核用户是否符合资格
        /// </summary>
        /// <param name="user">目标用户</param>
        void Audit(Author user);
    }
}
