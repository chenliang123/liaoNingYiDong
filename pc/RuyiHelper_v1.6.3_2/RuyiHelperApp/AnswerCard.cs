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
        /// <summary>
        /// 接收机连接监测
        /// </summary>
        public static int Card_Test()
        {
            StringBuilder sComs = new StringBuilder(256);
            int DeviceCnt = AnswersCollection.HX_EnumDevices(sComs);
            if (DeviceCnt <= 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public void Message()
        {
            //AnswersCollection.CallbackDelegate DeleFun = new AnswersCollection.CallbackDelegate(CallbackFun);

            if (!Global.g_haslessonOff)
            {
                AnswersCollection.HX_StopRegister();
                ///AnswersCollection.HX_RemovefromWhitelist("");   //只有点下课，才能解绑
                AnswersCollection.HX_CloseDevice();
                //AnswersCollection.HX_Release();
                Global.g_haslessonOff = false;    //点上课，初始化一次
            }


            if (acallback == null)
                acallback = new AnswersCollection.CallbackDelegate(CallbackFun);

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
            }

            AnswersCollection.HX_SetCallbackAddr(acallback);

            AnswersCollection.HX_UpdateTime();
            //AnswersCollection.HX_UnlockRegister("");
            // AnswersCollection.HX_RemovefromWhitelist(null);


            //AnswersCollection.HX_AddtoWhitelist("0854936875;1049486165");


            AnswersCollection.HX_EnableWhitelist(1);


            //AnswersCollection.HX_StopRegister();
            AnswersCollection.HX_StartRegister();


            //int rrt = AnswersCollection.HX_SetWorkMode(TBModeDef.HX_MODE_SINGLE, "");
            //int ret = AnswersCollection.HX_Start();

            ////int Reader = 0;
            ////if (AnswersCollection.HX_QueryReaderID(ref Reader) == 0)
            ////{
            ////    label1.Text = "班级：" + Reader;
            ////}
            ////else
            ////{
            ////    label1.Text = "查看接收器是否连接正常或插拔下";
            ////}
            //设置接收器ID
            AnswersCollection.HX_QueryReaderID(ref Reader);
        }

        public void CallbackFun(int iDevice, AnswersCollection.CALLBACK_MSG msg, int param1, string param2)
        {
            if (msg == AnswersCollection.CALLBACK_MSG.MSG_PULLEDOUT)
            {
                MessageBox.Show("设备拔出");
                if (isDati)
                {
                    AnswersCollection.HX_Stop();
                }                
                AnswersCollection.HX_CloseDevice();
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



                    if (answer_card == null)
                    {
                        answer_card = new AnswerCard();
                        answer_card.Raise = false;
                    }

                    if (answer_card.Raise)
                    {
                        Console.WriteLine("有人举手");
                        if (Global.RaiseStu == "")
                        {
                            Global.RaiseStu = s[7];
                        }
                        else
                        {
                            Global.RaiseStu += "|" + s[7];
                        }

                        if (Global.setSeatBtn)
                        {
                            Httpd.setSeatfn(Global.RaiseStu);
                        }
                        else
                        {
                            Httpd.setHandon(Global.RaiseStu);
                            //Console.WriteLine(Global.RaiseStu);
                        }
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

        public static void CardClose()
        {
            if (isDati)
            {
                int ret = AnswersCollection.HX_Stop();
                if (ret >= 0)
                {
                    isDati = false;
                }
                else
                {
                    ret = AnswersCollection.HX_Stop();  //如果失败，在尝试一下；
                    if (ret >= 0)
                    {
                        isDati = false;
                    }
                    else
                    {
                        answer_card.Message();
                    }
                }
            }                  
        }

        public static int AnswerStart()
        {
            Global.AnswerStu = "";
            AnswerStop();
            AnswersCollection.HX_SetWorkMode(TBModeDef.HX_MODE_SINGLE_MUL, "");             
            int temp = AnswersCollection.HX_Start();
            if(temp >= 0)
            {
                isDati = true;
            }
            else
            {
                Console.WriteLine("333");
            }
            return temp;
        }
        public static int AnswerSingleStart()
        {
            Global.AnswerStu = "";
            AnswerStop();
            AnswersCollection.HX_SetWorkMode(TBModeDef.HX_MODE_SINGLE, "");
            int temp = AnswersCollection.HX_Start();
            if (temp >= 0)
            {
                isDati = true;
            }
            else
            {
                Console.WriteLine("222");
            }
            return temp;
        }
        public static int JudgeAnswerStart()
        {
            Global.AnswerStu = "";
            AnswerStop();
            AnswersCollection.HX_SetWorkMode(TBModeDef.HX_MODE_SINGLE_judge, "");
            int temp = AnswersCollection.HX_Start();
            if (temp >= 0)
            {
                isDati = true;
            }
            else
            {
                Console.WriteLine("111");
            }
            return temp;
        }

        public static void RaiseStart()
        {
            //AnswerStop();
            Global.RaiseStu = "";
            if (answer_card == null)
            {
                answer_card = new AnswerCard();
                answer_card.Raise = true;
            }
            else
            {
                answer_card.Raise = true;
            }
            AnswersCollection.HX_SetWorkMode(TBModeDef.HX_MODE_RAISE, "");
            AnswersCollection.HX_Start();
        }
        public static void ReRaiseStart()
        {
            Global.RaiseStu = "";
            AnswerStop();
            AnswersCollection.HX_SetWorkMode(TBModeDef.HX_MODE_RAISE, "");
            AnswersCollection.HX_Start();
        }
        public static int AnswerStop()
        {
            int ret = 3;
            if( isDati)
            {
                ret = AnswersCollection.HX_Stop();
                if (ret >= 0)
                {
                    isDati = false;
                }
                else
                {
                    ret = AnswersCollection.HX_Reset();
                    if (ret >= 0)
                    {
                        isDati = false;
                    }
                }
            }
            return ret;
        }
        public static void ListClose()
        {
            AnswersCollection.HX_EnableWhitelist(0);
        }

        public static void ListOpen()
        {
            AnswersCollection.HX_EnableWhitelist(1);
        }

        public static void ListAdd()
        {
            AnswersCollection.HX_AddtoWhitelist("0854936875;1049486165");
        }

        public static void LsitClear()
        {
            AnswersCollection.HX_RemovefromWhitelist(null);
        }
    }
}