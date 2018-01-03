namespace RueHelper
{
    partial class Form_xiti1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_withoutAnswer = new System.Windows.Forms.Label();
            this.label_withAnswer = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_close = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_msgbox = new System.Windows.Forms.Panel();
            this.msgBox1 = new RueHelper.MsgBox(this);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_msgbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel1.Controls.Add(this.label_withoutAnswer);
            this.panel1.Controls.Add(this.label_withAnswer);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(679, 58);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // label_withoutAnswer
            // 
            this.label_withoutAnswer.AutoSize = true;
            this.label_withoutAnswer.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.label_withoutAnswer.ForeColor = System.Drawing.Color.White;
            this.label_withoutAnswer.Location = new System.Drawing.Point(463, 17);
            this.label_withoutAnswer.Name = "label_withoutAnswer";
            this.label_withoutAnswer.Size = new System.Drawing.Size(99, 24);
            this.label_withoutAnswer.TabIndex = 1;
            this.label_withoutAnswer.Text = "未答 :  5 人";
            // 
            // label_withAnswer
            // 
            this.label_withAnswer.AutoSize = true;
            this.label_withAnswer.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.label_withAnswer.ForeColor = System.Drawing.Color.White;
            this.label_withAnswer.Location = new System.Drawing.Point(124, 17);
            this.label_withAnswer.Name = "label_withAnswer";
            this.label_withAnswer.Size = new System.Drawing.Size(114, 24);
            this.label_withAnswer.TabIndex = 0;
            this.label_withAnswer.Text = "已答 ： 25人";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::RueHelper.Properties.Resources.spliter2;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(338, 58);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(2, 298);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label_close
            // 
            this.label_close.AutoSize = true;
            this.label_close.BackColor = System.Drawing.Color.White;
            this.label_close.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.label_close.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label_close.Location = new System.Drawing.Point(3, 328);
            this.label_close.Name = "label_close";
            this.label_close.Size = new System.Drawing.Size(25, 26);
            this.label_close.TabIndex = 2;
            this.label_close.Text = "X";
            this.label_close.Click += new System.EventHandler(this.label_close_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.label2.Location = new System.Drawing.Point(423, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "张三";
            this.label2.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.label5.Location = new System.Drawing.Point(606, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "张三";
            this.label5.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.label3.Location = new System.Drawing.Point(482, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "张三";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.label4.Location = new System.Drawing.Point(544, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "张三";
            this.label4.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.label1.Location = new System.Drawing.Point(362, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "张三";
            this.label1.Visible = false;
            // 
            // panel_msgbox
            // 
            this.panel_msgbox.Controls.Add(this.msgBox1);
            this.panel_msgbox.Location = new System.Drawing.Point(162, 109);
            this.panel_msgbox.Name = "panel_msgbox";
            this.panel_msgbox.Size = new System.Drawing.Size(359, 188);
            this.panel_msgbox.TabIndex = 8;
            // 
            // msgBox1
            // 
            this.msgBox1.BackColor = System.Drawing.Color.White;
            this.msgBox1.Location = new System.Drawing.Point(3, 2);
            this.msgBox1.Name = "msgBox1";
            this.msgBox1.Size = new System.Drawing.Size(359, 183);
            this.msgBox1.TabIndex = 0;
            // 
            // Form_xiti1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(678, 356);
            this.ControlBox = false;
            this.Controls.Add(this.panel_msgbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_close);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_xiti1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_xiti1";
            this.TopMost = true;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_xiti1_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_msgbox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_withoutAnswer;
        private System.Windows.Forms.Label label_withAnswer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_close;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_msgbox;
        private MsgBox msgBox1;
    }
}