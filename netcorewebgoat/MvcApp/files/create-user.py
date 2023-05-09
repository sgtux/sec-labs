#!/bin/python3
import os, random

passwords = []

with open('/root/rockyou.txt') as f:
    line = f.readline()
    while line != '':
        passwords.append(line.replace('\n', ''))
        line = f.readline()

password = random.choice(passwords)

os.system(f'useradd -m -s /bin/bash -p $(openssl passwd -1 {password}) app')