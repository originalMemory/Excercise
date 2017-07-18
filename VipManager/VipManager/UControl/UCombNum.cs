using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VipManager.UControl
{
    public partial class UCombNum : UserControl
    {
        public UCombNum()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取价格
        /// </summary>
        /// <returns></returns>
        public double GetPrice()
        {
            return Convert.ToDouble(txtPrice.Text);
        }

        /// <summary>
        /// 获取次数
        /// </summary>
        /// <returns></returns>
        public int GetNum()
        {
            return Convert.ToInt32(txtNum.Text);
        }
    }
}
