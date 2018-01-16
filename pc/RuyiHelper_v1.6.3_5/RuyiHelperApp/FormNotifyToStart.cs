using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;



namespace RueHelper
{
    public partial class FormNotifyToStart : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public FormNotifyToStart()//string title, string msg, int seconds
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            double taskbarHeight = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Height;

            StartPosition = FormStartPosition.Manual;
            SetDesktopLocation(screenWidth - this.Width, screenHeight - this.Height - (int)taskbarHeight);
            
            //label2.Text = msg;
            this.BringToFront();           
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //TODO:WQ 开始上课
            //EService.ShowSelectTeacher(true,1);

            string data = DateTime.Now.ToString("yyyyMMddHHmmss");
            string url = "http://221.179.197.232/Sso/login?appid=ca1b0dbcf9&callback_uri=http://127.0.0.1:8986/loginSuccess&clientid=" + data;
            Form1.formWeb = new FormWebBrowser();
             //Form1.formWeb.
            //Form1.formWeb.Document.body.width = "740px";
            Form1.formWeb.navigate(url);
            this.Hide();

            //Form1.fController.AllShow();
        }

        private void FormNotifyToStart_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void FormNotifyToStart_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
