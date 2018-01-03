using System.Drawing;
namespace RueHelper
{
    partial class FormController
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormController));
            this.pictureBox_main_pen = new System.Windows.Forms.PictureBox();
            this.pictureBox_main_folder = new System.Windows.Forms.PictureBox();
            this.pictureBox_main_xitis = new System.Windows.Forms.PictureBox();
            this.pictureBox_main_lessonover = new System.Windows.Forms.PictureBox();
            this.pictureBox_home = new System.Windows.Forms.PictureBox();
            this.pictureBox_return = new System.Windows.Forms.PictureBox();
            this.panel_pen = new System.Windows.Forms.Panel();
            this.panel_xiti = new System.Windows.Forms.Panel();
            this.pictureBox_main_exam = new System.Windows.Forms.PictureBox();
            this.pictureBox_main_collect = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_pen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_folder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_xitis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_lessonover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_home)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_return)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_exam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_collect)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_main_pen
            // 
            this.pictureBox_main_pen.BackgroundImage = global::RueHelper.Properties.Resources.controller_pen;
            this.pictureBox_main_pen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_main_pen.Location = new System.Drawing.Point(18, 0);
            this.pictureBox_main_pen.Name = "pictureBox_main_pen";
            this.pictureBox_main_pen.Size = new System.Drawing.Size(79, 62);
            this.pictureBox_main_pen.TabIndex = 0;
            this.pictureBox_main_pen.TabStop = false;
            this.pictureBox_main_pen.Click += new System.EventHandler(this.pictureBox_main_pen_click);
            // 
            // pictureBox_main_folder
            // 
            this.pictureBox_main_folder.BackgroundImage = global::RueHelper.Properties.Resources.controller_xitiInClass;
            this.pictureBox_main_folder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_main_folder.Location = new System.Drawing.Point(103, 0);
            this.pictureBox_main_folder.Name = "pictureBox_main_folder";
            this.pictureBox_main_folder.Size = new System.Drawing.Size(77, 63);
            this.pictureBox_main_folder.TabIndex = 1;
            this.pictureBox_main_folder.TabStop = false;
            this.pictureBox_main_folder.Click += new System.EventHandler(this.pictureBox_main_folder_Click);
            // 
            // pictureBox_main_xitis
            // 
            this.pictureBox_main_xitis.BackgroundImage = global::RueHelper.Properties.Resources.controller_xitis;
            this.pictureBox_main_xitis.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_main_xitis.Location = new System.Drawing.Point(364, -1);
            this.pictureBox_main_xitis.Name = "pictureBox_main_xitis";
            this.pictureBox_main_xitis.Size = new System.Drawing.Size(76, 63);
            this.pictureBox_main_xitis.TabIndex = 2;
            this.pictureBox_main_xitis.TabStop = false;
            this.pictureBox_main_xitis.Click += new System.EventHandler(this.pictureBox_main_xitis_Click);
            // 
            // pictureBox_main_lessonover
            // 
            this.pictureBox_main_lessonover.BackgroundImage = global::RueHelper.Properties.Resources.controller_lessonover;
            this.pictureBox_main_lessonover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_main_lessonover.Location = new System.Drawing.Point(443, -1);
            this.pictureBox_main_lessonover.Name = "pictureBox_main_lessonover";
            this.pictureBox_main_lessonover.Size = new System.Drawing.Size(75, 64);
            this.pictureBox_main_lessonover.TabIndex = 3;
            this.pictureBox_main_lessonover.TabStop = false;
            this.pictureBox_main_lessonover.Click += new System.EventHandler(this.pictureBox_main_lessonover_Click);
            // 
            // pictureBox_home
            // 
            this.pictureBox_home.BackgroundImage = global::RueHelper.Properties.Resources.controller_home;
            this.pictureBox_home.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_home.Location = new System.Drawing.Point(516, 0);
            this.pictureBox_home.Name = "pictureBox_home";
            this.pictureBox_home.Size = new System.Drawing.Size(59, 63);
            this.pictureBox_home.TabIndex = 4;
            this.pictureBox_home.TabStop = false;
            this.pictureBox_home.Visible = false;
            this.pictureBox_home.Click += new System.EventHandler(this.pictureBox_home_Click);
            // 
            // pictureBox_return
            // 
            this.pictureBox_return.BackgroundImage = global::RueHelper.Properties.Resources.controller_return;
            this.pictureBox_return.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_return.Location = new System.Drawing.Point(516, 0);
            this.pictureBox_return.Name = "pictureBox_return";
            this.pictureBox_return.Size = new System.Drawing.Size(59, 63);
            this.pictureBox_return.TabIndex = 4;
            this.pictureBox_return.TabStop = false;
            this.pictureBox_return.Click += new System.EventHandler(this.pictureBox_return_Click);
            // 
            // panel_pen
            // 
            this.panel_pen.Location = new System.Drawing.Point(0, 0);
            this.panel_pen.Name = "panel_pen";
            this.panel_pen.Size = new System.Drawing.Size(200, 100);
            this.panel_pen.TabIndex = 0;
            // 
            // panel_xiti
            // 
            this.panel_xiti.Location = new System.Drawing.Point(0, 0);
            this.panel_xiti.Name = "panel_xiti";
            this.panel_xiti.Size = new System.Drawing.Size(200, 100);
            this.panel_xiti.TabIndex = 0;
            // 
            // pictureBox_main_exam
            // 
            this.pictureBox_main_exam.BackgroundImage = global::RueHelper.Properties.Resources.controller_myExam;
            this.pictureBox_main_exam.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_main_exam.Location = new System.Drawing.Point(278, 0);
            this.pictureBox_main_exam.Name = "pictureBox_main_exam";
            this.pictureBox_main_exam.Size = new System.Drawing.Size(76, 63);
            this.pictureBox_main_exam.TabIndex = 7;
            this.pictureBox_main_exam.TabStop = false;
            this.pictureBox_main_exam.Click += new System.EventHandler(this.pictureBox_main_exam_Click);
            // 
            // pictureBox_main_collect
            // 
            this.pictureBox_main_collect.BackgroundImage = global::RueHelper.Properties.Resources.controller_myCollect;
            this.pictureBox_main_collect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox_main_collect.Location = new System.Drawing.Point(188, 0);
            this.pictureBox_main_collect.Name = "pictureBox_main_collect";
            this.pictureBox_main_collect.Size = new System.Drawing.Size(76, 63);
            this.pictureBox_main_collect.TabIndex = 8;
            this.pictureBox_main_collect.TabStop = false;
            this.pictureBox_main_collect.Click += new System.EventHandler(this.pictureBox_main_collect_Click);
            // 
            // FormController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(574, 67);
            this.Controls.Add(this.pictureBox_main_collect);
            this.Controls.Add(this.pictureBox_home);
            this.Controls.Add(this.pictureBox_return);
            this.Controls.Add(this.pictureBox_main_xitis);
            this.Controls.Add(this.pictureBox_main_folder);
            this.Controls.Add(this.pictureBox_main_pen);
            this.Controls.Add(this.pictureBox_main_lessonover);
            this.Controls.Add(this.pictureBox_main_exam);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainPosition = new System.Drawing.Point(42, 4);
            this.Name = "FormController";
            this.SkinBack = ((System.Drawing.Bitmap)(resources.GetObject("$this.SkinBack")));
            this.SkinSize = new System.Drawing.Size(625, 82);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormRue";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_pen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_folder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_xitis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_lessonover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_home)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_return)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_exam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main_collect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_main_pen;
        private System.Windows.Forms.PictureBox pictureBox_main_folder;
        private System.Windows.Forms.PictureBox pictureBox_main_xitis;
        private System.Windows.Forms.PictureBox pictureBox_main_lessonover;
        private System.Windows.Forms.PictureBox pictureBox_home;
        private System.Windows.Forms.PictureBox pictureBox_return;

        private System.Windows.Forms.Panel panel_pen;
        private System.Windows.Forms.Panel panel_xiti;
        private System.Windows.Forms.PictureBox pictureBox_main_exam;
        private System.Windows.Forms.PictureBox pictureBox_main_collect;
    }
}