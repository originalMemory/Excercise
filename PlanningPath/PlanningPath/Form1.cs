using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using System.IO;

namespace PlanningPath
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 地图点击模式
        /// </summary>
        MapClickMode EMapClickMode = MapClickMode.Normal;

        /// <summary>
        /// 转弯类型
        /// </summary>
        TurningType ETurningType = TurningType.HalfCir;

        /// 最长边对面点的垂线和对面边的夹角
        /// </summary>
        double ProjectAngle = 0.0;

        //double OmegaAngle = 0.0;

        /// <summary>
        /// 地块角度列表（逆时针）
        /// </summary>
        List<double> LiAngle = new List<double>();
        
        /// <summary>
        /// 地块边长列表（逆时针）
        /// </summary>
        List<double> LiLength = new List<double>();

        //选中地块的逆时针四顶点
        IPoint PA1;
        IPoint PB1;
        IPoint PC1;
        IPoint PD1;

        //作业范围的逆时针四顶点
        IPoint PA = new PointClass();
        IPoint PB = new PointClass();
        IPoint PC = new PointClass();
        IPoint PD = new PointClass();

        /// <summary>
        /// 作业幅宽
        /// </summary>
        double W = 0.0;
        /// <summary>
        /// 最小转弯半径
        /// </summary>
        double R = 0.0;
        /// <summary>
        /// 出线长度
        /// </summary>
        double E = 0.0;

        public Form1()
        {
            InitializeComponent();
            string dirPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string defaultShapefile = "newblock.shp";
            if (File.Exists(defaultShapefile))
            {
                OpenShapefile(dirPath + defaultShapefile);
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog xjOpenShpFileDialog = new OpenFileDialog();
            xjOpenShpFileDialog.Title = "打开矢量数据";
            xjOpenShpFileDialog.Filter = "矢量文件(*.shp)|*.shp";

            if (xjOpenShpFileDialog.ShowDialog() == DialogResult.OK)
            {
                string xjShpPath = xjOpenShpFileDialog.FileName;
                OpenShapefile(xjShpPath);
                axMapMain.MousePointer = esriControlsMousePointer.esriPointerArrow;
            }
        }

        private void OpenShapefile(string filePath)
        {
            string xjShpFolder = System.IO.Path.GetDirectoryName(filePath);
            string xjShpFileName = System.IO.Path.GetFileName(filePath);
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
            this.axMapMain.AddLayer(xjShpFeatureLayer);
            this.axMapMain.ActiveView.Refresh();
        }

        private void axMapMain_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 1 && EMapClickMode != MapClickMode.Normal)
            {
                //清除上次高亮图形
                IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
                pGraphicsContainer.DeleteAllElements();        

                axMapMain.Map.ClearSelection();        
                axMapMain.Refresh();
                axMapMain.CurrentTool = null;
                IFeatureLayer pFeatureLayer = axMapMain.get_Layer(0) as IFeatureLayer;
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                //SpatialFilter继承了QueryFilter
                IQueryFilter pQueryFilter = pSpatialFilter as IQueryFilter;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                if (EMapClickMode == MapClickMode.SelectBlock)
                {
                    //定位点击时所在点
                    IPoint pPoint = new PointClass();
                    pPoint.X = e.mapX;
                    pPoint.Y = e.mapY;
                    IGeometry pGeometry = pPoint as IGeometry;
                    
                    //选取点击位置所在的要素
                    pSpatialFilter.Geometry = pGeometry;
                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
                    IFeatureSelection pFeatureSelection = pFeatureLayer as IFeatureSelection;
                    pFeatureSelection.SelectFeatures(pQueryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    axMapMain.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

                    //要素存在时
                    IFeature pFeature = pFeatureCursor.NextFeature();
                    if (pFeature != null)
                    {
                        QueryBlock(pFeature);
                    }

                }
                else if (EMapClickMode == MapClickMode.QueryAttribute)
                {
                    IGeometry pGeometry = axMapMain.TrackRectangle();

                    List<IFeature> pFeatureList = new List<IFeature>();
                    pSpatialFilter.Geometry = pGeometry;
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
                    IFeatureSelection pFeatureSelection = pFeatureLayer as IFeatureSelection;
                    pFeatureSelection.SelectFeatures(pQueryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    IFeature pFeature = pFeatureCursor.NextFeature();
                    while (pFeature != null)
                    {
                        pFeatureList.Add(pFeature);
                        //将每个被选择的要素都加入列表中
                        pFeature = pFeatureCursor.NextFeature();
                        //光标指向下一个要素
                    }

                    AttributesForm pAttributeForm = new AttributesForm();
                    pAttributeForm.dataGridView1.RowCount = pFeatureList.Count + 1;
                    //设置边界风格
                    pAttributeForm.dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
                    //设置列数
                    pAttributeForm.dataGridView1.ColumnCount = pFeatureList[0].Fields.FieldCount;
                    for (int m = 0; m < pFeatureList[0].Fields.FieldCount; m++)
                    {
                        pAttributeForm.dataGridView1.Columns[m].HeaderText = pFeatureList[0].Fields.get_Field(m).AliasName;
                    }
                    //遍历要素
                    for (int i = 0; i < pFeatureList.Count; i++)
                    {
                        pFeature = pFeatureList[i];
                        for (int j = 0; j < pFeature.Fields.FieldCount; j++)
                        {
                            pAttributeForm.dataGridView1[j, i].Value = pFeature.get_Value(j).ToString();
                        }
                    }
                    axMapMain.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    pAttributeForm.Show();
                }
            }
        }

        private void btnSelectBlock_Click(object sender, EventArgs e)
        {
            ResetMapAndBtn();
            if (EMapClickMode != MapClickMode.SelectBlock)
            {
                EMapClickMode = MapClickMode.SelectBlock;
                axMapMain.MousePointer = esriControlsMousePointer.esriPointerIdentify;
                btnSelectBlock.Text = "取消提取";
            }
            else
            {
                EMapClickMode = MapClickMode.Normal;
            }
        }

        private void btnSelectAttribute_Click(object sender, EventArgs e)
        {
            ResetMapAndBtn();
            if (EMapClickMode != MapClickMode.QueryAttribute)
            {
                EMapClickMode = MapClickMode.QueryAttribute;
                axMapMain.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
                btnSelectAttribute.Text = "取消查询";
            }
            else
            {
                EMapClickMode = MapClickMode.Normal;
            }
        }

        /// <summary>
        /// 重置按钮和地图模块为初始状态
        /// </summary>
        private void ResetMapAndBtn()
        {
            btnSelectAttribute.Text = "查询属性";
            btnSelectBlock.Text = "提取地块";

            axMapMain.MousePointer = esriControlsMousePointer.esriPointerDefault;
            //清除上次高亮图形
            IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
            pGraphicsContainer.DeleteAllElements(); 
            //清除选中数据
            IActiveView pActiveView = axMapMain.ActiveView;
            IFeatureLayer pFeaturelayer = axMapMain.get_Layer(0) as IFeatureLayer;
            ESRI.ArcGIS.Carto.IFeatureSelection featureSelection = pFeaturelayer as ESRI.ArcGIS.Carto.IFeatureSelection;
            pActiveView.Refresh();
            featureSelection.Clear();
        }

        /// <summary>
        /// 查询选中地块信息
        /// </summary>
        /// <param name="pFeature">选中地块信息</param>
        private void QueryBlock(IFeature pFeature)
        {
            //标记地块顶点
            IPointCollection pPointCol = pFeature.Shape as IPointCollection;
            if (pPointCol.PointCount != 5)
            {
                MessageBox.Show("选中地块不是四边形地块！");
                return;
            }
            for (int i = 0; i < pPointCol.PointCount - 1; i++)
            {
                MarkPoint(pPointCol.get_Point(i));
            }

            

            //标记最长边
            ISegmentCollection pSegCol = pFeature.Shape as ISegmentCollection;
            ISegment pMaxEdge = pSegCol.get_Segment(0);   //最长边
            List<double> liEdge = new List<double>{pMaxEdge.Length};
            int maxIdx = 0;
            for (int i = 1; i < pSegCol.SegmentCount; i++)
            {
                ISegment pSeg = pSegCol.get_Segment(i);
                liEdge.Add(pSeg.Length);
                if (pSeg.Length > pMaxEdge.Length)
                {
                    pMaxEdge = pSeg;
                    maxIdx = i;
                }
                
            }

            LiLength.Clear();
            switch (maxIdx)
            {
                case 0:
                    LiLength = new List<double> { liEdge[0], liEdge[1], liEdge[2], liEdge[3] };
                    PA1 = pPointCol.get_Point(0);
                    PB1 = pPointCol.get_Point(1);
                    PC1 = pPointCol.get_Point(2);
                    PD1 = pPointCol.get_Point(3);
                    break;
                case 1:
                    LiLength = new List<double> { liEdge[1], liEdge[2], liEdge[3], liEdge[0] };
                    PA1 = pPointCol.get_Point(1);
                    PB1 = pPointCol.get_Point(2);
                    PC1 = pPointCol.get_Point(3);
                    PD1 = pPointCol.get_Point(0);
                    break;
                case 2:
                    LiLength = new List<double> { liEdge[2], liEdge[3], liEdge[0], liEdge[1] };
                    PA1 = pPointCol.get_Point(2);
                    PB1 = pPointCol.get_Point(3);
                    PC1 = pPointCol.get_Point(0);
                    PD1 = pPointCol.get_Point(1);
                    break;
                case 3:
                    LiLength = new List<double> { liEdge[3], liEdge[0], liEdge[1], liEdge[2] };
                    PA1 = pPointCol.get_Point(3);
                    PB1 = pPointCol.get_Point(0);
                    PC1 = pPointCol.get_Point(1);
                    PD1 = pPointCol.get_Point(2);
                    break;
                default:
                    break;
            }


            IPolyline pPolyline = new PolylineClass();
            pPolyline.FromPoint = pMaxEdge.FromPoint;
            pPolyline.ToPoint = pMaxEdge.ToPoint;
            MarkLine(pPolyline);

            tbMaxEdgeLength.Text = Math.Round(pMaxEdge.Length, 2).ToString();
            tbEdgeLength.Text = string.Join(";", liEdge.Select(x => Math.Round(x, 2)));

            //计算所有角度
            int centerIdx = 0;  //直线交点索引
            int preIdx = 3;  //第一条直线上的点的索引
            int nextIdx = 1;  //第二条直线上的点的索引
            List<double> liAngle = new List<double>();  //直线夹角列表
            maxIdx = centerIdx;     //最长边起始角度在liAngle的索引
            while (nextIdx < pPointCol.PointCount)
            {
                IPoint pCenterPoint=pPointCol.get_Point(centerIdx);
                double angle = ComputerAngleBetweenTwoLine(pCenterPoint, pPointCol.get_Point(preIdx), pPointCol.get_Point(nextIdx));
                liAngle.Add(angle);

                if (pCenterPoint.X == pMaxEdge.FromPoint.X && pCenterPoint.Y == pMaxEdge.FromPoint.Y)
                {
                    maxIdx = centerIdx;
                }
                preIdx = centerIdx;
                centerIdx = nextIdx;
                ++nextIdx;
            }
            //计算最长边对面点的垂线和对面边的夹角
            centerIdx = maxIdx;
            int oppositeIdx = GetNextLoopPoint(centerIdx + 1, pPointCol.PointCount);    //对面边上点的索引位置
            IPoint projectPoint = GetProjectivePoint(pPointCol.get_Point(centerIdx), pPointCol.get_Point(centerIdx + 1), pPointCol.get_Point(oppositeIdx)); //投影点
            centerIdx = oppositeIdx;
            oppositeIdx = GetNextLoopPoint(oppositeIdx, pPointCol.PointCount);
            ProjectAngle = ComputerAngleBetweenTwoLine(pPointCol.get_Point(centerIdx), projectPoint, pPointCol.get_Point(oppositeIdx));
            tbAngleAlpha.Text = Math.Round(ProjectAngle, 2).ToString();
            ProjectAngle = ProjectAngle * (Math.PI / 180);

            pPolyline.FromPoint = pPointCol.get_Point(centerIdx);
            pPolyline.ToPoint = projectPoint;
            MarkLine(pPolyline);

            tbAngle1.Text = Math.Round(liAngle[0], 2).ToString();
            tbAngle2.Text = Math.Round(liAngle[1], 2).ToString();
            tbAngle3.Text = Math.Round(liAngle[2], 2).ToString();
            tbAngle4.Text = Math.Round(liAngle[3], 2).ToString();

            labelAngle1.Text = "角度一：";
            labelAngle2.Text = "角度二：";
            labelAngle3.Text = "角度三：";
            labelAngle4.Text = "角度四：";
            LiAngle.Clear();
            liAngle = liAngle.Select(x => x * (Math.PI / 180)).ToList();
            switch (maxIdx)
            {
                case 0:
                    labelAngle1.Text = "角度一(最长边)：";
                    labelAngle2.Text = "角度二(最长边)：";
                    LiAngle = new List<double> { liAngle[0], liAngle[1], liAngle[2], liAngle[3] };
                    break;
                case 1:
                    labelAngle2.Text = "角度二(最长边)：";
                    labelAngle3.Text = "角度三(最长边)：";
                    LiAngle = new List<double> { liAngle[1], liAngle[2], liAngle[3], liAngle[0] };
                    break;
                case 2:
                    labelAngle3.Text = "角度三(最长边)：";
                    labelAngle4.Text = "角度四(最长边)：";
                    LiAngle = new List<double> { liAngle[2], liAngle[3], liAngle[0], liAngle[1] };
                    break;
                case 3:
                    labelAngle4.Text = "角度四(最长边)：";
                    labelAngle1.Text = "角度一(最长边)：";
                    LiAngle = new List<double> { liAngle[3], liAngle[0], liAngle[1], liAngle[2] };
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获取四边形地块（n+1个点描述）逆时针的下一个点
        /// </summary>
        /// <param name="nowIdx">当前点索引</param>
        /// <param name="num">点总数</param>
        /// <returns>下一个点的索引</returns>
        private int GetNextLoopPoint(int nowIdx, int num)
        {
            if (nowIdx == num - 1)
            {
                return 1;
            }
            else
            {
                return nowIdx + 1;
            }
        }

        /// <summary>
        /// 在地图上标记点
        /// </summary>
        /// <param name="pPoint">要标记的点</param>
        /// <param name="pRGB">颜色</param>
        /// <param name="size">大小</param>
        private void MarkPoint(IPoint pPoint, IRgbColor pRGB = null, double size = 3)
        {
            if (pRGB == null)
            {
                pRGB = GetRGB(96, 24, 192);
            }
            //设置标记的大小、颜色和类型
            ISimpleMarkerSymbol pMarkerSymbol= new SimpleMarkerSymbol();
            pMarkerSymbol.Size = 5;
            pMarkerSymbol.Color = pRGB;
            pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            //在地图上添加标记
            IMarkerElement pMarkerElement = new MarkerElementClass();
            IElement pElement = pMarkerElement as IElement;
            pElement.Geometry = pPoint as IGeometry;
            pMarkerElement.Symbol = pMarkerSymbol;
            IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
            pGraphicsContainer.AddElement(pElement, 0);

            axMapMain.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 在地图上标记线
        /// </summary>
        /// <param name="pLine">要标记的线</param>
        /// <param name="pRGB">颜色</param>
        /// <param name="width">线宽</param>
        private void MarkLine(IPolyline pPolyline, IRgbColor pRGB = null, double width = 2)
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
            IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
            pGraphicsContainer.AddElement(pElement, 0);

            axMapMain.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        Random rd = new Random();
        /// <summary>
        /// 生成一个随机的RGB颜色
        /// </summary>
        /// <returns>RGB颜色</returns>
        private IRgbColor GetRandomRGB()
        {
            int n=256;
            return GetRGB(rd.Next(0, n), rd.Next(0, n), rd.Next(0, n));
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

        /// <summary>
        ///  计算两根线夹角
        /// </summary>
        /// <param name="cen">两根线的交点</param>
        /// <param name="first">第一根线上某点</param>
        /// <param name="second">第二根线上某点</param>
        /// <returns>夹角</returns>

        public double ComputerAngleBetweenTwoLine(IPoint center, IPoint first, IPoint second)
        {
            const double M_PI = 3.1415926535897;

            double ma_x = first.X - center.X;
            double ma_y = first.Y - center.Y;
            double mb_x = second.X - center.X;
            double mb_y = second.Y - center.Y;
            double v1 = (ma_x * mb_x) + (ma_y * mb_y);
            double ma_val = Math.Sqrt(ma_x * ma_x + ma_y * ma_y);
            double mb_val = Math.Sqrt(mb_x * mb_x + mb_y * mb_y);
            double cosM = v1 / (ma_val * mb_val);
            double angleAMB = Math.Acos(cosM) * 180 / M_PI;
            return angleAMB;
        }

        private void 空间查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EMapClickMode = MapClickMode.QueryAttribute;
            axMapMain.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
        }

        private void btnComputeWidth_Click(object sender, EventArgs e)
        {
            //清除地图上无关信息
            btnSelectBlock.Text = "提取地块";
            IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
            pGraphicsContainer.DeleteAllElements();
            axMapMain.Map.ClearSelection();
            axMapMain.Refresh();
            EMapClickMode = MapClickMode.Normal;

            //获取机具信息并计算地头转弯区宽度
            R = Convert.ToDouble(tbMinTurnRadius.Text);
            W = Convert.ToDouble(tbWorkWidth.Text);
            E = Convert.ToDouble(tbOutLength.Text);
            double Y1 = ComputeAdjoinTurnArea(R, W, E, LiAngle[0]);
            double Y2 = ComputeAdjoinTurnArea(R, W, E, LiAngle[1]);
            double Y3 = ComputeOppositeTurnArea(R, W, E, ProjectAngle);
            tbTurnArea1.Text = Math.Round(Y1, 2).ToString();
            tbTurnArea2.Text = Math.Round(Y2, 2).ToString();
            tbTurnArea3.Text = Math.Round(Y3, 2).ToString();

            //计算A、B、C、D点坐标
            PA.X = PA1.X - (Y1 / Math.Sin(LiAngle[0])) * ((PA1.X - PB1.X) / LiLength[0]);
            PA.Y = PA1.Y - (Y1 / Math.Sin(LiAngle[0])) * ((PA1.Y - PB1.Y) / LiLength[0]);
            tbPointA.Text = string.Format("{0:F2},{1:F2}", PA.X, PA.Y);
            MarkPoint(PA);

            PB.X = PB1.X + (Y2 / Math.Sin(LiAngle[1])) * ((PA1.X - PB1.X) / LiLength[0]);
            PB.Y = PB1.Y + (Y2 / Math.Sin(LiAngle[1])) * ((PA1.Y - PB1.Y) / LiLength[0]);
            tbPointB.Text = string.Format("{0:F2},{1:F2}", PB.X, PB.Y);
            MarkPoint(PB);

            bool bTrunArea3 = (LiLength[2] * Math.Cos(ProjectAngle) - 3 * W)>0; //是否需要计算地头转弯区3
            if (bTrunArea3)
            {
                PC.X = PC1.X - ((Y2 / Math.Sin(LiAngle[2]) * Math.Abs((PC1.X - PD1.X) / LiLength[2]) + Y3 / Math.Sin(LiAngle[2]) * Math.Abs((PC1.X - PB1.X) / LiLength[1])));
                PC.Y = PC1.Y + ((Y2 / Math.Sin(LiAngle[2]) * Math.Abs((PC1.Y - PD1.Y) / LiLength[2]) - Y3 / Math.Sin(LiAngle[2]) * Math.Abs((PC1.Y - PB1.Y) / LiLength[1])));

                PD.X = PD1.X - ((Y3 / Math.Sin(LiAngle[3]) * (Math.Abs(PD1.X - PA1.X) / LiLength[3]) - Y1 / Math.Sin(LiAngle[3]) * (Math.Abs(PC1.X - PD1.X) / LiLength[2])));
                PD.Y = PD1.Y - (Y3 / Math.Sin(LiAngle[3]) * (Math.Abs(PD1.Y - PA1.Y) / LiLength[3]) + Y1 / Math.Sin(LiAngle[3]) * (Math.Abs(PD1.Y - PC1.Y) / LiLength[2]));
            }
            else
            {
                PC.X = PC1.X - Y2 / Math.Sin(LiAngle[2]) * (PC1.X - PD1.X) / LiLength[2];
                PC.Y = PC1.Y + Y2 / Math.Sin(LiAngle[2]) * (PD1.Y - PC1.Y) / LiLength[2];

                PD.X = PD1.X + Y1 / Math.Sin(LiAngle[3]) * (PC1.X - PD1.X) / LiLength[2];
                PD.Y = PD1.Y + Y1 / Math.Sin(LiAngle[3]) * (PC1.Y - PD1.Y) / LiLength[2];
            }
            tbPointC.Text = string.Format("{0:F2},{1:F2}", PC.X, PC.Y);
            MarkPoint(PC);
            tbPointD.Text = string.Format("{0:F2},{1:F2}", PD.X, PD.Y);
            MarkPoint(PD);

            //显示作业区域
            IPolyline pPolyline = new PolylineClass();
            IPointCollection pPointCol = pPolyline as IPointCollection;
            pPointCol.AddPoint(PA);
            pPointCol.AddPoint(PB);
            pPointCol.AddPoint(PC);
            pPointCol.AddPoint(PD);
            pPointCol.AddPoint(PA);
            MarkLine(pPolyline, GetRandomRGB(), 2);

            ComputePath();
        }

        /// <summary>
        /// 计算转弯路径
        /// </summary>
        private void ComputePath()
        {
            List<IPoint> liPt1 = new List<IPoint>();
            List<IPoint> liPt2 = new List<IPoint>();
            List<IPoint> liPtM = new List<IPoint>();
            int i = 1;
            IPoint pPointN1;
            IPoint pPointN2;
            IPoint pPointM;
            
            bool bN1 = ProjectAngle < Math.PI;
            IRgbColor pRGB = GetRandomRGB();
            while (true)
            {
                //计算当前直线路径与转弯区1和转弯区2的交点，以及转弯中线点
                pPointN1 = ComputeCrossPoint(1, i);
                pPointN2 = ComputeCrossPoint(2, i);
                if (i % 2 != 0)
                {
                    pPointM = ComputeMiddlePoint(1, i);
                }
                else
                {
                    pPointM = ComputeMiddlePoint(2, i);
                }
                //判断是否计算到最后一条线
                if (bN1)
                {
                    if (pPointN1.X > PD.X - W / 2)
                    {
                        break;
                    }
                }
                else
                {
                    if (pPointN2.X > PC.X - W / 2)
                    {
                        break;
                    }
                }

                liPt1.Add(pPointN1);
                liPt2.Add(pPointN2);
                liPtM.Add(pPointM);
                MarkPoint(pPointN1, pRGB);
                MarkPoint(pPointN2, pRGB);
                MarkPoint(pPointM, pRGB);
                i++;
            }
            pRGB = GetRandomRGB();
            IPolyline pPolylineWork = new PolylineClass();
            ISegmentCollection pSegCol = pPolylineWork as ISegmentCollection;
            //遍历所有点，建立路径
            IPoint pPointP, pPointQ;    //半圆起始点和结束点
            //ETurningType = TurningType.Pear;
            for (i = 0; i < 1; i++)
            {
                ILine pLine1 = new LineClass();
                ILine pLine2 = new LineClass();
                ILine pLine3 = new LineClass();
                ISegmentCollection pTurningPath = null;
                if (i % 2 == 0)
                {
                    pLine1.FromPoint = liPt2[i];
                    pLine1.ToPoint = liPt1[i];
                    pPointP = ComputeFishTailLinePoint(1, PointType.TurnStart, liPt1[i]);
                    pPointQ = ComputeFishTailLinePoint(1, PointType.TurnEnd, liPt1[i + 1]);
                    pLine2.FromPoint = liPt1[i];
                    pLine3.ToPoint = liPt1[i + 1];
                    switch (ETurningType)
                    {
                        case TurningType.HalfCir:
                            pTurningPath = ComputeTurnHalfCircle(1, liPtM[i], pPointP, pPointQ);
                            break;
                        case TurningType.Pear:
                            pTurningPath = ComputeTurnPear(1, liPtM[i], pPointP, pPointQ);
                            break;
                        case TurningType.FishTail:
                            pTurningPath = ComputeTurnFishTail(1, liPtM[i], pPointP, pPointQ);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    pLine1.FromPoint = liPt1[i];
                    pLine1.ToPoint = liPt2[i];
                    pPointP = ComputeFishTailLinePoint(2, PointType.TurnStart, liPt2[i]);
                    pPointQ = ComputeFishTailLinePoint(2, PointType.TurnEnd, liPt2[i + 1]);
                    pLine2.FromPoint = liPt2[i];
                    pLine3.ToPoint = liPt2[i + 1];
                    switch (ETurningType)
                    {
                        case TurningType.HalfCir:
                            pTurningPath = ComputeTurnHalfCircle(2, liPtM[i], pPointP, pPointQ);
                            break;
                        case TurningType.Pear:
                            pTurningPath = ComputeTurnPear(2, liPtM[i], pPointP, pPointQ);
                            break;
                        case TurningType.FishTail:
                            pTurningPath = ComputeTurnFishTail(1, liPtM[i], pPointP, pPointQ);
                            break;
                        default:
                            break;
                    }
                }
                MarkPoint(pPointP, pRGB);
                MarkPoint(pPointQ, pRGB);
                pLine2.ToPoint = pPointP;
                pLine3.FromPoint = pPointQ;

                //pSegCol.AddSegment(pLine1 as ISegment);
                //pSegCol.AddSegment(pLine2 as ISegment);
                pSegCol.AddSegmentCollection(pTurningPath);
                //pSegCol.AddSegment(pLine3 as ISegment);
            }

                ILine pEndLine = new LineClass();
            if (bN1)
            {
                pEndLine.FromPoint = liPt1[i];
                pEndLine.ToPoint = liPt2[i];
            }
            else
            {
                pEndLine.FromPoint = liPt2[i];
                pEndLine.ToPoint = liPt1[i];
            }
            //pSegCol.AddSegment(pEndLine as ISegment);

            MarkLine(pPolylineWork, GetRandomRGB(), 2);
        }

        /// <summary>
        /// 计算半圆形转弯路径
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <param name="pPointM">直线路径中线与地头边界的交点连线的中点</param>
        /// <param name="pPointP">转弯路径起始点</param>
        /// <param name="pPointQ">转弯路径结束点</param>
        /// <returns>转弯路径</returns>
        private ISegmentCollection ComputeTurnHalfCircle(int areaType, IPoint pPointM, IPoint pPointP, IPoint pPointQ)
        {
            ISegmentCollection pSegCol = new PolylineClass() as ISegmentCollection;
            IPoint pPointO = ComputeHalfCircleCenterPoint(areaType, pPointM);
            MarkPoint(pPointO);
            esriArcOrientation oriType = esriArcOrientation.esriArcClockwise;
            if (areaType == 2)
            {
                oriType = esriArcOrientation.esriArcCounterClockwise;
            }
            ICircularArc pCirArc = new CircularArcClass();
            pCirArc.PutCoords(pPointO, pPointP, pPointQ, oriType);
            pSegCol.AddSegment(pCirArc as ISegment);
            return pSegCol;
        }

        /// <summary>
        /// 计算梨形转弯路径
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <param name="pPointM">直线路径中线与地头边界的交点连线的中点</param>
        /// <param name="pPointP">转弯路径起始点</param>
        /// <param name="pPointQ">转弯路径结束点</param>
        /// <returns>转弯路径</returns>
        private ISegmentCollection ComputeTurnPear(int areaType, IPoint pPointM, IPoint pPointP, IPoint pPointQ)
        {
            //计算AB与Y轴顺时针夹角
            IPoint pPoint = new PointClass();
            pPoint.X = PB.X;
            pPoint.Y = PB.Y + 50;
            double omegaAngle = ComputerAngleBetweenTwoLine(PB, PA, pPoint);
            if (PB.X > PA.X)
            {
                omegaAngle = -omegaAngle;
            }
            omegaAngle = omegaAngle * Math.PI / 180;

            ISegmentCollection pSegCol = new PolylineClass() as ISegmentCollection;
            List<IPoint> liPointO = ComputePearCenterPoints(areaType, pPointM);
            MarkPoint(liPointO[0]);
            MarkPoint(liPointO[1]);
            MarkPoint(liPointO[2]);

            ICircularArc pCirArc1 = new CircularArcClass();
            ICircularArc pCirArc2 = new CircularArcClass();
            ICircularArc pCirArc3 = new CircularArcClass();
            double angle = Math.Acos((R + W / 2) / (2 * R));
            if (areaType == 1)
            {
                pCirArc1.PutCoordsByAngle(liPointO[0], -omegaAngle, angle, R);
                pCirArc2.PutCoordsByAngle(liPointO[1], Math.PI + angle - omegaAngle, -(Math.PI - 2 * angle), R);
                pCirArc3.PutCoordsByAngle(liPointO[2], Math.PI - angle - omegaAngle, angle, R);
            }
            else if (areaType == 2)
            {
                pCirArc1.PutCoordsByAngle(liPointO[0], -omegaAngle, -angle, R);
                pCirArc2.PutCoordsByAngle(liPointO[1], Math.PI - angle - omegaAngle, Math.PI + 2 * angle, R);
                pCirArc3.PutCoordsByAngle(liPointO[2], Math.PI + angle - omegaAngle, -angle, R);
            }
            pSegCol.AddSegment(pCirArc1 as ISegment);
            pSegCol.AddSegment(pCirArc2 as ISegment);
            pSegCol.AddSegment(pCirArc3 as ISegment);

            //IPolyline pPolylineWork = new PolylineClass();
            //ISegmentCollection pSegCol2 = pPolylineWork as ISegmentCollection;
            //ILine pLine = new LineClass();
            //pLine.FromPoint = liPointO[0];
            //pLine.ToPoint = liPointO[2];
            //pSegCol2.AddSegment(pLine as ISegment);
            //MarkLine(pPolylineWork, GetRandomRGB());

            return pSegCol;
        }

        /// <summary>
        /// 计算鱼形转弯路径
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <param name="pPointM">直线路径中线与地头边界的交点连线的中点</param>
        /// <param name="pPointP">转弯路径起始点</param>
        /// <param name="pPointQ">转弯路径结束点</param>
        /// <returns>转弯路径</returns>
        private ISegmentCollection ComputeTurnFishTail(int areaType, IPoint pPointM, IPoint pPointP, IPoint pPointQ)
        {
            //计算AB与Y轴顺时针夹角
            IPoint pPoint = new PointClass();
            pPoint.X = PB.X;
            pPoint.Y = PB.Y + 50;
            double omegaAngle = ComputerAngleBetweenTwoLine(PB, PA, pPoint);
            if (PB.X > PA.X)
            {
                omegaAngle = -omegaAngle;
            }
            omegaAngle = omegaAngle * Math.PI / 180;

            ISegmentCollection pSegCol = new PolylineClass() as ISegmentCollection;
            List<IPoint> liPointO = ComputeFishTailCenterPoints(areaType, pPointM);
            MarkPoint(liPointO[0]);
            MarkPoint(liPointO[1]);

            IPoint pPointS = ComputeFishTailArcPoint(areaType, liPointO[0]);
            IPoint pPointT = ComputeFishTailArcPoint(areaType, liPointO[1]);
            ILine pLine = new LineClass();

            ICircularArc pCirArc1 = new CircularArcClass();
            ICircularArc pCirArc2 = new CircularArcClass();
            double angle = Math.Acos((R + W / 2) / (2 * R));
            if (areaType == 1)
            {
                pLine.FromPoint = pPointT;
                pLine.ToPoint = pPointS;
                pCirArc1.PutCoords(liPointO[1], pPointP, pPointT, esriArcOrientation.esriArcClockwise);
                pCirArc2.PutCoords(liPointO[0], pPointS, pPointQ, esriArcOrientation.esriArcClockwise);
            }
            else if (areaType == 2)
            {
                pLine.FromPoint = pPointS;
                pLine.ToPoint = pPointT;
                pCirArc1.PutCoords(liPointO[0], pPointP, pPointS, esriArcOrientation.esriArcCounterClockwise);
                pCirArc2.PutCoords(liPointO[1], pPointT, pPointQ, esriArcOrientation.esriArcCounterClockwise);
            }
            pSegCol.AddSegment(pCirArc1 as ISegment);
            //pSegCol.AddSegment(pLine as ISegment);
            //pSegCol.AddSegment(pCirArc2 as ISegment);

            //IPolyline pPolylineWork = new PolylineClass();
            //ISegmentCollection pSegCol2 = pPolylineWork as ISegmentCollection;
            //ILine pLine = new LineClass();
            //pLine.FromPoint = liPointO[0];
            //pLine.ToPoint = liPointO[2];
            //pSegCol2.AddSegment(pLine as ISegment);
            //MarkLine(pPolylineWork, GetRandomRGB());

            return pSegCol;
        }

        /// <summary>
        /// 求直线外一点到该直线的投影点
        /// </summary>
        /// <param name="pLine1">线上任一点，两点描述一条线</param>
        /// <param name="pLine2">线上任一点，两点描述一条线</param>
        /// <param name="pOut">线外指定点</param>
        /// <returns>投影点</returns>
        private IPoint GetProjectivePoint(IPoint pLine1, IPoint pLine2, IPoint pOut)
        {
            IPoint pProject = new PointClass();
            double k = (pLine1.Y - pLine2.Y) / (pLine1.X - pLine2.X);
            if (k == 0) //垂线斜率不存在情况
            {
                pProject.X = pOut.X;
                pProject.Y = pLine1.Y;
            }
            else
            {
                double se = Math.Pow(pLine1.X - pLine2.X, 2) + Math.Pow(pLine1.Y - pLine2.Y, 2);
                double p = ((pOut.X - pLine1.X) * (pLine2.X - pLine1.X) + (pOut.Y - pLine1.Y) * (pLine2.Y - pLine1.Y));
                double r = p / se;
                pProject.X = pLine1.X + r * (pLine2.X - pLine1.X);
                pProject.Y = pLine1.Y + r * (pLine2.Y - pLine1.Y);
            }

            return pProject;
        }

        /// <summary>
        /// 计算相邻地头转弯区1,2
        /// </summary>
        /// <param name="r">农机最小转弯半径</param>
        /// <param name="w">农机作业幅宽</param>
        /// <param name="e">机组出线长度</param>
        /// <param name="angle">转弯区相邻角</param>
        /// <returns>转弯区宽度</returns>
        private double ComputeAdjoinTurnArea(double r, double w,double e,double angle)
        {
            double type = r - w / 2;
            if (r == 0 || w == 0)
            {
                MessageBox.Show("转弯半径或作业幅度为空，无法计算！");
                return 0;
            }
            if (type < 0)
            {
                MessageBox.Show("不满足计算条件，请重新输入参数！");
                return 0;
            }
            double y = 0.0;
            if (type == 0)
            {
                //半圆形
                if (cbMachineType.SelectedItem.ToString() == "悬挂式")
                {
                    y = w / 2 + w / 2 + w / 2 * Math.Abs(Math.Cos(angle)) + e * Math.Sin(angle);
                    ETurningType = TurningType.HalfCir;
                }
                else
                {
                    MessageBox.Show("农机类型不对，无法计算半圆形转弯！");
                }
            }
            else
            {
                // 鱼形尾
                if (cbMachineType.SelectedItem.ToString() == "悬挂式")
                {
                    y = r * Math.Sin(angle) + w / 2 * Math.Sin(angle) + r * Math.Abs(Math.Cos(angle)) + e * Math.Sin(angle);
                    ETurningType = TurningType.FishTail;
                }
                // 梨形
                else if (cbMachineType.SelectedItem.ToString() == "牵引式")
                {
                    y = r + w / 2 + Math.Sin(angle) * (Math.Sqrt(4 * r * r - (r + w / 2) * (r + w / 2))) + w * Math.Abs(Math.Cos(angle)) / 2 + e * Math.Sin(angle);
                    ETurningType = TurningType.Pear;
                }
            }
            return y;
        }
        /// <summary>
        /// 计算对边地头转弯区3
        /// </summary>
        /// <param name="r">农机最小转弯半径</param>
        /// <param name="w">农机作业幅宽</param>
        /// <param name="e">机组出线长度</param>
        /// <param name="angle">转弯区相邻角</param>
        /// <returns>转弯区宽度</returns>
        private double ComputeOppositeTurnArea(double r, double w, double e, double angle)
        {
            double type = r - w / 2;
            if (r == 0 || w == 0)
            {
                MessageBox.Show("转弯半径或作业幅度为空，无法计算！");
                return 0;
            }
            if (type < 0)
            {
                MessageBox.Show("不满足计算条件，请重新输入参数！");
                return 0;
            }
            double y = 0.0;
            if (type == 0)
            {
                //半圆形
                if (cbMachineType.SelectedItem.ToString() == "悬挂式")
                {
                    y = r + w / 2 + r * Math.Sin(angle) + Math.Abs(e * Math.Cos(angle));
                    ETurningType = TurningType.HalfCir;
                }
                else
                {
                    MessageBox.Show("农机类型不对，无法计算半圆形转弯！");
                }
            }
            else
            {
                // 鱼形尾
                if (cbMachineType.SelectedItem.ToString() == "悬挂式")
                {
                    y = r * Math.Sin(angle) + Math.Abs(r * Math.Cos(angle)) + Math.Abs(w * Math.Cos(angle) / 2) + Math.Abs(e * Math.Cos(angle));
                    ETurningType = TurningType.FishTail;
                }
                // 梨形
                else if (cbMachineType.SelectedItem.ToString() == "牵引式")
                {
                    y = w * Math.Sin(angle) / 2 + Math.Abs(Math.Cos(angle)) * (Math.Sqrt(4 * r * r - (r + w / 2) * (r + w / 2)) + e);
                    ETurningType = TurningType.Pear;
                }
            }
            return y;
        }

        /// <summary>
        /// 清空地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearMap_Click(object sender, EventArgs e)
        {
            IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
            pGraphicsContainer.DeleteAllElements();
            axMapMain.Map.ClearSelection();
            axMapMain.ClearLayers();
            axMapMain.Refresh();
        }

        /// <summary>
        /// 计算直线路径和地头转弯区边界交点
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <param name="i">交点索引</param>
        /// <returns>交点</returns>
        private IPoint ComputeCrossPoint(int areaType,int i)
        {
            IPoint pPoint = new PointClass();
            double angle = 0.0;
            double paraX = 0.0, paraY = 0.0;
            double x = 0.0, y = 0.0;
            if (areaType == 1)
            {
                angle = LiAngle[0];
                x = PA.X;
                y = PA.Y;
                paraX = (PD1.X - PA1.X) / LiLength[3];
                paraY = (PD1.Y - PA1.Y) / LiLength[3];
            }
            else if (areaType == 2)
            {
                angle = LiAngle[1];
                x = PB.X;
                y = PB.Y;
                paraX = (PC1.X - PB1.X) / LiLength[1];
                paraY = (PC1.Y - PB1.Y) / LiLength[1];
            }
            pPoint.X = x + W * (2 * i - 1) / (2 * Math.Sin(angle)) * paraX;
            pPoint.Y = y + W * (2 * i - 1) / (2 * Math.Sin(angle)) * paraY;
            return pPoint;
        }

        /// <summary>
        /// 计算地头转弯区1中第i个转弯处，相邻作业路径中线与地头边界的交点
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <param name="i">交点索引</param>
        /// <returns>交点</returns>
        private IPoint ComputeMiddlePoint(int areaType, int i)
        {
            IPoint pPoint = new PointClass();
            double angle = 0.0;
            double paraX = 0.0, paraY = 0.0;
            double x = 0.0, y = 0.0;
            if (areaType == 1)
            {
                angle = LiAngle[0];
                x = PA.X;
                y = PA.Y;
                paraX = (PD1.X - PA1.X) / LiLength[3];
                paraY = (PD1.Y - PA1.Y) / LiLength[3];
            }
            else if (areaType == 2)
            {
                angle = LiAngle[1];
                x = PB.X;
                y = PB.Y;
                paraX = (PC1.X - PB1.X) / LiLength[1];
                paraY = (PC1.Y - PB1.Y) / LiLength[1];
            }
            pPoint.X = x + W * i / Math.Sin(angle) * paraX;
            pPoint.Y = y + W * i / Math.Sin(angle) * paraY;
            return pPoint;
        }

        /// <summary>
        /// 计算地图转弯区中曲线路径顶点
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <param name="pointType">顶点类型，起点或终点</param>
        /// <param name="pPointN">路径与边界交点</param>
        /// <returns>顶点</returns>
        private IPoint ComputeFishTailLinePoint(int areaType, PointType pointType, IPoint pPointN)
        {
            IPoint pPoint = new PointClass();
            double angle = 0.0;
            double para = 0.0;
            if (areaType == 1)
            {
                angle = LiAngle[0];
                //当角1大于等于90度且求的是起始点，或角小于90度且求的是短边时
                if ((angle >= Math.PI / 2 && pointType == PointType.TurnStart) || (angle < Math.PI / 2 && pointType == PointType.TurnEnd))
                {
                    para = Math.Abs(W / (2 * Math.Tan(angle))) + E;
                }
                else
                {
                    para = E;
                }
            }
            else if (areaType == 2)
            {
                //当角1大于等于90度且求的是起始点，或角小于90度且求的是短边时
                angle = LiAngle[1];
                if ((angle >= Math.PI / 2 && pointType == PointType.TurnStart) || (angle < Math.PI / 2 && pointType == PointType.TurnEnd))
                {
                    para = -(Math.Abs(W / Math.Tan(angle)) + E);
                }
                else
                {
                    para = -E;
                }
            }
            pPoint.X = pPointN.X + para * (PA1.X - PB1.X) / LiLength[0];
            pPoint.Y = pPointN.Y + para * (PA1.Y - PB1.Y) / LiLength[0];
            return pPoint;
        }

        /// <summary>
        /// 计算地头转弯区中半圆形转弯路径圆心
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <returns>圆心</returns>
        private IPoint ComputeHalfCircleCenterPoint(int areaType, IPoint pPointM)
        {
            IPoint pPoint = new PointClass();
            double angle = 0.0;
            double para=0.0;
            if (areaType == 1)
            {
                angle = LiAngle[0];
                para = Math.Abs(W / (2 * Math.Tan(angle))) + E;
            }
            else if (areaType == 2)
            {
                angle = LiAngle[1];
                para = -(Math.Abs(W / (2 * Math.Tan(angle))) + E);
            }
            pPoint.X = pPointM.X + para * (PA1.X - PB1.X) / LiLength[0];
            pPoint.Y = pPointM.Y + para * (PA1.Y - PB1.Y) / LiLength[0];
            return pPoint;
        }

        /// <summary>
        /// 计算地头转弯区中梨形转弯路径圆心
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <returns>圆心</returns>
        private List<IPoint> ComputePearCenterPoints(int areaType, IPoint pPointM)
        {
            List<IPoint> liPointO = new List<IPoint>();
            liPointO.Add(new PointClass());
            liPointO.Add(new PointClass());
            liPointO.Add(new PointClass());
            double angle = 0.0;
            double para1 = 0.0, para2 = 0.0;
            double kx = 0.0, ky = 0.0;
            para2 = R + W / 2;
            kx = (PA1.X - PB1.X) / LiLength[0];
            ky = (PA1.Y - PB1.Y) / LiLength[0];
            if (areaType == 1)
            {
                angle = LiAngle[0];
                para1 = Math.Abs(W / (2 * Math.Tan(angle))) + E;
                liPointO[0].X = pPointM.X + para1 * kx - para2 * ky;
                liPointO[0].Y = pPointM.Y + para1 * ky + para2 * kx;
                liPointO[1].X = pPointM.X + (para1 + Math.Sqrt(4 * R * R - para2 * para2)) * kx;
                liPointO[1].Y = pPointM.Y + (para1 + Math.Sqrt(4 * R * R - para2 * para2)) * ky;
                liPointO[2].X = pPointM.X + para1 * kx + para2 * ky;
                liPointO[2].Y = pPointM.Y + para1 * ky - para2 * kx;
            }
            else if (areaType == 2)
            {
                angle = LiAngle[1];
                para1 = Math.Abs(W / (2 * Math.Tan(angle))) + E;
                liPointO[0].X = pPointM.X - para1 * kx - para2 * ky;
                liPointO[0].Y = pPointM.Y - para1 * ky + para2 * kx;
                liPointO[1].X = pPointM.X - (para1 + Math.Sqrt(4 * R * R - para2 * para2)) * kx;
                liPointO[1].Y = pPointM.Y - (para1 + Math.Sqrt(4 * R * R - para2 * para2)) * ky;
                liPointO[2].X = pPointM.X - para1 * kx + para2 * ky;
                liPointO[2].Y = pPointM.Y - para1 * ky - para2 * kx;
            }
            return liPointO;
        }


        /// <summary>
        /// 计算地头转弯区中鱼尾转弯路径圆心
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <returns>圆心</returns>
        private List<IPoint> ComputeFishTailCenterPoints(int areaType, IPoint pPointM)
        {
            List<IPoint> liPointO = new List<IPoint>();
            liPointO.Add(new PointClass());
            liPointO.Add(new PointClass());
            liPointO.Add(new PointClass());
            double angle = 0.0;
            double para1 = 0.0, para2 = 0.0;
            double kx = 0.0, ky = 0.0;
            para2 = R - W / 2;
            kx = (PA1.X - PB1.X) / LiLength[0];
            ky = (PA1.Y - PB1.Y) / LiLength[0];
            if (areaType == 1)
            {
                angle = LiAngle[0];
                para1 = Math.Abs(W / (2 * Math.Tan(angle))) + E;
            }
            else if (areaType == 2)
            {
                angle = LiAngle[1];
                para1 = -(Math.Abs(W / (2 * Math.Tan(angle))) + E);
                para2 = -para2;
            }

            liPointO[0].X = pPointM.X + para1 * kx - para2 * ky;
            liPointO[0].Y = pPointM.Y + para1 * ky + para2 * kx;
            liPointO[1].X = pPointM.X + para1 * kx + para2 * ky;
            liPointO[1].Y = pPointM.Y + para1 * ky - para2 * kx;
            return liPointO;
        }

        /// <summary>
        /// 计算地图转弯区中鱼尾形圆弧路径顶点
        /// </summary>
        /// <param name="areaType">转弯区种类，1、2、3</param>
        /// <param name="pPointO">当前圆弧的圆心</param>
        /// <returns>顶点</returns>
        private IPoint ComputeFishTailArcPoint(int areaType, IPoint pPointO)
        {
            IPoint pPoint = new PointClass();
            double kx = (PA1.X - PB1.X) / LiLength[0];
            double ky = (PA1.Y - PB1.Y) / LiLength[0];
            if (areaType == 2)
            {
                kx = -kx;
                ky = -ky;
            }
            pPoint.X = pPointO.X + R * kx;
            pPoint.Y = pPointO.Y + R * kx;
            return pPoint;
        }
    }

    /// <summary>
    /// 地图点击模式
    /// </summary>
    enum MapClickMode
    {
        /// <summary>
        /// 普通模式，支持工具栏操作
        /// </summary>
        Normal,
        /// <summary>
        /// 提取地块信息模式
        /// </summary>
        SelectBlock,
        /// <summary>
        /// 查看要素属性信息
        /// </summary>
        QueryAttribute
    }

    /// <summary>
    /// 点类型
    /// </summary>
    enum PointType
    {
        /// <summary>
        /// 转弯起始点
        /// </summary>
        TurnStart,
        /// <summary>
        /// 转弯结束点
        /// </summary>
        TurnEnd,
        /// <summary>
        /// 鱼尾形直线部分起始点
        /// </summary>
        FishTailLineStart,
        /// <summary>
        /// 鱼尾形直线部分结束点
        /// </summary>
        FishTailLineEnd,
    }

    /// <summary>
    /// 转弯类型
    /// </summary>
    enum TurningType
    {
        /// <summary>
        /// 半圆形
        /// </summary>
        HalfCir,
        /// <summary>
        /// 梨形
        /// </summary>
        Pear,
        /// <summary>
        /// 鱼尾形
        /// </summary>
        FishTail,
    }

}
