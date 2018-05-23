# coding: utf-8


# In[1]:


import pandas as pd
# from pandas.core.frame import DataFrame
import numpy as np
import matplotlib.pyplot as plt  # 绘图
import collections  # 字典键值按导入顺序排序功能

workdir = r'F:\1_Programing'
workdir_output = r'F:\1_Programing\Output1_POS_MS2&NL_screening.csv'
workdir_input = r'F:\1_Programing\Input1_POS_MS2.csv'
path = r'F:\1_Programing\Input2_CI list_POS.xlsx'

# In[2]:


FEATURE_ID = []
PEPMASS = []
SCANS = []
RTmin = []
CHARGE = []
MSLEVEL = []
ms2 = []
ms2plot = []
PATH = []
intenselist = []
mzlist = []
mzlistthreshold = []

NL = []
NL_MS1 = []
NL_MS2 = []


# In[3]:定义计算函数


def cal(mzlist, PEPMASS):
    #     print(mzlist)
    list1 = []
    list2 = []
    list3 = []
    listNL = []

    for id, item in enumerate(mzlist):
        if PEPMASS - item > 70:
            temp = round(float(PEPMASS - item), 3)
            #         print(PEPMASS-item)
            list1.append(temp)
            for item2 in mzlist:
                if item - item2 > 70:
                    temp2 = round(float(item - item2), 3)
                    list2.append(temp2)
    list3 = list1 + list2
    listNL = list(set(list3))
    list1.sort()
    list2.sort()
    listNL.sort()

    ###
    # def decideifcal(mzlist, intenselist):
    #    for rowid in mzlist:
    #        if rowid.split(' ')[1]/max(intenselist) >=0.05:
    # intensity/max(intenselist) >= 0.05
    ##

    #         if id>0 and mzlist[id-1]>mzlist[id]:
    #             temp2 = round(float(mzlist[id-1]-mzlist[id]),4)
    #             list2.append(str(mzlist[id-1])+'-'+str(mzlist[id])+'='+str(temp2))
    #     print(list1)

    return list1, list2, list3, listNL


# In[4]:提取ms2数据，绘图&批量保存，m/z标记

def plotingms2(mzlist, intenselist, scannum, rtmin, ms1, aimpath):
    rect = []
    if mzlist:
        plt.figure(figsize=(10.5, 4.5))
        rect = plt.bar(left=mzlist, height=intenselist, width=0.005, align="center", edgecolor='blue')
        plt.title('#%s RT: %s min Full ms2 %s@hcd32.5' % (scannum, rtmin, ms1), loc='right')
        plt.xlabel("m/z", fontstyle='italic')
        plt.ylabel("Intensity")
        plt.xlim(50, max(mzlist) * 1.1)
        plt.ylim(0, max(intenselist) * 1.1)
        plt.tick_params(axis='both', direction='out', top=False, right=False, width=1, length=5)
        ax = plt.gca()
        ax.yaxis.get_major_formatter().set_powerlimits((0, 1))  # 科学计数法
        autolabel(rect, intenselist)

        plt.savefig(aimpath, format='jpg', dpi=150)
        plt.clf()
        plt.close('all')


def autolabel(rects, intenselist):
    for rect in rects:
        height = rect.get_height()
        left = rect.get_x()
        if height / max(intenselist) >= 0.05:  # 标签阈值设定：大于最大强度的5%
            plt.text(rect.get_x(), height * 1.02, '%s' % left, fontsize=8)


# In[4]:定义parse_data数据解析函数


def parse_data(path):
    df = pd.read_csv(path, header=None)
    for rowid, item in df[0].iteritems():
        counter = 0
        if item[:3] == 'BEG':
            counter += 1
            mzlist = []
            mzlist_5 = []
            intenselist = []
            ms2_plot = []
        elif item[:3] == 'FEA':
            FEATURE_ID.append(item.split('=')[1])
        elif item[:3] == 'PEP':
            ms1 = round(float(item.split('=')[1]), 4)
            PEPMASS.append(ms1)
        elif item[:3] == 'SCA':
            scannum = item.split('=')[1]
            SCANS.append(scannum)
            aimpath = ('./POS MS2 plot/POS_MSMS_%s.jpg' % scannum)  # 命名图谱,正负离子请修改POS_NEG前缀
            relativepath = ('=HYPERLINK(\"./POS MS2 plot/POS_MSMS_%s.jpg\")' % scannum)  # 命名路径生成,正负离子请修改POS_NEG前缀
            PATH.append(relativepath)
        elif item[:3] == 'RTI':
            rtmin = (round(float(item.split('=')[1]) / 60, 3))
            RTmin.append(rtmin)
        elif item[:3] == 'CHA':
            CHARGE.append(item.split('=')[1])
        elif item[:3] == 'MSL':
            MSLEVEL.append(item.split('=')[1])
        elif item[:3] == 'END':
            #                mzlist.sort(reverse=True)
            ms2.append(mzlist)  # ms2 m/z数据
            ms2plot.append(ms2_plot)  # 生成ms2数据矩阵

            # ms2中mzlist阈值设定，丰度>5%
            for rowid, mz in enumerate(mzlist):
                if intenselist[rowid] / max(intenselist) >= 0.05:
                    mzlist_5.append(mz)
            mzlistthreshold.append(mzlist_5)
            #

            list1, list2, list3, listNL = cal(mzlist_5, PEPMASS[-1])
            NL.append(listNL)  # 中性丢失数据
            #        NL_MS1.append(list1)    #中性丢失：MS1-MS2
            #        NL_MS2.append(list2)    #中性丢失：MS2-MS2
            #                dfms2plot = DataFrame(ms2plot[counter-1])    #将ms2数据矩阵转换成dataframe，不需要了

            # 绘图，删除以下绘图代码可直接生成列表结果
            plotingms2(mzlist, intenselist, scannum, rtmin, ms1, aimpath)


        elif item == '' or item == None:
            print(None)
        else:
            temp = item.split(' ')[0]
            temp = float(('%.4f' % float(temp)).rstrip('0').rstrip('.'))
            mzlist.append(temp)

            tempintensity = item.split(' ')[1]
            tempintensity = float(('%.4f' % float(tempintensity)).rstrip('0').rstrip('.'))
            intenselist.append(tempintensity)

            ms2_plot.append(item)  # 生成ms2中的m/z及相应的intensity

            # ms2_plot.append([temp,tempabun])


# In[5]:m/z列和intensity列数据拆分

# from pandas.core.frame import DataFrame
# list转dataframe

# mzlist和intenslist提取

# ms2_1 = ms2plot[0]
# dfms2_1 = DataFrame(ms2_1)
#
# mzlist = []
# intenslist = []
# for rowid, item in dfms2_1[0].iteritems():
#    temp = item.split(' ')[0]
#    temp = float(('%.4f' % float(temp)).rstrip('0').rstrip('.'))
#    mzlist.append(temp)
#    tempabun = item.split(' ')[1]
#    tempabun = float(('%.4f' % float(tempabun)).rstrip('0').rstrip('.'))
#    intenslist.append(tempabun)
#
# mzlist
# intenslist

# MS2 m/z和intensity作图
# import matplotlib.pyplot as plt

# matplotlib.use('Agg')  作图时不显示图片

# 棒状图m/z标记
# def autolabel(rects):
#    for rect in rects:
#        height = rect.get_height()
#        left = rect.get_x()
#        plt.text(rect.get_x()-5, height*1.05, '%s' % left)

# 作图
# rect = plt.bar(left = mzlist,height = intenslist, width = 0.005, align = "center", edgecolor = 'blue')
# plt.title("ms2_1")
# plt.xlabel("m/z")
# plt.ylabel("Intensity")
# plt.tick_params(axis = 'both',direction='out',top = False,right = False,width = 1,length = 5)
# autolabel(rect)

# 图片保存
# aim = ('./ms2 plot/MSMS_%s.jpg' % item)
# plt.savefig(aim, format = 'jpg', dpi = 300)
#
#
# plt.savefig('G:\\1_Programing\\picture', format='png', dpi=300)
#
#
# plt.show()
#
##plt.axis(50, PREMASS, , )
##可将m/z最大值设置为 母离子+100
#
##图片输出
##
#
# aim = ('./ms2 plot/test.jpg')
# plt.savefig(aim, format = 'jpg', dpi = 300)

# import os
# cwd = os.getcwd()
# aim = cwd + '\picture'  # 创建用于存放图片的文件夹路径
# os.makedirs(aim)        # 按路径创建文件夹
# plt.savefig(aim, format='png', dpi=300)


# def mzthreshold(mzlist):
#    counter1 = 0
#    mzlist_5 = []
#    for id, mz in enumerate(mzlist):
#        if intenselist[counter1]/max(intenselist) >= 0.05:
#            mzlist_5.append(mz)
#        counter1 += 1
# mzthreshold(mzlist)


# In[8]:解析数据，生成表格

parse_data(workdir_input)

# parse_data(workdir+'\\1_POS_MS2 data_GNPS.csv')#1_POS_MS2 data_GNPS.csv
# parse_data(workdir+'\\2_NEG_MS2 data_GNPS.csv')
# parse_data(workdir+'\\GNPS (MS2_221) V2.csv')

column = [FEATURE_ID, PEPMASS, SCANS, RTmin, CHARGE, MSLEVEL, ms2, mzlistthreshold, NL, ms2plot, PATH]
column_name = ['FEATURE_ID', 'PEPMASS', 'SCANS', 'RTmin', 'CHARGE', 'MSLEVEL', 'MS2', 'mzlistthreshold', 'NL',
               'ms2plot', 'PATH']

fe = pd.DataFrame(np.column_stack(column), columns=column_name)

fe.to_csv(workdir_output, index=False)

# In[10]:自动筛查目标离子by王亚坤

df = pd.pandas.read_excel(path)

# init attribute_list

namelist = {}
namelist = collections.OrderedDict()


def attribute_dic():
    for attribute in df['Name']:
        namelist[attribute] = None


attribute_dic()


def init_attribute_dic():
    for attribute in MS2_standard_df['Name']:
        MS2_attribute_dic[attribute] = None


def check_MS2(x):
    for attribute, mass in zip(MS2_standard_df['Name'], MS2_standard_df['Mass']):
        if (mass - 0.005) < x < (mass + 0.005):
            MS2_attribute_dic[attribute] = 1


MS2_standard_df = df.loc[df['Type'] == 'MS2']  # 获取筛查离子列表中，需要筛查MS2离子的数据
MS2_matrix = []
MS2_value = fe['MS2'].as_matrix()  # 提取样品的MS2列数据
MS2_attribute_dic = {}
MS2_attribute_dic = collections.OrderedDict()  # 字典按照导入顺序排序
for MS2_value_row in MS2_value:  # row，提取某个SCAN的所有碎片离子或中性丢失
    init_attribute_dic()  # 生成初始识别列表
    for MS2_temp in MS2_value_row:  # value，遍历一个SCAN中的所有碎片离子或中性丢失
        check_MS2(MS2_temp)  # 提交需要筛查的碎片离子或中性丢失
    MS2_matrix.append(list(MS2_attribute_dic.values()))  # 在MS2_matrix中附加筛查结果
MS2_df = pd.DataFrame(MS2_matrix, columns=MS2_standard_df['Name'])


# init attribute_list
def init_attribute_dic2():
    for attribute in NL_standard_df['Name']:
        NL_attribute_dic[attribute] = None


def check_NL(x):
    for attribute, mass in zip(NL_standard_df['Name'], NL_standard_df['Mass']):
        #       print(attribute+'£º  '+str(x)+'  '+str(mass))
        if (mass - 0.005) < x < (mass + 0.005):
            NL_attribute_dic[attribute] = '1'


NL_standard_df = df.loc[df['Type'] == 'NL']
NL_matrix = []
NL_value = fe['NL'].as_matrix()
NL_attribute_dic = {}
for value_row in NL_value:  # row
    init_attribute_dic2()
    for temp in value_row:  # value
        check_NL(temp)
    #    print(NL_attribute_dic)
    NL_matrix.append(list(NL_attribute_dic.values()))
NL_df = pd.DataFrame(NL_matrix, columns=NL_standard_df['Name'])

# 按照提交顺序排序，MS2和NL交叉
screening_df = pd.concat([MS2_df, NL_df], axis=1)
list_dic = namelist.keys()

df = screening_df.T.reset_index()
# 设置成“category”数据类型
df['Name'] = df['Name'].astype('category')
# inplace = True，使 recorder_categories生效
df['Name'].cat.reorder_categories(list_dic, inplace=True)
# inplace = True，使 df生效
df.sort('Name', inplace=True)
df = df.T
df.columns = list_dic
df = df.drop(['Name'])

final_df = pd.concat([fe, df], axis=1)
final_df.to_csv(workdir_output, index=False)

# MS2和NL列分开不排序
# final_df = pd.concat([fe, MS2_df,NL_df],axis=1)
# final_df.to_csv(workdir_output,index=False)






