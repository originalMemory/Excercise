#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os
import logging
import json
import pymongo
from bson.objectid import ObjectId
import random
from datetime import *
import gensim
from gensim.models import word2vec
import time
import re


def random_color():
    '''
    生成随机html颜色代码
    :return: 随机html颜色代码
    '''

    colorArr = ['1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F']
    color = ""
    for i in range(6):
        color += colorArr[random.randint(0, 14)]
    return "#" + color


def str_is_num(s):
    '''
    判断字符串是否是数字
    :param s: 要判断的字符串
    :return: True：是数字；False：不是数字
    '''

    for c in s:
        if c == '.':
            continue
        if c < '0' or c > '9':
            return False
    return True


def log(msg):
    '''
    在屏幕输出时间日志
    :param msg: 要显示的信息
    :return:
    '''

    timeStamp = time.strftime('%Y-%m-%d %H:%M:%S', time.localtime(time.time()))
    print('{}：{}'.format(timeStamp, msg))


def cutTxt(data_file_path, cut_file_path, word_fre_file_path):
    '''
    对数据文件切词并统计词频
    :param data_file_path: 数据文件路径
    :param cut_file_path: 切词文件存放路径
    :param word_fre_file_path: 词频文件破碎路径
    :return:
    '''

    import jieba
    import jieba.posseg as pseg

    jieba.load_userdict('user_dict.txt')
    if not os.path.exists(cut_file_path):
        try:
            with open(data_file_path, 'r', encoding='utf-8') as f:
                text = f.read()  # 获取文本内容
        except BaseException as e:  # 因BaseException是所有错误的基类，用它可以获得所有错误类型
            print(Exception, ":", e)  # 追踪错误详细信息

        new_text = jieba.lcut(text, cut_all=False)  # 精确模式
        # 构建切词后的文件
        str_out = ' '.join(new_text).replace('，', '').replace('。', '').replace('？', '').replace('！', '') \
            .replace('“', '').replace('”', '').replace('：', '').replace('…', '').replace('（', '').replace('）', '') \
            .replace('—', '').replace('《', '').replace('》', '').replace('、', '').replace('‘', '') \
            .replace('’', '')  # 去掉标点符号
        with open(cut_file_path, 'w', encoding='utf-8') as f:
            f.write(str_out)
    else:
        with open(cut_file_path, 'r', encoding='utf-8') as f:
            new_text = f.read().split(' ')

    # 统计词频并存储
    if not os.path.exists(word_fre_file_path):
        word_fre = {}
        for word in new_text:
            # 不统计单个字、字符以及纯数字
            if len(word) < 2 or str_is_num(word):
                continue
            # isN=True
            # words = pseg.cut(word,False)
            # for tp_word,flag in words:
            #     # 如果切出的词和原词不一样，则应为自定义关键词，不判断记性
            #     if word!=tp_word:
            #         break
            #     # 只保留名词
            #     if not re.search(r'^n.*', flag):
            #         isN=False
            if word in word_fre:
                word_fre[word] += 1
            else:
                # print(word)
                word_fre[word] = 1
        with open(word_fre_file_path, 'w+', encoding='utf-8') as f:
            for word, count in word_fre.items():
                f.write('%s %d\n' % (word, count))


def w2cModelTrain(train_file_name, save_model_file):
    '''
    计算词向量
    :param train_file_name: 训练语料的路径
    :param save_model_file: 模型保存路径
    :return:
    '''

    # 模型训练，生成词向量
    logging.basicConfig(format='%(asctime)s : %(levelname)s : %(message)s', level=logging.INFO)
    sentences = word2vec.Text8Corpus(train_file_name)  # 加载语料
    model = gensim.models.Word2Vec(sentences, size=200)  # 训练skip-gram模型; 默认window=5
    model.save(save_model_file)
    model.wv.save_word2vec_format(save_model_file + ".bin", binary=True)  # 以二进制类型保存模型以便重用


def createGraphNode(id, cate_id, key, size, color, word_fre):
    '''
    创建echart关系图节点
    :param id:节点id
    :param cate_id:节点归属分组id
    :param key:节点关键词
    :param size:节点大小
    :param color:节点颜色
    :param word_fre:关键词词频
    :return:结点
    '''

    num = 0
    if key in word_fre:
        num = word_fre[key]
    node = {
        "id": id,
        "name": key,
        "symbolSize": size,
        "num": num,
        'itemStyle': {
            'color': color  # 关系图节点标记的颜色
        },
        "category": cate_id
    }
    return node


# 连接MongoDB数据库
g_mongoUrl = 'mongodb://sedbo:SfNLP1225@43.240.138.232:20000/admin'  # 数据库地址
g_client = pymongo.MongoClient(g_mongoUrl)
g_db = g_client.wecodata  # 数据库操作基类


def createWord2vecGraphJson(saveDir, proIdStr, yearStart=0, yearEnd=0, likeNum=5, depth=1, soureTypes='0;2'):
    '''
    计算生成词向量关系图json文件
    :param saveDir: 保存文件夹
    :param proIdStr: 项目id字符串
    :param yearStart: 计算数据开始年，为0时计算所有时间
    :param yearEnd: 计算数据结束年
    :param likeNum: 计算相似词数，默认5个
    :param depth: 计算几层，默认1层
    :param soureTypes: 信源类型、0是百度，1为微信，2为Bing。多个用分号相连
    :return:json数据
    '''

    col_SchKeyCate = g_db.Dnl_KeywordCategory
    col_SchKeyMap = g_db.Dnl_KeywordMapping

    # 获取项目对应关键词组及关键词
    proId = ObjectId(proIdStr)
    if '0' in soureTypes or '2' in soureTypes:
        cur_cates = col_SchKeyCate.find(
            {'ProjectId': proId, 'ParentId': ObjectId('000000000000000000000000'), 'IsDel': False}, {'Name': 1})
        cates = list(cur_cates)
        cate_ids = [cate['_id'] for cate in cates]
        cur_keys = col_SchKeyMap.find({'CategoryId': {'$in': cate_ids}, 'IsDel': False},
                                      {'Keyword': 1, 'CategoryId': 1, "KeywordId": 1})
    else:
        cur_cates = g_db.MediaKeywordCategory.find(
            {'ProjectId': proId, 'ParentId': ObjectId('000000000000000000000000'), 'IsDel': False}, {'Name': 1})
        cates = list(cur_cates)
        cate_ids = [cate['_id'] for cate in cates]
        cur_keys = g_db.MediaKeywordMapping.find({'CategoryId': {'$in': cate_ids}, 'IsDel': False},
                                      {'Keyword': 1, 'CategoryId': 1, "KeywordId": 1})

    keys = list(cur_keys)

    # 将关键词写入用户词典
    with open('{}\\{}_{}_{}_userDict.txt'.format(saveDir, proIdStr, yearStart, soureTypes), 'w+', encoding='utf-8') as f:
        for key in keys:
            word=key['Keyword'].split(' ')[-1]
            f.write('{} n\n'.format(word))

    # 文件
    soureTypes = soureTypes.replace(';', '-')
    if yearStart == 0:
        data_file_path = '{}\\{}_{}_{}_data.txt'.format(saveDir, proIdStr, yearStart, soureTypes)
        cut_file_path = '{}\\{}_{}_{}_cut.txt'.format(saveDir, proIdStr, yearStart, soureTypes)
        word_fre_file_path = '{}\\{}_{}_{}_wordFre.txt'.format(saveDir, proIdStr, yearStart, soureTypes)
    else:
        data_file_path = '{}\\{}_{}-{}_{}_data.txt'.format(saveDir, proIdStr, yearStart, yearEnd, soureTypes)
        cut_file_path = '{}\\{}_{}-{}_{}_cut.txt'.format(saveDir, proIdStr, yearStart, yearEnd, soureTypes)
        word_fre_file_path = '{}\\{}_{}-{}_{}_wordFre.txt'.format(saveDir, proIdStr, yearStart, yearEnd, soureTypes)

    # 如果数据文件不存在，下载数据文件
    if not os.path.exists(data_file_path):
        data_file = open(data_file_path, 'w+', encoding='utf-8')
        key_ids = list(x['KeywordId'] for x in keys)

        # 获取被删除的链接信息
        ex_links = list(g_db.LinkMapping.find({'ProjectId': proId, 'DataCleanStatus': 2}, {'LinkId': -1}))
        ex_link_ids = [x['LinkId'] for x in ex_links]
        links = []
        # 获取百度链接信息
        if '0' in soureTypes:
            log('获取百度链接')
            if yearStart == 0:  # 年为0时获取全部数据
                links = list(
                    g_db.BaiduLinkMain.find({'KeywordId': {'$in': key_ids}, '_id': {'$nin': ex_link_ids}},
                                            {'_id': 1, 'Title': 1, 'Description': 1}))
            else:
                links = list(g_db.BaiduLinkMain.find({'KeywordId': {'$in': key_ids},
                                                      'PublishAt': {'$gte': datetime(yearStart, 1, 1),
                                                                    '$lte': datetime(yearEnd, 12, 31)}},
                                                     {'_id': 1, 'Title': 1, 'Description': 1}))

            log('百度链接获取成功，写入中')
            # 查询链接正文并写入
            for link in links:
                query = list(g_db.BaiduLinkContent.find({'LinkId': link['_id']}, {'Content': 1}))
                if len(query):
                    data_file.write(query[0]['Content'] + '\n')
                elif 'Description' in link:
                    data_file.write(link['Title'] + '\n')
                    data_file.write(link['Description'] + '\n')
            log('百度写入结束')
        # 获取Bing链接信息
        if '2' in soureTypes:
            log('获取Bing链接')
            if yearStart == 0:  # 年为0时获取全部数据
                links = list(
                    g_db.BingLinkMain.find({'KeywordId': {'$in': key_ids}, '_id': {'$nin': ex_link_ids}},
                                           {'_id': 1, 'Title': 1, 'Description': 1}))
            else:
                links = list(g_db.BingLinkMain.find({'KeywordId': {'$in': key_ids},
                                                     'PublishAt': {'$gte': datetime(yearStart, 1, 1),
                                                                   '$lte': datetime(yearEnd, 12, 31)}},
                                                    {'_id': 1, 'Title': 1, 'Description': 1}))
            log('Bing链接获取成功，写入中')
            # 查询链接正文并写入
            for link in links:
                query = list(g_db.BingLinkContent.find({'LinkId': link['_id']}, {'Content': 1}))
                if len(query):
                    data_file.write(query[0]['Content'] + '\n')
                elif 'Description' in link:
                    data_file.write(link['Title'] + '\n')
                    data_file.write(link['Description'] + '\n')
            data_file.close()
            log('Bing写入结束')
        # 获取微信链接信息
        if '1' in soureTypes:
            log('获取微信链接')
            key_ids = [str(x) for x in key_ids]  # 从ObjectId转为string。因为微信链接里对应的是string格式
            if yearStart == 0:  # 年为0时获取全部数据
                links = list(
                    g_db.WXLinkMain.find({'KeywordId': {'$in': key_ids}, '_id': {'$nin': ex_link_ids}},
                                         {'_id': 1, 'Title': 1, 'Description': 1}))
            else:
                links = list(g_db.WXLinkMain.find({'KeywordId': {'$in': key_ids},
                                                   'PostTime': {'$gte': datetime(yearStart, 1, 1),
                                                                '$lte': datetime(yearEnd, 12, 31)}},
                                                  {'_id': 1, 'Title': 1, 'Description': 1}))
            log('微信链接获取成功，写入中')
            # 查询链接正文并写入
            for link in links:
                data_file.write(link['Title'] + '\n')
                query = list(g_db.WXLinkContent.find({'LinkId': link['_id']}, {'Content': 1}))
                if len(query):
                    data_file.write(query[0]['Content'] + '\n')
                elif 'Description' in link:
                    data_file.write(link['Description'] + '\n')
            data_file.close()
            log('微信写入结束')

    # 加载词频
    if not os.path.exists(word_fre_file_path):
        log('开始切词')
        cutTxt(data_file_path, cut_file_path, word_fre_file_path)

    word_fre = {}  # 词频字典
    with open(word_fre_file_path, 'r', encoding='utf-8') as f:
        tp = f.readline()
        while tp:
            arr = tp.split(' ')
            v = arr[1][:-1]
            try:
                word_fre[arr[0]] = int(v)
            except ValueError as e:
                print(e)
            tp = f.readline()

    log('加载训练文件')
    # 加载训练文件，获取最接近的like_num个词
    if yearStart == 0:
        model_file_path = '{}\\{}_{}_{}_model'.format(saveDir, proIdStr, yearStart, soureTypes)
    else:
        model_file_path = '{}\\{}_{}-{}_{}_model'.format(saveDir, proIdStr, yearStart,yearEnd, soureTypes)
    # 训练文件不存时加载切词文件并计算
    if not os.path.exists(model_file_path):
        if not os.path.exists(cut_file_path):
            log('开始切词')
            cutTxt(data_file_path, cut_file_path, word_fre_file_path)
        log('无训练文件，计算中')
        w2cModelTrain(cut_file_path, model_file_path)
    w2v_model = word2vec.Word2Vec.load(model_file_path)  # 词向量模型

    categories = []  # 关键词组
    nodes = []  # 关键词
    links = []  # 链接
    id = 0  # 关键词结点编号
    key2id = {}  # 关键词与编号字典
    key2cate_id = {}  # 关键词与词组编号字典
    i = 0
    # 遍历生成词组信息及原项目内关键词节点
    for cate in cates:
        # 生成随机颜色及词组信息
        color = random_color()
        new_cate = {
            "name": cate['Name'],
            "itemStyle": {"color": color}
        }
        categories.append(new_cate)
        # 获取词组内关键词
        tp_keys = [x['Keyword'] for x in keys if x['CategoryId'] == cate['_id']]
        # 遍历关键词，创建节点
        for key in tp_keys:
            key2id[key] = id
            node = createGraphNode(id, i, key, 50, color, word_fre)
            nodes.append(node)
            id += 1
            key2cate_id[key] = i
        i += 1

    # 遍历生成level层相似词
    keys = set([x['Keyword'] for x in keys])  # 要计算的词列表
    tp_keys = []  # 临时词列表
    while depth > 0:
        color = random_color()  # 随机一层节点的颜色
        i = 0
        for key in keys:
            if '中国农村假基督教的隐患' == key:
                df = 34
            i += 1
            # 计算关键词词的相关词列表
            try:
                sim_keys = w2v_model.most_similar(key.split(' ')[-1], topn=likeNum)  # like_num个最相关的
            except:
                continue
            log("{}层关键词[{}/{}]:{}".format(depth, i, len(keys), key))
            cate_id = key2cate_id[key]  # 归属关键词分组Id
            for like_key, value in sim_keys:
                if len(like_key) < 2 or str_is_num(like_key):
                    continue
                log("相似词：{}\t{}".format(like_key, value))
                value = round(value, 3)
                if not like_key in key2id:
                    # 相似词是新词时创建结点
                    key2id[like_key] = id
                    node = createGraphNode(id, cate_id, like_key, 40, color, word_fre)
                    key2cate_id[like_key] = cate_id
                    nodes.append(node)
                    id += 1
                link = {"source": key2id[key], "target": key2id[like_key], "value": value}
                links.append(link)
                tp_keys.append(like_key)
        keys = set(tp_keys)
        tp_keys = []
        depth -= 1

    data = {"nodes": nodes, "links": links, "categories": categories}
    # print(data)
    json_data = json.dumps(data)
    return json_data


def computeProjectW2cLoop():
    '''
    循环访问数据库判断是否需要计算
    :return:
    '''

    colW2c = g_db.WordVec
    while True:
        # 查询并获取参数
        queryPro = list(colW2c.find({'BotStatus': 0},
                                    {'_id': 1, 'ProjectId': 1, 'ProjectName': 1, 'SoureTypes': 1, 'YearStart': 1,
                                     'YearEnd': 1,
                                     'Num': 1, 'Depth': 1, 'BotStatus': 1, 'JData': -1}).limit(1))

        # 无需要计算的项目时，随机休息数分钟
        if not len(queryPro):
            lon = random.randint(3, 10)
            log('暂无需计算的项目，休眠%d分钟！' % lon)
            time.sleep(lon * 60)
            continue

        queryPro = queryPro[0]
        if 'SoureTypes' in queryPro:
            sourceTypes = queryPro['SoureTypes']
        else:
            sourceTypes = '0;2'
        # 读取参数json字符串，计算w2cJson数据
        log('开始项目 - %s' % queryPro['ProjectName'])
        log('起止时间：{}-{}\t相似词数：{}\t层数：{}'.format(queryPro['YearStart'], queryPro['YearEnd']
                                                , queryPro['Num'], queryPro['Depth']))
        colW2c.update({'_id': queryPro['_id']}, {'$set': {'BotStatus': 1}})
        dirCur = os.path.dirname(os.path.realpath(__file__)) + '\\w2cData'  # 当前目录
        jData = createWord2vecGraphJson(dirCur, queryPro['ProjectId'], queryPro['YearStart'], queryPro['YearEnd']
                                        , queryPro['Num'], queryPro['Depth'], sourceTypes)
        colW2c.update({'_id': queryPro['_id']}, {'$set': {
            'JData': jData,
            'BotAt': datetime.today() + timedelta(hours=8)
            , 'BotStatus': 2}})
        log('计算完毕！')


computeProjectW2cLoop()
