using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translate
{
    public partial class Form1 : Form
    {
        private KeyboardHook k_hook;
        public Form1()
        {
            InitializeComponent();
            k_hook = new KeyboardHook();
            k_hook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);//钩住键按下 
            //k_hook.KeyPressEvent += K_hook_KeyPressEvent;
            k_hook.Start();//安装键盘钩子
        }

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //判断按下的键（Alt + A）
            if (e.KeyValue == (int)Keys.T && (int)System.Windows.Forms.Control.ModifierKeys == (int)Keys.Control)
            {
                System.Windows.Forms.MessageBox.Show("ddd");
            }
        }

        private readonly string keyLoginArray = "ADMIN";
        private int keyLoginCount = 0;
        private void K_hook_KeyPressEvent(object sender, KeyPressEventArgs e)
        {
            if (true)
            {
                if (keyLoginArray[keyLoginCount] == (char)e.KeyChar)
                {
                    if (++keyLoginCount == keyLoginArray.Length)
                    {
                        MessageBox.Show("提示！");
                        keyLoginCount = 0;
                    }

                }
                else
                    keyLoginCount = 0;

            }
            MessageBox.Show("提示");
        }
    }
}
