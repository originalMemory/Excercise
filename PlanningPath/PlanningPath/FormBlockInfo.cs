using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanningPath
{
    public partial class FormBlockInfo : Form
    {
        public FormBlockInfo()
        {
            InitializeComponent();
        }

        public void SetInfo(double maxLength, List<double> liLength, double angleAlpha, List<double> liAngle, string angleLabel1, string angleLabel2, string angleLabel3, string angleLabel4)
        {
            tbMaxEdgeLength.Text = Math.Round(maxLength, 2).ToString();
            tbEdgeLength.Text = string.Join(";", liLength.Select(x => Math.Round(x, 2)));
            tbAngleAlpha.Text = Math.Round(angleAlpha, 2).ToString();

            tbAngle1.Text = Math.Round(liAngle[0], 2).ToString();
            tbAngle2.Text = Math.Round(liAngle[1], 2).ToString();
            tbAngle3.Text = Math.Round(liAngle[2], 2).ToString();
            tbAngle4.Text = Math.Round(liAngle[3], 2).ToString();
            labelAngle1.Text = angleLabel1;
            labelAngle2.Text = angleLabel2;
            labelAngle3.Text = angleLabel3;
            labelAngle4.Text = angleLabel4;
        }
    }
}
