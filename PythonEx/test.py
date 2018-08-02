#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os, shutil
import re

with open( 'F:\\test.txt', 'r', encoding="ansi") as f:
    text=f.read()

with open( 'F:\\test.txt', 'w', encoding="ansi") as f:
    newText= re.sub(r"1\d{3}'>","",text)
    f.write(newText)
    print("替换完成")
