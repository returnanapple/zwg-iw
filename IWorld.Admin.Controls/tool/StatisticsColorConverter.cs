using System;
using System.Windows.Media;
using System.Windows.Data;

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 关于统计数字的颜色的转换器（绑定于 IsChecked）
    /// </summary>
    public class StatisticsColorConverter : IValueConverter
    {
        /// <summary>
        /// 在将源数据传递到目标以在 UI 中显示之前，对源数据进行修改
        /// </summary>
        /// <param name="value">正传递到目标的源数据</param>
        /// <param name="targetType">目标依赖项属性需要的数据的 System.Type</param>
        /// <param name="parameter">要在转换器逻辑中使用的可选参数</param>
        /// <param name="culture">转换的区域性</param>
        /// <returns>要传递到目标依赖项属性的值</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double v = 0;
            if (value != null && double.TryParse(value.ToString(), out v))
            {
                Color color = Color.FromArgb(100, 85, 85, 85);
                if (v > 0)
                {
                    color = Colors.Red;
                }
                else if (v < 0)
                {
                    color = Colors.Green;
                }

                return new SolidColorBrush(color);
            }

            return null;
        }

        /// <summary>
        /// 在将目标数据传递到源对象之前，对目标数据进行修改。此方法仅在 System.Windows.Data.BindingMode.TwoWay 绑定中进行调用
        /// </summary>
        /// <param name="value">正传递到源的目标数据</param>
        /// <param name="targetType">源对象需要的数据的 System.Type</param>
        /// <param name="parameter">要在转换器逻辑中使用的可选参数</param>
        /// <param name="culture">转换的区域性</param>
        /// <returns>要传递到源对象的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
