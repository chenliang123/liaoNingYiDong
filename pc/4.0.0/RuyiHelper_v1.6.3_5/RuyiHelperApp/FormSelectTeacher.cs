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
    public partial class FormSelectTeacher : Form
    {
        //public FormSelectTeacherTop childForm;
        public FormSelectTeacherTop childForm;
        private Point _Position;//拖动窗体
        public FormSelectTeacher(int type)
        {
            InitializeComponent();

            this.Opacity = 0.80; // 窗体透明度
            this.childForm = new FormSelectTeacherTop(this, type);
            this.childForm.Owner = this;    // 这支所属窗体                
            this.childForm.Dock = DockStyle.Fill;
            this.childForm.Show();
            this.childForm.BringToFront();
            childForm.Location = new Point(this.Location.X, this.Location.Y);
            this.childForm.Size = new Size(this.Size.Width, this.Height);

            //mouseControl();  
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            UpdateStyles();
            this.ShowInTaskbar = false;
            childForm.ShowInTaskbar = false;
        }
        private void HideForm(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
                childForm.Location = h_myPosittion;
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
    }
}
