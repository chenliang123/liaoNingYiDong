﻿using System;
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
            EService.ShowSelectTeacher(true,1);
            this.Hide();
        }

        private void FormNotifyToStart_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void FormNotifyToStart_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
