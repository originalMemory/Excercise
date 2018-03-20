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
    if type==1:
        cj=http.cookiejar.lwp_cookie_str()
        cj.set_cookie(ma)

    # 传入创建好的Request对象
    response = request.urlopen(req)

    # 读取响应信息并解码
    tp_html = response.read()
    charset = chardet.detect(tp_html)
    print(charset)
    tp_html = tp_html.decode(charset['encoding'])
    return tp_html


def get_gal_basic_info(filename):
    """
    根据文件名解析GAL基础数据
    :param filename: GAL文件名
    :return: 基础gal信息
    """

    gal_info = {}  # 读取的文件信息辞典
    # 获取名称
    filename = os.path.splitext(filename)[0]
    name = re.sub(r'[[(](.+?)[])]', '', filename)
    name = re.sub(r'DL版|パッケージ版|※自炊|パケ版|認証回避パッチ同梱', '', name).strip()
    name = name.replace('～', '〜').replace('♪', '♪')
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
        if re.match(r'file|iso|mdf', info):
            # file_type = None
            if 'file' in info:
                file_type = GalFileType.File
            else:
                file_type = GalFileType.ISO
            gal_info['fileType'] = file_type
            print('文件类型：%s' % file_type.name)
        # 公司
        if re.match(r'[\u0800-\u4e00A-Za-z]', info) and not re.match(r'file|iso|mdf', info):
            company = info
            print('制作公司：%s' % company)
            gal_info['company'] = company
        # 密码
        if re.match(r'密码', info):
            pw = re.search(r'(?<=[:：]).+', info).group()
            print('密码：%s' % pw)
            gal_info['pw'] = pw
    return gal_info


def get_getchu_gal_info(file_path, target_dir, **basic_info):
    """
    获取getchu抓取的Galgame信息
    :param file_path: 源文件路径
    :param target_dir: 保存的目标文件夹
    :param basic_info: 基础信息词典
    :return: 无
    """

    gal_name = basic_info['name']
    name2 = gal_name.encode('EUC-JP')

    gal_name = parse.quote(name2)
    # gal_name = parse.quote(basic_info['name'])

    if basic_info['name'] == '裸の王様':
        gal_name = '%CD%E7%A4%CE%B2%A6%CD%CD'

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

    gal_type = '[PCゲーム・アダルト]'  # 游戏类型的标识
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
        if item_type[0] != gal_type:
            continue
        link = node.xpath('.//a[@class="blueb"]')[0]
        item_url = link.get('href')
        item_url = item_url.replace('../', base_url) + '&gc=gc'
        print(item_url)
        break

    if not len(items) or not len(item_url):
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
    useless_re = re.compile(r'\n|（このブランドの作品一覧）|\[一覧\]')
    for item in sale_info_node:
        tp_nodes = item.getchildren()
        if tp_nodes[0].text:  # 非分隔线
            key = tp_nodes[0].text.replace('：', '').strip()
            value = tp_nodes[1].xpath('string(.)')
            value = re.sub(useless_re, '', value).strip()
            sale_info[key] = value

    # 写入上传信息
    source_title = item_tree.xpath('//*[@id="soft-title"]/text()')[0].strip()
    unname_str = r"[\/\\\:\*\?\"\<\>\|]"  # '/ \ : * ? " < > |'
    true_title = re.sub(unname_str, "_", source_title)  # 替换为下划线

    target_dir += '[{}] {}\\'.format(sale_info['ブランド'], true_title)

    # 创建整理后的文件夹
    if not os.path.exists(target_dir):
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

    # 写入超链接
    with open(target_dir + true_title + '.url', 'w', encoding="utf-8") as f:
        url_txt = ['[InternetShortcut]\n', 'URL=' + item_url + '\n', 'IconFile=C:\Windows\system32\SHELL32.dll\n',
                   'IconIndex=13\n']
        f.writelines(url_txt)
    # 写入介绍信息
    with open(target_dir + '介绍.txt', 'w', encoding="utf-8") as f:
        f.write('名称：{}'.format(source_title))
        for key, value in sale_info.items():
            f.write('{}：{}\n'.format(key, value))
        f.write('\n\n')
    print(sale_info)

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
        else:
            cg_url = node.replace('/brandnew', base_url + 'brandnew')
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

    gal_name = basic_info['name']
    name2 = gal_name.encode('utf-8')

    gal_name = parse.quote(name2)
    # gal_name = parse.quote(basic_info['name'])

    if basic_info['name'] == '裸の王様':
        gal_name = '%CD%E7%A4%CE%B2%A6%CD%CD'

    base_url = 'http://www.dlsite.com/'  # 链接前缀

    search_url = 'http://www.dlsite.com/pro/fsr/=/language/jp/keyword/{}/work_category%5B0%5D/pc/' \
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

    if not len(items) or not len(item_url):
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

    target_dir += '[{}] {}\\'.format(sale_info['ブランド'], true_title)

    # 创建整理后的文件夹
    if not os.path.exists(target_dir):
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

    # 写入超链接
    with open(target_dir + true_title + '.url', 'w', encoding="utf-8") as f:
        url_txt = ['[InternetShortcut]\n', 'URL=' + item_url + '\n', 'IconFile=C:\Windows\system32\SHELL32.dll\n',
                   'IconIndex=13\n']
        f.writelines(url_txt)
    # 写入介绍信息
    with open(target_dir + '介绍.txt', 'w', encoding="utf-8") as f:
        f.write('名称：{}'.format(source_title))
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
    cg_node = item_tree.xpath('//ul[@class="controller_items"]//img/@src')
    for node in cg_node:
        cg_url = node.replace('resize', 'modpub').replace('_300x300', '')
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


# basic_info = get_gal_basic_info(file_path)
# http://www.getchu.com/php/nsearch.phtml?search_keyword={0}&list_count=30&sort=sales&sort2=down&search_title=&search_brand=&search_person=&search_jan=&search_isbn=&genre=all&start_date=&end_date=&age=&list_type=list&search=search

gal_dir = 'G:\Gal\拔作\未汉化\\'
tar_dir = 'G:\Gal\拔作\整理\\'
unfind_dir = 'G:\Gal\无信息\\'

for file in os.listdir(gal_dir):
    print(file)
    path = gal_dir + file
    basic_info = get_gal_basic_info(file)
    getchu_result = get_getchu_gal_info(path, tar_dir, **basic_info)
    if not getchu_result:
        dlsite_result = get_dlsite_gal_info(path, tar_dir, **basic_info)
        if not dlsite_result:
            gal_name = os.path.basename(path)
            new_file_path = unfind_dir + gal_name
            shutil.move(path, new_file_path)

# tpstr = '%A5%AD%A5%E2%A5%E1%A5%F3%A4%C7%A4%E2%B5%F0%BA%AC%A4%CA%A4%E9%BF%CD%A4%CE%BA%CA%A4%F2%BC%AB%CA%AC%A4%CE%A5%E2%A5%CE%A4%CB%A4%C7%A4%AD%A4%EB%21+%A1%C1%C8%FE%BF%CD%BA%CA%C3%A3%A4%F2%BC%F5%C0%BA%A5%A2%A5%AF%A5%E1%A4%B5%A4%BB%A4%DE%A4%AF%A4%EC%21%CC%DC%BB%D8%A4%BB%21%A5%CF%A1%BC%A5%EC%A5%E0%A5%DE%A5%F3%A5%B7%A5%E7%A5%F3%A2%F6%A1%C1'
# str = 'キモメンでも巨根なら人の妻を自分のモノにできる!+〜美人妻達を受精アクメさせまくれ!目指せ!ハーレムマンション♪〜'
# stp = 'キモメンでも巨根なら人の妻を自分のモノにできる! ～美人妻達を受精アクメさせまくれ!目指せ!ハーレムマンション♪～'
# print(tpstr)
# print(str)
# print(stp)
# print(parse.quote(str.encode('euc-jp')))
# str2=stp.replace('～', '〜').replace('♪', '♪')
# str2 = str2.encode('euc-jp')
# print(parse.quote(str2))
