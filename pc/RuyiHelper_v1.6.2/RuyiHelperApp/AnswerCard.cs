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

        public AnswerCard()
        {
            Message();
        }

        /// <summary>
        /// 接收机连接监测
        /// </summary>
        public void Message()
        {
            AnswersCollection.CallbackDelegate DeleFun = new AnswersCollection.CallbackDelegate(CallbackFun);

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
                if (device > 0)
                {
                    Console.Write(device);
                    break;
                }
            }

            int aaaa = AnswersCollection.TB_SetCallbackAddr(DeleFun);
            Console.Write(aaaa);
            AnswersCollection.TB_UpdateTime(device, 2000);

            AnswersCollection.TB_EnableWhitelist(device, 1, 2000);    //旧版本关闭白名单，支持数字答题

            int rrt = AnswersCollection.TB_SetWorkMode(device, TBModeDef.HX_MODE_SINGLE, "", 2000);
        }

        public void CallbackFun(int iDevice, AnswersCollection.CALLBACK_MSG msg, int param1, string param2)
        {
            if (msg == AnswersCollection.CALLBACK_MSG.MSG_PULLEDOUT)
            {
                MessageBox.Show("设备拔出");
            }

            if (msg == AnswersCollection.CALLBACK_MSG.MSG_ANSWER_DATA)
            {
                for (int i = 0; i < param1; i++)
                {
                    Console.WriteLine(param2);
                    string[] s = param2.ToString().Split(new char[] { '"' });

                    //this.BeginInvoke((MethodInvoker)delegate
                    //{
                    //   textBox1.AppendText("StudentID:" + s[3] + " " + "CardID:" + s[7] + " " + "answer：" + s[11] + "\r\n");
                    //});

                    if (answer_card.Raise)
                    {

                    }
                    else
                    {
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
                                //foreach (string eachString in listString)
                                //{
                                //    if (eachString.IndexOf(s[3]) >= 0)
                                //    {
                                //        listString.Remove(eachString);
                                //        listString.Add(s[3] + ":" + s[11].Replace(" ", ""));
                                //    }
                                //}
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
            }
            else if (msg == AnswersCollection.CALLBACK_MSG.MSG_DOUTE_DATA)
            {
                Console.WriteLine(param2);
                string[] s2 = param2.ToString().Split(new char[] { '"' });

                //this.BeginInvoke((MethodInvoker)delegate
                //{
                //   textBox1.AppendText("StudentID:" + s2[3] + " " + "CardID:" + s2[7] + "\r\n");
                //});
            }

            if (msg == AnswersCollection.CALLBACK_MSG.MSG_ERROR)
            {
                Console.Write("ErrCode:{0:D} ", param1);
                Console.WriteLine("Description:" + param2);
            }
        }
    }
}