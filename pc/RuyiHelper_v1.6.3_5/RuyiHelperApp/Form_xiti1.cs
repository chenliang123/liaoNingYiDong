using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RueHelper
{
    public partial class Form_xiti1 : FormWithMsgBox
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form_xiti1()
        {
            InitializeComponent();
            UpdateStudent();

            this.panel_msgbox.Visible = false;
            //Type(this, 20, 0.15);//圆角
            //Type(this.panel1, 20, 0.1);
        }
        public void UpdateStudent()
        {
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            //获取数据
            for (int i = 0; i < 20; i++ )
                list1.Add("张三");
            for (int i = 0; i < 10; i++)
                list2.Add("李四");

            label_withAnswer.Text = "已答 : "+list1.Count+"人";
            label_withoutAnswer.Text = "未答 : " + list2.Count + "人";
            
            for (int i = 1; i <= list1.Count; i++)
            {
                int line = 1 + (i-1) / 5;
                int col = i % 5;
                if (col == 0)
                    col = 5;
                Label label = new Label();
                label.AutoSize = true;
                label.Font = new System.Drawing.Font("微软雅黑", 11F);
                label.Location = new System.Drawing.Point(25 + (col - 1) * 60, 75 + (line - 1) * 50);
                label.Name = "label"+i;
                label.Size = new System.Drawing.Size(50, 20);
                label.TabIndex = 3;
                label.Text = "张三三";
                this.Controls.Add(label);
            }
            //显示数据
            for (int i = 1; i <= list2.Count; i++)
            {
                int line = 1 + (i-1)/5;
                int col = i % 5;
                if (col == 0)
                    col = 5;
                Label label = new Label();
                label.AutoSize = true;
                label.Font = new System.Drawing.Font("微软雅黑", 11F);
                label.Location = new System.Drawing.Point(360 + (col - 1) * 60, 75 + (line - 1) * 50);
                label.Name = "label" + i;
                label.Size = new System.Drawing.Size(50 , 20);
                label.TabIndex = 3;
                label.Text = "张三三";
                this.Controls.Add(label);
            }
        }


        private void label_close_Click(object sender, EventArgs e)
        {
            this.panel_msgbox.Visible = true;
            this.panel_msgbox.BringToFront();      
            //this.WindowState = FormWindowState.Minimized;
            //this.Dispose();
            //this.Close();
        }
        public override void Cancel()
        {
            //继续作答
            this.WindowState = FormWindowState.Minimized;
            this.Dispose();
            this.Close();

        }
        public override void OK()
        {
            //结束作答
            this.WindowState = FormWindowState.Minimized;
            this.Dispose();
            this.Close();
        }
        private void Form_xiti1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            } 
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            } 
        }
    }
}
