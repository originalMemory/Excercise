#!/usr/bin/env python
# -*- coding: utf-8 -*-

import re
# import cv2 as cv
# import tools
# import dwc_w2c
from datetime import *
import os,sys,shutil


# dir_path=r'F:\文档\MuMu共享文件夹\jpg'
# target_file=r'F:\文档\MuMu共享文件夹\all.txt'
# tools.sort_txt(dir_path,target_file)

# import pytesseract
# from PIL import Image
#
# image = Image.open('F:\\4.png')
# code = pytesseract.image_to_string(image,lang='chi_sim')
# print(code)

# tools.img_cut_tran(r'F:\\文档\\MuMu共享文件夹',target_dir=r'F:\\文档\\MuMu共享文件夹\\jpg',target_ex='.jpg',left=0,top=40,right=810,bottom=1400)
# tools.img_cut_tran(r'F:\\文档\\MuMu共享文件夹\\jpg',target_ex='.jpg')
# tools.sort_txt(r'F:\\文档\\MuMu共享文件夹\\jpg',r'F:\\文档\\MuMu共享文件夹\\all.txt')


# dwc_w2c.createWord2vecGraphJson(r'F:\w2c', '57d0d269fbd6fc08b464004a', 2017, 3, 3, r'F:\文档\HBuilderProject\test\data2.json')

# dwc_w2c.computeProjectW2cLoop()
# print(os.path.dirname(os.path.realpath(__file__))+'\\w2cData')
# print(datetime.today()+timedelta(hours=8))
# print(datetime.date(2018, 1, 1))

filepath=r'E:\无限，王座之下 连载至654.txt'
newfile=open('E:\\newtext.txt','w+')
with open(filepath,'r') as f:
    while True:
        line=f.readline()
        if not line:
            break
        if re.match(r'\d{3}$',line):
            print(line)
            continue
        newfile.write(line)
newfile.close()
print('结束')
