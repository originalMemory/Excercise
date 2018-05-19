# -*- coding: utf-8 -*-
# @Time    : 2018/5/14 11:11
# @Author  : Jianyang-Hu
# @Email   : jianyang1993@163.com
# @File    : threedays_out_review.py
# @Software: PyCharm

import xlrd  # 读取模块
from openpyxl.styles import numbers
from openpyxl import load_workbook

wb_day_sum = load_workbook('F:\A.xlsx')  # 当天表
sheet_day_sum = wb_day_sum.worksheets[0]

rd_review1 = load_workbook('F:\B.xlsx')  # 外出考察表
sheet_review1 = rd_review1.worksheets[0]

# 按行遍历A表，先看外出考察是为否是
nrows = sheet_day_sum.max_row  # A表最大行数
b_nrows = sheet_review1.max_row  # B表最大行数

for i in range(3, nrows):
    review_result_sum = sheet_day_sum['M' + str(i)].value  # A表的“外出考察”

    # if review_result_sum == "是":
    #   for j in range(2,b_nrows):
    #     review_result = sheet_review1['A' + str(j)].value  # B表的“分行名”
    #     if sheet_day_sum['B' + str(i)] == review_result:     # A表中为是的分行名字在B表中出现了
    #         review_result_sum = ''
    #     else:
    #         append_num = b_nrows + 1
    #         review_result[append_num] = review_result_sum
    #         print(review_result[append_num])

    if review_result_sum == "要":
        is_has = False
        for j in range(2, b_nrows + 1):
            review_result = sheet_review1['A' + str(j)].value  # B表的“分行名”
            if sheet_day_sum['B' + str(i)].value == review_result:  # A表中为是的分行名字在B表中出现了
                sheet_day_sum['M' + str(i)].value = ''
                print(sheet_day_sum['B' + str(i)].value, '否')
                is_has = True
                break
        if not is_has:
            new_row = [sheet_day_sum['B' + str(i)].value, '是']
            sheet_review1.append(new_row)
            print(new_row)

wb_day_sum.save('F:\everyday_tracking.xlsx')
rd_review1.save('F:\B_review.xlsx')
