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
import os, shutil
import http.cookiejar

"""
GAL安装类型
"""


class GalFileType(Enum):
    File = 0  # 硬盘版
    ISO = 1  # 映像安装版


def get_filename_without_ex(file_path):
    """
    获取文件路径中不带扩展名的文件名
    :param file_path: 文件路径
    :return: 不带扩展名的文件名
    """
    (file_dir, temp_filename) = os.path.split(file_path)
    (shot_name, extension) = os.path.splitext(temp_filename)
    return shot_name


def get_html(url, type=0):
    """
    获取html页面
    :param url:网页链接
    :return: 解析过的网页源码
    """

    head = dict()
    # 写入User Agent信息
    head[
        'User-Agent'] = 'Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) ' \
                        'Chrome/27.0.1453.94 Safari/537.36'
    # 创建Request对象
    req = request.Request(url, headers=head)
    if type == 1:
        pass

    # 传入创建好的Request对象
    response = request.urlopen(req)

    # 读取响应信息并解码
    tp_html = response.read()
    charset = chardet.detect(tp_html)
    tp_html = tp_html.decode(charset['encoding'])
    return tp_html


def get_gal_basic_info(filename, base_dir):
    """
    根据文件名解析GAL基础数据
    :param filename: GAL文件名
    :return: 基础gal信息
    """

    gal_info = {}  # 读取的文件信息辞典
    # 获取名称
    filename = os.path.splitext(filename)[0]
    name = re.sub(r'[[(（【](.+?)[])）】]', '', filename)
    name = re.sub(r'DL版|パッケージ版|パッケージ版|※自炊|パケ版|認証回避パッチ同梱|say花火', '', name).strip()
    # name = name.replace('～', '〜').replace('♪', '♪').replace('－', '-')
    name = name.split('+')[0].replace('！', '!')
    gal_info['name'] = name
    print('游戏名：%s' % name)

    # 获取所有相关信息
    infos = re.findall(r'(?<=[[(])(.+?)(?=[])])', filename)
    for info in infos:
        # 上传日期
        if info.isdigit():
            date = time.strptime(info, '%y%m%d')
            print('上传时间：%s' % (time.strftime('%Y-%m-%d', date)))
            gal_info['update'] = date
        # 文件类型
        if re.search(r'file|iso|mdf', info):
            # file_type = None
            if 'file' in info:
                file_type = GalFileType.File
            else:
                file_type = GalFileType.ISO
            gal_info['fileType'] = file_type
            print('文件类型：%s' % file_type.name)
        # 公司
        if re.search(r'[\u0800-\u4e00A-Za-z]', info) and not re.search(r'file|iso|mdf', info):
            company = info
            print('制作公司：%s' % company)
            gal_info['company'] = company
        # 密码
        if re.search(r'密码', info):
            pw = re.search(r'(?<=[:：]).+', info).group()
            print('密码：%s' % pw)
            gal_info['pw'] = pw
        # 如果是文件夹则再遍历一层
        elif os.path.isdir(base_dir + filename):
            for x in os.listdir(base_dir + filename):
                if re.search(r'密码', x):
                    pw = re.search(r'(?<=[:：]).+(?=\.)', x).group()
                    print('密码：%s' % pw)
                    gal_info['pw'] = pw

        # 后宫
        if '后宫' in info:
            print('后宫：是')
            gal_info['harem'] = '是'
    gal_info['pw'] = '⑨'
    gal_info['source'] = '灵梦御所'

    return gal_info


def get_getchu_gal_info(file_path, target_dir, **basic_info):
    """
    获取getchu抓取的Galgame信息
    :param file_path: 源文件路径
    :param target_dir: 保存的目标文件夹
    :param basic_info: 基础信息词典
    :return: 无
    """
    name_re = re.compile(r'～|♪|－')
    gal_name = re.split(name_re, basic_info['name'])[0]
    name2 = gal_name.encode('EUC-JP')

    gal_name = parse.quote(name2)
    # gal_name = parse.quote(basic_info['name'])

    base_url = 'http://www.getchu.com/'  # 链接前缀

    search_url = 'http://www.getchu.com/php/nsearch.phtml?search_keyword={0}&list_count=30&sort=sales&sort2=' \
                 'down&genre=pc_soft&list_type=list&search=search'.format(gal_name)

    # search_url = 'http://www.getchu.com/php/nsearch.phtml?genre=all&search_keyword={}' \
    #              '&check_key_dtl=1&submit='.format(gal_name)

    search_html = get_html(search_url)
    time.sleep(1)

    # 使用xpath解析获取链接信息
    tree = etree.HTML(search_html)
    items = tree.xpath('//*[@class="display"]/li')

    gal_type_re = r'PCゲーム・アダルト|同人・アダルト'  # 游戏类型的标识
    item_url = ''
    # 获取商品链接
    gal_name = basic_info['name']
    reg_name = ''  # 匹配字符串，只匹配前几个字符
    if len(gal_name) >= 4:
        reg_name = gal_name[0:3]
    else:
        reg_name = gal_name
    for node in items:
        item_name = node.xpath('.//a[@class="blueb"]/text()')[0]
        if reg_name not in item_name:
            continue
        item_type = node.xpath('.//span[@class="orangeb"]/text()')
        if not re.search(gal_type_re, item_type[0]):
            continue
        link = node.xpath('.//a[@class="blueb"]')[0]
        item_url = link.get('href')
        item_url = item_url.replace('../', base_url) + '&gc=gc'
        print(item_url)
        break

    if 'てにおはっ' in basic_info['name']:
        item_url = 'http://www.getchu.com/soft.phtml?id=792515&gc=gc'
    if not len(item_url):
        print('该Galgame在Getchu上没有信息！')
        return False

    # item_url='http://www.getchu.com/soft.phtml?id=787288&gc=gc'
    # http://www.getchu.com/soft.phtml?id=925462&gc=gc
    # 获取商品页面

    item_html = get_html(item_url)
    item_tree = etree.HTML(item_html)

    # 获取销售相关信息
    """
    cover_url = item_tree.xpath('//*[@id="soft_table"]/tr[1]/td[1]/a/@href')[0]
    cover_url = cover_url.replace('./', base_url)  # 封面链接
    request.urlretrieve(cover_url, target_dir + 'cover.jpg')  # 保存封面
    print(cover_url)
    """

    sale_info_node = item_tree.xpath('//*[@id="soft_table"]/tr[2]/th/table/tr')
    sale_info = {}  # 销售相关信息
    useless_re = re.compile(r'\n|（このブランドの作品一覧）|\[一覧\]|（このサークルの作品一覧）')
    for item in sale_info_node:
        tp_nodes = item.getchildren()
        if tp_nodes[0].text:  # 非分隔线
            key = tp_nodes[0].text.replace('：', '').strip()
            if not len(key):
                continue
            value = tp_nodes[1].xpath('string(.)')
            value = re.sub(useless_re, '', value).strip()
            sale_info[key] = value

    # 写入上传信息
    source_title = item_tree.xpath('//*[@id="soft-title"]/text()')[0].strip()
    print(source_title)
    unname_str = r"[\/\\\:\*\?\"\<\>\|]"  # '/ \ : * ? " < > |'
    true_title = re.sub(unname_str, "_", source_title)  # 替换为下划线
    if 'ブランド' in sale_info:
        company = re.sub(unname_str, "_", sale_info['ブランド'])  # 替换为下划线
    if 'サークル' in sale_info:
        company = re.sub(unname_str, "_", sale_info['サークル'])  # 替换为下划线

    target_dir += '[{}] {}\\'.format(company, true_title)

    # 创建整理后的文件夹
    if os.path.exists(target_dir):
        # 循环遍历，确认已存在安装文件
        for root, dirs, files in os.walk(target_dir):
            for x in files:
                if re.search(r'.zip|.rar|.7z|.iso|.mds|.mdf', x) or os.path.isdir(x):
                    print('该Gal已存在')
                    os.system('pause')
                    if os.path.isfile(file_path):
                        os.remove(file_path)
                    else:
                        shutil.rmtree(file_path)
                        # os.removedirs(file_path)
                    return True
    else:
        os.makedirs(target_dir)
    with open(target_dir + '上传信息.txt', 'w', encoding="utf-8") as f:
        if 'update' in basic_info:
            f.write('上传时间：{}\n'.format(time.strftime('%Y/%m/%d', basic_info['update'])))
        if 'fileType' in basic_info:
            if basic_info['fileType'] == GalFileType.File:
                f.write('文件类型：硬盘版\n')
            else:
                f.write('文件类型：映像安装版\n')
        if 'pw' in basic_info:
            f.write('密码：{}\n'.format(basic_info['pw']))
        if 'harem' in basic_info:
            f.write('后宫：是\n')
        if 'source' in basic_info:
            f.write('来源：{}\n'.format(basic_info['source']))

    # 写入超链接
    with open(target_dir + true_title + '.url', 'w', encoding="utf-8") as f:
        url_txt = ['[InternetShortcut]\n', 'URL=' + item_url + '\n', 'IconFile=C:\Windows\system32\SHELL32.dll\n',
                   'IconIndex=13\n']
        f.writelines(url_txt)
    # 写入介绍信息
    with open(target_dir + '介绍.txt', 'w', encoding="utf-8") as f:
        f.write('名称：{}\n'.format(source_title))
        for key, value in sale_info.items():
            f.write('{}：{}\n'.format(key, value))
        f.write('\n\n')
    # print(sale_info)

    # 摘要
    story_node = item_tree.xpath('//*[@class="tablebody"]')

    with open(target_dir + '介绍.txt', 'a', encoding="utf-8") as f:
        f.write('ストーリー\n')
        for node in story_node:
            story = node.xpath('string(.)').strip()
            if len(story) > 20:
                f.write(story)

    # CG
    cg_node = item_tree.xpath('//a[@class="highslide"]/@href')
    for node in cg_node:
        if './' in node:
            cg_url = node.replace('./', base_url)
        elif not re.match('http://image.getchu.com/', node):
            cg_url = node.replace('/brandnew', base_url + 'brandnew')
        else:
            cg_url = node
        if '.http' in cg_url:
            cg_url=cg_url.replace('.http','http')
        print(cg_url)
        cg_name = os.path.basename(cg_url)
        new_url_path = target_dir + cg_name
        if os.path.exists(new_url_path):
            continue
        request.urlretrieve(cg_url, new_url_path)  # 保存CG
        time.sleep(1)

    # 移动安装包
    gal_name = os.path.basename(file_path)
    new_file_path = target_dir + gal_name
    shutil.move(file_path, new_file_path)
    return True


def get_dlsite_gal_info(file_path, target_dir, **basic_info):
    """
    获取dlsite抓取的Galgame信息
    :param file_path: 源文件路径
    :param target_dir: 保存的目标文件夹
    :param basic_info: 基础信息词典
    :return: 无
    """

    name_re = re.compile(r'～|♪|－')
    gal_name = re.split(name_re, basic_info['name'])[0]
    name2 = gal_name.encode('utf-8')

    gal_name = parse.quote(name2)
    # gal_name = parse.quote(basic_info['name'])

    if basic_info['name'] == '裸の王様':
        gal_name = '%CD%E7%A4%CE%B2%A6%CD%CD'

    base_url = 'http://www.dlsite.com/'  # 链接前缀

    # http://www.dlsite.com/pro/fsr/=/language/jp/keyword/HYPNOS+FUCK/per_page/30/from/fs.header
    search_url = 'http://www.dlsite.com/pro/fsr/=/language/jp/keyword/{}/' \
                 'per_page/30/from/fs.header'.format(gal_name)

    search_html = get_html(search_url)
    time.sleep(1)

    # 使用xpath解析获取链接信息
    tree = etree.HTML(search_html)
    items = tree.xpath('//*[@class="work_1col_table"]/tr')

    item_url = ''
    # 获取商品链接
    gal_name = basic_info['name']
    reg_name = ''  # 匹配字符串，只匹配前几个字符
    if len(gal_name) >= 4:
        reg_name = gal_name[0:3]
    else:
        reg_name = gal_name
    for node in items:
        item_name = node.xpath('.//dt[@class="work_name"]/a')[0]  # 游戏名及链接部分
        if reg_name not in item_name.text:
            continue
        item_url = item_name.get('href')
        print(item_url)
        break

    if '恥辱の虜' in basic_info['name']:
        item_url = 'http://www.dlsite.com/maniax/work/=/product_id/RJ199855.html'
    if not len(item_url):
        print('该Galgame在dlsite上没有信息！')
        return False

    # item_url='http://www.getchu.com/soft.phtml?id=787288&gc=gc'
    # http://www.getchu.com/soft.phtml?id=925462&gc=gc
    # 获取商品页面
    item_html = get_html(item_url)
    item_tree = etree.HTML(item_html)

    # 获取销售相关信息

    sale_info_node = item_tree.xpath('//*[@id="work_right_inner"]//tr')
    sale_info = {}  # 销售相关信息
    # useless_re = re.compile(r'\n|（このブランドの作品一覧）|\[一覧\]')
    for item in sale_info_node:
        tp_nodes = item.getchildren()
        if tp_nodes[0].text:  # 非分隔线
            key = tp_nodes[0].text.strip()
            value = tp_nodes[1].xpath('string(.)').strip()
            # value = re.sub(useless_re, '', value).strip()
            sale_info[key] = value

    # 写入上传信息
    source_title = item_tree.xpath('//a[@itemprop="url"]/text()')[0].strip()
    unname_str = r"[\/\\\:\*\?\"\<\>\|]"  # '/ \ : * ? " < > |'
    true_title = re.sub(unname_str, "_", source_title)  # 替换为下划线
    if 'ブランド名' in sale_info:
        company = re.sub(unname_str, "_", sale_info['ブランド名'])  # 替换为下划线
    elif 'サークル名' in sale_info:
        company = re.sub(unname_str, "_", sale_info['サークル名'])  # 替换为下划线
    elif '作家' in sale_info:
        company = re.sub(unname_str, "_", sale_info['作家'])  # 替换为下划线
    else:
        company = re.sub(unname_str, "_", sale_info['著者'])  # 替换为下划线

    target_dir += '[{}] {}\\'.format(company, true_title)

    # 创建整理后的文件夹
    if os.path.exists(target_dir):
        # 循环遍历，确认已存在安装文件
        for root, dirs, files in os.walk(target_dir):
            for x in files:
                if re.search(r'.zip|.rar|.7z|.iso|.mds|.mdf', x) or os.path.isdir(x):
                    print('该Gal已存在')
                    os.system('pause')
                    if os.path.isfile(file_path):
                        os.remove(file_path)
                    else:
                        shutil.rmtree(file_path)
                        # os.removedirs(file_path)
                    return True
    else:
        os.makedirs(target_dir)
    with open(target_dir + '上传信息.txt', 'w', encoding="utf-8") as f:
        if 'update' in basic_info:
            f.write('上传时间：{}\n'.format(time.strftime('%Y/%m/%d', basic_info['update'])))
        if 'fileType' in basic_info:
            if basic_info['fileType'] == GalFileType.File:
                f.write('文件类型：硬盘版\n')
            else:
                f.write('文件类型：映像安装版\n')
        if 'pw' in basic_info:
            f.write('密码：{}\n'.format(basic_info['pw']))
        if 'source' in basic_info:
            f.write('来源：{}\n'.format(basic_info['source']))

    # 写入超链接
    with open(target_dir + true_title + '.url', 'w', encoding="utf-8") as f:
        url_txt = ['[InternetShortcut]\n', 'URL=' + item_url + '\n', 'IconFile=C:\Windows\system32\SHELL32.dll\n',
                   'IconIndex=13\n']
        f.writelines(url_txt)
    # 写入介绍信息
    with open(target_dir + '介绍.txt', 'w', encoding="utf-8") as f:
        f.write('名称：{}\n'.format(source_title))
        for key, value in sale_info.items():
            f.write('{}：{}\n'.format(key, value))
        f.write('\n\n')
    print(sale_info)

    # 摘要
    story_node = item_tree.xpath('//*[@itemprop="description"]')

    with open(target_dir + '介绍.txt', 'a', encoding="utf-8") as f:
        f.write('作品内容\n')
        for node in story_node:
            story = node.xpath('string(.)').strip()
            if len(story) > 20:
                f.write(story)

    # CG
    cg_main_url = item_tree.xpath('//ul[@class="slider_items trans"]/li/img/@src')[0]
    cg_main_url = cg_main_url.replace('//', 'http://')
    print(cg_main_url)
    cg_name = os.path.basename(cg_main_url)
    new_url_path = target_dir + cg_name
    if not os.path.exists(new_url_path):
        request.urlretrieve(cg_main_url, new_url_path)  # 保存CG
    time.sleep(1)

    cg_url = item_tree.xpath('//div[@class="work_slider_comp"]/a/@href')
    if cg_url:
        cg_url = cg_url[0]
        cg_html = get_html(cg_url)
        cg_tree = etree.HTML(cg_html)
        cg_node = cg_tree.xpath('//div[@id="work_sample_img"]/img/@src')
        for node in cg_node:
            tp_cg_url = node.replace('//', 'http://')
            print(tp_cg_url)
            cg_name = os.path.basename(tp_cg_url)
            new_url_path = target_dir + cg_name
            if os.path.exists(new_url_path):
                continue
            request.urlretrieve(tp_cg_url, new_url_path)  # 保存CG
            time.sleep(1)

    # 移动安装包
    gal_name = os.path.basename(file_path)
    new_file_path = target_dir + gal_name
    shutil.move(file_path, new_file_path)
    return True


gal_dir = 'G:\Gal\拔作\未整理\\'
tar_dir = 'G:\Gal\拔作\整理\\'
unfind_dir = 'G:\Gal\拔作\无信息\\'

for dir in os.listdir(gal_dir):
    print(dir)
    path = gal_dir + dir
    basic_info = get_gal_basic_info(dir, gal_dir)
    getchu_result = get_getchu_gal_info(path, tar_dir, **basic_info)
    if not getchu_result:
        dlsite_result = get_dlsite_gal_info(path, tar_dir, **basic_info)
        if not dlsite_result:
            print('该Gal无相关信息')
            gal_name = os.path.basename(path)
            new_file_path = unfind_dir + gal_name
            if os.path.exists(path):
                shutil.move(path, new_file_path)

# for dir_name in os.listdir(tar_dir):
#     title=dir_name.split(']')[1].strip()
#     num=0
#     for y in os.listdir(tar_dir):
#         if title in y:
#             num+=1
#     if num==2:
#         print(title)
