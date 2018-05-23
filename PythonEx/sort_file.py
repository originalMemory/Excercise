# -*- coding: utf-8 -*-

'''
本文档用于对下载回来的图片文件夹内其中文件进行整理
作者：吴厚波
创建时间：2018-5-19
更新时间：2018-5-21 15:34:35
'''

import shutil, os
import re

base_path = 'H:\开车\Cosplay&写真\少女映画\\'
re_video = re.compile(r'mp4|mkv|avi|rmvb|MTS|flv')
re_nochange_name = re.compile(r'IMG|DSC')
dirs = []
for name in os.listdir(base_path):
    if os.path.isdir(base_path + name):
        dirs.append(name)

for x in dirs:
    new_name = re.sub(r'[[(（【](.+?)[])）】]', '', x).strip()
    old_dir = base_path + x
    old_names = [y for y in os.listdir(old_dir)]
    num = len(old_names)
    video_num = 0
    is_has_video = False
    for y in old_names:
        if re_video.search(y):
            video_num += 1
    if video_num > 0:
        if video_num<num:
            new_name = new_name + ' ({}P+{}V)'.format(num - video_num, video_num)
        else:
            new_name = new_name + ' ({}V)'.format(video_num)
    else:
        new_name = new_name + ' ({}P)'.format(num)
    print('原文件夹名：{}\t新文件夹名：{}'.format(x, new_name))
    new_dir = base_path + new_name
    if not os.path.exists(new_dir):
        os.mkdir(new_dir)
    old_dir += '\\'
    new_dir += '\\'
    zero_add = 10
    if num >= 100:
        zero_add = 100
    i = 1
    for y in os.listdir(old_dir):
        (shot_name, extension) = os.path.splitext(y)
        if re_video.search(extension):
            shutil.move(old_dir + y, new_dir + y)
            continue
        new_filename = ''
        if re_nochange_name.search(shot_name):
            if i < zero_add:
                if zero_add == 10:
                    new_filename = '0{}{}'.format(i, extension)
                else:
                    new_filename = '00{}{}'.format(i, extension)
            else:
                new_filename = '{}{}'.format(i, extension)
        else:
            new_filename = y
        print('原文件名：{}\\{}\t新文件名：{}\\{}'.format(x, y, new_name, new_filename))
        shutil.move(old_dir + y, new_dir + new_filename)
        i += 1
    try:
        os.rmdir(old_dir)
    except Exception as ex:
        print("文件夹名不变")  # 提示：错误信息，目录不是空的
print('整理完毕！')
