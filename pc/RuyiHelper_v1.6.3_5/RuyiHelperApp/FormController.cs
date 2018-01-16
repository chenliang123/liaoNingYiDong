using CCWin;
using RueHelper.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RueHelper
{
    public partial class FormController : SkinMain
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        private int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        private FormCaptureScreen2 formDraw = null;
        private Form_xiti1 form_xiti1 = null;
        private Form_xiti2 form_xiti2 = null;
        #region 【文件夹-试题】的控件：全体作答，抢答，随机，分组
        PictureBox pictureBox_xiti_select = new System.Windows.Forms.PictureBox();
        PictureBox pictureBox_xiti_truefalse = new System.Windows.Forms.PictureBox();
        #endregion

        #region 【文件夹-试题-全体作答】的控件： 统计，结束作答
        Label label_CountOfNoAnswer = new System.Windows.Forms.Label();
        Label label_CountOfAnswer = new System.Windows.Forms.Label();
        PictureBox pictureBox_xiti_splitter = new System.Windows.Forms.PictureBox();
        PictureBox pictureBox_xiti_stop = new System.Windows.Forms.PictureBox();
        PictureBox pictureBox_xiti_stat = new System.Windows.Forms.PictureBox();
        #endregion

        #region 【画笔】的控件
        PictureBox pictureBox_rub = new System.Windows.Forms.PictureBox();
        PictureBox pictureBox_dot3 = new System.Windows.Forms.PictureBox();
        PictureBox pictureBox_dot2 = new System.Windows.Forms.PictureBox();
        PictureBox pictureBox_dot1 = new System.Windows.Forms.PictureBox();
        PictureBox pictureBox_blue = new System.Windows.Forms.PictureBox();
        PictureBox pictureBox_yellow = new System.Windows.Forms.PictureBox();
        PictureBox pictureBox_red = new System.Windows.Forms.PictureBox();
        #endregion

        private int path = 0;
        public FormController()
        {
            InitializeComponent();
            int top = SystemInformation.WorkingArea.Height - this.Height;
            StartPosition = FormStartPosition.Manual;
            SetDesktopLocation((screenWidth - this.Width) / 2, top);

            this.ShowInTaskbar = false;
            //this.WindowState = FormWindowState.Minimized;

            CreatePenPage();
            CreateXitiPage();

            ShowMainPage();
        }
        public void Display(bool bShow)
        {
            try
            {
                if (bShow)
                {
                    this.Show();
                    this.AllShow();
                    this.ShowInTaskbar = false;
                    this.SetTopMost(true);
                    this.TopMost = true;
                }
                else
                {
                    this.Hide();
                    this.AllHide();
                    this.SetShowInTaskbar(false);
                }
            }
            catch (Exception e)
            {

            }
        }
        #region 首页区
        public void ShowMainPage()
        {
            path = 0;//首页区
            this.pictureBox_main_pen.Visible = true;
            this.pictureBox_main_folder.Visible = true;
            this.pictureBox_main_xitis.Visible = true;
            this.pictureBox_main_collect.Visible = true;
            this.pictureBox_main_lessonover.Visible = true;
            this.pictureBox_main_exam.Visible = true;
            this.pictureBox_home.Visible = false;

            this.pictureBox_main_pen.BackgroundImage = Properties.Resources.controller_pen;

            this.pictureBox_main_pen.Size = new System.Drawing.Size(86, 62);
            this.pictureBox_main_pen.Location = new System.Drawing.Point(12, 0);

            this.pictureBox_main_folder.Size = new System.Drawing.Size(102, 63);//课堂出题
            this.pictureBox_main_folder.Location = new System.Drawing.Point(80, 0);

            this.pictureBox_main_collect.Size = new System.Drawing.Size(76, 63);
            this.pictureBox_main_collect.Location = new System.Drawing.Point(200, 0);

            this.pictureBox_main_xitis.Size = new System.Drawing.Size(76, 63);
            this.pictureBox_main_xitis.Location = new System.Drawing.Point(300, 0);

            this.pictureBox_main_exam.Size = new System.Drawing.Size(76, 63);
            this.pictureBox_main_exam.Location = new System.Drawing.Point(400, 0);

            this.pictureBox_main_lessonover.Size = new System.Drawing.Size(47, 63);
            this.pictureBox_main_lessonover.Location = new System.Drawing.Point(500, 0);

            HidePen();
            HideXiti();
        }
        private void HideMainPage()
        {
            this.pictureBox_main_pen.Visible = false;
            this.pictureBox_main_folder.Visible = false;
            this.pictureBox_main_xitis.Visible = false;
            this.pictureBox_main_collect.Visible = false;
            this.pictureBox_main_lessonover.Visible = false;
            this.pictureBox_main_exam.Visible = false;
            this.pictureBox_home.Visible = true;
        }

        private void pictureBox_main_pen_click(object sender, EventArgs e)
        {
            if (!this.panel_pen.Visible)
            {
                this.ShowPen();
            }
        }
        private void pictureBox_main_folder_Click(object sender, EventArgs e)
        {
            this.ShowFolders();
        }
        private void pictureBox_main_xitis_Click(object sender, EventArgs e)
        {
            this.ShowXitis();
        }
        private void pictureBox_main_xiti_Click(object sender, EventArgs e)
        {
            this.ShowXiti();
        }
        private void pictureBox_main_exam_Click(object sender, EventArgs e)
        {
            //截屏
            Size size = new System.Drawing.Size(screenWidth, SystemInformation.WorkingArea.Height - 80);
            this.TopMost = false;
            try
            {
                Bitmap bmp = ScreenCapture.captureScreen(0, 0);
                string base64 = Util.ImgToBase64String(bmp);
                if (true)
                {
                    //调用web
                    string url = Application.StartupPath + @"\html\exam.html";
                    Form1.formWeb = new FormWebBrowser();
                    Form1.formWeb.navigate(url);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
        private void pictureBox_main_lessonover_Click(object sender, EventArgs e)
        {
            //EService.ShowSelectTeacher(true, 2);
            FormLessonOver f = new FormLessonOver();
            f.Show();
        }
        #endregion


        #region   画笔区panel_pen----创建，显示和隐藏，功能键函数
        private void CreatePenPage()
        {
            if (this.panel_pen == null)
                this.panel_pen = new System.Windows.Forms.Panel();
            this.panel_pen.Hide();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormController));
            
            this.panel_pen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_rub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_dot3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_dot2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_dot1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_blue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_yellow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_red)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_pen
            // 
            this.panel_pen.BackgroundImage = Properties.Resources.controller_panel_pen;
            this.panel_pen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_pen.Controls.Add(pictureBox_rub);
            this.panel_pen.Controls.Add(pictureBox_dot3);
            this.panel_pen.Controls.Add(pictureBox_dot2);
            this.panel_pen.Controls.Add(pictureBox_dot1);
            this.panel_pen.Controls.Add(pictureBox_blue);
            this.panel_pen.Controls.Add(pictureBox_yellow);
            this.panel_pen.Controls.Add(pictureBox_red);
            this.panel_pen.Location = new System.Drawing.Point(104, 0);
            this.panel_pen.Name = "panel_pen";
            this.panel_pen.Size = new System.Drawing.Size(408, 68);
            this.panel_pen.TabIndex = 5;
            // 
            // pictureBox_rub
            // 
            pictureBox_rub.BackColor = System.Drawing.Color.Transparent;
            pictureBox_rub.BackgroundImage = Properties.Resources.pen_rub0;
            pictureBox_rub.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_rub.Location = new System.Drawing.Point(356, 0);
            pictureBox_rub.Name = "pictureBox_rub";
            pictureBox_rub.Size = new System.Drawing.Size(45, 63);
            pictureBox_rub.TabIndex = 6;
            pictureBox_rub.TabStop = false;
            pictureBox_rub.Click += new System.EventHandler(this.pictureBox_rub_Click);
            pictureBox_rub.MouseLeave += new System.EventHandler(this.pictureBox_rub_MouseLeave);
            // 
            // pictureBox_dot3
            // 
            pictureBox_dot3.BackColor = System.Drawing.Color.Transparent;
            pictureBox_dot3.BackgroundImage = Properties.Resources.pen_dot30;
            pictureBox_dot3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_dot3.Location = new System.Drawing.Point(293, -1);
            pictureBox_dot3.Name = "pictureBox_dot3";
            pictureBox_dot3.Size = new System.Drawing.Size(53, 64);
            pictureBox_dot3.TabIndex = 5;
            pictureBox_dot3.TabStop = false;
            pictureBox_dot3.Click += new System.EventHandler(this.pictureBox_dot3_Click);
            // 
            // pictureBox_dot2
            // 
            pictureBox_dot2.BackColor = System.Drawing.Color.Transparent;
            pictureBox_dot2.BackgroundImage = Properties.Resources.pen_dot20;
            pictureBox_dot2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_dot2.Location = new System.Drawing.Point(237, 0);
            pictureBox_dot2.Name = "pictureBox_dot2";
            pictureBox_dot2.Size = new System.Drawing.Size(54, 63);
            pictureBox_dot2.TabIndex = 4;
            pictureBox_dot2.TabStop = false;
            pictureBox_dot2.Click += new System.EventHandler(this.pictureBox_dot2_Click);
            // 
            // pictureBox_dot1
            // 
            pictureBox_dot1.BackColor = System.Drawing.Color.Transparent;
            pictureBox_dot1.BackgroundImage = Properties.Resources.pen_dot10;
            pictureBox_dot1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_dot1.Location = new System.Drawing.Point(187, 0);
            pictureBox_dot1.Name = "pictureBox_dot1";
            pictureBox_dot1.Size = new System.Drawing.Size(55, 63);
            pictureBox_dot1.TabIndex = 3;
            pictureBox_dot1.TabStop = false;
            pictureBox_dot1.Click += new System.EventHandler(this.pictureBox_dot1_Click);
            // 
            // pictureBox_blue
            // 
            pictureBox_blue.BackColor = System.Drawing.Color.Transparent;
            pictureBox_blue.BackgroundImage = Properties.Resources.pen_blue0;
            pictureBox_blue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_blue.Location = new System.Drawing.Point(124, 0);
            pictureBox_blue.Name = "pictureBox_blue";
            pictureBox_blue.Size = new System.Drawing.Size(58, 63);
            pictureBox_blue.TabIndex = 2;
            pictureBox_blue.TabStop = false;
            pictureBox_blue.Click += new System.EventHandler(this.pictureBox_blue_Click);
            // 
            // pictureBox_yellow
            // 
            pictureBox_yellow.BackColor = System.Drawing.Color.Transparent;
            pictureBox_yellow.BackgroundImage = Properties.Resources.pen_yellow0;
            pictureBox_yellow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_yellow.Location = new System.Drawing.Point(68, 0);
            pictureBox_yellow.Name = "pictureBox_yellow";
            pictureBox_yellow.Size = new System.Drawing.Size(58, 63);
            pictureBox_yellow.TabIndex = 1;
            pictureBox_yellow.TabStop = false;
            pictureBox_yellow.Click += new System.EventHandler(this.pictureBox_yellow_Click);
            // 
            // pictureBox_red
            // 
            pictureBox_red.BackColor = System.Drawing.Color.Transparent;
            pictureBox_red.BackgroundImage = Properties.Resources.pen_red0;
            pictureBox_red.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_red.Location = new System.Drawing.Point(13, 0);
            pictureBox_red.Name = "pictureBox_red";
            pictureBox_red.Size = new System.Drawing.Size(63, 63);
            pictureBox_red.TabIndex = 0;
            pictureBox_red.TabStop = false;
            pictureBox_red.Click += new System.EventHandler(this.pictureBox_red_Click);

            this.Controls.Add(this.panel_pen);

            this.panel_pen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(pictureBox_rub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_dot3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_dot2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_dot1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_blue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_yellow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_red)).EndInit();
            this.ResumeLayout(false);
        }
        public void ShowPen()
        {
            path = 10;//画笔区
            HideMainPage();
            //HidePen();
            //HideFolders();
            //HideXiti();

            this.pictureBox_main_pen.Visible = true;
            this.panel_pen.Visible = true;

            CreateDrawRegion();

            //设置默认的线形
            pictureBox_yellow.BackgroundImage = Properties.Resources.pen_yellow1;
            pictureBox_dot2.BackgroundImage = Properties.Resources.pen_dot21;

            pictureBox_red.BackgroundImage = Properties.Resources.pen_red0;
            pictureBox_blue.BackgroundImage = Properties.Resources.pen_blue0;
            pictureBox_dot1.BackgroundImage = Properties.Resources.pen_dot10;
            pictureBox_dot3.BackgroundImage = Properties.Resources.pen_dot30;

            formDraw.setLineWidth(6);
            formDraw.setLineColor("#fff400");
        }
        private void CreateDrawRegion()
        {
            if (formDraw != null)
            {
                formDraw.Close();
                formDraw = null;
            }
            try
            {
                formDraw = new FormCaptureScreen2();
                formDraw.setLineWidth(6);
                formDraw.setLineColor("#0f0");
                formDraw.Show();
            }
            catch (Exception ex)
            {

            }
        }
        private void HidePen()
        {
            this.panel_pen.Visible = false;
            if(formDraw!=null)
            {
                formDraw.Dispose();
                formDraw.Close();
                formDraw = null;
            }
        }
        private void pictureBox_dot3_Click(object sender, EventArgs e)
        {
            formDraw.setLineWidth(9);
            this.pictureBox_dot1.BackgroundImage = Properties.Resources.pen_dot10;
            this.pictureBox_dot2.BackgroundImage = Properties.Resources.pen_dot20;
            this.pictureBox_dot3.BackgroundImage = Properties.Resources.pen_dot31;
            this.pictureBox_rub.BackgroundImage = Properties.Resources.pen_rub0;
        }
        private void pictureBox_dot2_Click(object sender, EventArgs e)
        {
            formDraw.setLineWidth(6);
            this.pictureBox_dot1.BackgroundImage = Properties.Resources.pen_dot10;
            this.pictureBox_dot2.BackgroundImage = Properties.Resources.pen_dot21;
            this.pictureBox_dot3.BackgroundImage = Properties.Resources.pen_dot30;
            this.pictureBox_rub.BackgroundImage = Properties.Resources.pen_rub0;
        }
        private void pictureBox_dot1_Click(object sender, EventArgs e)
        {
            formDraw.setLineWidth(3);
            this.pictureBox_dot1.BackgroundImage = Properties.Resources.pen_dot11;
            this.pictureBox_dot2.BackgroundImage = Properties.Resources.pen_dot20;
            this.pictureBox_dot3.BackgroundImage = Properties.Resources.pen_dot30;
            this.pictureBox_rub.BackgroundImage = Properties.Resources.pen_rub0;
        }
        private void pictureBox_blue_Click(object sender, EventArgs e)
        {
            formDraw.setLineColor("#00E3FF");
            this.pictureBox_blue.BackgroundImage = Properties.Resources.pen_blue1;
            this.pictureBox_yellow.BackgroundImage = Properties.Resources.pen_yellow0;
            this.pictureBox_red.BackgroundImage = Properties.Resources.pen_red0;
            this.pictureBox_rub.BackgroundImage = Properties.Resources.pen_rub0;
        }
        private void pictureBox_yellow_Click(object sender, EventArgs e)
        {
            formDraw.setLineColor("#fff400");
            this.pictureBox_blue.BackgroundImage = Properties.Resources.pen_blue0;
            this.pictureBox_yellow.BackgroundImage = Properties.Resources.pen_yellow1;
            this.pictureBox_red.BackgroundImage = Properties.Resources.pen_red0;
            this.pictureBox_rub.BackgroundImage = Properties.Resources.pen_rub0;
        }
        private void pictureBox_red_Click(object sender, EventArgs e)
        {
            formDraw.setLineColor("#FF0000");
            this.pictureBox_blue.BackgroundImage = Properties.Resources.pen_blue0;
            this.pictureBox_yellow.BackgroundImage = Properties.Resources.pen_yellow0;
            this.pictureBox_red.BackgroundImage = Properties.Resources.pen_red1;
            this.pictureBox_rub.BackgroundImage = Properties.Resources.pen_rub0;
        }
        private void pictureBox_rub_Click(object sender, EventArgs e)
        {
            this.pictureBox_rub.BackgroundImage = Properties.Resources.pen_rub1;

            //TODO-wq：清空
            formDraw.ClearView();

            string lineColor = formDraw.getLineColor();
            int lineWidth = formDraw.getLineWidth();
            //CreateDrawRegion();
            //formDraw.setLineColor(lineColor);
            //formDraw.setLineWidth(lineWidth);
        }
        private void pictureBox_rub_MouseLeave(object sender, EventArgs e)
        {
            //TODO-wq：清空
            this.pictureBox_rub.BackgroundImage = Properties.Resources.pen_rub0;
        }
        
        #endregion


        #region 课堂出题 panel_folder
        public void ShowFolders()
        {
            this.AllHide();

            //this.Height = 0;
            path = 20;//文件夹
            Bitmap bmp = ScreenCapture.captureScreen(0, 0);
            string base64 = Util.ImgToBase64String(bmp);

            int imgHeight = bmp.Height;
            int imgWidth = bmp.Width;

            string imgName = DateTime.Now.ToString("yyyyMMdd") + "-" + Global.getSchoolID() + "-" + Global.getClassID() + "-" + DateTime.Now.ToString("HHmmss") + ".png";
            string imgDir = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(imgDir))
                Directory.CreateDirectory(imgDir);

            string imgPath = imgDir + "\\" + imgName;
            bmp.Save(imgPath);

            Common.uploadPicture(imgPath);

            this.AllShow();
            this.TopMost = false;

            string imgPathEncoded = System.Web.HttpUtility.UrlEncode(imgPath, Encoding.UTF8);
            string url = Application.StartupPath + @"\html\xiti.html?#" + imgName;
            Form1.formWeb = new FormWebBrowser();
            Form1.formWeb.navigate(url);
        }

        public void showColection()
        {
            this.TopMost = false;

            string url = Application.StartupPath + @"\html\vue.html";
            Form1.formWeb = new FormWebBrowser();
            Form1.formWeb.navigate(url);


        }



        //试题
        private void pictureBox_folder_exam_Click(object sender, EventArgs e)
        {
            this.ShowXiti();
        }
        
        #endregion

        #region 文件夹|试题区 panel_xiti
        private void CreateXitiPage()
        {
            if (this.panel_xiti == null)
                this.panel_xiti = new System.Windows.Forms.Panel();
            this.panel_xiti.Hide();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormController));
            
            this.panel_xiti.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_xiti_select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_xiti_truefalse)).BeginInit();

            ((System.ComponentModel.ISupportInitialize)(pictureBox_xiti_splitter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_xiti_stop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_xiti_stat)).BeginInit();
            this.SuspendLayout();
            
            // 
            // panel_xiti
            // 
            panel_xiti.BackgroundImage = Properties.Resources.controller_panel;
            panel_xiti.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            panel_xiti.Controls.Add(pictureBox_xiti_truefalse);
            panel_xiti.Controls.Add(pictureBox_xiti_select);

            panel_xiti.Controls.Add(label_CountOfNoAnswer);
            panel_xiti.Controls.Add(label_CountOfAnswer);
            panel_xiti.Controls.Add(pictureBox_xiti_splitter);
            panel_xiti.Controls.Add(pictureBox_xiti_stop);
            panel_xiti.Controls.Add(pictureBox_xiti_stat);

            panel_xiti.Location = new System.Drawing.Point(100, 0);
            panel_xiti.Name = "panel_xiti";
            panel_xiti.Size = new System.Drawing.Size(400, 68);
            panel_xiti.TabIndex = 5;
            // 
            // pictureBox_xiti_random
            // 
            pictureBox_xiti_select.BackColor = System.Drawing.Color.Transparent;
            pictureBox_xiti_select.BackgroundImage = Properties.Resources.controller_xiti_select;
            pictureBox_xiti_select.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_xiti_select.Location = new System.Drawing.Point(60, 0);
            pictureBox_xiti_select.Name = "选择题";
            pictureBox_xiti_select.Size = new System.Drawing.Size(104, 64);
            pictureBox_xiti_select.TabIndex = 8;
            pictureBox_xiti_select.TabStop = false;
            pictureBox_xiti_select.Click += new System.EventHandler(this.pictureBox_xiti_select_Click);

            // 
            // pictureBox_xiti_group
            // 
            pictureBox_xiti_truefalse.BackColor = System.Drawing.Color.Transparent;
            pictureBox_xiti_truefalse.BackgroundImage = Properties.Resources.controller_xiti_truefalse;
            pictureBox_xiti_truefalse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_xiti_truefalse.Location = new System.Drawing.Point(230, 0);
            pictureBox_xiti_truefalse.Name = "pictureBox_xiti_group";
            pictureBox_xiti_truefalse.Size = new System.Drawing.Size(104, 64);
            pictureBox_xiti_truefalse.TabIndex = 7;
            pictureBox_xiti_truefalse.TabStop = false;
            pictureBox_xiti_truefalse.Click += new System.EventHandler(this.pictureBox_xiti_truefalse_Click);


            //全部作答-下级按钮
            // 
            // label_CountOfNoAnswer
            // 
            label_CountOfNoAnswer.AutoSize = true;
            label_CountOfNoAnswer.BackColor = System.Drawing.Color.Transparent;
            label_CountOfNoAnswer.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label_CountOfNoAnswer.ForeColor = System.Drawing.Color.White;
            label_CountOfNoAnswer.Location = new System.Drawing.Point(286, 35);
            label_CountOfNoAnswer.Name = "label_CountOfNoAnswer";
            label_CountOfNoAnswer.Size = new System.Drawing.Size(81, 20);
            label_CountOfNoAnswer.TabIndex = 10;
            label_CountOfNoAnswer.Text = "未答：12人";
            // 
            // label_CountOfAnswer
            // 
            label_CountOfAnswer.AutoSize = true;
            label_CountOfAnswer.BackColor = System.Drawing.Color.Transparent;
            label_CountOfAnswer.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label_CountOfAnswer.ForeColor = System.Drawing.Color.White;
            label_CountOfAnswer.Location = new System.Drawing.Point(286, 9);
            label_CountOfAnswer.Name = "label_CountOfAnswer";
            label_CountOfAnswer.Size = new System.Drawing.Size(81, 20);
            label_CountOfAnswer.TabIndex = 9;
            label_CountOfAnswer.Text = "已答：12人";
            // 
            // pictureBox_xiti_splitter
            // 
            pictureBox_xiti_splitter.BackColor = System.Drawing.Color.Transparent;
            pictureBox_xiti_splitter.BackgroundImage = Properties.Resources.controller_splitter;
            pictureBox_xiti_splitter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_xiti_splitter.Location = new System.Drawing.Point(267, 0);
            pictureBox_xiti_splitter.Name = "pictureBox_splitter";
            pictureBox_xiti_splitter.Size = new System.Drawing.Size(10, 64);
            pictureBox_xiti_splitter.TabIndex = 8;
            pictureBox_xiti_splitter.TabStop = false;
            // 
            // pictureBox_xiti_stop
            // 
            pictureBox_xiti_stop.BackColor = System.Drawing.Color.Transparent;
            pictureBox_xiti_stop.BackgroundImage = Properties.Resources.controller_xiti_stop0;
            pictureBox_xiti_stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_xiti_stop.Location = new System.Drawing.Point(153, -1);
            pictureBox_xiti_stop.Name = "pictureBox_xiti_stop";
            pictureBox_xiti_stop.Size = new System.Drawing.Size(104, 64);
            pictureBox_xiti_stop.TabIndex = 7;
            pictureBox_xiti_stop.TabStop = false;
            pictureBox_xiti_stop.Click += new System.EventHandler(this.pictureBox_xiti_stop_Click);
            // 
            // pictureBox_xiti_stat
            // 
            pictureBox_xiti_stat.BackColor = System.Drawing.Color.Transparent;
            pictureBox_xiti_stat.BackgroundImage = Properties.Resources.controller_xiti_stat0;
            pictureBox_xiti_stat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBox_xiti_stat.Location = new System.Drawing.Point(34, 0);
            pictureBox_xiti_stat.Name = "pictureBox_xiti_stat";
            pictureBox_xiti_stat.Size = new System.Drawing.Size(104, 64);
            pictureBox_xiti_stat.TabIndex = 6;
            pictureBox_xiti_stat.TabStop = false;
            pictureBox_xiti_stat.Click += new System.EventHandler(this.pictureBox_xiti_stat_Click);

            this.Controls.Add(this.panel_xiti);
            this.panel_pen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(pictureBox_xiti_select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBox_xiti_truefalse)).EndInit();
            this.ResumeLayout(false);
        }
        public void ShowXitis()
        {
            path = 20;
            this.TopMost = false;
            string url = Application.StartupPath + @"\html\tiku.html";
            Form1.formWeb = new FormWebBrowser();
            Form1.formWeb.navigate(url);
        }
        public void ShowXiti()
        {
            path = 22;//文件夹-试题
            HideMainPage();
            HidePen();
            //HideXiti();
            this.pictureBox_main_pen.Visible = false;
            this.panel_xiti.Visible = true;
            this.pictureBox_home.Visible = false;
            this.pictureBox_return.Visible = true;

            switchXitiPage(1);
        }
        private void HideXiti()
        {
            this.panel_xiti.Visible = false;
            this.pictureBox_home.Visible = false;
            this.pictureBox_return.Visible = false;

            this.pictureBox_xiti_stat.Visible = false;
            this.pictureBox_xiti_stop.Visible = false;
            this.pictureBox_xiti_splitter.Visible = false;
            this.label_CountOfAnswer.Visible = false;
            this.label_CountOfNoAnswer.Visible = false;
        }

        private void pictureBox_xiti_truefalse_Click(object sender, EventArgs e)
        {
            pictureBox_xiti_click();
        }

        private void pictureBox_xiti_select_Click(object sender, EventArgs e)
        {
            pictureBox_xiti_click();
        }
        private void pictureBox_xiti_click()
        {
            this.panel_xiti.Visible = true;
            this.pictureBox_home.Visible = false;
            this.pictureBox_return.Visible = true;
            switchXitiPage(2);
        }
        private void switchXitiPage(int level)
        {
            if (level == 1)
            {
                path = 22;//文件夹-试题
                this.pictureBox_xiti_select.Visible = true;
                this.pictureBox_xiti_truefalse.Visible = true;

                this.pictureBox_xiti_stat.Visible = false;
                this.pictureBox_xiti_stop.Visible = false;
                this.pictureBox_xiti_splitter.Visible = false;
                this.label_CountOfAnswer.Visible = false;
                this.label_CountOfNoAnswer.Visible = false;
            }
            else
            {
                path = 220;//文件夹-试题-全部作答
                this.pictureBox_xiti_select.Visible = false;
                this.pictureBox_xiti_truefalse.Visible = false;

                this.pictureBox_xiti_stat.Visible = true;
                this.pictureBox_xiti_stop.Visible = true;
                this.pictureBox_xiti_splitter.Visible = true;
                this.label_CountOfAnswer.Visible = true;
                this.label_CountOfNoAnswer.Visible = true;
            }
        }
        //作答情况
        private void pictureBox_xiti_stat_Click(object sender, EventArgs e)
        {
            pictureBox_xiti_stat.BackgroundImage = Properties.Resources.controller_xiti_stat1;
            pictureBox_xiti_stop.BackgroundImage = Properties.Resources.controller_xiti_stop0;

            //弹出一个对话框
            if(form_xiti1==null)
                form_xiti1 = new Form_xiti1();
            form_xiti1.Show();
            if(form_xiti2!=null)
                form_xiti2.Hide();
        }
        //结束答题
        private void pictureBox_xiti_stop_Click(object sender, EventArgs e)
        {
            pictureBox_xiti_stat.BackgroundImage = Properties.Resources.controller_xiti_stat0;
            pictureBox_xiti_stop.BackgroundImage = Properties.Resources.controller_xiti_stop1;
            //弹出一个对话框
            if (form_xiti2 == null)
                form_xiti2 = new Form_xiti2();
            if (form_xiti1 != null)
                form_xiti1.Hide();
            form_xiti2.Show();
        }
        #endregion


        private void pictureBox_home_Click(object sender, EventArgs e)
        {
            //批注截屏上传
            Bitmap bmp = ScreenCapture.captureScreen(0, 0);
            string base64 = Util.ImgToBase64String(bmp);
            int imgHeight = bmp.Height;
            int imgWidth = bmp.Width;
            string imgName = Global.getSchoolID() + "-" + Global.getClassID() + "-" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + ".jpg";
            string imgDir = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(imgDir))
                Directory.CreateDirectory(imgDir);
            string imgPath = imgDir + "\\" + imgName;
            bmp.Save(imgPath);
            Common.uploadPicture(imgPath);
            Common.doPost("addDrawViewEvent","filename=" + imgName);

            this.ShowMainPage();
        }
        private void pictureBox_return_Click(object sender, EventArgs e)
        {
            if (path == 220)
            {
                switchXitiPage(1);
                return;
            }

            this.ShowMainPage();
        }

        private void pictureBox_main_collect_Click(object sender, EventArgs e)
        {
            this.showColection();
        }

 
    }
}
