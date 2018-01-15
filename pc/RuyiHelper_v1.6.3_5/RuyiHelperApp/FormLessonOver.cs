using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Media;
using RueHelper.model;
using Newtonsoft.Json.Linq;
using RueHelper.util;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RueHelper
{
    public partial class FormLessonOver : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        public FormLessonOver()
        {
            InitializeComponent();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form1.ShowController(true);

            this.WindowState = FormWindowState.Minimized;
            this.Dispose();
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //TODO: 下课事件处理
            Form1.ShowController(false);
            Common.setLessonOff(0, "", "");

            this.WindowState = FormWindowState.Minimized;
            this.Dispose();
            this.Close();
        }
    }
}
