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
    public partial class Form_xiti2 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form_xiti2()
        {
            InitializeComponent();
            UpdateStudent();

        }
        public void UpdateStudent()
        {
            List<string> list1 = new List<string>();
            //模拟数据
            list1.Add("ABCD|1,2,4,3,5,6,7,8,9,10,11,12,13,14");
            list1.Add("ABD|15,16,17,18,19,20");
            list1.Add("ABC|21,22,23,24,25");
            list1.Add("AB|26,27,28,29,30.31,32,33");
            list1.Add("C|34,35,36,37,38,39,40");

            label_answer.Text = "正确答案: "+"ABCD";
            label_answerRatio.Text = "答题率： 80%";
            label_rightAnswerRatio.Text = "正确率： 60%";

            List<string> xData = new List<string>();
            List<int> yData = new List<int>();
            //chart1.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
            //chart1.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
            //chart1.Series[0].Points.DataBindXY(xData, yData);

            int line = 1;
            for (int i = 0; i < list1.Count; i++)
            {
                string str = list1[i];
                string key = str.Split('|')[0];
                xData.Add(key);//饼图数据

                string[] szUid = str.Split('|')[1].Split(',');
                yData.Add(szUid.Length);//饼图数据
               
                Label labelKey = new Label();
                labelKey.AutoSize = true;
                labelKey.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
                labelKey.Location = new System.Drawing.Point(20, 75 + (line - 1) * 30);
                labelKey.Name = "label" + i;
                labelKey.Size = new System.Drawing.Size(50, 20);
                labelKey.TabIndex = 3;
                labelKey.Text = key+"("+szUid.Length+"人)";////////
                this.Controls.Add(labelKey);

                int col = 1;
                for(int j=0; j<szUid.Length; j++)
                {
                    col = j % 5;
                    if(j>0 && j%5==0)
                    {
                        line++;
                    }
                    string uid = szUid[i];
                    string studentName = "张三三";
                    StudentInfo si = Global.getStudentByUid(Int32.Parse(uid));
                    if(si!=null)
                        studentName = si.Name;
                    
                    Label label = new Label();
                    label.AutoSize = true;
                    label.Font = new System.Drawing.Font("微软雅黑", 11F);
                    label.Location = new System.Drawing.Point(120 + col * 60, 75 + (line - 1) * 30);
                    label.Name = "label" + studentName;
                    label.Size = new System.Drawing.Size(50, 20);
                    label.TabIndex = 3;
                    label.Text = studentName;/////////
                    this.Controls.Add(label);
                }
                line++;
                
            }

            chart1.Series[0]["PieLabelStyle"] = "InSide";//将文字移到外侧

            chart1.Series[0].Points.DataBindXY(xData, yData);
        }


        private void label_close_Click(object sender, EventArgs e)
        {
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
