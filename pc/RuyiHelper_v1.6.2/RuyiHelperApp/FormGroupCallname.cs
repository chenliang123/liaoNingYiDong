﻿using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RueHelper.util;


namespace RueHelper
{
    public partial class FormGroupCallname : Form
    {
        [DllImport("user32.dll", EntryPoint = "AnimateWindow")]
        private static extern bool AnimateWindow(IntPtr handle, int ms, int flags);

        public const int AW_HOR_POSITIVE = 0X1;//左->右
        public const int AW_HOR_NEGATIVE = 0X2;//右->左
        public const int AW_VER_POSITIVE = 0X4;//上->下
        public const int AW_VER_NEGATIVE = 0X8;//下->上
        public const int AW_CENTER = 0X10;
        public const int AW_HIDE = 0X10000;
        public const int AW_ACTIVATE = 0X20000;//逐渐显示
        public const int AW_SLIDE = 0X40000;
        public const int AW_BLEND = 0X80000;
        public const int AW_L2R = 0X40001;
        public const int AW_R2L = 0X40002;
        public const int AW_U2D = 0X40004;
        public const int AW_D2U = 0X40008;

        public enum Effect { Roll, Center, Hide, Slide, Blend }
        private static int[] effmap = { 0, 0x10, 0X10000, 0x40000, 0x80000 };
        private static int[] dirmap = { 1, 5, 4, 6, 2, 10, 8, 9 };
        private Hashtable stuPushCount = new Hashtable();

        //0x20010);   // 居中逐渐显示。
        //0xA0000); // 淡入淡出效果。
        //0x60004); // 自上向下。
        //0x20004); // 自上向下。
        //0x10010);    // 居中逐渐隐藏。
        //0x90000); // 淡入淡出效果。
        //0x50008); // 自下而上。
        //0x10008); // 自下而上。
        public static void Animate(Control ctl, Effect effect, int msec, int angle)
        {
            int flags = effmap[(int)effect];
            if (ctl.Visible) { flags |= 0x10000; angle += 180; }
            else
            {
                if (ctl.TopLevelControl == ctl) flags |= 0x20000;
                else if (effect == Effect.Blend) throw new ArgumentException();
            }
            flags |= dirmap[(angle % 360) / 45];
            bool ok = AnimateWindow(ctl.Handle, msec, flags);
            if (!ok) throw new Exception("Animation failed");
            ctl.Visible = !ctl.Visible;
        }
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public delegate void InvokeHide(bool hide);

        AutoResetEvent are = new AutoResetEvent(false);
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        ArrayList al;
        ArrayList alText;
        ArrayList clickstate;
        public ArrayList _rightList;
        public Form11 f11;
        System.Media.SoundPlayer sp = new SoundPlayer(RueHelper.Properties.Resources.click1);
        System.Media.SoundPlayer sp_clap = new SoundPlayer(RueHelper.Properties.Resources.clap);
        string _classid = Global.getClassID() + "";
        string _lessonid = Global.getLessonID() + "";
        HashSet<int> _resutlSet = new HashSet<int>();
        public string _xitiId = "";
        public System.Timers.Timer t;
        public System.Timers.Timer t1;
        public string _callnamsStr = "";
        public string _rewardStr = "";
        public string _criticizeStr = "";
        private System.Timers.Timer _panelTimer;
        int inTimer = 0;
        public DateTime tm_create = DateTime.Now;

        public string RESULT = "";
        public int mode = 0;
        private Label[] szLabel = null;
        public int r = 85;
        public string answer = "";
        private bool bWithXitiResource = false;

        private Label[] szRandNumber = new Label[10];
        private Label[] szRandGroupname = new Label[10];
        private Label[] szRandPoint = new Label[10];
        private Label[] szRandDash = new Label[10];
        public FormGroupCallname(int _count)
        {
            Log.Info("FormGroup.create");
            al = new ArrayList();
            alText = new ArrayList();
            clickstate = new ArrayList();

            szLabel = new Label[60];
            for(int i=0; i<60; i++)
            {
                Label lb = new System.Windows.Forms.Label();
                lb.AutoSize = true;
                lb.Font = new System.Drawing.Font("微软雅黑", 28F);
                lb.ForeColor = System.Drawing.Color.LimeGreen;
                lb.Size = new System.Drawing.Size(153, 50);
                lb.Visible = false;
                szLabel[i] = lb;
                this.Controls.Add(lb);
            }

            for (int i = 0; i < 10; i++)
            {
                Label lb = new System.Windows.Forms.Label();
                lb.AutoSize = true;
                lb.Font = new System.Drawing.Font("微软雅黑", 28F);
                lb.ForeColor = System.Drawing.Color.LimeGreen;
                lb.Size = new System.Drawing.Size(153, 50);
                lb.Visible = false;
                szRandNumber[i] = lb;
                this.Controls.Add(lb);

                Label lb2 = new System.Windows.Forms.Label();
                lb2.AutoSize = true;
                lb2.Font = new System.Drawing.Font("微软雅黑", 28F);
                lb2.ForeColor = System.Drawing.Color.LimeGreen;
                lb2.Size = new System.Drawing.Size(153, 50);
                lb2.Visible = false;
                szRandGroupname[i] = lb2;
                this.Controls.Add(lb2);

                Label lb3 = new System.Windows.Forms.Label();
                lb3.AutoSize = true;
                lb3.Font = new System.Drawing.Font("微软雅黑", 28F);
                lb3.ForeColor = System.Drawing.Color.LimeGreen;
                lb3.Size = new System.Drawing.Size(153, 50);
                lb3.Visible = false;
                szRandPoint[i] = lb3;
                this.Controls.Add(lb3);

                Label lb4 = new System.Windows.Forms.Label();
                lb4.AutoSize = true;
                lb4.Font = new System.Drawing.Font("微软雅黑", 28F);
                lb4.ForeColor = System.Drawing.Color.LimeGreen;
                lb4.Size = new System.Drawing.Size(153, 50);
                lb4.Visible = false;
                szRandDash[i] = lb4;
                this.Controls.Add(lb4);
                
            }

            InitializeComponent();
            
            this.TopMost = true;
#if DEBUG
            this.TopMost = false;//PPTPractise
#endif
            this.pictureBox_top1.Size = new System.Drawing.Size(184, 88);
            this.pictureBox_top2.Size = new System.Drawing.Size(184, 88);
            this.pictureBox_top3.Size = new System.Drawing.Size(184, 88);
            this.pictureBox_top1.Visible = false;
            this.pictureBox_top2.Visible = false;
            this.pictureBox_top3.Visible = false;
            Log.Info("FormHandon Timer_start(Theout) now...");
        }

        public void Reward(string group,int result)
        {
            HideRank();
            Global.rewardGroup(group, result);

            showGroupMembers(group);

            panel1.Hide();
            panel2.Hide();
            label_title.Text = group + "组成员评分： " + result;
            label_title.Show();
            label_title.Left = (screenWidth - label_title.Width) / 2;
            label_title.Top = screenHeight * 2 / 10;
            label_title.Font = new System.Drawing.Font("微软雅黑", 36F);

            this.Hide();
            this.Show();
            this.BringToFront();

            //3秒显示，自动隐藏
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 4000;
            t.Tick += new EventHandler(t_Hide);
            t.Start();
        }
        public void Callname(string group)
        {
            HideRank();
            try
            {
                showGroupMembers(group);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            panel1.Hide();
            panel2.Hide();
            label_title.Text = "请 " + group + " 组成员回答问题";
            label_title.Show();
            label_title.Left = (screenWidth - label_title.Width) / 2;
            label_title.Top = screenHeight * 2 / 10;
            label_title.Font = new System.Drawing.Font("微软雅黑", 36F);


            
            this.Hide();
            this.Show();
            this.BringToFront();

            //3秒显示，自动隐藏
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 3000;
            t.Tick += new EventHandler(t_Hide);
            t.Start();
        }

        void t_Hide(object sender, EventArgs e)
        {
            this.Visible = false;
            ((System.Windows.Forms.Timer)sender).Stop();
        }
        public void showGroupMembers(string group)
        {
            for (int i = 0; i < 60; i++)
            {
                szLabel[i].Visible = false;
            }
            this.Height = screenHeight;
            this.Width = screenWidth;
            string[] szName = getGroupmembers(group);
            int nName = szName.Count();
            int nRow = 0;
            int nCol = 0;
            int colWidth = 0;
            double widPercent = 0.6;
            if (nName < 10)
            {
                nCol = 3;
                widPercent = 0.6;
            }
            else if (nName < 20)
            {
                nCol = 4;
                widPercent = 0.6;
            }
            else if (nName < 30)
            {
                nCol = 5;
                widPercent = 0.6;
            }
            else if (nName < 40)
            {
                nCol = 7;
                widPercent = 0.8;
            }
            else if (nName < 49)
            {
                nCol = 8;
                widPercent = 0.9;
            }
            else
            {
                nCol = 10;
                widPercent = 0.95;
            }
            nRow = 1 + nName / nCol;
            colWidth = (int)(screenWidth * widPercent) / nCol;

            for (int i = 0; i < szName.Length; i++)
            {
                int _line = 1 + i / nCol;
                int _col = i - (_line - 1) * nCol;

                szLabel[i].Top = (int)(screenHeight * (3 + _line)) / 10;
                szLabel[i].Left = (int)(screenWidth * (1 - widPercent)) / 2 + colWidth * _col + (colWidth - 80) / 2;
                szLabel[i].Text = szName[i];
                szLabel[i].Visible = true;
            }
        }
        public void Handon()
        {
            mode = 1;
            int panelH = SetPanel_Group();

            Log.Info("FormHandon _xitiId=" + _xitiId + ", SetPanel over...");
            this.Text = "分组提问";

            this.Height = panelH;
            this.Width = screenWidth;
            StartPosition = FormStartPosition.Manual;
            SetDesktopLocation(0, screenHeight - this.Height); 
            this.Hide();
            this.Show();
            this.BringToFront();
            panel1.Show();
            panel1.BringToFront();

            t = new System.Timers.Timer(200);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Theout);
            t.Enabled = true;
            t.AutoReset = true;
        }
        public void Xiti(string questionstr, string rid)
        {
            mode = 2;
            this.Text = "分组练习";
            if (rid != null && rid.Length > 0 && questionstr.Length>0)
            {
                bWithXitiResource = true;
                Xiti xiti = JsonOper.DeserializeJsonToObject<Xiti>(questionstr);
                answer = xiti.answer;
                string content = xiti.content;
                string html = "<div style=\"border-top:solid 3px #0B95C8; width:100%; margin-top:30px; padding-top:70px;\"><table align=\"center\"><tr><td style=\"font-family:微软雅黑; font-size:32px;\">" + content + "</td></tr></table></div>";
                this.webBrowser1.DocumentText = html;
            }

            int panelH = SetPanel_Group();
            if (bWithXitiResource)
            {
                this.Width = screenWidth;
                this.Height = screenHeight;
                StartPosition = FormStartPosition.Manual;
                SetDesktopLocation(0, 0);
            }
            else
            {
                this.Height = panelH;
                this.Width = screenWidth;
                StartPosition = FormStartPosition.Manual;
                SetDesktopLocation(0, screenHeight - this.Height);
            }

            this.Hide();
            this.Show();
            this.BringToFront();
            panel1.Show();
            panel1.BringToFront();

            t = new System.Timers.Timer(200);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Theout);
            t.Enabled = true;
            t.AutoReset = true;
        }
        public string[] getGroupmembers(string group)
        {
            string uids = "";
            if (Global.m_grouplist != null)
            {
                Group[] szGroup = Global.m_grouplist.grouplist;
                foreach (Group g in szGroup)
                {
                    if (g.name == group)
                    {
                        uids = g.uids;
                        break;
                    }
                }
            }

            StudentInfo[] szSI = Global.g_StudentInfoArray;
            if(uids.Length > 0)
            {
                string[] szID = uids.Split(',');
                string[] szNames = new string[szID.Length];
                for (int i = 0; i < szID.Length; i++)
                {
                    if (szID[i].Length == 0)
                        continue;

                    int nId = Int32.Parse(szID[i]);
                    foreach (StudentInfo si in szSI)
                    {
                        int _id = Int32.Parse(si.ID);
                        if (_id == nId)
                        {
                            szNames[i] = si.Name;
                            break;
                        }
                    }
                }
                return szNames;
            }
            else
            {
                return new string[0];
            }
            
        }
        public string getGroup(int seat)
        {
            string groupname = "";
            int uid = 0;
            StudentInfo[] szSI = Global.g_StudentInfoArray;
            foreach (StudentInfo si in szSI)
            {
                string strSeat = si.SEAT.Replace("-","");
                if (strSeat.Length>0 && Int32.Parse(strSeat)==seat)
                {
                    uid = Int32.Parse(si.ID);
                    break;
                }
            }

            if(Global.m_grouplist!=null)
            {
                Group[] szGroup = Global.m_grouplist.grouplist;
                foreach (Group g in szGroup)
                {
                    if (g.uids.IndexOf(","+uid) >= 0 || g.uids.IndexOf(uid + ",") >= 0)
                    {
                        groupname = g.name;
                        break;
                    }
                }
            }
            
            return groupname;
        }
        public void HideEvent(bool bHide)
        {
            if (this.InvokeRequired)
            {
                InvokeHide cb = new InvokeHide(HideEvent);
                this.Invoke(cb, new object[] { bHide });
                return;
            }
            if (bHide)
            {
               // this.Hide();
            }else{
                this.Show();
                this.BringToFront();
            }
                
        }
        public void AppendCriticize(int uid)
        {
            if (_criticizeStr.IndexOf(uid + ":") >= 0)
                return;

            DateTime tm_now = DateTime.Now;
            TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
            TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
            TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
            int timeDiff = timespan.Minutes * 60 + timespan.Seconds;

            if(uid > 0)
            {
                string pair = uid + ":" + timeDiff;
                if (_criticizeStr.Length > 0)
                    _criticizeStr += ",";
                _criticizeStr += pair;
            }
        }

        public string GetCallname()
        {
            return _callnamsStr;
        }
        public string GetRewarded()
        {
            return _rewardStr;
        }
        public string GetCriticize()
        {
            return _criticizeStr;
        }
        public string GetResult()
        {
            return RESULT;
        }

        private int SetPanel(string numberstr)
        {
            ClassInfo ci = JsonOper.DeserializeJsonToObject<ClassInfo>(numberstr);
            StudentInfo[] si = ci.Data.Student;
            int _count = screenWidth / 51;
            int _line = ci.Data.StudentCount % _count > 0 ? ci.Data.StudentCount / _count + 1 : ci.Data.StudentCount / _count;
            int panelHeight = _line * 50 + 10;///////////////////////
            if (_line == 2)
                panelHeight = _line * 50 + 60;
            else if (_line == 1)
                panelHeight = _line * 50 + 110;

            int panelLH = this.screenHeight - panelHeight;

            this.panel1.Location = new System.Drawing.Point(0, 0);//panelLH
            this.panel1.Size = new System.Drawing.Size(this.screenWidth, panelHeight);

            this.panel2.Location = new System.Drawing.Point(0, 0);//panelLH
            this.panel2.Size = new System.Drawing.Size(this.screenWidth, panelHeight);
            this.panel1.BringToFront();
            //this.panel2.Show();
            
            label_top3_1.Text = "";
            label_top3_2.Text = "";
            label_top3_3.Text = "";

            int top3_top = (panelHeight - label_top3_1.Size.Height) / 2 - 10;
            label_top3_1.Text = " ";
            label_top3_2.Text = " ";
            label_top3_3.Text = " ";
            label_top3_1.Top = top3_top;
            label_top3_2.Top = top3_top;
            label_top3_3.Top = top3_top;

            int highInterval = 10;//三行的行间距
            if (_line == 2)
                highInterval = 25;//两行变三行,两行的行间距
            else if (_line == 1)
                highInterval = 55;

            int _lw = 0;
            int _br = 1;
            int _locationWidth = 0;
            int _locationHeight = 40;
            int nStudentCount = ci.Data.StudentCount;
            for (int i = 1; i <= nStudentCount; i++)
            {
                _locationHeight = (_lw * 40) + ((_lw + 1) * highInterval);
                if (i == 1 || _br % 2 == 0)
                {
                    _locationWidth = (screenWidth - _count * 51) / 2 + 5;
                }
                else
                {
                    _locationWidth += 51;
                }
                if (i % _count == 0)
                {
                    _br *= 2;
                    if (_lw <= _line)
                    {
                        _lw++;
                    }
                }
                else
                {
                    _br=1;
                }
                PictureBox pic = new PictureBox();
                AnswerCount ac = new AnswerCount();
                ac.ImageWidth = 40;
                ac.ImagesHeight = 40;
                ac.FontStyle = System.Drawing.FontStyle.Bold;
                ac.AnswerFamily = "微软雅黑";
                ac.AnswerFontSize = 15.75F;
                Image lbimg = ac.DrawingArcFill(1, 1, System.Drawing.Color.FromArgb(254, 80, 79), 0, 41, System.Drawing.Color.FromArgb(254, 80, 79));
                Image textimg = ac.DrawingString(Brushes.White, i + "");
                bool ishasnum = false;

                for (int j = 0; j < nStudentCount; j++)
                {
                    string seat0 = ci.Data.Student[j].SEAT.Replace("-", "");
                    if (seat0 == "")
                        seat0 = "0";
                    int nSeat0 = Util.toInt(seat0);
                    if (i == nSeat0)
                    {
                        ishasnum = true;
                        break;
                    }
                }
                if (ishasnum)
                {
                    textimg = ac.DrawingString(Brushes.DimGray, i + "");
                    lbimg = ac.DrawingArcFill(1, 1, System.Drawing.Color.FromArgb(204, 204, 204), 2, 41, System.Drawing.Color.White);
                }
                pic.Location = new System.Drawing.Point(_locationWidth, _locationHeight);
                pic.Name = "click_" + i;
                pic.Size = new System.Drawing.Size(41, 41);
                pic.TabIndex = 0;
                pic.Image = lbimg;
                PictureBox text = new PictureBox();
                text.BackColor = System.Drawing.Color.Transparent;
                text.Location = new System.Drawing.Point(0, 0);
                text.Name = "text_" + i;
                text.Size = new System.Drawing.Size(41, 41);
                text.TabIndex = 0;
                text.Image = textimg;
                pic.Controls.Add(text);
                al.Add(pic);
                alText.Add(text);
                clickstate.Add(0);
                this.panel1.Controls.Add(pic);
            }
            return panelHeight;
        }

        private void SetPanel_WithXiti(string numberstr)
        {
            int panelHeight = 140;
            int panelLH = this.screenHeight - panelHeight;

            this.webBrowser1.Size = new System.Drawing.Size(screenWidth, screenHeight - panelHeight - 80);
            this.panel1.Location = new System.Drawing.Point(0, panelLH);
            this.panel1.Size = new System.Drawing.Size(this.screenWidth, panelHeight);

            this.panel2.Location = new System.Drawing.Point(0, panelLH);
            this.panel2.Size = new System.Drawing.Size(this.screenWidth, panelHeight);


        }
        private int SetPanel_Group()
        {
            int panelHeight = 140;
            int panelLH = this.screenHeight - panelHeight;

            Group[] glist;
            int _count = 0;
            int _width = 0;
            if (Global.m_grouplist != null)
            {
                glist = Global.m_grouplist.grouplist;
                _count = glist.Length;
                _width = (this.screenWidth) / _count;
            }
            else
            {
                glist = new Group[0];
            }



            if (bWithXitiResource)
            {
                this.webBrowser1.Size = new System.Drawing.Size(screenWidth, screenHeight - panelHeight - 80);
                this.panel1.Location = new System.Drawing.Point(0, panelLH);
                webBrowser1.Show();
                pictureBox_header.Visible = true;
            }
            else
            {
                webBrowser1.Hide();
                this.panel1.Location = new System.Drawing.Point(0, 0);//panelLH
            }
            this.panel1.Size = new System.Drawing.Size(this.screenWidth, panelHeight);
            this.panel1.BringToFront();
            int _lw = 0;
            int _br = 1;
            int _locationWidth = 0;
            int _locationHeight = 20;
            for (int i = 1; i <= _count; i++)
            {
                if(i <= _count/2)
                    _locationWidth = (_width - r)*6 / 10 + _width * (i - 1);
                else
                    _locationWidth = (_width - r)*4 / 10 + _width * (i - 1);

                _locationWidth = (_width - r) * 5 / 10 + _width * (i - 1);

                PictureBox pic = new PictureBox();
                AnswerCount ac = new AnswerCount();

                ac.ImageWidth = r;
                ac.ImagesHeight = r;
                ac.FontStyle = System.Drawing.FontStyle.Bold;
                ac.AnswerFamily = "微软雅黑";
                ac.AnswerFontSize = 18F;
                Image lbimg = ac.DrawingArcFill(1, 1, System.Drawing.Color.FromArgb(254, 80, 79), 0, r+1, System.Drawing.Color.FromArgb(254, 80, 79));
                Image textimg = ac.DrawingString(Brushes.White, glist[i-1].name + "组");
                textimg = ac.DrawingString(Brushes.DimGray, glist[i-1].name + "组");
                lbimg = ac.DrawingArcFill(1, 1, System.Drawing.Color.FromArgb(204, 204, 204), 2, r+1, System.Drawing.Color.White);

                pic.Location = new System.Drawing.Point(_locationWidth, _locationHeight);
                pic.Name = "click_" + i;
                pic.Size = new System.Drawing.Size(r+1, r+1);
                pic.TabIndex = 0;
                pic.Image = lbimg;
                PictureBox text = new PictureBox();
                text.BackColor = System.Drawing.Color.Transparent;
                text.Location = new System.Drawing.Point(0, 0);
                text.Name = "text_" + i;
                text.Size = new System.Drawing.Size(r, r);
                text.TabIndex = 0;
                text.Image = textimg;
                pic.Controls.Add(text);
                al.Add(pic);
                alText.Add(text);
                clickstate.Add(0);
                this.panel1.Controls.Add(pic);
            }
            return panelHeight;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;//最小化
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void Theout(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Interlocked.Exchange(ref inTimer, 1) == 0)
            {
                string json = "";
                if(mode==1)
                    json = Common.GetHandon();
                else
                    json = Common.GetXitiResult();

                Log.Debug("Theout.  "+json);

                
                if (json.Length>0)
                {
                    string data = json;

                    DateTime tm_now = DateTime.Now;
                    TimeSpan createtimespan = new TimeSpan(tm_create.Ticks);
                    TimeSpan nowtimespan = new TimeSpan(tm_now.Ticks);
                    TimeSpan timespan = nowtimespan.Subtract(createtimespan).Duration();
                    int timeDiff = timespan.Minutes*60 + timespan.Seconds;
                    Log.Info("Theout. handon=" + data + ", timediff=" + timeDiff);

                    //0-3:56AAA549,H|0-4:56AAA54A,H|
                    string context = "";
                    string[] szItem = data.Split('|');
                    for (int i = 0; i < szItem.Length; i++)
                    {
                        string item = szItem[i];
                        int num = Convert.ToInt16(item.Split(':')[0].ToString());
                        string answer = "";
                        if (mode == 1)
                            answer = "H";
                        else
                            answer = item.Split(':')[1];

                        string groupname = getGroup(num);
                        context = groupname + ":" + answer + ":" + timeDiff;

                        int _count = 0;
                        if (stuPushCount.Contains(num))
                        {
                            _count = (int)stuPushCount[num]+1;
                            stuPushCount.Remove(num);
                        }
                        stuPushCount.Add(num, _count);

                        if (_resutlSet.Contains(num))
                        {
                            ;//重复按
                        }
                        else
                        {
                            //_resutlSet.Add(num);//20160520 不缓存，因为经常导致不通知PAD。
                            Log.Debug("LabelStateEvent id=" + num + ",group="+groupname+", al.Count=" + al.Count);

                            if(Global.m_grouplist !=null)
                            {
                                int groupsize = Global.m_grouplist.grouplist.Length;
                                Group[] szGroup = Global.m_grouplist.grouplist;
                                for (int j = 0; j < groupsize; j++)
                                {
                                    PictureBox lb = (PictureBox)al[j];
                                    PictureBox text = (PictureBox)alText[j];
                                    if (szGroup[j].name == groupname && (int)clickstate[j] == 0)
                                    {
                                        LabelStateEvent(context, lb, text, j + 1);
                                        clickstate[j] = 1;
                                        Log.Debug("LabelStateEvent id=" + num +  ",group="+groupname+", al.Count=" + al.Count + ", ok.");
                                    }
                                }
                            }
                        }
                    }
                }
                Interlocked.Exchange(ref inTimer, 0); 
            }
        }

        public void LabelStateEvent(string context, PictureBox pic, PictureBox text,int i)
        {
            if (pic.InvokeRequired)
            {
                InvokeLabelState labelCallback = new InvokeLabelState(LabelStateEvent);
                pic.Invoke(labelCallback, new object[] { context, pic, text, i });
            }
            else
            {
                string[] szItem = context.Split(':');
                AnswerCount ac = new AnswerCount();
                ac.ImageWidth = r;
                ac.ImagesHeight = r;
                ac.FontStyle = System.Drawing.FontStyle.Bold;
                ac.AnswerFamily = "微软雅黑";
                ac.AnswerFontSize = 15.75F;
                pic.Image = ac.DrawingArcFill(1, 1, System.Drawing.Color.FromArgb(69, 175, 101), 0, r+1, System.Drawing.Color.FromArgb(69, 175, 101));
                text.Image = ac.DrawingString(Brushes.White, szItem[0] + "组");
                if (Global.Sound())
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    //sp.Play();
                }
                RESULT += (RESULT.Length > 0 ? "," : "") + context;
                Log.Debug("Result=" + RESULT);
            }
        }

        public void LabelStateEvent2(string context)
        {
            if (this.InvokeRequired)
            {
                InvokeLabelState2 labelCallback = new InvokeLabelState2(LabelStateEvent2);
                this.Invoke(labelCallback, new object[] { context });
            }
            else
            {
                
                RESULT += (RESULT.Length > 0 ? "," : "") + context;
                Log.Debug("Result=" + RESULT);
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Win32.AnimateWindow(this.Handle, 200, Win32.AW_SLIDE | Win32.AW_HIDE | Win32.AW_BLEND);
        }
        
        public string StopT()
        {
            if(t.Enabled)
                t.Enabled = false;
            string answerStr = RESULT.Replace(":H","");// fo.ReadFile();
            Log.Info("StopT() answerStr=" + answerStr);
            return answerStr;
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {
            this.Hide();
            Global.panelshow = 0;
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            this.Hide();
            Global.panelshow = 0;
        }

        public void HideRank()
        {
            for (int i = 0; i < 10; i++)
            {
                szRandNumber[i].Visible = false;
                szRandGroupname[i].Visible = false;
                szRandDash[i].Visible = false;
                szRandPoint[i].Visible = false;

            }
            pictureBox_top1.Visible = false;
            pictureBox_top2.Visible = false;
            pictureBox_top3.Visible = false;
        }
        public void ShowRank()
        {
            this.Height = screenHeight;
            this.Width = screenWidth;



            this.label_title.Visible = false;
            this.webBrowser1.Visible = false;
            this.panel1.Visible = false;
            this.panel2.Visible = false;
            this.pictureBox_header.Visible = true;

            List<Group> szGroup = Global.getGroupRanking();
            for (int i = 0; i < 10 && i<szGroup.Count(); i++)
            {
                szRandDash[i].Font = new System.Drawing.Font("黑体", 10F);
                szRandDash[i].ForeColor = System.Drawing.Color.LimeGreen;

                if(i<3){
                    szRandNumber[i].Text = "NO." + (i + 1);
                    szRandGroupname[i].Text = " "+szGroup[i].name + "组";
                    szRandPoint[i].Text = szGroup[i].point + "分";

                    szRandGroupname[i].Image = global::RueHelper.Properties.Resources.bg_top3;
                    szRandGroupname[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                    szRandNumber[i].Left = (int)(screenWidth * 0.4) / 10;
                    szRandGroupname[i].Left = (int)(screenWidth * 1.75) / 10;
                    szRandDash[i].Left = (int)(screenWidth * 3) / 10;
                    szRandPoint[i].Left = (int)(screenWidth * 3.6) / 10;

                    pictureBox_top1.Left = (int)(screenWidth * 1.5) / 10;
                    pictureBox_top2.Left = (int)(screenWidth * 1.5) / 10;
                    pictureBox_top3.Left = (int)(screenWidth * 1.5) / 10;


                    int topL = (int)(screenHeight * (1.8 + 3 * i)) / 10;
                    szRandNumber[i].Top = topL;
                    szRandGroupname[i].Top = topL;
                    szRandPoint[i].Top = topL-8;

                    //----Font & Color----
                    szRandNumber[i].Font = new System.Drawing.Font("微软雅黑", 30F);//微软雅黑
                    szRandGroupname[i].Font = new System.Drawing.Font("微软雅黑", 30F);
                    szRandPoint[i].Font = new System.Drawing.Font("微软雅黑", 38F);

                    szRandNumber[i].ForeColor = System.Drawing.Color.Black;
                    szRandGroupname[i].ForeColor = System.Drawing.Color.LimeGreen;
                    szRandPoint[i].ForeColor = System.Drawing.Color.LimeGreen;

                    szRandNumber[i].Visible = true;
                    szRandGroupname[i].Visible = true;
                    szRandPoint[i].Visible = true;


                    if (screenWidth > 1024)
                    {
                        szRandDash[i].Text = "---------";
                        szRandDash[i].Top = topL + 18;
                        szRandDash[i].TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                        szRandDash[i].Visible = true;
                    }
                    if(i==0)
                    {
                        pictureBox_top1.Top = szRandGroupname[0].Top - 10;
                        pictureBox_top1.Visible = true;
                    }
                    else if (i == 1)
                    {
                        pictureBox_top2.Top = szRandGroupname[1].Top - 10;
                        pictureBox_top2.Visible = true;
                    }
                    else
                    {
                        pictureBox_top3.Top = szRandGroupname[2].Top - 10;
                        pictureBox_top3.Visible = true;
                    }
                }
                else
                {
                    szRandNumber[i].Text = "NO." + (i + 1);
                    szRandGroupname[i].Text = szGroup[i].name + "组";
                    szRandPoint[i].Text = szGroup[i].point + "分";
                    if (screenWidth > 1024)
                        szRandDash[i].Text = "  --------------------------  ";
                    else
                        szRandDash[i].Text = "  -------------------  ";
                    szRandNumber[i].Left = (int)(screenWidth * 5.5) / 10;
                    szRandGroupname[i].Left = (int)(screenWidth * 6.5) / 10;
                    szRandDash[i].Left = (int)(screenWidth * 7.2) / 10;
                    szRandPoint[i].Left = (int)(screenWidth * 8.8) / 10;

                    int topR = (int)(screenHeight * (1.5 + 1.1 * (i - 3))) / 10;
                    szRandNumber[i].Top = topR;
                    szRandGroupname[i].Top = topR;
                    szRandPoint[i].Top = topR;
                    szRandDash[i].Top = topR+8;

                    //----Font & Color----
                    szRandNumber[i].Font = new System.Drawing.Font("微软雅黑", 22F);
                    szRandGroupname[i].Font = new System.Drawing.Font("微软雅黑", 22F);
                    szRandPoint[i].Font = new System.Drawing.Font("微软雅黑", 22F);

                    szRandNumber[i].ForeColor = System.Drawing.Color.Black;
                    szRandGroupname[i].ForeColor = System.Drawing.Color.Black;
                    szRandPoint[i].ForeColor = System.Drawing.Color.Black;

                    szRandDash[i].TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                    szRandPoint[i].TextAlign = System.Drawing.ContentAlignment.MiddleRight;

                    szRandNumber[i].Visible = true;
                    szRandGroupname[i].Visible = true;
                    szRandPoint[i].Visible = true;
                    szRandDash[i].Visible = true;
                }

                szRandGroupname[i].BringToFront();
            }

            //3秒显示，自动隐藏
            this.Show();
            this.BringToFront();

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 30000;
            t.Tick += new EventHandler(t_Hide);
            t.Start();
        }
    }
}
