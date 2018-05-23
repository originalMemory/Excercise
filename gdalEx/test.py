# coding: utf-8

import gdal

dataset = gdal.Open(r"F:\MATLAB练习\source.tif")

filename = r'F:\arcMap\田块分布.shp'

dataSource = driver.Open(filename,0)

if dataSource is None:
    print 'could not open'

print 'done!'

layer = dataSource.GetLayer(0)
n = layer.GetFeatureCount()

print 'feature count:',