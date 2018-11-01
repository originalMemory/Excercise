#!/usr/bin/env python
# -*- coding: utf-8 -*-

"""
本文件用于对hdf文件进行校正
作者：吴厚波
创建时间：2018-6-10
更新时间：2018-6-11
"""

import gdal
import json
import numpy
import os
import re

class hdf_geolocation:
    _source_file_reg = None  # 源hdf文件正则表达式
    _source_folder = None  # 原hdf文件夹路径
    _target_folder = None  # 目标envi文件夹路径
    _geoloc_path = None  # 校正所需经纬度对照表文件路径
    _latitude_start = -90  # 起始纬度
    _latitude_end = -90  # 结束纬度
    _longitude_start = 0  # 起始经度
    _longitude_end = 180  # 结束经度
    _width = 0  # 输出图像宽度
    _height = 0  # 输出图像高度
    _xRes = 0  # x方向分辨率大小
    _yRes = 0  # y方向分辨率大小
    # _name = 0  # 本次任务名
    _hdf_file_paths = []  # 本次要处理的所有hdf文件路径
    _select_bands = []  # 本次要转换的波段列表
    _vrt_path = 'temp.vrt'  # 临时校正参数文件路径

    def __init__(self, task_path):
        """
        初始化
        :param task_path: 任务json文件路径
        """

        # 读取json文件
        with open(task_path, 'r', encoding='utf-8') as f:
            temp = json.load(f)
            tasks = temp['ProjTaskItems'][0]
        self._width = tasks['width']
        self._height = tasks['height']
        self._geoloc_path = tasks['nomFile']
        self._xRes = tasks['resLon']
        self._yRes = tasks['resLat']
        centerLon = tasks['centerLon']
        centerLat = tasks['centerLat']
        self._source_file_reg = tasks['fileFilter']
        self._source_folder = tasks['inDirOrFile']
        self._target_folder = tasks['outDirOrFile']
        self._select_bands = tasks['selectBands']

        # region 计算经纬度范围
        all_lon = self._width * self._xRes
        tp = centerLon - all_lon / 2
        if tp > 0 and tp < 180:
            self._longitude_start = tp
        tp = centerLon + all_lon / 2
        if tp > 0 and tp < 180:
            self._longitude_end = tp

        all_lat = self._width * self._xRes
        tp = centerLat - all_lat / 2
        if tp > -90 and tp < 90:
            self._latitude_start = tp
        tp = centerLat + all_lat / 2
        if tp > -90 and tp < 90:
            self._latitude_end = tp
        # endregion

        # 获取要处理的所有hdf文件路径
        recom_hdf = re.compile(self._source_file_reg)  # 构建正则匹配
        for root, dirs, files in os.walk(self._source_folder):
            for x in files:
                if re.search(recom_hdf, x):
                    path = os.path.join(root, x)
                    self._hdf_file_paths.append(path)
                    print(path)

        print('参数解析完成！')

    def start(self):
        """
        校正hdf文件
        :return:
        """
        # 检查参数
        if self._width == 0 or self._height == 0 or self._geoloc_path == None or self._xRes == 0 or self._yRes == 0 or \
                self._source_folder == None or self._source_file_reg == None or self._target_folder == None:
            print('参数有误！')
            return -1

        # 遍历校正hdf文件
        i = 1
        for hdf_path in self._hdf_file_paths:
            self.construct_vrt(hdf_path)
            # 获取新的文件名
            filename = os.path.split(hdf_path)[1][:-4]
            # 删除目标文件夹中已存在文件
            for root, dir, files in os.walk(self._target_folder):
                for x in files:
                    if re.search(filename, x):
                        os.remove(os.path.join(root, x))
            new_path = os.path.join(self._target_folder, filename + '.nc')  # 输出文件名
            # 成功
            gdal.Warp(new_path, self._vrt_path, geoloc=True, format='ENVI', outputBounds=
            [self._longitude_start, self._latitude_start, self._longitude_end, self._latitude_end], dstSRS='EPSG:4326',
                      width=self._width, height=self._height)  # xRes=0.04,yRes=0.04 指定输出面积后无法指定分辨率
            os.remove(self._vrt_path)  # 删除临时校正参数文件
            print('[{}/{}]{}已校正！'.format(i, len(self._hdf_file_paths), filename))
            i += 1
        return 0

    def construct_vrt(self, hdf_path):
        """
        构造一个hdf文件的完整的校正用vrt文件
        :param hdf_path:hdf文件路径
        """

        gdal.AllRegister()
        # 生成第一个波段的vrt文件做模板
        basic_band_path = 'HDF5:"{}"://{}'
        band_path = basic_band_path.format(hdf_path, self._select_bands[0])
        dataset = gdal.Open(band_path, gdal.GA_ReadOnly)
        gdal.Translate(self._vrt_path, dataset, format='VRT')
        dataset = None
        # 写入geolocation信息
        # region 废弃XML写入方法
        # tree=ET.parse(vrt_path)
        # root=tree.getroot()
        # vatRasterBand=root.find('VRTRasterBand')
        # geo_met=Element('Metadata',{'domain':'GEOLOCATION'})
        # line_off=Element('MDI',{'key':'LINE_OFFSET'})
        # line_off.text='1'
        # geo_met.append(line_off)
        # line_step=Element('MDI',{'key':'LINE_STEP'})
        # line_step.text='1'
        # geo_met.append(line_step)
        # pixel_off=Element('MDI',{'key':'PIXEL_OFFSET'})
        # pixel_off.text='1'
        # geo_met.append(pixel_off)
        # pixel_step=Element('MDI',{'key':'PIXEL_STEP'})
        # pixel_step.text='1'
        # geo_met.append(pixel_step)
        # srs=Element('MDI',{'key':'SRS'})
        # srs.text='GEOGCS["WGS84",DATUM["WGS_1984",SPHEROID["WGS84",6378137,298.257223563,\
        # AUTHORITY["EPSG","7030"]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY["EPSG","6326"]],PRIMEM["Greenwich",0,AUTHORITY\
        # ["EPSG","8901"]],UNIT["degree",0.0174532925199433,AUTHORITY["EPSG","9108"]],AUTHORITY["EPSG","4326"]]'
        # geo_met.append(srs)
        # x_band=Element('MDI',{'key':'X_BAND'})
        # x_band.text='1'
        # geo_met.append(x_band)
        # x_dataset=Element('MDI',{'key':'X_DATASET'})
        # x_dataset.text='FY4A_4000.dat'
        # geo_met.append(x_dataset)
        # y_band=Element('MDI',{'key':'Y_BAND'})
        # y_band.text='1'
        # geo_met.append(y_band)
        # y_dataset=Element('MDI',{'key':'Y_DATASET'})
        # y_dataset.text='FY4A_4000.dat'
        # geo_met.append(y_dataset)
        # root.append(geo_met)
        # tree.write(vrt_path, encoding="utf-8",xml_declaration=True)
        # endregion
        met_loc = """
  <Metadata domain="GEOLOCATION">  
    <MDI key="LINE_OFFSET">-1</MDI>  
    <MDI key="LINE_STEP">1</MDI>  
    <MDI key="PIXEL_OFFSET">-1</MDI>  
    <MDI key="PIXEL_STEP">1</MDI>  
    <MDI key="SRS">GEOGCS["WGS84",DATUM["WGS_1984",SPHEROID\["WGS84",6378137,298.257223563,AUTHORITY["EPSG","7030"]]\
,TOWGS84[0,0,0,0,0,0,0],AUTHORITY["EPSG","6326"]],PRIMEM["Greenwich",0,AUTHORITY["EPSG","8901"]],UNIT["degree"\
,0.0174532925199433,AUTHORITY["EPSG","9108"]],AUTHORITY["EPSG","4326"]]</MDI>  
    <MDI key="X_BAND">1</MDI>  
    <MDI key="X_DATASET">{}</MDI>  
    <MDI key="Y_BAND">2</MDI>  
    <MDI key="Y_DATASET">{}</MDI>  
  </Metadata>""".format(self._geoloc_path, self._geoloc_path)
        with open(self._vrt_path, 'r', encoding='utf-8') as f:
            content = f.read()
            pos = content.find('</Metadata>')
        with open(self._vrt_path, 'w', encoding='utf-8') as f:
            if pos != -1:
                pos += 11
                content = content[:pos] + met_loc + content[pos:]
                f.write(content)
            else:
                print('无数据')
        print('Geolocation及波段1/{}构建完成'.format(len(self._select_bands)))

        pos = content.find('</VRTRasterBand>') + 16  # 定位至波段1位置之后
        remain_bands = ''  # 余下所有波段的相关信息
        # 写入其余波段的信息
        for i in range(1, len(self._select_bands)):
            vrt_path2 = 'temp2.vrt'
            band_path = basic_band_path.format(hdf_path, self._select_bands[i])
            dataset = gdal.Open(band_path, gdal.GA_ReadOnly)
            gdal.Translate(vrt_path2, dataset, format='VRT')
            dataset = None
            with open(vrt_path2, 'r') as f:
                tp_con = f.read()
                tp_band = re.search(r'<VRTRasterBand[\s\S]*?</VRTRasterBand>', tp_con).group()
                tp_band = re.sub('band="1"', 'band="{}"'.format(i + 1), tp_band)
                remain_bands += '\n  ' + tp_band
                print('波段{}/{}构建完成'.format(i + 1, len(self._select_bands)))
        os.remove(vrt_path2)  # 删除临时构造文件
        with open(self._vrt_path, 'w', encoding='utf-8') as f:
            if pos > 15:
                content = content[:pos] + remain_bands + content[pos:]
                f.write(content)
            else:
                print('{}波段无数据'.format(i + 1))


hdf = hdf_geolocation('ProjTask.json')
result = hdf.start()
if result < 0:
    print('运行出错！')

# region 历史代码（暂废）
# temp_data=gdal.Open('temp.nc',gdal.GA_Update)
# form=temp_data.GetGeoTransform()
# form=list(form)
# form[0]=form[0]+104.7
# print(form)
# temp_data.SetGeoTransform(form)

# temp_data=gdal.Warp('','01.vrt',geoloc=True,format='MEM',outputBounds = [60, 0, 150, 60])
# width=temp_data.RasterXSize
# height=temp_data.RasterYSize
# print(temp_data.GetDescription())
# c=temp_data.GetRasterBand(1)
# print(c.GetDescription())
# arr=c.ReadAsArray()
# enviDriver=gdal.GetDriverByName('ENVI')
# target=enviDriver.Create('main.nc',width,height,2) #,options=["INTERLEAVE=BIP"]
# bind1=target.GetRasterBand(1)
# bind1.WriteArray(arr,0,0)

# target.SetProjection(temp_data.GetProjection())
# target.SetGeoTransform(temp_data.GetGeoTransform())
# target=enviDriver.CreateCopy('main.nc',temp_data)
# temp_data=gdal.Open('main.nc',gdal.GA_Update)
# form=temp_data.GetGeoTransform()
# form=list(form)
# form[0]=form[0]+104.7


# file_path='HDF5:"FY4A-_AGRI--_N_DISK_1047E_L1-_FDI-_MULT_NOM_20170731070000_20170731071459_4000M_V0001.HDF"://NOMChannel08'
# # file_path='FY4A_4000.dat'
#
# ind=gdal.Info(file_path,showMetadata=False)
# print(ind)
# dataset=gdal.Open(file_path,gdal.GA_ReadOnly)
# gdal.Translate('08.vrt',dataset,format='VRT')
# met=dataset.GetMetadata("SUBDATASETS")
# print(met)
# print(dataset.GetDescription())
# band=dataset.GetRasterBand(1)
# print('像素数：{} X {}'.format(dataset.RasterXSize,dataset.RasterYSize))


# target=gdal.Open('01.nc',gdal.GA_Update)
# target.WriteRaster(0,0,temp_data.RasterXSize,temp_data.RasterYSize,sdf)
# df =123

#
# data=band.ReadAsArray()
# print(a[0,0])
# print(a[100,100])
# print(a[500,500])
# print(a[1500])
# endregion

# 注册所有的驱动
ogr.RegisterAll()
# 为了支持中文路径，请添加下面这句代码
gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES")
# 为了使属性表字段支持中文，请添加下面这句
gdal.SetConfigOption("SHAPE_ENCODING", "gb2312")

strVectorFile = "F:\\巴盟_水系\\nm_bameng_shuixi.shp"
# 打开数据
ds = ogr.Open(strVectorFile, 0)
if (not ds):
    print("打开文件失败！")
else:
    print("打开文件成功！")

dv = ogr.GetDriverByName("GeoJSON")
if (not dv):
    print("打开驱动失败！")
else:
    print("打开驱动成功！")
dv.CopyDataSource(ds, "F:\\shuiku.geojson")
print("转换成功！")
