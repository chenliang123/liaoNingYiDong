using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RueHelper
{
    public class AnswerCard
    {
        private static AnswerCard answer_card;//全局答题卡对象

        private int device = 0;
        public bool Raise = false;   //举手控制开关        
        public static int Reader = 0;
        public static bool isDati = false;  //是否处于答题状态
        private static AnswersCollection.CallbackDelegate acallback = null;

        public AnswerCard()
        {
            Message();
        }

        /// <summary>
        /// 接收机连接监测
        /// </summary>
        public void Message()
        {
            if (acallback == null)
                acallback = new AnswersCollection.CallbackDelegate(CallbackFun);
            //AnswersCollection.CallbackDelegate DeleFun = new AnswersCollection.CallbackDelegate(CallbackFun);

            int ret1 = AnswersCollection.TB_Init();

            StringBuilder sComs = new StringBuilder(256);
            int DeviceCnt = AnswersCollection.TB_EnumDevices(sComs);
            if (DeviceCnt <= 0)
            {
                return;
            }

            string[] Devices = sComs.ToString().Split(';');
            List<int> list_devices = new List<int>();
            foreach (string s in Devices)
            {
                if (s.Trim() == string.Empty)
                {
                    continue;
                }
                device = AnswersCollection.TB_OpenDevice(s.ToString());
                Global.device = device;
                if (device > 0)
                {
                    Console.Write(device);
                    break;
                }
            }

            AnswersCollection.TB_SetCallbackAddr(acallback);

            AnswersCollection.TB_UpdateTime(device, 3000);

            AnswersCollection.TB_EnableWhitelist(device, 1, 3000);    //旧版本关闭白名单，支持数字答题
            
            AnswersCollection.TB_RemovefromWhitelist(device, char.MinValue, 3000);  //每次启动删除白名单

            //AnswersCollection.TB_AddtoWhitelist(device, "0009145950", 3000);
            //AnswersCollection.TB_SetWorkMode(device, TBModeDef.HX_MODE_SINGLE, "", 2000);
        }

        public void CallbackFun(int iDevice, AnswersCollection.CALLBACK_MSG msg, int param1, string param2)
        {
            if (msg == AnswersCollection.CALLBACK_MSG.MSG_PULLEDOUT)
            {
                MessageBox.Show("设备拔出");
            }           
            if (msg == AnswersCollection.CALLBACK_MSG.MSG_TEST_DATA)
            {
                    string[] s = param2.ToString().Split(new char[] { '"' });
                    if (Global.AnswerStu == "")
                    {
                        if (s[7].Replace(" ", "") == "√")
                        {
                            Global.judgeAnsewer = "R";
                            Global.AnswerStu = s[3] + ":" + Global.judgeAnsewer;
                        }
                        else if (s[7].Replace(" ", "") == "×")
                        {
                            Global.judgeAnsewer = "W";
                            Global.AnswerStu = s[3] + ":" + Global.judgeAnsewer;
                        }
                        else
                        {
                            Global.AnswerStu = s[3] + ":" + s[7].Replace(" ", "");
                        }
                    }
                    else
                    {
                        if (Global.AnswerStu.IndexOf(s[3]) >= 0)
                        {
                            //有的话去重
                            string[] stringArray = Global.AnswerStu.Split('|');
                            List<string> listString = new List<string>(stringArray);
                            int count = listString.Count;
                            for (int t = 0; t < count; t++)
                            {
                                if (listString[t].IndexOf(s[3]) >= 0)
                                {
                                    listString.Remove(listString[t]);
                                    if (s[7].Replace(" ", "") == "√")
                                    {
                                        Global.judgeAnsewer = "R";
                                        listString.Add(s[3] + ":" + Global.judgeAnsewer);
                                    }
                                    else if (s[7].Replace(" ", "") == "×")
                                    {
                                        Global.judgeAnsewer = "W";
                                        listString.Add(s[3] + ":" + Global.judgeAnsewer);
                                    }
                                    else
                                    {
                                        listString.Add(s[3] + ":" + s[7].Replace(" ", ""));
                                    }
                                }
                            }
                            Global.AnswerStu = string.Join("|", listString);
                        }
                        else
                        {
                            //没有
                            if (s[7].Replace(" ", "") == "√")
                            {
                                Global.judgeAnsewer = "R";
                                Global.AnswerStu += "|" + s[3] + ":" + Global.judgeAnsewer;
                            }
                            else if (s[7].Replace(" ", "") == "×")
                            {
                                Global.judgeAnsewer = "W";
                                Global.AnswerStu += "|" + s[3] + ":" + Global.judgeAnsewer;
                            }
                            else
                            {
                                Global.AnswerStu += "|" + s[3] + ":" + s[7].Replace(" ", "");
                            }
                        }
                    }
                    Httpd.setPracticeResult(Global.AnswerStu);
            }
        }
    }
}