#!/usr/bin/env python
# -*- coding: utf-8 -*-

import re
from enum import Enum, unique
from lxml import etree
from urllib import request
import ssl
import chardet

ssl._create_default_https_context = ssl._create_unverified_context

# import dwc_w2c
from datetime import *
import os, sys, shutil


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

@unique
class ImagType(Enum):
    Pixiv = 0
    Yande = 1

def get_html(url):
    """
    获取html页面
    :param url:网页链接
    :return: 解析过的网页源码
    """

    head = dict()
    # 写入User Agent信息
    head['User-Agent'] = 'Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) ' \
                        'Chrome/27.0.1453.94 Safari/537.36'
    head['Accept-Language']='zh-CN'
    # 创建Request对象
    req = request.Request(url, headers=head)

    # 传入创建好的Request对象
    response = request.urlopen(req)

    # 读取响应信息并解码
    tp_html = response.read()
    # charset = chardet.detect(tp_html)
    tp_html = tp_html.decode('utf-8')
    return tp_html

# def sort_out_imginfo(dir, type):
#     pattern_no=r'pixiv_(?<no>\d+?)_'
#     nos=[]
#     for root, dirs, files in os.walk(dir):
#         for file in files:
#             nos.append(re.search(pattern_no,file).group('no'))
#             break
#
#     for no in nos:
#         url="https://www.pixiv.net/member_illust.php?mode=medium&illust_id={}".format(no)
#         html=tools.get_html(url)
#         dfs=34

url="https://www.pixiv.net/member_illust.php?mode=medium&illust_id=66353625"
html=get_html(url)

tree = etree.HTML(html)
title = tree.xpath('//title/text()')[0]
print(title)
groups=re.search(r'】「(?P<desc>.*?)」.*?/「(?P<author>.*?)」.+?\s\[pixi',title).groups()
name=groups['desc']
author=groups['author']
print('结束')
