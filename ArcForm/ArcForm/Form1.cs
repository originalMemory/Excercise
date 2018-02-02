using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GlobeCore;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace ArcForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            IFeature dsf;
            IPoint ds;
            IFeatureLayer lay=axMapControl1.get_Layer(0)as IFeatureLayer;
            var cla=lay.FeatureClass;
            cla.ShapeType
            
    }
}
