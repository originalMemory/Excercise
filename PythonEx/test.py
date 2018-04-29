#!/usr/bin/env python
# -*- coding: utf-8 -*-

import pymysql.cursors
import time

time_start = time.time()
time_last=time_start

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

# 获取所有的计划并以编号建立词典
cursor.execute("select money,no from plan")
tp_data = cursor.fetchall()

time_now=time.time()
print('读取计划表耗时：{}s/{}s'.format(round(time_now-time_last,2),round(time_now-time_start,2)))
time_last=time_now

plans = {}
i=0
for x in tp_data:
    if not x['no']:
        continue
    tp_no = x['no']
    # 判断本编号是否已建立词典
    if tp_no in plans:
        # 是则直接添加
        tp_plan = plans[tp_no]
        tp_plan.append(x['money'])
    else:
        # 否则新建编号记录列表再添加
        tp_plan = [x['money']]
        plans[tp_no] = tp_plan
    i+=1

time_now=time.time()
print('分类计划表耗时：{}s/{}s'.format(round(time_now-time_last,4),round(time_now-time_start,2)))
time_last=time_now

    
# 获取所有实际还款，并建立词典
nos=set()
reals = {}
cursor.execute("select id,return_date,money,name,no from `real`")
tp_data = cursor.fetchall()

time_now=time.time()
print('读取实际表耗时：{}s/{}s'.format(round(time_now-time_last,2),round(time_now-time_start,2)))
time_last=time_now

i=0
for x in tp_data:
    tp_no = x['no']
    nos.add(tp_no)
    # 判断本编号是否已建立词典
    if tp_no in reals:
        # 是则直接添加
        tp_real=reals[tp_no]
        tp_real.append(x)
    else:
        # 否则新建编号记录列表再添加
        tp_real=[x]
        reals[tp_no]=tp_real
    i+=1

time_now=time.time()
print('分类实际表耗时：{}s/{}s'.format(round(time_now-time_last,4),round(time_now-time_start,2)))
time_last=time_now

no_len = len(nos)
i=0
for no in nos:
    # 获取该编号还款计划
    if not no in plans:
        continue
    now_plans = plans[no]

    # 获取实际还款期数
    now_reals=reals[no]
    # 对其按时间升序（快速排序）
    for x in range(1, len(now_reals)):
        for j in range(x-1,-1,-1):
            a=now_reals[j]['return_date']
            b=now_reals[j + 1]['return_date']
            if a>b:
                temp=now_reals[j]
                now_reals[j]=now_reals[j + 1]
                now_reals[j + 1]=temp
            else:
                break

    if no == 'A0838000006':
        adf = 24

    # 将计划与实际还款进行对比，计算其实际还款数量
    now_period = 0  # 当前还款期数，最后写入时加1
    now_money = 0  # 当前还款金额
    plan_money = now_plans[0]  # 当前应还款金额累计，与期数对应，初始为第1期
    for info in now_reals:
        # 在值不为空时叠加（反例：编号：HXYLcredit201706283794467）
        if info['money']:
            now_money += info['money']  # 叠加本次还款金额
        # 判断当前还款是否大于当前应还款
        if now_money > plan_money:
            # 大于时计算还至第几期
            while now_money > plan_money:
                # 判断是否已还至最后一期，至最后一期时不再往后计算
                if now_period == len(now_plans) - 1:
                    break
                now_period += 1
                plan_money += now_plans[now_period]
        time_now = time.time()
        print('[{}/{}]\t编号：{}\t姓名：{}\t还款时间：{}\t金额：{}/{}\t期数：{}'.format(i, no_len,tp_no,
                                                                         info['name'], info['return_date'],
                                                                         info['money'],now_plans[now_period],
                                                                         now_period + 1))
        # 写入
        sql = "update `real` set period={} where id={}".format(now_period + 1, info['id'])
        cursor.execute(sql)
        db.commit()
    time_now = time.time()
    print('本编号计算耗时：{}s/{}s'.format(round(time_now - time_last, 4), round(time_now - time_start, 2)))
    time_last = time_now
    i += 1

print('计算完毕！，累计耗时{}s'.format(round(time.time() - time_start, 2)))
cursor.close()
db.close()
