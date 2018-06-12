#!/usr/bin/env python
# -*- coding: utf-8 -*-

import gdal
import numpy

file_path='FY4A_4000.dat'

# ind=gdal.Info(file_path,showMetadata=False)
# print(ind)
dataset=gdal.Open(file_path,gdal.GA_Update)
met=dataset.GetMetadata("SUBDATASETS")
print(met)
print(dataset.GetDescription())
band=dataset.GetRasterBand(1)
print('像素数：{} X {}'.format(dataset.RasterXSize,dataset.RasterYSize))

data=band.ReadAsArray()
print(data[0,0])
print(data[100,100])
print(data[500,500])
print(data[1500])
# for i in range(2748):
#     print(i)
#     for j in range(2748):
#         data[i][j]+=104.7
    # print(data[i][1])
# print(data[0,0])
# print(data[100,100])
# print(data[500,500])
# print(data[1500])
# band.WriteArray(data)

