#!/usr/bin/env python
# -*- coding: utf-8 -*-

import cv2
import numpy as np
import sys
import collections

import pHash
import histogram2
import dHash
from PIL import Image

def flatten_float(x):
    result = []
    for el in x:
        if isinstance(x, collections.Iterable) and not isinstance(el, float):
            result.extend(flatten_float(el))
        else:
            result.append(el)
    return result


def pHash2(imgfile):
    """get image pHash value"""
    #加载并调整图片为32x32灰度图片
    img=cv2.imread(imgfile, 0)
    img=cv2.resize(img,(64,64),interpolation=cv2.INTER_CUBIC)

        #创建二维列表
    h, w = img.shape[:2]
    vis0 = np.zeros((h,w), np.float32)
    vis0[:h,:w] = img       #填充数据

    #二维Dct变换
    vis1 = cv2.dct(cv2.dct(vis0))
    #cv.SaveImage('a.jpg',cv.fromarray(vis0)) #保存图片
    vis1.resize(32,32)

    #把二维list变成一维list
    img_list=flatten_float(vis1.tolist())

    #计算均值
    avg = sum(img_list)*1./len(img_list)
    avg_list = ['0' if i<avg else '1' for i in img_list]

    #得到哈希值
    return ''.join(['%x' % int(''.join(avg_list[x:x+4]),2) for x in range(0,32*32,4)])

'''
cv2.imread
flags>0时表示以彩色方式读入图片 
flags=0时表示以灰度图方式读入图片 
flags<0时表示以图片的本来的格式读入图片

interpolation - 插值方法。共有5种：
１）INTER_NEAREST - 最近邻插值法
２）INTER_LINEAR - 双线性插值法（默认）
３）INTER_AREA - 基于局部像素的重采样（resampling using pixel area relation）。对于图像抽取（image decimation）来说，这可能是一个更好的方法。但如果是放大图像时，它和最近邻法的效果类似。
４）INTER_CUBIC - 基于4x4像素邻域的3次插值法
５）INTER_LANCZOS4 - 基于8x8像素邻域的Lanczos插值
http://blog.csdn.net/u012005313/article/details/51943442
'''

def hammingDist(s1, s2):
    assert len(s1) == len(s2)
    return sum([ch1 != ch2 for ch1, ch2 in zip(s1, s2)])



def cp(file1,file2):
    HASH1 = pHash2(file1)
    HASH2 = pHash2(file2)
    # print(HASH1)
    # print(HASH2)
    dif = hammingDist(HASH1, HASH2)
    print(dif)
    out_score = 1 - dif * 1. / (32 * 32 / 4)
    print(out_score)

def per(num):
    # print(num)
    return 1 - num * 1. / (8 * 8/2)

# cp("img\\1.jpg","img\\2.jpg")
# cp("img\\1.jpg","img\\3.jpg")
# cp("img\\1.jpg","img\\4.jpg")
# cp("img\\1.jpg","img\\5.jpg")
# cp("img\\11.jpg","img\\12.jpg")

print(per(pHash.classify_DCT(Image.open("img\\13.jpg"),Image.open("img\\13_1.png"))))
print(dHash.classfiy_dHash(Image.open("img\\13.jpg"),Image.open("img\\13_1.png")))
print()
print(per(pHash.classify_DCT(Image.open("img\\13.jpg"),Image.open("img\\14.jpg"))))
print(dHash.classfiy_dHash(Image.open("img\\13.jpg"),Image.open("img\\14.jpg")))
print()
print(per(pHash.classify_DCT(Image.open("img\\15.jpeg"),Image.open("img\\16.jpg"))))
print(dHash.classfiy_dHash(Image.open("img\\15.jpeg"),Image.open("img\\16.jpg")))
print()
print(per(pHash.classify_DCT(Image.open("img\\17.jpg"),Image.open("img\\18.jpg"))))
print(dHash.classfiy_dHash(Image.open("img\\17.jpg"),Image.open("img\\18.jpg")))
print()
print(per(pHash.classify_DCT(Image.open("img\\11.jpg"),Image.open("img\\12.jpg"))))
print(dHash.classfiy_dHash(Image.open("img\\11.jpg"),Image.open("img\\12.jpg")))

