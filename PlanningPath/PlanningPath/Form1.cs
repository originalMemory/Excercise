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
        #region 通用变量
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
        #endregion

        #region 控件事件
        public Form1()
        {
            InitializeComponent();
            //默认加载地图
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

        /// <summary>
        /// 地图点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMapMain_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            //当为鼠标左键单击且处于需要对地图进行操作时
            if (e.button == 1 && EMapClickMode != MapClickMode.Normal)
            {
                //清除之前绘制的临时图形
                IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
                pGraphicsContainer.DeleteAllElements();

                //清除上次高亮部分
                axMapMain.Map.ClearSelection();        
                axMapMain.Refresh();
                axMapMain.CurrentTool = null;

                //获取地图上的第一个图层
                IFeatureLayer pFeatureLayer = axMapMain.get_Layer(0) as IFeatureLayer;

                //创建筛选条件
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                //IQueryFilter是IQueryFilter的基类，很多接口为保障通用性，均是以基类为参数类型，实际使用时需要先转为基类
                //文件中其他强转格式的也是同样的原因
                IQueryFilter pQueryFilter = pSpatialFilter as IQueryFilter;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;    //用交集作为筛选条件判断
                //feature游标，用来获取查询到的feature
                IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
                //将Feature图层视为一个特征选择器
                IFeatureSelection pFeatureSelection = pFeatureLayer as IFeatureSelection;

                //如果为提取地块信息模式
                if (EMapClickMode == MapClickMode.SelectBlock)
                {
                    //定位并生成点击时所在点
                    IPoint pPoint = new PointClass();
                    pPoint.X = e.mapX;
                    pPoint.Y = e.mapY;

                    //选取点击位置所在的要素。
                    //IGeometry是点、线、面等地图元素的基类，很多接口为保障通用性，均是以基类为参数类型，实际使用时需要先转为基类
                    IGeometry pGeometry = pPoint as IGeometry;
                    //设置筛选条件中用作判断条件的元素
                    pSpatialFilter.Geometry = pGeometry;
                    //查询，并将所有查询结果创建一个新的选择（选择的部分会被高亮）
                    pFeatureSelection.SelectFeatures(pQueryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

                    //获取查询的第一个feature
                    IFeature pFeature = pFeatureCursor.NextFeature();
                    if (pFeature != null)
                    {
                        QueryBlock(pFeature);
                    }

                }
                //如果为查询属性模式（feature的属性信息，如所有人，归属地等自定义信息）
                else if (EMapClickMode == MapClickMode.QueryAttribute)
                {
                    //从地图上选取一个矩形区域。类似的还可以选取线或圆形区域
                    IGeometry pGeometry = axMapMain.TrackRectangle();

                    List<IFeature> pFeatureList = new List<IFeature>(); //和选中矩形区域有交集的feature列表
                    //设置筛选条件中用作判断条件的元素
                    pSpatialFilter.Geometry = pGeometry;
                    //查询，并将所有查询结果创建一个新的选择（选择的部分会被高亮）
                    pFeatureSelection.SelectFeatures(pQueryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    //获取所有查询到的feature
                    IFeature pFeature = pFeatureCursor.NextFeature();
                    while (pFeature != null)
                    {
                        pFeatureList.Add(pFeature);
                        //将每个被选择的要素都加入列表中
                        pFeature = pFeatureCursor.NextFeature();
                        //光标指向下一个要素
                    }
                    //此处是新建了一个窗口单独用来显示查询到的属性信息
                    AttributesForm pAttributeForm = new AttributesForm();
                    pAttributeForm.dataGridView1.RowCount = pFeatureList.Count + 1;
                    //设置边界风格
                    pAttributeForm.dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
                    //设置列数及列标题
                    pAttributeForm.dataGridView1.ColumnCount = pFeatureList[0].Fields.FieldCount;
                    for (int m = 0; m < pFeatureList[0].Fields.FieldCount; m++)
                    {
                        pAttributeForm.dataGridView1.Columns[m].HeaderText = pFeatureList[0].Fields.get_Field(m).AliasName;
                    }
                    //遍历feature
                    for (int i = 0; i < pFeatureList.Count; i++)
                    {
                        pFeature = pFeatureList[i];
                        for (int j = 0; j < pFeature.Fields.FieldCount; j++)
                        {
                            //填入属性值
                            pAttributeForm.dataGridView1[j, i].Value = pFeature.get_Value(j).ToString();
                        }
                    }
                    pAttributeForm.Show();
                }
                //局部刷新地图，只刷新创建的地图选择部分。也可以全局刷新
                axMapMain.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            }
        }

        /// <summary>
        /// 提取地块事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectBlock_Click(object sender, EventArgs e)
        {
            ResetMapAndBtn();
            if (EMapClickMode != MapClickMode.SelectBlock)
            {
                EMapClickMode = MapClickMode.SelectBlock;
                //修改鼠标类型
                axMapMain.MousePointer = esriControlsMousePointer.esriPointerIdentify;
                btnSelectBlock.Text = "取消提取";
            }
            else
            {
                EMapClickMode = MapClickMode.Normal;
            }
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
            EMapClickMode = MapClickMode.Normal;
            axMapMain.CurrentTool = null;
        }
        
        /// <summary>
        /// 属性查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 属性查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EMapClickMode = MapClickMode.QueryAttribute;
            //修改鼠标类型
            axMapMain.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
        }

        /// <summary>
        /// 计算地头宽度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnComputeWidth_Click(object sender, EventArgs e)
        {
            //清除地图上无关信息并重置地图点击模式
            ResetMapAndBtn();
            EMapClickMode = MapClickMode.Normal;
            axMapMain.CurrentTool = null;

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
            //DrawPoint(PA);

            PB.X = PB1.X + (Y2 / Math.Sin(LiAngle[1])) * ((PA1.X - PB1.X) / LiLength[0]);
            PB.Y = PB1.Y + (Y2 / Math.Sin(LiAngle[1])) * ((PA1.Y - PB1.Y) / LiLength[0]);
            tbPointB.Text = string.Format("{0:F2},{1:F2}", PB.X, PB.Y);
            //DrawPoint(PB);

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
            //DrawPoint(PC);
            tbPointD.Text = string.Format("{0:F2},{1:F2}", PD.X, PD.Y);
            //DrawPoint(PD);

            //显示作业区域
            IPolyline pPolyline = new PolylineClass();
            IPointCollection pPointCol = pPolyline as IPointCollection;
            pPointCol.AddPoint(PA);
            pPointCol.AddPoint(PB);
            pPointCol.AddPoint(PC);
            pPointCol.AddPoint(PD);
            pPointCol.AddPoint(PA);
            DrawLine(pPolyline, GetRandomRGB(), 2);
        }

        /// <summary>
        /// 计算并绘制路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            ComputePath();
        }
        #endregion

        #region 辅助函数
        /// <summary>
        /// 打开shp格式地图文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        private void OpenShapefile(string filePath)
        {
            string xjShpFolder = System.IO.Path.GetDirectoryName(filePath);
            string xjShpFileName = System.IO.Path.GetFileName(filePath);
            //创建工作工厂+工作空间
            IWorkspaceFactory xjShpWsF = new ShapefileWorkspaceFactory();
            IFeatureWorkspace xjShpFWs = (IFeatureWorkspace)xjShpWsF.OpenFromFile(xjShpFolder, 0);
            //从文件中读取并新建矢量图层：要素+名称
            IWorkspace xjShpWs = xjShpWsF.OpenFromFile(xjShpFolder, 0);
            IFeatureClass xjShpFeatureClass = xjShpFWs.OpenFeatureClass(xjShpFileName);
            IFeatureLayer xjShpFeatureLayer = new FeatureLayer();
            xjShpFeatureLayer.FeatureClass = xjShpFeatureClass;
            xjShpFeatureLayer.Name = xjShpFeatureClass.AliasName;
            //加载刷新
            this.axMapMain.AddLayer(xjShpFeatureLayer);
            this.axMapMain.ActiveView.Refresh();
        }

        /// <summary>
        /// 重置按钮和地图模块为初始状态
        /// </summary>
        private void ResetMapAndBtn()
        {
            btnSelectBlock.Text = "提取地块";
            //重置鼠标类型
            axMapMain.MousePointer = esriControlsMousePointer.esriPointerDefault;

            //清除之前绘制的临时图形
            IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
            pGraphicsContainer.DeleteAllElements();

            //清除上次高亮部分
            axMapMain.Map.ClearSelection();
            axMapMain.Refresh();
            axMapMain.CurrentTool = null;
        }

        /// <summary>
        /// 判断多边形是否为顺时针
        /// </summary>
        /// <param name="pPointCol">多边形</param>
        /// <returns>是否为顺时针</returns>
        private bool IsClock(IGeometry shape)
        {
            IPointCollection pPointCol = shape as IPointCollection;
            IPoint pPoint1 = pPointCol.get_Point(0);
            IPoint pPoint2 = pPointCol.get_Point(1);
            IPoint pPoint3 = pPointCol.get_Point(2);

            double z = (pPoint2.X - pPoint1.X) * (pPoint3.Y - pPoint2.Y) - (pPoint2.Y - pPoint1.Y) * (pPoint3.X - pPoint2.X);

            return z < 0;
        }

        /// <summary>
        /// 获取逆时针地块的下一个点
        /// </summary>
        /// <param name="nowIdx">当前点索引</param>
        /// <param name="num">点总数</param>
        /// <returns>下一个点的索引</returns>
        private int GetNextLoopPoint(int nowIdx, int num)
        {
            if (nowIdx == num - 1)
            {
                return 0;
            }
            else
            {
                return nowIdx + 1;
            }
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
            //使用向量余弦公式计算
            double ma_x = first.X - center.X;
            double ma_y = first.Y - center.Y;
            double mb_x = second.X - center.X;
            double mb_y = second.Y - center.Y;
            double v1 = (ma_x * mb_x) + (ma_y * mb_y);
            double ma_val = Math.Sqrt(ma_x * ma_x + ma_y * ma_y);
            double mb_val = Math.Sqrt(mb_x * mb_x + mb_y * mb_y);
            double cosM = v1 / (ma_val * mb_val);
            double angleAMB = Math.Acos(cosM) * 180 / Math.PI;  //Math系列三角函数计算无论是要求参数或是返回值，但凡是涉及角的均为弧度制！
            return angleAMB;
        }

        /// <summary>
        /// 在地图上画点
        /// </summary>
        /// <param name="pPoint">要画的点</param>
        /// <param name="pRGB">颜色</param>
        /// <param name="size">大小</param>
        private void DrawPoint(IPoint pPoint, IRgbColor pRGB = null, double size = 5)
        {
            //设置点的大小、颜色和类型。根据要素的类型不同，使用不同的Symbol类，如画线时使用的就是ISimpleLineSymbol
            ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbol();
            pMarkerSymbol.Size = size;
            if (pRGB != null)
            {
                pMarkerSymbol.Color = pRGB;
            }
            pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            //创建地图元素，同样根据不同的元素创建不同类型的Element
            IMarkerElement pMarkerElement = new MarkerElementClass();
            IElement pElement = pMarkerElement as IElement;
            //设置元素内容和外形
            pElement.Geometry = pPoint as IGeometry;
            pMarkerElement.Symbol = pMarkerSymbol;
            //获取当前地图并视为IGraphicsContainer。在地图上绘制临时内容（不新建图层）时通常使用该对象
            IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
            //将元素添加到地图上，并指定索引位置
            pGraphicsContainer.AddElement(pElement, 0);
            //局部刷新地图
            axMapMain.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 在地图上画线
        /// </summary>
        /// <param name="pLine">要标记的线</param>
        /// <param name="pRGB">颜色</param>
        /// <param name="width">线宽</param>
        private void DrawLine(IPolyline pPolyline, IRgbColor pRGB = null, double width = 2)
        {
            //设置线的类型、宽度和颜色
            ISimpleLineSymbol lineSymbol = new SimpleLineSymbolClass();
            lineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            lineSymbol.Width = width;
            if (pRGB != null)
            {
                lineSymbol.Color = pRGB;
            }

            //创建地图元素，同样根据不同的元素创建不同类型的Element
            ILineElement pLineElement = new LineElementClass();
            pLineElement.Symbol = lineSymbol;
            //设置元素内容和外形
            IElement pElement = pLineElement as IElement;
            pElement.Geometry = pPolyline as IGeometry;
            //获取当前地图并视为IGraphicsContainer。在地图上绘制临时内容（不新建图层）时通常使用该对象
            IGraphicsContainer pGraphicsContainer = axMapMain.Map as IGraphicsContainer;
            //将元素添加到地图上，并指定索引位置
            pGraphicsContainer.AddElement(pElement, 0);
            //局部刷新地图
            axMapMain.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 随机数
        /// </summary>
        Random Rd = new Random();
        /// <summary>
        /// 生成一个随机的RGB颜色
        /// </summary>
        /// <returns>RGB颜色</returns>
        private IRgbColor GetRandomRGB()
        {
            int n = 256;
            return GetRGB(Rd.Next(0, n), Rd.Next(0, n), Rd.Next(0, n));
        }

        /// <summary>
        /// 根据RGB值生成Arcengine可用的RGB颜色
        /// </summary>
        /// <param name="r">红色</param>
        /// <param name="g">绿色</param>
        /// <param name="b">蓝色</param>
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
        #endregion

        #region 转弯路径算法
        /// <summary>
        /// 查询选中feature的地块信息
        /// </summary>
        /// <param name="pFeature">选中feature</param>
        private void QueryBlock(IFeature pFeature)
        {
            //pFeature.Shape为选中feature对应的地块。根据地图类型不同，可能为点、多点、线和面
            //IPointCollection是将其视为点的集合，通过对点集合操作查询或修改地块信息。ISegmentCollection同理。
            IPointCollection pPointCol = pFeature.Shape as IPointCollection;
            ISegmentCollection pSegCol = pFeature.Shape as ISegmentCollection;
            //此处判断四边形地块的顶点数为5是因为终点和起点是同一个点，要有5个点才能有4条边。
            if (pPointCol.PointCount != 5)
            {
                MessageBox.Show("选中地块不是四边形地块！");
                return;
            }
            //判断多边形是否为顺时针。当前的路径计算算法只针对逆时针，顺时针的尚未完善
            bool bClock = IsClock(pFeature.Shape);
            List<IPoint> liPoint = new List<IPoint>();  //顶点列表
            List<ISegment> liEdge = new List<ISegment>();   //边列表
            //从起点开始按逆时针读取点和边
            if (bClock)
            {
                for (int i = pPointCol.PointCount - 1; i >= 1; i--)
                {
                    liPoint.Add(pPointCol.get_Point(i));
                    liEdge.Add(pSegCol.get_Segment(i - 1));
                }
            }
            else
            {
                for (int i = 0; i < pPointCol.PointCount - 1; i++)
                {
                    liPoint.Add(pPointCol.get_Point(i));
                    liEdge.Add(pSegCol.get_Segment(i));
                }
            }

            ////标记顶点
            //for (int i = 0; i < liPoint.Count; i++)
            //{
            //    DrawPoint(pPointCol.get_Point(i));
            //}

            //遍历所有边并获取最长边
            ISegment pMaxEdge = liEdge[0];   //最长边
            List<double> liLength = new List<double> { pMaxEdge.Length };
            int maxIdx = 0;
            for (int i = 1; i < liEdge.Count; i++)
            {
                ISegment pSeg = liEdge[i];
                liLength.Add(pSeg.Length);
                if (pSeg.Length > pMaxEdge.Length)
                {
                    pMaxEdge = pSeg;
                    maxIdx = i;
                }

            }

            //计算作业区域
            LiLength.Clear();
            int i1 = 0, i2 = 0, i3 = 0, i4 = 0;
            i1 = maxIdx;
            i2 = GetNextLoopPoint(i1, liPoint.Count);
            i3 = GetNextLoopPoint(i2, liPoint.Count);
            i4 = GetNextLoopPoint(i3, liPoint.Count);
            LiLength = new List<double> { liLength[i1], liLength[i2], liLength[i3], liLength[i4] };
            PA1 = liPoint[i1];
            PB1 = liPoint[i2];
            PC1 = liPoint[i3];
            PD1 = liPoint[i4];

            //将作业区域用线绘制出来。绘线需为IPolyline类型的元素
            IPolyline pPolyline = new PolylineClass();
            //使用FromPoint和ToPoint可以绘制单根直线，若想绘制多线段或包含曲线，需将其视为IPointCollection或ISegmentCollection后再操作
            pPolyline.FromPoint = pMaxEdge.FromPoint;
            pPolyline.ToPoint = pMaxEdge.ToPoint;
            DrawLine(pPolyline);


            //计算所有角度
            int centerIdx = 0;  //直线交点索引，从起点开始
            int preIdx = 3;  //第一条直线上的点的索引
            int nextIdx = 1;  //第二条直线上的点的索引
            List<double> liAngle = new List<double>();  //直线夹角列表
            maxIdx = centerIdx;     //最长边起始角度在liAngle的索引
            do
            {
                //获取当前顶点并计算顶点角度
                IPoint pCenterPoint = liPoint[centerIdx];
                double angle = ComputerAngleBetweenTwoLine(pCenterPoint, liPoint[preIdx], liPoint[nextIdx]);
                liAngle.Add(angle);
                //判断是否是最长边的起始角
                if (pCenterPoint.X == pMaxEdge.FromPoint.X && pCenterPoint.Y == pMaxEdge.FromPoint.Y)
                {
                    maxIdx = centerIdx;
                }
                //计算下一个顶点
                preIdx = centerIdx;
                centerIdx = nextIdx;
                nextIdx = GetNextLoopPoint(nextIdx, liPoint.Count);
            } while (nextIdx != 1);
            //计算最长边对面点的垂线和对面边的夹角
            int tpIdx = GetNextLoopPoint(maxIdx, liPoint.Count);    //最长边结束点
            int oppositeIdx = GetNextLoopPoint(tpIdx, liPoint.Count);    //对面边上点的索引位置
            IPoint projectPoint = ComputerProjectivePoint(liPoint[maxIdx], liPoint[tpIdx], liPoint[oppositeIdx]); //投影点
            centerIdx = oppositeIdx;
            oppositeIdx = GetNextLoopPoint(oppositeIdx, liPoint.Count);
            ProjectAngle = ComputerAngleBetweenTwoLine(liPoint[centerIdx], projectPoint, liPoint[oppositeIdx]);
            //将角度转化为弧度
            ProjectAngle = ProjectAngle * (Math.PI / 180);

            //pPolyline.FromPoint = liPoint[centerIdx];
            //pPolyline.ToPoint = projectPoint;
            //MarkLine(pPolyline);

            string angleLabel1 = "角度一：";
            string angleLabel2 = "角度二：";
            string angleLabel3 = "角度三：";
            string angleLabel4 = "角度四：";
            LiAngle.Clear();
            liAngle = liAngle.Select(x => x * (Math.PI / 180)).ToList();
            switch (maxIdx)
            {
                case 0:
                    angleLabel1 = "角度一(最长边)：";
                    angleLabel2 = "角度二(最长边)：";
                    LiAngle = new List<double> { liAngle[0], liAngle[1], liAngle[2], liAngle[3] };
                    break;
                case 1:
                    angleLabel2 = "角度二(最长边)：";
                    angleLabel3 = "角度三(最长边)：";
                    LiAngle = new List<double> { liAngle[1], liAngle[2], liAngle[3], liAngle[0] };
                    break;
                case 2:
                    angleLabel3 = "角度三(最长边)：";
                    angleLabel4 = "角度四(最长边)：";
                    LiAngle = new List<double> { liAngle[2], liAngle[3], liAngle[0], liAngle[1] };
                    break;
                case 3:
                    angleLabel4 = "角度四(最长边)：";
                    angleLabel1 = "角度一(最长边)：";
                    LiAngle = new List<double> { liAngle[3], liAngle[0], liAngle[1], liAngle[2] };
                    break;
                default:
                    break;
            }
            //展示相关信息
            FormBlockInfo info = new FormBlockInfo();
            info.SetInfo(pMaxEdge.Length, liLength, ProjectAngle / Math.PI * 180, liAngle, angleLabel1, angleLabel2, angleLabel3, angleLabel4);
            info.Show();
        }

        /// <summary>
        /// 计算转弯路径
        /// </summary>
        private void ComputePath()
        {
            List<IPoint> liPt1 = new List<IPoint>();    //转弯区1顶点集合
            List<IPoint> liPt2 = new List<IPoint>();    //转弯区2顶点集合
            List<IPoint> liPtM = new List<IPoint>();    //相邻直线路径中线点集合
            int i = 1;
            IPoint pPointN1;
            IPoint pPointN2;
            IPoint pPointM;
            
            bool bN1 = ProjectAngle < Math.PI;  //判断哪一个转弯区较长
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
                //DrawPoint(pPointN1, pRGB);
                //DrawPoint(pPointN2, pRGB);
                //DrawPoint(pPointM, pRGB);
                i++;
            }
            pRGB = GetRandomRGB();
            //此处因为多线段包含直线和曲线，故用ISegmentCollection的方式生成
            IPolyline pPolylineWork = new PolylineClass();
            ISegmentCollection pSegCol = pPolylineWork as ISegmentCollection;
            //遍历所有直线，建立路径
            IPoint pPointP, pPointQ;    //半圆起始点和结束点
            for (i = 0; i < liPt1.Count - 1; i++)
            {
                //创建三条直线。直线的创建只需设置起止点
                ILine pLine1 = new LineClass();
                ILine pLine2 = new LineClass();
                ILine pLine3 = new LineClass();
                ISegmentCollection pTurningPath = null;
                //偶数时在转弯区1转弯
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
                //奇数时在转弯区2转弯
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
                            pTurningPath = ComputeTurnFishTail(2, liPtM[i], pPointP, pPointQ);
                            break;
                        default:
                            break;
                    }
                }
                //DrawPoint(pPointP, pRGB);
                //DrawPoint(pPointQ, pRGB);
                pLine2.ToPoint = pPointP;
                pLine3.FromPoint = pPointQ;

                pSegCol.AddSegment(pLine1 as ISegment);
                pSegCol.AddSegment(pLine2 as ISegment);
                pSegCol.AddSegmentCollection(pTurningPath);
                pSegCol.AddSegment(pLine3 as ISegment);
            }
            //结束时的直线
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
            pSegCol.AddSegment(pEndLine as ISegment);

            DrawLine(pPolylineWork, GetRandomRGB(), 2);
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
            //创建转弯路径
            ISegmentCollection pSegCol = new PolylineClass() as ISegmentCollection;
            IPoint pPointO = ComputeHalfCircleCenterPoint(areaType, pPointM);
            //DrawPoint(pPointO);
            //设置绘制圆弧时是顺时针还是逆时针绘制
            esriArcOrientation oriType = esriArcOrientation.esriArcClockwise;
            if (areaType == 2)
            {
                oriType = esriArcOrientation.esriArcCounterClockwise;
            }
            //创建曲线，有按圆心和起止点创建和按圆心、半径和起止角（弧度制）创建两种
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

            //创建转弯路径
            ISegmentCollection pSegCol = new PolylineClass() as ISegmentCollection;
            List<IPoint> liPointO = ComputePearCenterPoints(areaType, pPointM);
            //DrawPoint(liPointO[0]);
            //DrawPoint(liPointO[1]);
            //DrawPoint(liPointO[2]);

            ICircularArc pCirArc1 = new CircularArcClass();
            ICircularArc pCirArc2 = new CircularArcClass();
            ICircularArc pCirArc3 = new CircularArcClass();
            double angle = Math.Acos((R + W / 2) / (2 * R));
            if (areaType == 1)
            {
                //按圆心、半径和起止角（弧度制）创建圆弧
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

            //创建转弯路径
            ISegmentCollection pSegCol = new PolylineClass() as ISegmentCollection;
            List<IPoint> liPointO = ComputeFishTailCenterPoints(areaType, pPointM);
            //DrawPoint(liPointO[0]);
            //DrawPoint(liPointO[1]);

            IPoint pPointT = ComputeFishTailArcPoint(areaType, liPointO[0]);
            IPoint pPointS = ComputeFishTailArcPoint(areaType, liPointO[1]);
            //DrawPoint(pPointT);
            //DrawPoint(pPointS);
            ILine pLine = new LineClass();
            pLine.FromPoint = pPointT;
            pLine.ToPoint = pPointS;

            ICircularArc pCirArc1 = new CircularArcClass();
            ICircularArc pCirArc2 = new CircularArcClass();
            double angle = Math.Acos((R + W / 2) / (2 * R));
            if (areaType == 1)
            {
                pCirArc1.PutCoords(liPointO[0], pPointP, pPointT, esriArcOrientation.esriArcClockwise);
                pCirArc2.PutCoords(liPointO[1], pPointS, pPointQ, esriArcOrientation.esriArcClockwise);
            }
            else if (areaType == 2)
            {
                pCirArc1.PutCoords(liPointO[0], pPointP, pPointT, esriArcOrientation.esriArcCounterClockwise);
                pCirArc2.PutCoords(liPointO[1], pPointS, pPointQ, esriArcOrientation.esriArcCounterClockwise);
            }
            pSegCol.AddSegment(pCirArc1 as ISegment);
            pSegCol.AddSegment(pLine as ISegment);
            pSegCol.AddSegment(pCirArc2 as ISegment);

            return pSegCol;
        }

        /// <summary>
        /// 求直线外一点到该直线的投影点
        /// </summary>
        /// <param name="pLine1">线上任一点，两点描述一条线</param>
        /// <param name="pLine2">线上任一点，两点描述一条线</param>
        /// <param name="pOut">线外指定点</param>
        /// <returns>投影点</returns>
        private IPoint ComputerProjectivePoint(IPoint pLine1, IPoint pLine2, IPoint pOut)
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
            double y = 0.0;
            if (type <= 0)
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
            double y = 0.0;
            if (type <= 0)
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
            }

            liPointO[0].X = pPointM.X + para1 * kx + para2 * ky;
            liPointO[0].Y = pPointM.Y + para1 * ky - para2 * kx;
            liPointO[1].X = pPointM.X + para1 * kx - para2 * ky;
            liPointO[1].Y = pPointM.Y + para1 * ky + para2 * kx;
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
            pPoint.Y = pPointO.Y + R * ky;
            return pPoint;
        }
        #endregion
    }

    #region 枚举
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
        /// 查看feature属性信息（feature的属性信息，如所有人，归属地等自定义信息）
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
    #endregion
}
