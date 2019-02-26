#!/usr/bin/env python
# -*- coding: utf-8 -*-

"""
本文档搜索已整理好的Gal然后写入Mysql
"""

import os
import re
import pymysql.cursors
import time

'''获取文件的大小,结果保留两位小数，单位为MB'''


def get_FileSize(filePath):
    fsize = os.path.getsize(filePath)
    fsize = fsize / float(1024 * 1024)
    return round(fsize, 2)


basic_path = 'H:\Gal\拔作\整理\\'

config = {
    'host': 'localhost',  # 地址
    'user': 'root',  # 用户名
    'passwd': 'kdi1994',  # 密码
    'db': 'myacgn_info',  # 使用的数据库名
    'charset': 'utf8',  # 编码类型
    'cursorclass': pymysql.cursors.DictCursor  # 按字典输出
}

db = pymysql.connect(**config)
cursor = db.cursor()

# cursor.execute('select * from galgame')
# data = cursor.fetchone()
# print(data)

cover_re = re.compile(r'package.jpg|main.jpg')
time_re = re.compile(r'\d{4}[-/\\年]\d{1,2}[-/\\月]\d{1,2}')

# 遍历获取公司名和游戏名
i = 0
dirs = os.listdir(basic_path)
num = len(dirs)
for dir_name in dirs:
    i += 1
    gal = {
        'title': '',
        'company': '',
        'cover_path': '',
        'staff': '',
        'description': '',
        'passwd': '',
        'tags': '',
        'is_chinese': False,
        'is_nuki': True,
        'size': 0,
        'sale_website': '',
        'source': '',
        'create_time': time.strftime("%Y-%m-%d %H:%M:%S", time.localtime()),
        'modify_time': '0000-00-00',
        'publish_time': '',
        'is_played': False,
        'upload_time': '0000-00-00',
        'local_path': '',
        'language': '日语',
        'country': '日本',
        'is_haaremu': False
    }
    print('当前整理游戏[{}/{}]：{}'.format(i, num, dir_name))
    dir_path = basic_path + dir_name
    company = dir_name.split(']')[0][1:].strip()  # 公司名
    title = dir_name.split(']')[1].strip()  # 游戏名
    gal['title'] = title
    gal['company'] = company
    gal['local_path'] = dir_path
    # 判断名称内是否含有不能作为文件名的字符
    is_reName = False
    if '_' in company or '_' in title:
        is_reName = True

    is_repeated = False

    if '未来ラジオと人' in dir_path:
        df=34

    cover_path = ''  # 封面地址
    for file_name in os.listdir(dir_path):
        file_path = '{}\\{}'.format(dir_path, file_name)
        # 获取staff信息与游戏介绍
        if '介绍' in file_name:
            with open(file_path, encoding='utf-8') as f:
                infos = f.readlines()
                if is_reName:
                    gal['title'] = infos[0].split('：')[1].strip()
                    gal['company'] = infos[1].split('：')[1].strip()
                if infos[0].split('：')[0] == '名称':
                    gal['title'] = infos[0].split('：')[1].strip()

                # 查询游戏是否已保存过相关信息，是则跳过该次信息
                cursor.execute("select Id from galgame where title='{}'".format(gal['title']))
                data = cursor.fetchone()
                if data:
                    is_repeated = True
                    continue

                is_staff = True
                space_num = 0
                for info in infos:
                    info = info.strip()
                    if is_staff and not len(info):
                        space_num += 1
                        if space_num == 2:
                            is_staff = False
                        continue
                    if 'ストーリー' in info or '作品内容' in info:
                        continue
                    if (is_staff):
                        gal['staff'] += info + '\r\n'
                    elif len(gal['staff']):
                        gal['description'] += info + '\r\n'

                    # 获取发布时间
                    if gal['publish_time']=='':
                        publish_time = re.search(time_re, info)
                        if publish_time:
                            gal['publish_time'] = publish_time.group()
                            if '年' in publish_time.group():
                                gal['publish_time'] = re.sub(r'年|月', '/', gal['publish_time'])
                            # else:
                            #     gal['publish_time'] = time.strptime(publish_time.group(), '%Y/%m/%d')

                    # 获取标签
                    if not len(gal['tags']) and ('ジャンル' in info or 'カテゴリ' in info):
                        gal['tags'] = info.split('：')[1].strip()

        # 判断是否是图片，以第1张为封面
        if not len(cover_path) and re.search(cover_re, file_name):
            cover_path = file_path
            gal['cover_path'] = cover_path
        # # 判断是否含有密码
        # if '密码' in file_name:
        #     pw = re.search(r'(?<=密码[:：]).+?(?=[\]\.])', file_name)
        #     if pw:
        #         gal['passwd'] = pw.group()

        # 获取上传时间及密码
        if '上传信息' in file_name:
            with open(file_path, encoding='utf-8') as f:
                infos = f.readlines()
                for info in infos:
                    info = info.strip()
                    if '上传时间' in info:
                        uptime = re.search(r'(?<=上传时间[:：]).+', info).group()
                        # gal['upload_time'] = time.strptime(uptime, '%Y/%m/%d')
                        gal['upload_time'] = uptime
                    if '密码' in info:
                        gal['passwd'] = info.split('：')[1]
                        if gal['passwd'] == '终点':
                            gal['source'] = '终点'
                    if '来源' in info:
                        gal['source'] = info.split('：')[1]
                    if '后宫' in info:
                        gal['is_haaremu'] = True

        # 获取文件大小
        if re.search(r'.zip|.rar|.7z|.iso|.mds|.mdf', file_name):
            gal['size'] += get_FileSize(file_path)
        if os.path.isdir(file_path):
            for x in os.listdir(file_path):
                if re.search(r'.zip|.rar|.7z|.iso|.mds|.mdf', x):
                    gal['size'] += get_FileSize('{}\\{}'.format(file_path, x))

        # 获取销售链接
        if re.search(r'.url', file_name):
            with open(file_path, encoding='utf-8') as f:
                infos = f.read()
                gal['sale_website'] = re.search(r'(?<=URL=).+', infos).group().strip()

    if is_repeated:
        continue

    # 写入mysql
    gal['title'] = db.escape(gal['title'])
    gal['company'] = db.escape(gal['company'])
    gal['description'] = db.escape(gal['description'])
    gal['staff'] = db.escape(gal['staff'])
    gal['local_path'] = db.escape(gal['local_path'])
    gal['cover_path'] = db.escape(gal['cover_path'])
    sql = "insert into galgame(title,company,cover_path,staff,description,passwd,tags,is_chinese,is_nuki," \
          "size,sale_website,source,create_time,modify_time,publish_time,is_played,upload_time,local_path," \
          "language,country,is_haaremu) values({},{},{},{},{},'{}','{}',{},{},{},'{}','{}'," \
          "'{}','{}','{}'" \
          ",{},'{}',{},'{}','{}',{})".format(
        gal['title'], gal['company'], gal['cover_path'], gal['staff'], gal['description'], gal['passwd'],
        gal['tags'], gal['is_chinese'], gal['is_nuki'], gal['size'], gal['sale_website'], gal['source'],
        gal['create_time'], gal['modify_time'], gal['publish_time'], gal['is_played'], gal['upload_time'],
        gal['local_path'], gal['language'], gal['country'], gal['is_haaremu'])

    cursor.execute(sql)
    db.commit()
cursor.close()
db.close()
