#!/usr/bin/env python
# -*- coding: utf-8 -*-


import xlrd  # 读取模块
from openpyxl.styles import numbers
from openpyxl import load_workbook

# 读取表格
wb_day_sum = load_workbook('F:\Summary _business_tracking\每天业务追踪规定汇总.xlsx')  # 当天表
sheet_day_sum = wb_day_sum.worksheets[0]
rd_apply1 = xlrd.open_workbook('F:\Summary _business_tracking\day1_apply_money.xls')  # apply1
sheet_apply1 = rd_apply1.sheets()[0]
rd_overdue1 = xlrd.open_workbook('F:\Summary _business_tracking\day1_overdue_rate.xls')  # overdue1
sheet_overdue1 = rd_overdue1.sheets()[0]


def get_table_info(sheet, search_data, search_col, target_col):
    """
    查询表格获取对应行的指定数据
    表格为xlrd格式，数据仅有数字和百分比字符串两种
    :param sheet: 要查询的表格
    :param search_data: 要对比的属性内容
    :param search_col: 要对比的属性所在列
    :param target_col: 要查找的属性所在列
    :return: 查找的属性值
    """
    nrows = sheet.nrows
    for i in range(nrows):
        cp_data = sheet.cell(i, search_col).value  # 进行对比的单元格内容
        if search_data == cp_data:
            data = sheet.cell(i, target_col).value
            if isinstance(data, str) and '%' in data:  # 判断是否为字符串形式的百分比
                data = float(data[:-1]) / 100  # 是则将字符串转换为小数
            return data
    return -1


# 申请单量-放款量表1，匹配分行名称，获取申请单量
nrows = sheet_day_sum.max_row  # 行数
for i in range(3, nrows):
    name = sheet_day_sum['B' + str(i)].value
    print('分行名 ： %s' % name)

    # 获取day1的累计申请
    apply1_num = get_table_info(sheet_apply1, name + '分公司', 0, 5)
    if apply1_num >= 0:
        sheet_day_sum['C' + str(i)] = apply1_num
        print('day1 累计申请：%s' % apply1_num)
    else:
        print('无值')

    # 获取day1的累计放款
    apply1_money = get_table_info(sheet_apply1, name + '分公司', 0, 2)
    if apply1_money >= 0:
        sheet_day_sum['D' + str(i)] = apply1_money
        print('day1 累计放款：%s' % apply1_money)
    else:
        print('无值')

    # 获取day1的9-30天逾期(逾期率)
    overdue1_value = get_table_info(sheet_overdue1, name + '分公司', 0, 4)
    sheet_day_sum['E' + str(i)].number_format = numbers.FORMAT_PERCENTAGE_00
    if overdue1_value >= 0:
        sheet_day_sum['E' + str(i)] = overdue1_value
        print('day1 9-30天逾期(逾期率)：%s' % overdue1_value)
    else:
        print('无值')

    nrate = 0  # 任务达标次数

    # 对比申请数
    task_num = sheet_day_sum['J' + str(i)].value  # 每日申请任务
    sheet_day_sum['G' + str(i)].number_format = numbers.FORMAT_GENERAL
    if apply1_num > task_num:
        sheet_day_sum['G' + str(i)] = 1
        nrate += 1
    else:
        sheet_day_sum['G' + str(i)] = 0

    # 对比累计放款金额
    task_money = sheet_day_sum['K' + str(i)].value  # 每日放款任务
    task_money *= 10000
    if apply1_money > task_money:
        sheet_day_sum['H' + str(i)] = 1
        nrate += 1
    else:
        sheet_day_sum['H' + str(i)] = 0

    # 对比9-30天逾期(逾期率)
    if overdue1_value >= 0 and overdue1_value < 0.01:
        sheet_day_sum['I' + str(i)] = 1
        nrate += 1
    else:
        sheet_day_sum['I' + str(i)] = 0

    # 判断总体是否达标
    if nrate >= 2:
        sheet_day_sum['F' + str(i)] = '否'
    else:
        sheet_day_sum['F' + str(i)] = '是'

wb_day_sum.save('F:\\test.xlsx')
