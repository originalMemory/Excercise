#!/usr/bin/env python
# -*- coding: utf-8 -*-

from openpyxl import load_workbook
from openpyxl import Workbook
from openpyxl.styles import numbers
import datetime
from dateutil.relativedelta import relativedelta

wb = load_workbook('F:\\期数对现金流和逾期影响分析.xlsx')  # 总表
sheet_data = wb.worksheets[2]
days = []
days.append(0)  # 为0位赋值，让日期与索引一致
# 按日期初始化
for i in range(1, 32):
    days.append([])

# 遍历表格，生成数据
n_of_rows = sheet_data.max_row  # 总行数
col_date = 'L'
col_money = 'M'
col_num = 'Q'
percent = 0.1
end_date=sheet_data[col_date + '2'].value   # 总数据的结束时间
for i in range(2, n_of_rows + 1):  # openpyxl的行列从1开始
    tp_now_date = sheet_data[col_date + str(i)].value  # 当前日期
    day = tp_now_date.day
    now_money = sheet_data['M' + str(i)].value+ sheet_data['N' + str(i)].value # 本金和利息
    base_money=sheet_data['O' + str(i)].value+sheet_data['P' + str(i)].value    # 当天的服务费和咨询费
    now_num = sheet_data[col_num + str(i)].value  # 期数
    tp_end_date = tp_now_date + relativedelta(months=now_num)  # 本期结束日期
    # 刷新最终结束时间
    if tp_end_date>end_date:
        end_date=tp_end_date

    info = [now_money, tp_now_date, tp_end_date, base_money]
    days[day].append(info)
    per = int(n_of_rows * percent)
    if i == per:
        print('读取数据：{}%'.format(round(percent * 100, 2)))
        percent += 0.1

# 建立新表
wb_result=Workbook()
sheet_result = wb_result.active
col_date='A'
sheet_result[col_date+'1'].value='日期'
col_sum_money = 'B'
sheet_result[col_sum_money + '1'].value = '当日总还款'
now_date=sheet_data[col_date + '2'].value  # 从最早的时间开始
i=2   # 从第二行开始录入
while now_date<tp_end_date:
    # 设置格式为日期
    sheet_result[col_date + str(i)].number_format = numbers.FORMAT_DATE_YYYYMMDD2
    sheet_result[col_date+str(i)].value=now_date
    sum_money = 0  # 累计金额
    day = now_date.day   # 第几日

    datas = days[day]  # 同一日期的所有数据
    # 遍历所有数据，计算今日应还的金额
    for data in datas:
        # 判断是否在起止时间内，在则累加
        if data[1] <= now_date and now_date <= data[2]:
            sum_money += data[0]
            # 判断是否是创建时间，是则加上服务费和咨询费
            if data[1] == now_date:
                sum_money+=data[3]
        # 因为时间是增序的，所以可以删除过期时间的数据以减少后续计算量
        elif now_date > data[2]:
            datas.remove(data)

    sum_money = round(sum_money, 2)
    print("{} 总额：{}".format(now_date.strftime('%Y-%m-%d'), sum_money))
    sheet_result[col_sum_money + str(i)].number_format = numbers.FORMAT_NUMBER_00
    sheet_result[col_sum_money + str(i)].value = sum_money
    now_date=now_date+relativedelta(days=1) # 往前推一天
    i+=1


wb_result.save('F:\\result.xlsx')
print('全部计算完毕')
