using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace VipManager.Helper
{
    /// <summary>
    /// Winform控件事件
    /// </summary>
    public static class ControlEvent
    {
        #region 数字输入限制
        /// <summary>
        /// 限制只输入整数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void NumLimit(object sender, KeyPressEventArgs e)
        {
            char cp = e.KeyChar;
            if ((cp >= '0' && cp <= '9') || cp == (char)Keys.Back || cp == (char)Keys.Enter || cp == (char)Keys.Tab)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 限制只输入整数和浮点数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DoubleLimit(object sender, KeyPressEventArgs e)
        {
            char cp = e.KeyChar;
            if ((cp >= '0' && cp <= '9') || cp == '.' || cp == (char)Keys.Back || cp == (char)Keys.Enter || cp == (char)Keys.Tab)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        #endregion
    }
}
