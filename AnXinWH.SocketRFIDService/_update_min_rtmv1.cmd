@ECHO OFF
echo *******************请以管理员身份运行此脚本***************************

net stop AnXinWH.SocketRFIDService

mkdir ..\RtmV1

ECHO 当前目录：%CD%

del /Q ..\Debug\log
del /Q ..\RtmV1\log

XCOPY ..\Debug\AnXinWH.SocketRFIDService* ..\RtmV1\ /e /y

cd ..\RtmV1

net start AnXinWH.SocketRFIDService

echo pause 