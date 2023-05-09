#!/bin/bash
TZ='America/Sao_Paulo'
export TZ

service ssh start

/root/create-user.py

exec "$@"