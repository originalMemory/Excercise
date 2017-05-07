using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void SetProgressValue(int value)
        {
            this.progressBar1.Value = value;
            this.label1.Text = "progress:" + value.ToString() + "%";

            if (value == this.progressBar1.Maximum - 1) this.Close();
        }
    }
}
