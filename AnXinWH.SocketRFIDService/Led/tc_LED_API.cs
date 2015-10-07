﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace AnXinWH.SocketRFIDService.Led
{
    public class tc_LED_API
    {
        //动态库 LedOpt.dl 放在目录下  增加using System.Runtime.InteropServices
        //LedOpt.dl 为标准动态库，和调用windows  API 一样的使用方法

        /// <summary>
        /// //波特率
        /// </summary>
        /// <param name="BaudRate"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetBaudRate(int BaudRate);

        /// <summary>
        /// //串口连接
        /// </summary>
        /// <param name="iConnectType"></param>
        /// <param name="sConnectParam"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_Connect(short iConnectType, string sConnectParam);

        /// <summary>
        /// //断开串口连接
        /// </summary>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_Disconnect();

        /// <summary>
        /// //网口卡设置ip
        /// </summary>
        /// <param name="AIP"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_setIP(string AIP);
        /// <summary>
        /// //清屏
        /// </summary>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_ClearSP();

        /// <summary>
        /// //校时
        /// </summary>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_Timing();
        /// <summary>
        /// //设置亮度
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetLight(int Value);

        /// <summary>
        /// //定时开关机
        /// </summary>
        /// <param name="BootHour"></param>
        /// <param name="BootMinute"></param>
        /// <param name="DownHour"></param>
        /// <param name="DownMinute"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetSwitcher(int BootHour, int BootMinute, int DownHour, int DownMinute);

        /// <summary>
        /// //取消定时开关机
        /// </summary>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_CancelSwitcher();

        /// <summary>
        /// //设置屏参
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="ColorType"></param>
        /// <param name="DataPolar"></param>
        /// <param name="OEPolar"></param>
        /// <param name="ScanType"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetDisplay(int Width, int Height, int ColorType, int DataPolar, int OEPolar, int ScanType);

        /// <summary>
        /// //设置动画方式
        /// </summary>
        /// <param name="EntryMode"></param>
        /// <param name="iRegionID"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetAnimation(int EntryMode, int iRegionID);

        /// <summary>
        /// //设置边框
        /// </summary>
        /// <param name="BorderType"></param>
        /// <param name="iRegionID"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetBorder(int BorderType, int iRegionID);

        /// <summary>
        /// //设置移动速度
        /// </summary>
        /// <param name="iStep"></param>
        /// <param name="iRegionID"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetSpeed(int iStep, int iRegionID);

        /// <summary>
        /// //设置停留时间
        /// </summary>
        /// <param name="iTime"></param>
        /// <param name="iRegionID"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetKeepTime(int iTime, int iRegionID);

        /// <summary>
        /// //设置语音卡调用字库
        /// </summary>
        /// <param name="ifont"></param>
        /// <param name="iRegionID"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetFont(int ifont, int iRegionID);

        /// <summary>
        /// //设置字体颜色
        /// </summary>
        /// <param name="Color"></param>
        /// <param name="iRegionID"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetFontColor(int Color, int iRegionID);

        /// <summary>
        /// //设置分区
        /// </summary>
        /// <param name="Count"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SetPartition(int Count, int X, int Y, int Width, int Height);

        /// <summary>
        /// //发送普通文本
        /// </summary>
        /// <param name="sDisplayStr"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SendInfo(string sDisplayStr);

        /// <summary>
        /// //发送字幕
        /// </summary>
        /// <param name="AText"></param>
        /// <param name="AType"></param>
        /// <param name="AColor"></param>
        /// <param name="AiW"></param>
        /// <param name="AiH"></param>
        /// <param name="ArcW"></param>
        /// <param name="ArcH"></param>
        /// <param name="AFont"></param>
        /// <param name="AStyle"></param>
        /// <param name="AAlign"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_SendSubtitles(string AText, int AType, int AColor, int AiW, int AiH, int ArcW, int ArcH, string AFont, int AStyle, int AAlign);

        /// <summary>
        /// //获取错误信息
        /// </summary>
        /// <param name="sLastError"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_GetLastError(string sLastError);

        /// <summary>
        /// //设置485设备号
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        [DllImport("LedOpt.dll")]
        public static extern short TC_LED_Set485Id(int DeviceId = 1);


        /// <summary>
        /// //发送普文本信息
        /// </summary>
        /// <param name="sendTxT"></param>
        /// <returns></returns>
        public bool SendText(string sendTxT, int EntryMode, int BorderType, int iSpeed, int iKeepTime, int iFontColor, int iRegionID)
        {

            TC_LED_SetAnimation(EntryMode, iRegionID);   //动画
            TC_LED_SetBorder(BorderType, iRegionID);       //边框
            TC_LED_SetSpeed(iSpeed, iRegionID);  //速度
            TC_LED_SetKeepTime(iKeepTime, iRegionID);  //停留时间
            TC_LED_SetFontColor(iFontColor, iRegionID);    //字体颜色

            short ret = TC_LED_SendInfo(sendTxT);
            if (ret != 0)
            {
                //MessageBox.Show("通讯失败!");
                return false;
            }
            else
            {
                //MessageBox.Show("Success!");
                return true;
            }
        }
        /// <summary>
        /// //发送字幕
        /// </summary>
        /// <param name="sendTxT"></param>
        /// <param name="EntryMode"></param>
        /// <param name="BorderType"></param>
        /// <param name="iSpeed"></param>
        /// <param name="iKeepTime"></param>
        /// <param name="iFontColor"></param>
        /// <param name="iRegionID"></param>
        /// <returns></returns>
        public bool SendTextSub(string sendTxT, int PingColorIndex, int FontColorIndex, int aiW, int aiH, int aicW, int aich, int EntryMode, int BorderType, int iSpeed, int iKeepTime, int iFontColor, int iRegionID)
        {

            TC_LED_SetAnimation(EntryMode, iRegionID);   //动画
            TC_LED_SetBorder(BorderType, iRegionID);       //边框
            TC_LED_SetSpeed(iSpeed, iRegionID);  //速度
            TC_LED_SetKeepTime(iKeepTime, iRegionID);  //停留时间
            TC_LED_SetFontColor(iFontColor, iRegionID);    //字体颜色

            short ret = TC_LED_SendSubtitles(sendTxT, PingColorIndex, FontColorIndex, aiW, aiH,
                     aicW, aich, "宋体", 1, 0);
            if (ret != 0)
            {
                //MessageBox.Show("通讯失败!");
                return false;
            }
            else
            {
                //MessageBox.Show("Success!");
                return true;
            }
        }
        /// <summary>
        /// //清屏
        /// </summary>
        /// <returns></returns>
        public bool CleanScreen()
        {
            short ret = TC_LED_ClearSP();
            if (ret != 0)
            {
                //MessageBox.Show("通讯失败!");
                return false;
            }
            else
            {
                //MessageBox.Show("Success!");
                return true;
            }
        }
        /// <summary>
        /// /校时
        /// </summary>
        /// <returns></returns>
        public bool SetTiming()
        {
            short ret = TC_LED_Timing();
            if (ret != 0)
            {
                //MessageBox.Show("通讯失败!");
                return false;
            }
            else
            {
                //MessageBox.Show("Success!");
                return true;
            }
        }
        /// <summary>
        /// /校时
        /// </summary>
        /// <returns></returns>
        public bool SetIp(string IpStr)
        {
            short ret = TC_LED_setIP(IpStr.Trim());
            if (ret != 0)
            {
                //MessageBox.Show("通讯失败!");
                return false;
            }
            else
            {
                //MessageBox.Show("Success!");
                return true;
            }
        }
        public string getIpbyUDP()
        {
            UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 120);
            byte[] buf = Encoding.Default.GetBytes("EASYNET");

            client.Send(buf, buf.Length, endpoint);
            byte[] ReceiveBuf = client.Receive(ref endpoint);
            string msg = Encoding.Default.GetString(ReceiveBuf);

            if (msg != "NETEASY")
            {
                //MessageBox.Show("广播失败！");
                return null;
            }
            else
            {
                return endpoint.Address.ToString();
                //MessageBox.Show(endpoint.Address.ToString());
            }
        }
    }
}