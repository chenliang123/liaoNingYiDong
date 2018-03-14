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
    public partial class FormNotify : Form
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void CallUpdateForm(string title, string msg);
        public delegate void CallClose();
        public int m_code = 0;
        public System.Windows.Forms.Timer tm_hide;
        public static bool m_PPTImgExporting = false;
        public FormNotify()
        {
            
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            double taskbarHeight = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Height;

            this.Height = 50;
            this.Width = 55;
            this.BackColor = System.Drawing.Color.FromArgb(251, 251, 251);
            StartPosition = FormStartPosition.Manual;
            SetDesktopLocation(screenWidth - this.Width,5);
            
            this.TopMost = true;
            this.BringToFront();
            this.Hide();
            
        }
       
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (Form1.formWeb == null)
            {
                Form1.formWeb = new FormWebBrowser();
            }
            Form1.formWeb.Hide();
        }

    }
}
