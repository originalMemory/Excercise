# coding: utf-8
import gdal, gdalconst
from osgeo import osr
import numpy as np

gdal.AllRegister()
gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES")
file_path='FY4A-_AGRI--_N_DISK_1047E_L1-_FDI-_MULT_NOM_20170731070000_20170731071459_4000M_V0001.HDF'
dataset=gdal.Open(file_path)
# print('数据投影：')
# print(dataset.GetProjection())
# print('数据的大小（行，列）：')
# print('(%s %s)' % (dataset.RasterYSize, dataset.RasterXSize))

sublist=dataset.GetMetadata("SUBDATASETS")
stp=sublist['SUBDATASET_1_NAME']
dataset=None
subdata=gdal.Open(stp,gdal.GA_ReadOnly)
print(subdata.GetDescription())
band=subdata.GetRasterBand(1)
print('像素数：{} X {}'.format(subdata.RasterXSize,subdata.RasterYSize))
Cols = subdata.RasterXSize
Rows = subdata.RasterYSize
width = subdata.RasterXSize
height = subdata.RasterYSize
data=band.ReadRaster()
sad=data.encode('utf-8')
print(sad)
# dat=band.ReadAsArray(100,100,5,5,10,10)
# print(dat)

