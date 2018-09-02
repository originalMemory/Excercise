using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.IO;

namespace ArcForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //IFeature dsf;
            //IPoint ds;
            //IFeatureLayer lay = axMapControl1.get_Layer(0) as IFeatureLayer;
            //var cla = lay.FeatureClass;
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog xjOpenShpFileDialog = new OpenFileDialog();
            xjOpenShpFileDialog.Title = "打开矢量数据";
            xjOpenShpFileDialog.Filter = "矢量文件(*.shp)|*.shp";


            if (xjOpenShpFileDialog.ShowDialog() == DialogResult.OK)
            {
                string xjShpPath = xjOpenShpFileDialog.FileName;
                string xjShpFolder = System.IO.Path.GetDirectoryName(xjShpPath);
                string xjShpFileName = System.IO.Path.GetFileName(xjShpPath);
                //工作工厂+工作空间
                IWorkspaceFactory xjShpWsF = new ShapefileWorkspaceFactory();
                IFeatureWorkspace xjShpFWs = (IFeatureWorkspace)xjShpWsF.OpenFromFile(xjShpFolder, 0);
                //新建矢量图层：要素+名称
                IWorkspace xjShpWs = xjShpWsF.OpenFromFile(xjShpFolder, 0);
                IFeatureClass xjShpFeatureClass = xjShpFWs.OpenFeatureClass(xjShpFileName);
                IFeatureLayer xjShpFeatureLayer = new FeatureLayer();
                xjShpFeatureLayer.FeatureClass = xjShpFeatureClass;
                xjShpFeatureLayer.Name = xjShpFeatureClass.AliasName;
                //加载刷新
                this.axMapControl1.AddLayer(xjShpFeatureLayer);
                this.axMapControl1.ActiveView.Refresh();
            }

            IPoint p = new ESRI.ArcGIS.Geometry.Point();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            //ILayer layer = axMapControl1.get_Layer(0);
            //IGeoDataset dataset = layer as IGeoDataset;
            //var ren = dataset.SpatialReference;
            //var name = ren.Name;
            //int edf = 234;

            CreateShape();

            ////计算点是否在路径上
            ////获取路径
            //IFeatureLayer layer = axMapControl1.get_Layer(0) as IFeatureLayer;
            //IFeatureClass pFeatCla=layer.FeatureClass;
            //IFeature pFeature=pFeatCla.GetFeature(0);
            //IGeometry pGeometry=pFeature.Shape;
            //IPolyline pPoly=pGeometry as IPolyline;

            //IPoint test = new PointClass();
            //test.PutCoords(12943885.463977, 4857805.079523);

            //ISegmentCollection pSegCol = pPoly as ISegmentCollection;
            //for (int i = 0; i < pSegCol.SegmentCount; i++)
            //{
            //    ISegment pSeg = pSegCol.get_Segment(i);
            //    IPoint start = pSeg.FromPoint;
            //    double x1 = start.X, y1 = start.Y;
            //    IPoint end = pSeg.ToPoint;
            //    double x2 = end.X, y2 = end.Y;

            //    ISegmentCollection pcol2 = new PathClass();
            //    pcol2.AddSegment(pSeg);
            //    IGeometryCollection  pPol2 = new PolylineClass();
            //    pPol2.AddGeometry(pcol2 as IGeometry);

            //    ITopologicalOperator topo = test as ITopologicalOperator;
            //    IGeometry buffer = topo.Buffer(0.01); //缓冲一个极小的距离  
            //    topo = buffer as ITopologicalOperator;
            //    //IPoint tpPoint = topo as IPoint;
            //    //double x3 = tpPoint.X, y3 = tpPoint.Y;
            //    IGeometryCollection pgeo = topo.Intersect(pSeg as IGeometry, esriGeometryDimension.esriGeometry0Dimension) as IGeometryCollection;
            //    bool result = false;
            //    if (pgeo.GeometryCount > 0)
            //        MessageBox.Show("点在直线上");
                
            //}
        }

        void CreateShape()
        {
            string strFolder = @"F:\arcMap";
            string strFile = "block" + ".shp";
            FileInfo fFile = new FileInfo(strFolder + @"\" + strFile);

            string shapeFullName = strFolder + @"\" + strFile;
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(strFolder, 0);

            IFeatureClass pFeatureClass;

            //创建不同类型的字段
            int i;
            double tmpLongitude;
            double tmpLatitude;
            IPoint pPoint;

            IFields pFields = new Fields();
            IFieldsEdit pFieldsEdit = (IFieldsEdit)pFields;
            IField pFiled = new Field();
            IFieldEdit pFieldEdit = (IFieldEdit)pFiled;

            //设置坐标系并定义几何类型
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironment();
            IGeographicCoordinateSystem pGCS;
            pGCS = spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            pFieldEdit.Name_2 = "SHAPE";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            #region 点创建
            //IGeometryDef pGeoDef = new GeometryDef();
            //IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
            //pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;

            //pGeoDefEdit.SpatialReference_2 = pGCS;
            //pFieldEdit.GeometryDef_2 = pGeoDef;
            //pFieldsEdit.AddField(pFiled);

            //pFiled = new Field();
            //pFieldEdit = (IFieldEdit)pFiled;
            //pFieldEdit.Name_2 = "经度";
            //pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
            //pFieldsEdit.AddField(pFiled);
            ////创建shp
            //pFeatureClass = pFeatureWorkspace.CreateFeatureClass(strFile, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

            //tmpLongitude = 11616.61347551;
            //tmpLatitude = 3956.63814757;
            //pPoint = new ESRI.ArcGIS.Geometry.Point();
            //pPoint.X = tmpLongitude;
            //pPoint.Y = tmpLatitude;
            //IFeature pFeature = pFeatureClass.CreateFeature();
            //pFeature.Shape = pPoint;
            //pFeature.Store();

            //pPoint = new ESRI.ArcGIS.Geometry.Point();
            //pPoint.X = tmpLongitude - 100;
            //pPoint.Y = tmpLatitude - 100;
            //pFeature = pFeatureClass.CreateFeature();
            //pFeature.Shape = pPoint;
            //pFeature.set_Value(pFeature.Fields.FindField("经度"), tmpLongitude.ToString("F4"));

            //pFeature.Store();
            #endregion

            #region 线创建
            //IGeometryDef pGeoDef = new GeometryDef();
            //IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
            //pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;

            //pGeoDefEdit.SpatialReference_2 = pGCS;
            //pFieldEdit.GeometryDef_2 = pGeoDef;
            //pFieldsEdit.AddField(pFiled);

            ////创建shp
            //pFeatureClass = pFeatureWorkspace.CreateFeatureClass(strFile, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

            //tmpLongitude = 116.276984;
            //tmpLatitude = 39.9445685;
            //pPoint = new ESRI.ArcGIS.Geometry.Point();
            //pPoint.PutCoords(12943884.706671, 4857804.266050);

            ////定义一个多义线对象
            //IPolyline pPolyline = new ESRI.ArcGIS.Geometry.PolylineClass();
            ////定义一个点的集合
            //IPointCollection ptclo = pPolyline as IPointCollection;
            ////定义一系列要添加到多义线上的点对象，并赋初始值

            //ptclo.AddPoint(pPoint);

            //pPoint = new ESRI.ArcGIS.Geometry.Point();
            //pPoint.PutCoords(12943885.463977, 4857805.079523);

            //ptclo.AddPoint(pPoint);

            //IFeature pFeature = pFeatureClass.CreateFeature();
            //pFeature.Shape = pPolyline as IPolyline;
            //pFeature.Store();
            #endregion

            #region 面创建
            IGeometryDef pGeoDef = new GeometryDef();
            IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
            pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;

            pGeoDefEdit.SpatialReference_2 = pGCS;
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pFieldsEdit.AddField(pFiled);

            pFiled = new Field();
            pFieldEdit = (IFieldEdit)pFiled;
            pFieldEdit.Name_2 = "名称";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldsEdit.AddField(pFiled);
            //创建shp
            pFeatureClass = pFeatureWorkspace.CreateFeatureClass(strFile, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

            IPointCollection pointPolygon = new PolygonClass(); 
            pPoint = new PointClass();
            pPoint.PutCoords(116.445171, 40.187373);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(116.445698, 40.187436);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(116.446457, 40.184014);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(116.445927, 40.183817);
            pointPolygon.AddPoint(pPoint);
            
            IFeature pFeature = pFeatureClass.CreateFeature();
            pFeature.Shape = pointPolygon as IGeometry;
            pFeature.set_Value(pFeature.Fields.FindField("名称"), "地块1");
            pFeature.Store();

            pointPolygon = new PolygonClass();
            pPoint = new PointClass();
            pPoint.PutCoords(116.445698, 40.187436);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(116.446257, 40.187502);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(116.446967, 40.184236);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(116.446457, 40.184014);
            pointPolygon.AddPoint(pPoint);

            pFeature = pFeatureClass.CreateFeature();
            pFeature.Shape = pointPolygon as IGeometry;
            pFeature.set_Value(pFeature.Fields.FindField("名称"), "地块2");
            pFeature.Store();

            #endregion

            IGeometry polyline;
            polyline = axMapControl1.TrackLine();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPoint gcs = new ESRI.ArcGIS.Geometry.Point();
            gcs.PutCoords(116.444913, 40.179516);
            IPoint prj = GCStoPRJ(gcs, 4326, 3857);
            MessageBox.Show(string.Format("x={0},y={1}", prj.X, prj.Y));
        }


        private IPoint GCStoPRJ(IPoint pPoint, int GCSType, int PRJType)
        {
            ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
            pPoint.SpatialReference = pSRF.CreateGeographicCoordinateSystem(GCSType);
            pPoint.Project(pSRF.CreateProjectedCoordinateSystem(PRJType));
            return pPoint;
        }
    }

    //public class EditTool
    //{
    //    private ILayer m_pCurrentLayer;     //当前编辑图层
    //    private IMap m_pMap;                //地图控件中地图
    //    private IFeature m_pFeature;        //当前编辑要素
    //    private IPoint m_pPoint;            //鼠标点击位置
    //    private IDisplayFeedback m_pFeedback;   //用于地图显示
    //    private bool m_bInUse;              //判断是否编辑
    //    private IPointCollection m_pPointCollection;    //当前要素的点集

    //    public void StartEditing()
    //    {
    //        try
    //        {
    //            if (m_pCurrentLayer == null)
    //                return;
    //            //判断图层类型
    //            if (!(m_pCurrentLayer is IGeoFeatureLayer))
    //                return;
    //            IFeatureLayer pFeatureLayer = m_pCurrentLayer as IFeatureLayer;
    //            //获取编辑的要素类
    //            IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
    //            if (pDataset == null)
    //                return;
    //            //获取WorkspaceEdit
    //            IWorkspaceEdit pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
    //            if (!pWorkspaceEdit.IsBeingEdited())
    //            {
    //                pWorkspaceEdit.StartEditing(true);      //开启编辑流程
    //                pWorkspaceEdit.EnableUndoRedo();        //设置Undo/Redo为可用
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message);
    //        }
    //    }

    //    public void SaveEdit()
    //    {
    //        try
    //        {
    //            if (m_pCurrentLayer == null)
    //                return;
    //            //判断图层类型
    //            if (!(m_pCurrentLayer is IGeoFeatureLayer))
    //                return;
    //            IFeatureLayer pFeatureLayer = m_pCurrentLayer as IFeatureLayer;
    //            //获取编辑的要素类
    //            IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
    //            if (pDataset == null)
    //                return;
    //            //获取WorkspaceEdit
    //            IWorkspaceEdit pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
    //            if (pWorkspaceEdit.IsBeingEdited())
    //            {
    //                bool hasEdit = false;
    //                pWorkspaceEdit.HasEdits(ref hasEdit);
    //                bool bSave = false;
    //                if (hasEdit)
    //                {
    //                    pWorkspaceEdit.StopEditing(bSave);
    //                }
    //            }
    //            if (!pWorkspaceEdit.IsBeingEdited())
    //            {
    //                pWorkspaceEdit.StartEditing(true);      //开启编辑流程
    //                pWorkspaceEdit.EnableUndoRedo();        //设置Undo/Redo为可用
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message);
    //        }
    //    }

        
    //}
}
