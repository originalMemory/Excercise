#!/usr/bin/env python
# -*- coding: utf-8 -*-

import gdal, gdalconst
from osgeo import osr
import numpy as np

gdal.AllRegister()
source_file_path='HDF5:"FY4A-_AGRI--_N_DISK_1047E_L1-_FDI-_MULT_NOM_20170731070000_20170731071459_4000M_V0001.HDF"://NOMChannel01'
# file_path='F:\\gdal\\FY4A_4000.dat'

class CImageGeoLocWarp:
    m_pszSrcFile = None  # ! 原始数据路径
    m_pszDstFile = None  # ! 结果数据路径
    m_pszFormat = None  # ! 结果数据格式
    m_pProcess = None  # ! 进度条指针
    m_strWkt=None
    def __init__(self,pszSrcFile, pszDstFile, pszFormat = "HFA",pProcess = None):
        """
        构造函数，地理查找表校正
        :param pszSrcFile:原始数据路径
        :param pszDstFile:结果数据路径
        :param pszFormat:结果数据格式
        :param pProcess:进度条指针
        """
        m_pszSrcFile = pszSrcFile # ! 原始数据路径
        m_pszDstFile = pszDstFile # ! 结果数据路径
        m_pszFormat = pszFormat # ! 结果数据格式
        m_pProcess = pProcess # ! 进度条指针
        m_Resampling = RM_Bilinear # ! 重采样方式，默认为双线性
        m_strWkt = osr.SRS_WKT_WGS84 # 投影只能是WGS84

    def GetSuggestedWarpOutput(self,iHeight, iWidth, padfGeoTransform = None,  padfExtent = None):
        """
        获取输出参数信息
        :param iHeight:输出图像高度
        :param iWidth:输出图像宽度
        :param padfGeoTransform:输出GeoTransform六参数
        :param padfExtent:输出图像四至范围
        :return:是否计算成功，以及错误代码
        """
        if self.m_pszDstFile==None or self.m_pszSrcFile==None:
            return -1
        gdal.AllRegister()
        hSrcDS =gdal.Open(self.m_pszSrcFile,gdal.GA_ReadOnly)
        if hSrcDS==None:
            print('打开待纠正影像失败！')
            return -2
        if self.m_strWkt==None:
            print('目标投影不存在！')
            return -1
        papszWarpOptions ={
            "METHOD": "GEOLOC_ARRAY",
            "DST_SRS":str(self.m_strWkt)
        }
