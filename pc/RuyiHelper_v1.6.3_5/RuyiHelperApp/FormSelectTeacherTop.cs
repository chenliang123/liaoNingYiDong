using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RueHelper
{
    //http://blog.csdn.net/wanlong360599336/article/details/7553568
    public partial class FormSelectTeacherTop : Form
    {
        private FormSelectTeacher fParent;
        private Point _Position;//拖动窗体
        public FormSelectTeacherTop(FormSelectTeacher f, int type)
        {
            fParent = f;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            UpdateStyles();

            InitializeComponent();
            _Position = new Point();

            if (type == 1)
            {
                InitForm();
            }
            else
            {
                InitForm_LessonOver();
            }
            
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.Top = 30;
            this.pictureBox1.BackColor = Color.White;

            this.pictureBox1.BringToFront();//将背景图片放到最下面
            //this.panel1.BackColor = Color.Transparent;//将Panel设为透明
            //this.panel1.Parent = this.picturebox1;//将panel父控件设为背景图片控件
            //this.panel1.BringToFront();//将panel放在前面
        }

        public void InitForm()
        {
            int count = 8;
            int line = 1;
            int courseid = 0;
            int marginX = 70;
            int paddingX = (Width-4*this.pictureBox1.Width - 2*marginX)/3;

            //屏蔽测试控件 和下课控件
            this.pictureBox1.Visible = false;
            this.pictureBox2.Visible = false;
            this.pictureBox_lessonover_btn.Visible = false;
            this.pictureBox_lessonover_star1.Visible = false;
            this.pictureBox_lessonover_star2.Visible = false;
            this.pictureBox_lessonover_star3.Visible = false;
            this.label_star1.Visible = false;
            this.label_star2.Visible = false;
            this.label_star3.Visible = false;
            this.label_reward.Visible = false;

            this.label2.Text = "请选择老师";
            HashSet<Int32> courseIdSet = new HashSet<Int32>();
            if (Global.g_TeacherArray != null)
            {
                count = Global.g_TeacherArray.Length;
                int i = 0;
                foreach (User u in Global.g_TeacherArray)
                {
                    if(i>8)
                        break;
                    if (i++ > 3)
                        line = 2;
                    PictureBox box = new PictureBox();
                    box.Parent = this;
                    box.BackColor = System.Drawing.Color.White;
                    box.Size = new System.Drawing.Size(this.pictureBox1.Width, this.pictureBox1.Height);
                    box.TabIndex = (i + 1);
                    box.TabStop = false;

                    //box.Name = "pictureBox" + (i + 1);
                    box.Name = ""+u.courseid;
                    courseid = u.courseid;

                    box.Click += new System.EventHandler(this.pictureBox_SelectTeacher_Click);
                    int posX = marginX + i % 4 * (pictureBox1.Width + paddingX);
                    int posY = 80 + (line - 1) * (pictureBox1.Height + 20);
                    box.Location = new System.Drawing.Point(posX, posY);
                    //box.BackgroundImage = global::RueHelper.Properties.Resources.c11;
                    //box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;

                    //在指定位置画图
                    Bitmap image = new Bitmap(box.Size.Width, box.Size.Height);
                    Graphics device = Graphics.FromImage(image);
                    int locationX = 15;
                    int locationY = 20;
                   
                    //device.DrawImage(global::RueHelper.Properties.Resources.c11, locationX, locationY); //用你想要的位置画picturebox2
                    //box.Image = image;


                    Label label11 = new Label();
                    label11.Parent = box;
                    if (i == 0)
                        setLabel_top(label11, "第一节");
                    else if (i == 1)
                        setLabel_top(label11, "第二节");
                    else if (i == 2)
                        setLabel_top(label11, "第三节");
                    else if (i == 3)
                        setLabel_top(label11, "第四节");
                    else if (i == 4)
                        setLabel_top(label11, "第五节");
                    else if (i == 5)
                        setLabel_top(label11, "第六节");
                    else if (i == 6)
                        setLabel_top(label11, "第七节");
                    else if (i == 7)
                        setLabel_top(label11, "第八节");

                    if (courseid == 11)
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c11;
                        device.DrawImage(global::RueHelper.Properties.Resources.c11, locationX, locationY);
                    else if (courseid == 12)
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c12;
                        device.DrawImage(global::RueHelper.Properties.Resources.c12, locationX, locationY);
                    else if (courseid == 13)
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c13;
                        device.DrawImage(global::RueHelper.Properties.Resources.c13, locationX, locationY);
                    else if (courseid == 14)
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c14;
                        device.DrawImage(global::RueHelper.Properties.Resources.c14, locationX, locationY);
                    else if (courseid == 15)
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c15;
                        device.DrawImage(global::RueHelper.Properties.Resources.c15, locationX, locationY);
                    else if (courseid == 16)
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c16;
                        device.DrawImage(global::RueHelper.Properties.Resources.c16, locationX, locationY);
                    else if (courseid == 17)
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c17;
                        device.DrawImage(global::RueHelper.Properties.Resources.c17, locationX, locationY);
                    else if (courseid == 18)
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c18;
                        device.DrawImage(global::RueHelper.Properties.Resources.c18, locationX, locationY);
                    else if (courseid == 19)
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c19;
                        device.DrawImage(global::RueHelper.Properties.Resources.c19, locationX, locationY);
                    //else if (courseid == 20)
                    //    box.BackgroundImage = global::RueHelper.Properties.Resources.c20;
                    //else if (courseid == 21)
                    //    box.BackgroundImage = global::RueHelper.Properties.Resources.c21;
                    else
                        //box.BackgroundImage = global::RueHelper.Properties.Resources.c22;
                        device.DrawImage(global::RueHelper.Properties.Resources.c22, locationX, locationY);
                    box.Image = image;//结束画图

                    Label label12 = new Label();
                    label12.Parent = box;
                    setLabel_bottom(label12, u.name);
                }
            }
            
        }

        public void InitForm_LessonOver()
        {
            int count = 5;

            int width = 90;
            int height = 120;

            int line = 1;
            int courseid = 0;
            int marginX = 30;
            int paddingX = (Width - count * width - 2 * marginX) / (count-1);
            this.pictureBox1.Visible = false;
            this.pictureBox2.Visible = false;
            this.label2.Text = "课堂统计";
            for (int i = 0; i <= count; i++)
            {
                PictureBox box = new PictureBox();
                box.Parent = this;
                box.BackColor = System.Drawing.Color.White;
                box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                box.Size = new System.Drawing.Size(width, height);
                box.TabIndex = (i + 1);
                box.TabStop = false;
                box.Name = "lessonover_" + (i+1);

                int posX = marginX + i % 5 * (width + paddingX);
                int posY = 80 + (line - 1) * (height + 20);
                box.Location = new System.Drawing.Point(posX, posY);

                //在指定位置画图
                Bitmap image = new Bitmap(box.Size.Width, box.Size.Height);
                Graphics device = Graphics.FromImage(image);
                int locationX = 15;
                int locationY = 20;
                Label label11 = new Label();
                label11.Parent = box;
                //#2ADAFA 1
                //#F97A0C 2
                //#3EF13E 3
                //#FF3E6E 4
                //#FF9E03 5
                if (i == 0){
                    setLabel_LessonOver(label11, "5", "#2ADAFA");
                    box.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_1;
                }
                else if (i == 1){
                    setLabel_LessonOver(label11, "10%", "#F97A0C");
                    box.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_2;
                }
                else if (i == 2){
                    setLabel_LessonOver(label11, "10%", "#3EF13E");
                    box.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_3;
                }
                else if (i == 3){
                    setLabel_LessonOver(label11, "20次", "#FF3E6E");
                    box.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_4;
                }
                else if (i == 4){
                    setLabel_LessonOver(label11, "25次", "#FF9E03");
                    box.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_5;
                }
            }


        }
        //点击选择老师
        public void pictureBox_SelectTeacher_Click(object sender, EventArgs e)
        {
            //http://172.18.201.3:8989/user.do?action=login&courseid=12&callback=angular.callbacks._8
            PictureBox box = (PictureBox)sender;
            string name = box.Name;
            int courseid = int.Parse(name);
            foreach (User u in Global.g_TeacherArray)
            {
                if(u.courseid == courseid)
                {
                    Dictionary<String, String> pList = new Dictionary<String, String>();
                    pList.Add("courseid", "" + courseid);
                    Httpd.handleLogin(pList,"");

                    //TODO:WQ 开始上课
                    EService.ShowSelectTeacher(false,1);

                    //调用控制栏
                    Form1.ShowController(true);
                }
            }
        }
        public void setLabel_top(Label label,String text)
        {
            label.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.ForeColor = System.Drawing.Color.Black;
            label.Name = "label1";
            label.TabIndex = 0;
            label.Text = text;
            label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            label.Location = new System.Drawing.Point(0, 5);//
            label.Click += new System.EventHandler(this.HideForm);

            label.Visible = false;
        }
        public void setLabel_LessonOver(Label label, String text, String color)
        {
            label.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.ForeColor = System.Drawing.ColorTranslator.FromHtml(color);
            label.BackColor = Color.Transparent;
            label.Name = "label1";
            label.TabIndex = 0;
            label.Text = text;
            label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            label.Location = new System.Drawing.Point(20, 33);//
            label.Visible = true;
            label.Size = new System.Drawing.Size(50, 30);


            // 
            // label3
            // 
            //this.label3.AutoSize = true;
            //this.label3.BackColor = System.Drawing.Color.Maroon;
            //this.label3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            //this.label3.Location = new System.Drawing.Point(60, 266);
            //this.label3.Name = "label3";
            //this.label3.Size = new System.Drawing.Size(29, 19);
            //this.label3.TabIndex = 4;
            //this.label3.Text = "20";

        }
        public void setLabel_bottom(Label label,String text)
        {
            label.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.ForeColor = System.Drawing.Color.Black;
            label.Text = text;
            int posX = (this.pictureBox1.Width - label.Width) / 2;
            label.Name = "label1";
            label.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //label.Location = new System.Drawing.Point(posX, this.pictureBox1.Height-30);//
            label.Location = new System.Drawing.Point(posX, this.pictureBox1.Height - 40);//
            label.Click += new System.EventHandler(this.HideForm);
        }
        private void HideForm(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            fParent.Dispose();
            fParent = null;
            this.Dispose();
            this.Close();
        }

        private void Drag(object sender, MouseEventArgs e)
        {
            _Position.X = e.X;
            _Position.Y = e.Y;
        }

        private void DragGo(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point h_myPosittion = MousePosition;
                h_myPosittion.Offset(-_Position.X, -_Position.Y);
                Location = h_myPosittion;
            }
        }
        private void Type(Control sender, int p_1,double p_2)
        {
            GraphicsPath oPath = new GraphicsPath();
            oPath.AddClosedCurve(new Point[]{
                new Point(0,sender.Height/p_1),
                new Point(sender.Width/p_1,0),
                new Point((sender.Width-sender.Width/p_1),0),
                new Point(sender.Width,sender.Height/p_1),

                new Point(sender.Width, sender.Height-sender.Height/p_1),
                new Point(sender.Width-sender.Width/p_1, sender.Height),
                new Point(sender.Width/p_1,sender.Height),
                new Point(0,sender.Height-sender.Height/p_1)},(float)p_2);
            sender.Region = new Region(oPath);
        }

        private void FormSelectTeacher_Resize(object sender, EventArgs e)
        {
            this.Region = null;
            SetWindowRegion();
            //Type(this, 60, 0.02);
        }
        public void SetWindowRegion()
        {
            GraphicsPath formPath = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 10, this.Width, this.Height - 10);
            formPath = GetRoundedRectPath(rect, 10);
            this.Region = new Region(formPath);
        }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();
            path.AddArc(arcRect, 180, 90);//left up

            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);//right up

            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);//left bottom

            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }

        //确定下课
        private void pictureBox_lessonover_btn_Click(object sender, EventArgs e)
        {
            //TODO: 下课事件处理
            Form1.ShowController(false);

            this.WindowState = FormWindowState.Minimized;
            fParent.Dispose();
            fParent = null;
            this.Dispose();
            this.Close();

        }
    }
}
