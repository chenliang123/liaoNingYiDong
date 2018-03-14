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

            int ret1 = AnswersCollection.HX_Init();

            StringBuilder sComs = new StringBuilder(256);
            int DeviceCnt = AnswersCollection.HX_EnumDevices(sComs);
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
                device = AnswersCollection.HX_OpenDevice(s.ToString());
                Global.device = device;
                if (device > 0)
                {
                    Console.Write(device);
                    break;
                }
            }
            AnswersCollection.HX_SetCallbackAddr(acallback);
            AnswersCollection.HX_UpdateTime();

            AnswersCollection.HX_EnableWhitelist(1);

            AnswersCollection.HX_StartRegister();

            AnswersCollection.HX_QueryReaderID(ref Reader);
        }

        public void CallbackFun(int iDevice, AnswersCollection.CALLBACK_MSG msg, int param1, string param2)
        {
            if (msg == AnswersCollection.CALLBACK_MSG.MSG_PULLEDOUT)
            {
                MessageBox.Show("设备拔出");
            }
            if (msg == AnswersCollection.CALLBACK_MSG.MSG_ANSWER_DATA)
            {
                    string[] s = param2.ToString().Split(new char[] { '"' });
                    if (Global.AnswerStu == "")
                    {
                        if (s[11].Replace(" ", "") == "√")
                        {
                            Global.judgeAnsewer = "R";
                            Global.AnswerStu = s[7] + ":" + Global.judgeAnsewer;
                        }
                        else if (s[11].Replace(" ", "") == "×")
                        {
                            Global.judgeAnsewer = "W";
                            Global.AnswerStu = s[7] + ":" + Global.judgeAnsewer;
                        }
                        else
                        {
                            Global.AnswerStu = s[7] + ":" + s[11].Replace(" ", "");
                        }
                    }
                    else
                    {
                        if (Global.AnswerStu.IndexOf(s[7]) >= 0)
                        {
                            //有的话去重
                            string[] stringArray = Global.AnswerStu.Split('|');
                            List<string> listString = new List<string>(stringArray);
                            int count = listString.Count;
                            for (int t = 0; t < count; t++)
                            {
                                if (listString[t].IndexOf(s[7]) >= 0)
                                {
                                    listString.Remove(listString[t]);
                                    if (s[11].Replace(" ", "") == "√")
                                    {
                                        Global.judgeAnsewer = "R";
                                        listString.Add(s[7] + ":" + Global.judgeAnsewer);
                                    }
                                    else if (s[11].Replace(" ", "") == "×")
                                    {
                                        Global.judgeAnsewer = "W";
                                        listString.Add(s[7] + ":" + Global.judgeAnsewer);
                                    }
                                    else
                                    {
                                        listString.Add(s[7] + ":" + s[11].Replace(" ", ""));
                                    }
                                }
                            }
                            Global.AnswerStu = string.Join("|", listString);
                        }
                        else
                        {
                            //没有
                            if (s[11].Replace(" ", "") == "√")
                            {
                                Global.judgeAnsewer = "R";
                                Global.AnswerStu += "|" + s[7] + ":" + Global.judgeAnsewer;
                            }
                            else if (s[11].Replace(" ", "") == "×")
                            {
                                Global.judgeAnsewer = "W";
                                Global.AnswerStu += "|" + s[7] + ":" + Global.judgeAnsewer;
                            }
                            else
                            {
                                Global.AnswerStu += "|" + s[7] + ":" + s[11].Replace(" ", "");
                            }
                        }
                    }
                    Httpd.setPracticeResult(Global.AnswerStu);
            }
        }

        public static void JudgeSel()
        {
            AnswersCollection.HX_Stop();
            AnswersCollection.HX_SetWorkMode(TBModeDef.HX_MODE_SINGLE_judge, "");  //单题模式判断题
            AnswersCollection.HX_Start();
        }

        public static void MulSel()
        {
            AnswersCollection.HX_Stop();
            AnswersCollection.HX_SetWorkMode(TBModeDef.HX_MODE_SINGLE_MUL, "");  //单题模式多选
            AnswersCollection.HX_Start();
        }
        public static void ExitAnswer()
        {
            AnswersCollection.HX_Stop();                    //答题卡退出答题模式
        }
    }
}