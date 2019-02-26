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
            string strFile = "newblock" + ".shp";
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
            //IGeographicCoordinateSystem pGCS;
            //pGCS = spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            IProjectedCoordinateSystem pPrj;
            pPrj = spatialReferenceFactory.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_Zone_38);
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

            //pGeoDefEdit.SpatialReference_2 = pPrj;
            //pFieldEdit.GeometryDef_2 = pGeoDef;
            //pFieldsEdit.AddField(pFiled);

            ////创建shp
            //pFeatureClass = pFeatureWorkspace.CreateFeatureClass(strFile, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

            //ILine pLine = new LineClass();

            //pPoint = new ESRI.ArcGIS.Geometry.Point();
            //pPoint.PutCoords(38708267.3691335, 4452391.75361639);

            ////定义一个多义线对象
            //IPolyline pPolyline = new ESRI.ArcGIS.Geometry.PolylineClass();
            ////定义一个点的集合
            //IPointCollection ptclo = pPolyline as IPointCollection;
            ////定义一系列要添加到多义线上的点对象，并赋初始值

            //ptclo.AddPoint(pPoint);

            //pPoint = new ESRI.ArcGIS.Geometry.Point();
            //pPoint.PutCoords(38708297.23653, 4452391.79756713);


            //ptclo.AddPoint(pPoint);

            //IFeature pFeature = pFeatureClass.CreateFeature();
            //pFeature.Shape = pPolyline as IPolyline;
            //pFeature.Store();
            #endregion

            #region 面创建
            IGeometryDef pGeoDef = new GeometryDef();
            IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
            pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;

            pGeoDefEdit.SpatialReference_2 = pPrj;
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
            pPoint.PutCoords(38708244.9302144, 4453281.28979934);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(38708320.2072986, 4452888.14487805);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(38708364.7490306, 4452911.26756388);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(38708289.624359, 4453289.52341905);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(38708244.9302144, 4453281.28979934);
            pointPolygon.AddPoint(pPoint);

            IFeature pFeature = pFeatureClass.CreateFeature();
            pFeature.Shape = pointPolygon as IGeometry;
            pFeature.set_Value(pFeature.Fields.FindField("名称"), "地块1");
            pFeature.Store();

            pointPolygon = new PolygonClass();
            pPoint = new PointClass();
            pPoint.PutCoords(38708289.624359, 4453289.52341905);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(38708364.7490306, 4452911.26756388);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(38708407.5103741, 4452937.11997987);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(38708337.0348187, 4453298.16561611);
            pointPolygon.AddPoint(pPoint);
            pPoint = new PointClass();
            pPoint.PutCoords(38708289.624359, 4453289.52341905);
            pointPolygon.AddPoint(pPoint);

            pFeature = pFeatureClass.CreateFeature();
            pFeature.Shape = pointPolygon as IGeometry;
            pFeature.set_Value(pFeature.Fields.FindField("名称"), "地块2");
            pFeature.Store();

            #endregion

            //IGeometry polyline;
            //polyline = axMapControl1.TrackLine();
        }

        //public void OnMouseDown(int Button, int Shift, int X, int Y)
        //{
        //    IMxDocument mxDoc = m_App.Document as IMxDocument;
        //    IActiveView activeView = mxDoc.FocusMap as IActiveView;
        //    IScreenDisplay screenDisplay = activeView.ScreenDisplay;
        //    screenDisplay.StartDrawing(screenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
        //    screenDisplay.SetSymbol(new SimpleMarkerSymbolClass());
        //    screenDisplay.DrawPoint(mxDoc.CurrentLocation);
        //    screenDisplay.FinishDrawing();
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            //IFeatureLayer layer = axMapControl1.get_Layer(0) as IFeatureLayer;
            //IFeatureClass pFeatCla = layer.FeatureClass;
            //IFeature pFeature = pFeatCla.GetFeature(0);
            //IGeometry pGeometry = pFeature.Shape;
            //IPolyline pPoly = pGeometry as IPolyline;

            IPoint gcs = new ESRI.ArcGIS.Geometry.Point();
            string[] strs = textBox1.Text.Split(',');

            gcs.PutCoords(Convert.ToDouble(strs[0]), Convert.ToDouble(strs[1]));
            IPoint prj = PRJtoGCS(gcs);
            string prjStr = string.Format("{0},{1}", gcs.X, gcs.Y);
            textBox2.Text = prjStr;
            //prjStr = string.Format("{0},{1}", pPoly.FromPoint.X, pPoly.FromPoint.Y);
            //textBox1.Text = prjStr;
            //double[] cood = lonLat2Mercator(Convert.ToDouble(strs[0]), Convert.ToDouble(strs[1]));
            //string prjStr = string.Format("{0},{1}", cood[0], cood[1]);

            //textBox2.Copy();
        }

        public double[] lonLat2Mercator(double X,double Y)
        {
            double[] db = new double[2];
            double x = X * 20037508.34 / 180;
            double y = Math.Log(Math.Tan((90 + Y) * Math.PI / 360)) / (Math.PI / 180);
            y = y * 20037508.34 / 180;
            db[0] = x;
            db[1] = y;
            return db;
        }

        private IPoint GCStoPRJ(IPoint pPoint, int GCSType = 4326, int PRJType=2414)
        {
            ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
            pPoint.SpatialReference = pSRF.CreateGeographicCoordinateSystem(GCSType);
            pPoint.Project(pSRF.CreateProjectedCoordinateSystem(PRJType));
            return pPoint;
        }

        private IPoint PRJtoGCS(IPoint pPoint, int PRJType=2414, int GCSType = 4326)
        {
            ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
            pPoint.SpatialReference = pSRF.CreateProjectedCoordinateSystem(PRJType);
            pPoint.Project(pSRF.CreateGeographicCoordinateSystem(GCSType));
            return pPoint;
        }

        double PI = 3.14159265358979323846;
        private void button2_Click(object sender, EventArgs e)
        {
            IFeatureLayer layer = axMapControl1.get_Layer(0) as IFeatureLayer;
            IFeatureClass pFeatCla = layer.FeatureClass;
            IFeature pFeature = pFeatCla.GetFeature(0);
            IGeometry pGeometry = pFeature.Shape;
            IPolyline pPoly = pGeometry as IPolyline;
            ISegmentCollection pSegCol = pGeometry as ISegmentCollection;
            ILine pLine = pSegCol.get_Segment(0) as ILine;

            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironment();
            IGeographicCoordinateSystem pGCS;
            pGCS = spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
            IProjectedCoordinateSystem pPrj;
            pPrj = spatialReferenceFactory.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Beijing1954_3_Degree_GK_Zone_38);

            ILine pLine2 = new LineClass();
            IPoint pPoint1 = new Point(); //已知点
            IPoint pPoint2 = new PointClass();
            //pPoint1.PutCoords(116.444976049068, 40.1795178557287);
            //pPoint2.PutCoords(116.444961462922, 40.1790692418292);
            pPoint1.PutCoords(38708252.36, 4452408.462);
            pPoint2.PutCoords(38708252.49, 4452358.606);
            pLine2.FromPoint = pPoint1;
            pLine2.ToPoint = pPoint2;
            //pLine2.SpatialReference = pGCS;
            pLine2.SpatialReference = pPrj;

            int df = 234;
            //inPoint.X = pPoly.FromPoint.X + 5;
            //inPoint.Y = pPoly.FromPoint.Y+2;
            //IPoint outPoint = new PointClass(); //曲线上到输入点距离最小的点；
            //double distAlongCurveFrom = 0; //曲线其实点到输出点部分的长度
            //double distFromCurve = 0;//输出点到输入点的距离
            //bool isRightSide = true;//输入点是否在曲线的右边
            //pPoly.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, inPoint, false, outPoint, ref distAlongCurveFrom, ref distFromCurve, ref isRightSide);
            //double lineHeading = pLine.Angle * 180 / PI;
            ////因为当前角度是以四象限X为起点的逆时针角度，对应的是地图东部
            ////故需要转成以北为起点的顺时针角度
            //if (lineHeading <= 90)
            //{
            //    //1、3、4象限
            //    lineHeading = 90 - lineHeading;
            //}
            //else
            //{
            //    //2象限
            //    lineHeading = 360 + (90 - lineHeading);
            //}
            //textBox1.Text = lineHeading.ToString();

            //double len = pPoly.Length;
            //textBox2.Text = len.ToString();

            //IPoint pStartPoint = new PointClass();
            //pStartPoint.PutCoords(250, 300);
            //IPoint pCenterPoint = new PointClass();
            //pCenterPoint.PutCoords(300, 300);
            //IPoint pEndPoint = new PointClass();
            //pEndPoint.PutCoords(350, 300);

            //ICircularArc pCirArc = new CircularArcClass();
            ////pCirArc.PutCoords(pCenterPoint, pStartPoint, pEndPoint, esriArcOrientation.esriArcCounterClockwise);
            //pCirArc.PutCoordsByAngle(pCenterPoint, 140 * Math.PI / 180, 0 * Math.PI / 180, 50);
            ////pCirArc.Radius = 50;
            //double a = Math.Acos(0.5);
            //IPolyline pPolyline = new PolylineClass();
            //ISegmentCollection pSegCol = pPolyline as ISegmentCollection;
            //pSegCol.AddSegment(pCirArc as ISegment);
            //MarkLine(pPolyline,null,3);
        }

        /// 粗略判断一个已知点是否在线上        
        private bool isPointOnLine(IPoint pPoint, IPolyline myLine)
        {
            ITopologicalOperator topo = pPoint as ITopologicalOperator;
            IGeometry buffer = topo.Buffer(0.00001); //缓冲一个极小的距离
            topo = buffer as ITopologicalOperator;
            IGeometryCollection pgeo = topo.Intersect(myLine, esriGeometryDimension.esriGeometry0Dimension) as IGeometryCollection;
            bool result = false;
            if (pgeo.GeometryCount > 0)
                result = true;
            return result;
        }

        #region==判断点是否在直线上 ==
        /// <summary>
        /// Determines whether [is point on line] [the specified points].
        /// </summary>
        /// <param name="points">The point.</param>
        /// <param name="lineStartPoint">The line start point.</param>
        /// <param name="lineEndPoint">The line end point.</param>
        /// <returns>
        ///   <c>true</c> if [is point on line] [the specified points]; otherwise, <c>false</c>.
        /// </returns>
        public bool isPointOnLine(IPoint point, IPoint lineStartPoint, IPoint lineEndPoint)
        {
            //(P-P1)*(P2-P1)=0 向量差乘是否为0; 
            double X21, Y21, X10, Y10;
            X21 = lineEndPoint.X - lineStartPoint.X;
            Y21 = lineEndPoint.Y - lineStartPoint.Y;
            X10 = point.X - lineStartPoint.X;
            Y10 = point.Y - lineStartPoint.Y;

            //向量乘积为0则该点在线上
            double vectorValue = X21 * Y10 - X10 * Y21;
            if (vectorValue != 0.0)
                return false;
            else
            {
                double xMin = Math.Min(lineStartPoint.X, lineEndPoint.X);
                double xMax = Math.Max(lineEndPoint.X, lineStartPoint.X);
                double yMin = Math.Min(lineStartPoint.Y, lineEndPoint.Y);
                double yMax = Math.Max(lineEndPoint.Y, lineStartPoint.Y);

                //判断点是否是在该线的延长线上
                if (xMin <= point.X && point.X <= xMax && yMin <= point.Y && point.Y <= yMax)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            IGraphicsContainer pGraphicsContainer = axMapControl1.Map as IGraphicsContainer;
            pGraphicsContainer.DeleteAllElements();
            axMapControl1.Map.ClearSelection();
            axMapControl1.ClearLayers();
            axMapControl1.Refresh();
        }

        /// <summary>
        /// 在地图上标记点
        /// </summary>
        /// <param name="pPoint">要标记的点</param>
        /// <param name="pRGB">颜色</param>
        /// <param name="size">大小</param>
        private void MarkPoint(IPoint pPoint, IRgbColor pRGB = null, double size = 5)
        {
            if (pRGB == null)
            {
                pRGB = GetRGB(96, 24, 192);
            }
            //设置标记的大小、颜色和类型
            ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbol();
            pMarkerSymbol.Size = 5;
            pMarkerSymbol.Color = pRGB;
            pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            //在地图上添加标记
            IMarkerElement pMarkerElement = new MarkerElementClass();
            IElement pElement = pMarkerElement as IElement;
            pElement.Geometry = pPoint as IGeometry;
            pMarkerElement.Symbol = pMarkerSymbol;
            IGraphicsContainer pGraphicsContainer = axMapControl1.Map as IGraphicsContainer;
            pGraphicsContainer.AddElement(pElement, 0);

            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 在地图上标记线
        /// </summary>
        /// <param name="pLine">要标记的线</param>
        /// <param name="pRGB">颜色</param>
        /// <param name="width">线宽</param>
        private void MarkLine(IPolyline pPolyline, IRgbColor pRGB = null, double width = 6)
        {
            ISimpleLineSymbol lineSymbol = new SimpleLineSymbolClass();
            lineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            lineSymbol.Width = width;

            if (pRGB != null)
            {
                //pRGB = GetRGB(96, 192, 24);
                lineSymbol.Color = pRGB;
            }

            //在地图上添加标记
            ILineElement pLineElement = new LineElementClass();
            pLineElement.Symbol = lineSymbol;

            IElement pElement = pLineElement as IElement;
            pElement.Geometry = pPolyline as IGeometry;
            IGraphicsContainer pGraphicsContainer = axMapControl1.Map as IGraphicsContainer;
            pGraphicsContainer.AddElement(pElement, 0);

            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 根据RGB值生成Arcengine颜色
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>IRgbColor对象</returns>
        private IRgbColor GetRGB(int r, int g, int b)
        {
            IRgbColor pColor;
            pColor = new RgbColorClass();
            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
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
