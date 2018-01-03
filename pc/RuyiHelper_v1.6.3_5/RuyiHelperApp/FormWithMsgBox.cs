using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RueHelper
{
    public abstract class FormWithMsgBox : Form
    {
        public abstract void Cancel();
        public abstract void OK();
    }
}
