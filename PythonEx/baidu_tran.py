# /usr/bin/env python
# coding=utf8

import http.client
import hashlib
import urllib
import random

'''百度翻译'''


def badiu_tran(source):
    appid = '20180330000141696'
    secretKey = 'T0cdT4oaaY73TaJ1G6vp'

    httpClient = None
    myurl = '/api/trans/vip/translate'
    q = source
    fromLang = 'en'
    toLang = 'zh'
    salt = random.randint(32768, 65536)

    sign = appid + q + str(salt) + secretKey
    m1 = hashlib.md5.new()
    m1.update(sign)
    sign = m1.hexdigest()
    myurl = myurl + '?appid=' + appid + '&q=' + urllib.quote(
        q) + '&from=' + fromLang + '&to=' + toLang + '&salt=' + str(salt) + '&sign=' + sign

    try:
        httpClient = http.client.HTTPConnection('api.fanyi.baidu.com')
        httpClient.request('GET', myurl)

        # response是HTTPResponse对象
        response = httpClient.getresponse()
        target = response.read()
        return target
    except (Exception) as e:
        print(e)
    finally:
        if httpClient:
            httpClient.close()
