#!/user/bin/env python
# coding=utf-8
"""
@project : PythonEx
@ide     : PyCharm
@file    : algorithm
@author  : wuhoubo
@desc    : 
@create  : 2019/7/28 18:56:18
@update  :
"""
import heapq
import time


class Solution:
    """
    @param a: An integer
    @param b: An integer
    @return: The sum of a and b
    """

    def aplusb(self, a, b):
        # write your code here
        c = a ^ b
        d = (a & b) << 1
        return c + d

    """
        @param: n: An integer
        @return: An integer, denote the number of trailing zeros in n!
        """

    def trailingZeros(self, n):
        count = 0
        while n:
            if n % 5 == 0:
                count += 1
            n -= 1

    """
   @param k: An integer
   @param n: An integer
   @return: An integer denote the count of digit k in 1..n
   """

    def digitCounts(self, k, n):
        # write your code here
        i = 1
        count = 0
        if k == 0:
            # 只有个位数的情况下
            count = 1
            # 跳过1位，因为最高位的0没有意义
            while int(n / (i * 10)) != 0:
                high = int(n / (i * 10))
                count += high
                i *= 10
            return count

        while int(n / i) != 0:
            current = int(n / i % 10)
            high = int(n / (i * 10))  # 当前位右侧数
            low = n % i  # 当前位右侧数
            # k 大于当前位时为 右侧数 * i
            if k > current:
                count += high * i
            # k 等于当前位时为 右侧数 * i + 左侧数 + 1
            elif k == current:
                count += high * i + low + 1
            # k 小于当前位时为 （右侧数+1） * i
            else:
                count += (high + 1) * i
            i *= 10
        return count

    def nthUglyNumber(self, n):
        index2, index3, index5 = 0, 0, 0
        record = [1]  # 将所有丑数按升序存放在数组中
        # 列表后面的某个元素，一定是前面的某个元素乘2、乘3、或乘5得到的
        m2, m3, m5 = record[index2] * 2, record[index3] * 3, record[index3] * 5
        while len(record) != n:
            while record[-1] >= m2:
                index2 += 1
                m2 = record[index2] * 2
            while record[-1] >= m3:
                index3 += 1
                m3 = record[index3] * 3
            while record[-1] >= m5:
                index5 += 1
                m5 = record[index5] * 5
            record.append(min(m2, m3, m5))
        return record[-1]
    


# write your code here


num = 10000000
start = time.perf_counter()
print(Solution().nthUglyNumber(num))
print(str(time.perf_counter()-start))
start = time.perf_counter()
print(Solution().nthUglyNumber2(num))
print(str(time.perf_counter()-start))
