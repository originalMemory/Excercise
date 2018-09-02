#!/usr/bin/env python
# -*- coding: utf-8 -*-

import ogr
import gdal

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