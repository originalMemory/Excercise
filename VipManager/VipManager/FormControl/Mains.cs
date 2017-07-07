using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;

namespace VipManager.FormControl
{
    public partial class Mains : Skin_Color
    {
        public Mains()
        {
            InitializeComponent();
        }

        private void Mains_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MIAddVip_Click(object sender, EventArgs e)
        {
            
        }
    }
}
