nums = []
for i in nums:
    for j in nums:
        if abs(i - j) <= 0.0005:
            j = i
