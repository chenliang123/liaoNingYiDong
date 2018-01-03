using System.Drawing;
namespace RueHelper
{
    partial class FormSelectTeacher_Login
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_msg = new System.Windows.Forms.Label();
            this.alphaBlendTextBox1 = new RueHelper.AlphaBlendTextBox();
            this.textBox1 = new RueHelper.AlphaBlendTextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label_reward = new System.Windows.Forms.Label();
            this.label_star1 = new System.Windows.Forms.Label();
            this.label_star2 = new System.Windows.Forms.Label();
            this.label_star3 = new System.Windows.Forms.Label();
            this.pictureBox_lessonover_btn = new System.Windows.Forms.PictureBox();
            this.pictureBox_lessonover_star1 = new System.Windows.Forms.PictureBox();
            this.pictureBox_lessonover_star2 = new System.Windows.Forms.PictureBox();
            this.pictureBox_lessonover_star3 = new System.Windows.Forms.PictureBox();
            this.label_Title = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lessonover_btn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lessonover_star1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lessonover_star2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lessonover_star3)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 24F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(620, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "x";
            this.label1.Click += new System.EventHandler(this.HideForm);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::RueHelper.Properties.Resources.ad;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(34, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(265, 352);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::RueHelper.Properties.Resources.login;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Controls.Add(this.label_msg);
            this.panel1.Controls.Add(this.alphaBlendTextBox1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Location = new System.Drawing.Point(307, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 302);
            this.panel1.TabIndex = 2;
            // 
            // label_msg
            // 
            this.label_msg.AutoSize = true;
            this.label_msg.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_msg.ForeColor = System.Drawing.Color.White;
            this.label_msg.Location = new System.Drawing.Point(98, 193);
            this.label_msg.Name = "label_msg";
            this.label_msg.Size = new System.Drawing.Size(121, 20);
            this.label_msg.TabIndex = 7;
            this.label_msg.Text = "用户名或密码错误";
            // 
            // alphaBlendTextBox1
            // 
            this.alphaBlendTextBox1.BackAlpha = 0;
            this.alphaBlendTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.alphaBlendTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.alphaBlendTextBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.alphaBlendTextBox1.ForeColor = System.Drawing.Color.White;
            this.alphaBlendTextBox1.Location = new System.Drawing.Point(99, 152);
            this.alphaBlendTextBox1.MaxLength = 20;
            this.alphaBlendTextBox1.Name = "alphaBlendTextBox1";
            this.alphaBlendTextBox1.Size = new System.Drawing.Size(179, 22);
            this.alphaBlendTextBox1.TabIndex = 6;
            this.alphaBlendTextBox1.Text = "请输入密码";
            this.alphaBlendTextBox1.TextChanged += new System.EventHandler(this.alphaBlendTextBox1_TextChanged);
            this.alphaBlendTextBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.alphaBlendTextBox1_MouseDown);
            // 
            // textBox1
            // 
            this.textBox1.BackAlpha = 0;
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(99, 100);
            this.textBox1.MaxLength = 20;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(179, 22);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "请输入姓名";
            this.textBox1.CursorChanged += new System.EventHandler(this.textBox1_CursorChanged);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::RueHelper.Properties.Resources.loginBtn;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(110, 235);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(109, 27);
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.Login);
            // 
            // label_reward
            // 
            this.label_reward.AutoSize = true;
            this.label_reward.Font = new System.Drawing.Font("黑体", 13F);
            this.label_reward.ForeColor = System.Drawing.Color.White;
            this.label_reward.Location = new System.Drawing.Point(297, 240);
            this.label_reward.Name = "label_reward";
            this.label_reward.Size = new System.Drawing.Size(80, 18);
            this.label_reward.TabIndex = 4;
            this.label_reward.Text = "奖励之星";
            // 
            // label_star1
            // 
            this.label_star1.AutoSize = true;
            this.label_star1.Font = new System.Drawing.Font("黑体", 13F);
            this.label_star1.ForeColor = System.Drawing.Color.White;
            this.label_star1.Location = new System.Drawing.Point(59, 285);
            this.label_star1.Name = "label_star1";
            this.label_star1.Size = new System.Drawing.Size(134, 18);
            this.label_star1.TabIndex = 6;
            this.label_star1.Text = "张三      X 37";
            // 
            // label_star2
            // 
            this.label_star2.AutoSize = true;
            this.label_star2.Font = new System.Drawing.Font("黑体", 13F);
            this.label_star2.ForeColor = System.Drawing.Color.White;
            this.label_star2.Location = new System.Drawing.Point(263, 285);
            this.label_star2.Name = "label_star2";
            this.label_star2.Size = new System.Drawing.Size(134, 18);
            this.label_star2.TabIndex = 8;
            this.label_star2.Text = "张三      X 37";
            // 
            // label_star3
            // 
            this.label_star3.AutoSize = true;
            this.label_star3.Font = new System.Drawing.Font("黑体", 13F);
            this.label_star3.ForeColor = System.Drawing.Color.White;
            this.label_star3.Location = new System.Drawing.Point(460, 285);
            this.label_star3.Name = "label_star3";
            this.label_star3.Size = new System.Drawing.Size(134, 18);
            this.label_star3.TabIndex = 9;
            this.label_star3.Text = "张三      X 37";
            // 
            // pictureBox_lessonover_btn
            // 
            this.pictureBox_lessonover_btn.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_lessonover_btn.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_btn;
            this.pictureBox_lessonover_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_lessonover_btn.Location = new System.Drawing.Point(279, 345);
            this.pictureBox_lessonover_btn.Name = "pictureBox_lessonover_btn";
            this.pictureBox_lessonover_btn.Size = new System.Drawing.Size(118, 36);
            this.pictureBox_lessonover_btn.TabIndex = 5;
            this.pictureBox_lessonover_btn.TabStop = false;
            this.pictureBox_lessonover_btn.Click += new System.EventHandler(this.pictureBox_lessonover_btn_Click);
            // 
            // pictureBox_lessonover_star1
            // 
            this.pictureBox_lessonover_star1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_lessonover_star1.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_start;
            this.pictureBox_lessonover_star1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_lessonover_star1.Location = new System.Drawing.Point(107, 277);
            this.pictureBox_lessonover_star1.Name = "pictureBox_lessonover_star1";
            this.pictureBox_lessonover_star1.Size = new System.Drawing.Size(36, 33);
            this.pictureBox_lessonover_star1.TabIndex = 7;
            this.pictureBox_lessonover_star1.TabStop = false;
            // 
            // pictureBox_lessonover_star2
            // 
            this.pictureBox_lessonover_star2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_lessonover_star2.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_start;
            this.pictureBox_lessonover_star2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_lessonover_star2.Location = new System.Drawing.Point(313, 277);
            this.pictureBox_lessonover_star2.Name = "pictureBox_lessonover_star2";
            this.pictureBox_lessonover_star2.Size = new System.Drawing.Size(36, 33);
            this.pictureBox_lessonover_star2.TabIndex = 10;
            this.pictureBox_lessonover_star2.TabStop = false;
            // 
            // pictureBox_lessonover_star3
            // 
            this.pictureBox_lessonover_star3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_lessonover_star3.BackgroundImage = global::RueHelper.Properties.Resources.lessonover_start;
            this.pictureBox_lessonover_star3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_lessonover_star3.Location = new System.Drawing.Point(510, 277);
            this.pictureBox_lessonover_star3.Name = "pictureBox_lessonover_star3";
            this.pictureBox_lessonover_star3.Size = new System.Drawing.Size(36, 33);
            this.pictureBox_lessonover_star3.TabIndex = 11;
            this.pictureBox_lessonover_star3.TabStop = false;
            // 
            // label_Title
            // 
            this.label_Title.AutoSize = true;
            this.label_Title.Font = new System.Drawing.Font("黑体", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Title.ForeColor = System.Drawing.Color.White;
            this.label_Title.Location = new System.Drawing.Point(305, 27);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(80, 17);
            this.label_Title.TabIndex = 3;
            this.label_Title.Text = "课堂总结";
            this.label_Title.Visible = false;
            // 
            // FormSelectTeacher_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(664, 415);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox_lessonover_star1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_Title);
            this.Controls.Add(this.label_star1);
            this.Controls.Add(this.pictureBox_lessonover_star3);
            this.Controls.Add(this.pictureBox_lessonover_btn);
            this.Controls.Add(this.label_reward);
            this.Controls.Add(this.pictureBox_lessonover_star2);
            this.Controls.Add(this.label_star2);
            this.Controls.Add(this.label_star3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectTeacher_Login";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.Black;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DragGo);
            this.Resize += new System.EventHandler(this.FormSelectTeacher_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lessonover_btn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lessonover_star1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lessonover_star2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lessonover_star3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private AlphaBlendTextBox textBox1;
        private AlphaBlendTextBox alphaBlendTextBox1;
        private System.Windows.Forms.Label label_msg;

        //下课控件
        private System.Windows.Forms.Label label_reward;
        private System.Windows.Forms.PictureBox pictureBox_lessonover_btn;
        private System.Windows.Forms.Label label_star1;
        private System.Windows.Forms.Label label_star2;
        private System.Windows.Forms.Label label_star3;
        private System.Windows.Forms.PictureBox pictureBox_lessonover_star1;
        private System.Windows.Forms.PictureBox pictureBox_lessonover_star2;
        private System.Windows.Forms.PictureBox pictureBox_lessonover_star3;
        private System.Windows.Forms.Label label_Title;
    }
}

