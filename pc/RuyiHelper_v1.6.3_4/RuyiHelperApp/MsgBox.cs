using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RueHelper
{
    public partial class MsgBox : UserControl
    {
        private FormWithMsgBox form;
        public MsgBox(FormWithMsgBox f)
        {
            form = f;
            InitializeComponent();
        }
        public MsgBox(string title,string msg, string btn1,string btn2)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.Cancel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form.OK();
        }
    }
}
