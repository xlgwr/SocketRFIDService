@ECHO OFF
echo *******************���Թ���Ա������д˽ű�***************************

net stop AnXinWH.SocketRFIDService

mkdir ..\RtmV1

ECHO ��ǰĿ¼��%CD%

del /Q log

XCOPY ..\Debug\AnXinWH.SocketRFIDService* ..\RtmV1\ /e /y

cd ..\RtmV1

del /Q log

net start AnXinWH.SocketRFIDService

echo pause 