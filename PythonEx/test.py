#!/usr/bin/env python
# -*- coding: utf-8 -*-

import gdal, gdalconst
from osgeo import osr
import numpy as np

gdal.AllRegister()
gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES")
file_path='F:\\gdal\\FY4A_4000.dat'
dataset=gdal.Open(file_path)
# print('数据投影：')
# print(dataset.GetProjection())
# print('数据的大小（行，列）：')
# print('(%s %s)' % (dataset.RasterYSize, dataset.RasterXSize))

# sublist=dataset.GetMetadata("SUBDATASETS")
# stp=sublist['SUBDATASET_1_NAME']
# dataset=None
# subdata=gdal.Open(stp,gdal.GA_ReadOnly)
print(dataset.GetDescription())
band=dataset.GetRasterBand(2)
print('像素数：{} X {}'.format(dataset.RasterXSize,dataset.RasterYSize))

data=band.ReadAsArray()
print(data[0,0])
print(data[100,100])
print(data[500,500])
print(data[1500])
# print(data)
# dat=band.ReadAsArray(100,100,5,5,10,10)
# print(dat)