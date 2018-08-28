#!/usr/bin/env python
# -*- coding: utf-8 -*-

import pymysql.cursors
import time

time_start = time.time()

# 连接数据库
config = {
    'host': 'localhost',  # 地址
    'user': 'root',  # 用户名
    'passwd': 'kdi1994',  # 密码
    'db': 'myacgn_info',  # 使用的数据库名
    'charset': 'utf8',  # 编码类型
    'cursorclass': pymysql.cursors.DictCursor  # 按dict输出，无此属性时按list输出
}

db = pymysql.connect(**config)
cursor = db.cursor()

# 建立资产编号列表
cursor.execute("select distinct no from `real`")
data = cursor.fetchall()
nos = []
for x in data:
    nos.append(x['no'])

i = 1
no_len = len(nos)
for no in nos:
    if i<22706:
        i+=1
        continue
    # 获取该编号还款计划
    plans = []
    cursor.execute("select money from plan where no='{}' order by return_date".format(no))
    plans = cursor.fetchall()

    if not len(plans):
        i+=1
        continue

    # 获取实际还款期数
    reals = []
    cursor.execute("select id,return_date,money,name from `real` where no='{}' order by return_date".format(no))
    reals = cursor.fetchall()

    if no == 'A0838000006':
        adf = 24

    # 将计划与实际还款进行对比，计算其实际还款数量
    now_period = 0  # 当前还款期数，最后写入时加1
    now_money = 0  # 当前还款金额
    plan_money = plans[0]['money']  # 当前应还款金额累计，与期数对应，初始为第1期
    for info in reals:
        # 在值不为空时叠加（反例：编号：HXYLcredit201706283794467）
        if info['money']:
            now_money += info['money']  # 叠加本次还款金额
        # 判断当前还款是否大于当前应还款
        if now_money > plan_money:
            # 大于时计算还至第几期
            while now_money > plan_money:
                # 判断是否已还至最后一期，至最后一期时不再往后计算
                if now_period == len(plans) - 1:
                    break
                now_period += 1
                plan_money += plans[now_period]['money']
        time_now = time.time()
        print('[{}/{}]({}s)\t编号：{}\t姓名：{}\t还款时间：{}\t金额：{}\t期数：{}'.format(i, no_len,
                                                                         round(time_now - time_start, 2), no,
                                                                         info['name'], info['return_date'],
                                                                         info['money'],
                                                                         now_period + 1))
        # 写入
        sql = "update `real` set period={} where id={}".format(now_period + 1, info['id'])
        cursor.execute(sql)
        db.commit()
    i += 1

print('计算完毕！')
cursor.close()
db.close()
