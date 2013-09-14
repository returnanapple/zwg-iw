using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IWorld.Admin.Framework;

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 注册机器人
    /// </summary>
    public class RI
    {
        /// <summary>
        /// 注册
        /// </summary>
        public void Register()
        {
            //获取目标程序集
            Assembly assembly = Assembly.GetExecutingAssembly();

            #region 注册弹窗

            assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(t => t == typeof(IPop)))
                .ToList().ForEach(_type =>
                    {
                        bool isPop = Enum.GetNames(typeof(Pop)).Contains(_type.Name);
                        if (!isPop)
                        {
                            return;
                        }

                        IPop pop = assembly.CreateInstance(_type.FullName) as IPop;
                        var conditions = pop.GetMonitorConditions();
                        Messager.Default.RegisterRecipients(conditions, (message) =>
                            {
                                if (!conditions.Any(condition => message.Licit(condition)))
                                {
                                    return;
                                }
                                IPop tool = assembly.CreateInstance(_type.FullName) as IPop;
                                tool.Message = message;
                                tool.Show();
                            });
                    });

            #endregion
        }
    }
}
