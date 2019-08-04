#!/user/bin/env python
# coding=utf-8
"""
@project : PythonEx
@ide     : PyCharm
@file    : str_encrypt
@author  : wuhoubo
@desc    : 
@create  : 2019/8/4 0:21:49
@update  :
"""

import requests
import re


def is_chinese(char):
    """
    判断是否是中文
    :param char: 要判断的字符
    :return:
    """
    if '\u4e00' <= char <= '\u9fa5':
        return True
    else:
        return False


def encrypt(char):
    """
    加密字符
    :param char: 要判断的字符
    :return:
    """
    if char.isalpha() or is_chinese(char):
        return str(ord(char))
    else:
        return char


def decrypt(char):
    """
    解密字符
    :param char: 要判断的字符
    :return:
    """
    if char.isnumeric() and int(char) > 9:
        return chr(int(char))
    else:
        return char


msg = input("请输入要加密的信息：")

encrypt_list = []
for char in msg:
    encrypt_list.append(encrypt(char))

encrypt_msg = "|".join(encrypt_list)
print("加密的信息为：" + encrypt_msg)

encrypt_list = encrypt_msg.split("|")
decrypt_msg = ""
for char in encrypt_list:
    decrypt_msg += decrypt(char)

print("解密的信息为：" + decrypt_msg)
