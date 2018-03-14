#!/usr/bin/env python
# -*- coding: utf-8 -*-

"""
本文档根据文件名获取GAl基础信息，如名称、公司等。
再从Getchu和dlsite上搜索并获取如简介、发布日期等详细信息。
最后将GAL的信息统一保存并将相关信息录入至数据库中。
"""

import re
import time
from enum import Enum
from urllib import request
from urllib import parse
import chardet
from lxml import etree


# GAL安装类型
class GalFileType(Enum):
    File = 0  # 硬盘版
    ISO = 1  # 光盘安装版


file_path = '(18禁ゲーム) [160610] [アパタイト] アナル＊ティーチャー～感度良肛！？逆レッスン～ DL版 (files)[密码：终点]'


def get_gal_basic_info(filename):
    # 根据文件名解析GAL基础数据

    gal_info = {}  # 读取的文件信息辞典
    # 获取名称
    name = re.sub(r'[[(](.+?)[])]', '', filename)
    name = re.sub(r'DL版|パッケージ版|※自炊', '', name).strip()
    gal_info['name'] = name

    # 获取所有相关信息
    infos = re.findall(r'(?<=[[(])(.+?)(?=[])])', filename)
    for info in infos:
        # 上传日期
        if info.isdigit():
            date = time.strptime(info, '%y%m%d')
            print('上传时间：%s' % (time.strftime('%Y-%m-%d', date)))
            gal_info['update'] = date
        # 文件类型
        if re.match(r'file|iso', info):
            # file_type = None
            if 'file' in info:
                file_type = GalFileType.File
            else:
                file_type = GalFileType.ISO
            gal_info['fileType'] = file_type
            print('文件类型：%s' % file_type.name)
        # 公司
        if re.match(r'[\u0800-\u4e00A-Za-z]', info) and not re.match(r'file|iso', info):
            company = info
            print('制作公司：%s' % company)
            gal_info['company'] = company
        # 密码
        if re.match(r'密码', info):
            pw = re.search(r'(?<=[:：]).+', info).group()
            print('密码：%s' % pw)
            gal_info['pw'] = pw
    return gal_info


basic_info = get_gal_basic_info(file_path)
# http://www.getchu.com/php/nsearch.phtml?search_keyword={0}&list_count=30&sort=sales&sort2=down&search_title=&search_brand=&search_person=&search_jan=&search_isbn=&genre=all&start_date=&end_date=&age=&list_type=list&search=search
gal_name = parse.quote(basic_info['name'])
url = 'http://www.getchu.com/php/nsearch.phtml?search_keyword={0}&list_count=30&sort=sales&sort2='\
    'down&genre=all&list_type=list&search=search'.format(gal_name)

head = {}
# 写入User Agent信息
head[
    'User-Agent'] = 'Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) '\
    'Chrome/27.0.1453.94 Safari/537.36'
# 创建Request对象
req = request.Request(url, headers=head)
# 传入创建好的Request对象
response = request.urlopen(req)
# 读取响应信息并解码
html = response.read()
charset = chardet.detect(html)
html = html.decode(charset['encoding'])

# 使用xpath解析获取链接信息
tree = etree.HTML(html)
items = tree.xpath('//*[@class="display"]/li')

gal_type = '[PCゲーム・アダルト]'  # 游戏类型的标识
base_url = 'http://www.getchu.com/'  # 链接前缀
item_url = ''  # 商品链接
for node in items:
    item_type = node.xpath('.//span[@class="orangeb"]/text()')
    if item_type[0] != gal_type:
        continue
    link = node.xpath('//div[@id="detail_block"]/div//a[1]')[0]
    item_url = link.get('href')
    item_url = item_url.replace('../', base_url) + '&gc=gc'
    print(item_url)
    break

# http://www.getchu.com/soft.phtml?id=925462&gc=gc
# 获取商品页面
req = request.Request(item_url, headers=head)
response = request.urlopen(req)
item_html = response.read()
charset = chardet.detect(item_html)
item_html = item_html.decode(charset['encoding'])
item_tree = etree.HTML(item_html)

# 获取销售相关信息
cover_url = item_tree.xpath('//*[@id="soft_table"]/tr[1]/td[1]/a/@href')[0]
cover_url = cover_url.replace('./', base_url)  # 封面链接
print(cover_url)

sale_info_node = item_tree.xpath('//*[@id="soft_table"]/tr[2]/th/table/tr')
sale_info = {}  # 销售相关信息
useless_re = re.compile(r'\n|（このブランドの作品一覧）|\[一覧\]')
for item in sale_info_node:
    tp_nodes = item.getchildren()
    if tp_nodes[0].text:  # 非分隔线
        key = tp_nodes[0].text.replace('：', '').strip()
        value = tp_nodes[1].xpath('string(.)')
        value = re.sub(useless_re, '', value).strip()
        sale_info[key] = value
print(sale_info)

# 摘要
story_node=item_tree.xpath('//div[@class="tablebody"')[0]
story=story_node.xpath('string(.)')
print(story)