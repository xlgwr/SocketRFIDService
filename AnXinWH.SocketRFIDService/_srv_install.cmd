@echo on
@echo *******************���Թ���Ա������д˽ű�***************************

set binPath=%CD%

echo "%binPath%\AnXinWH.SocketRFIDService.exe"

@echo ���ڰ�װ...(sc��ʽҪ��,=��ǰ�����пո�,����Ҫ�пո�)
sc create AnXinWH.SocketRFIDService binPath= "%binPath%\AnXinWH.SocketRFIDService.exe" displayname= "AnXinWH.SocketRFIDService" start= "auto"

sc description AnXinWH.SocketRFIDService "�����Զ��������ݽӿ�,����˷��񱻽��ã����޷��������ݡ�" 

@echo ��װ���!  start= "auto"
@echo ����װλ��: %binPath%
@echo �������´�����ϵͳ���Զ�����
@echo   ���� 
@echo ʹ������:  net start AnXinWH.SocketRFIDService    �ֹ���������
@echo ʹ������:  sc delete AnXinWH.SocketRFIDService ж�ط���
@echo .
@echo .
@pause