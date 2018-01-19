using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Windows.Forms;


namespace RueHelper
{
    [ComVisible(true)]
    public partial class FormWebBrowser : Form
    {
        public EService server;
        public FormWebBrowser()
        {
            IEVersion.BrowserEmulationSet();

            InitializeComponent();
            initWebForm();
        }
        public FormWebBrowser(int width, int height, string path)
        {
            IEVersion.BrowserEmulationSet();

            InitializeComponent();
            //-------------------------
            try
            {
                this.webBrowser1.ObjectForScripting = this;//与c#代码通信，这句是关键
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            this.ClientSize = new System.Drawing.Size(width, height);
            this.webBrowser1.Navigate(path);
            this.TopMost = false;
            this.Hide();
        }
        public void initWebForm()
        {

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            double taskbarHeight = screenHeight - SystemInformation.WorkingArea.Height;

            //小窗口居中
            //StartPosition = FormStartPosition.Manual;
            //int top = screenHeight - this.Height - (int)taskbarHeight- 64;
            //int left = (screenWidth - this.Width) / 2;

            //全屏，留下任务栏
            this.ClientSize = new System.Drawing.Size(screenWidth, screenHeight);
            int top = 0;
            int left = 0;
            SetDesktopLocation(left, top);
            try
            {
                this.webBrowser1.ObjectForScripting = this;//与c#代码通信，这句是关键
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            //string path = Application.StartupPath + @"\html\vue.html";
            //this.webBrowser1.Navigate(path);
            this.TopMost = true;
            this.Hide();
        }
        public void navigate(String url)
        {
            //url = "http://ie.icoa.cn/";
            this.Show();
            this.TopMost = false;
            this.webBrowser1.Navigate(url);
        }
        public void showWeb()
        {
            this.Show();
        }
        public void setShow()
        {
           //this.webBrowser1.Document.GetElementsByTagName("body")[0].SetAttribute("width","400px");
           this.webBrowser1.Width = 600;
        }
        public void hideWeb()
        {
            int temp = extiAnswer();
            if (temp >= 0)
            {
                this.Hide();
            }
        }
        public int extiAnswer()
        {
            return 12;
        }
        public void getScreen()
        {
            //FormPPTPractise.screenshot();
        }
        public string getList()
        {
            string str = "";
            User[] arrStr = Global.g_Studentlist.ToArray();
            for (int i = 0; i < arrStr.Length; i++)
            {
                str += "{" + "\"cardid\":" + arrStr[i].cardid + ",\"classid\":" + arrStr[i].classid + ",\"id\":" + arrStr[i].id + ",\"seat\":" + arrStr[i].seat + ",\"tanswer\":\"\"" + ",\"name\":\"" + arrStr[i].name + "\"}|";
            }
            return str;
        }
        public string getAnswerList()
        {
            return Global.AnswerStu;
        }
        public string getInfor()
        {
            string data = Global.getSchoolID() + "," + Global.getTeacherID() + "," + Global.getClassID() + "," + Global.getCourseID() + "," + Global.getLessonID();
            return data;
        }
        public int recieveData(string answer, string type)
        {
            int temp = 0;
            if (server == null)
            {
                server = new EService();
            };
            string courseid = Global.getCourseID().ToString();
            string classid = Global.getClassID().ToString();
            string lessonid = Global.getLessonID().ToString();
            server.HandonOver("-1", "");
           
            server.SingleProjectiveInPPT(courseid, classid, lessonid);
            
            server.SetAnswer(answer);
            return temp;
        }
    }
}