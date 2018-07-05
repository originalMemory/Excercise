#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os,shutil

def get_filename_without_ex(file_path):
    """
    获取文件路径中不带扩展名的文件名
    :param file_path: 文件路径
    :return: 不带扩展名的文件名
    """
    (file_dir, temp_filename) = os.path.split(file_path)
    (shot_name, extension) = os.path.splitext(temp_filename)
    return shot_name

# target_dir=r'C:\下载\18acg\[こやまいち]リアルすぎるVRでやり放題の俺![中国翻訳]'
# i=1
# for root, dirs, files in os.walk(target_dir):
#     for x in files:
#         name=get_filename_without_ex(x)
#         path=os.path.join(root,name+'.jpg')
#         shutil.move(os.path.join(root,x),path)
#         print(x)
