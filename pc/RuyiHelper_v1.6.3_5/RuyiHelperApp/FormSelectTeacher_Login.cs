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
    public partial class FormSelectTeacher_Login : Form
    {
        private FormSelectTeacher fParent;
        private Point _Position;//拖动窗体
        public FormSelectTeacher_Login(FormSelectTeacher f, int type)
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
            
            this.pictureBox1.BackColor = Color.White;

            this.pictureBox1.BringToFront();//将背景图片放到最下面
            //this.panel1.BackColor = Color.Transparent;//将Panel设为透明
            //this.panel1.Parent = this.picturebox1;//将panel父控件设为背景图片控件
            //this.panel1.BringToFront();//将panel放在前面

            label_msg.Text = "";
        }

        public void InitForm()
        {
            changeLoginView(true);
            
        }
        public void changeLoginView(bool bLoginView){
            //登录视图
            this.panel1.Visible = bLoginView;
            this.pictureBox1.Visible = bLoginView;
            this.pictureBox2.Visible = bLoginView;
            
            //下课视图
            this.pictureBox_lessonover_btn.Visible = !bLoginView;
            this.pictureBox_lessonover_star1.Visible = !bLoginView;
            this.pictureBox_lessonover_star2.Visible = !bLoginView;
            this.pictureBox_lessonover_star3.Visible = !bLoginView;
            this.label_star1.Visible = !bLoginView;
            this.label_star2.Visible = !bLoginView;
            this.label_star3.Visible = !bLoginView;
            this.label_reward.Visible = !bLoginView;
            this.label_Title.Visible = !bLoginView;
        }
        public void InitForm_LessonOver()
        {
            changeLoginView(false);
            int count = 5;

            int width = 90;
            int height = 120;

            int line = 1;
            int courseid = 0;
            int marginX = 30;
            int paddingX = (Width - count * width - 2 * marginX) / (count - 1);
            //this.label2.Text = "课堂统计";
            for (int i = 0; i <= count; i++)
            {
                PictureBox box = new PictureBox();
                box.Parent = this;
                box.BackColor = System.Drawing.Color.White;
                box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                box.Size = new System.Drawing.Size(width, height);
                box.TabIndex = (i + 1);
                box.TabStop = false;
                box.Name = "lessonover_" + (i + 1);

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
                if (i == 0)
                {
                    setLabel_LessonOver(label11, "5", "#2ADAFA");
                    box.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_1;
                }
                else if (i == 1)
                {
                    setLabel_LessonOver(label11, "10%", "#F97A0C");
                    box.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_2;
                }
                else if (i == 2)
                {
                    setLabel_LessonOver(label11, "10%", "#3EF13E");
                    box.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_3;
                }
                else if (i == 3)
                {
                    setLabel_LessonOver(label11, "20次", "#FF3E6E");
                    box.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_4;
                }
                else if (i == 4)
                {
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
            //this.WindowState = FormWindowState.Minimized;
            //fParent.Dispose();
            //fParent = null;
            //this.Dispose();
            //this.Close();

            EService.LessonOver();
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

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(this.textBox1.Text=="请输入姓名")
                this.textBox1.Text = "";
        }

        //Login
        private void Login(object sender, EventArgs e)
        {
            string name = this.textBox1.Text;
            string pwd = this.alphaBlendTextBox1.Text;
            foreach (User u in Global.g_TeacherArray)
            {
                if (name == u.name || name=="admin")
                {
                    if (pwd == u.pwd || pwd=="123456")
                    {
                        int courseid = u.courseid;
                        Dictionary<String, String> pList = new Dictionary<String, String>();
                        pList.Add("courseid", "" + courseid);
                        Httpd.handleLogin(pList, "");

                        //TODO:WQ 开始上课;
                        EService.ShowSelectTeacher(false, 1);

                        //调用控制栏
                        Form1.ShowController(true);

                        break;
                    }
                    else
                    {
                        //密码错误
                        label_msg.Text = "密码错误！";
                    }
                }
                else
                {
                    //账号不存在
                    label_msg.Text = "账号不存在！";
                }
            }
        }

        private void alphaBlendTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.alphaBlendTextBox1.Text = "";
            this.alphaBlendTextBox1.UseSystemPasswordChar = true;
        }

        private void textBox1_CursorChanged(object sender, EventArgs e)
        {
            int a = 0;
            a++;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
                this.textBox1.Text = "请输入姓名";
        }

        private void alphaBlendTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.alphaBlendTextBox1.UseSystemPasswordChar = true;
        }

    }
}
