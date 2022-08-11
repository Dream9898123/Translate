using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Translate.Translate;
using Translate.Translate.BaiduEngine;

namespace Translate
{
    public partial class 代码翻译小工具 : Form
    {


        private KeyboardHook k_hook;
        private MouseHook m_hook;
        private ClipBoard c_board;
        private TipForm tipForm;
        private KeyboardEvent k_event;
        private TranslateEngineBase translateEngineBase;
        public 代码翻译小工具()
        {
            InitializeComponent();
            //调用初始化托盘显示函数  
            InitialTray();
            //k_hook = new KeyboardHook();
            //k_hook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);//钩住键按下 
            ////k_hook.KeyPressEvent += K_hook_KeyPressEvent;
            //k_hook.Start();//安装键盘钩子

            m_hook = new MouseHook();
            m_hook.MouseDownEvent += new System.Windows.Forms.MouseEventHandler(hook_MouseDown);
            m_hook.Start();

            c_board = new ClipBoard();
            tipForm = new TipForm();
            tipForm.Visible = false;

            k_event = new KeyboardEvent();

            translateEngineBase = new TranslateEngineBaidu();
            settingbuttons = MouseButtons.Middle;

            this.comboBox1.SelectedIndex = 2;
        }
        private MouseButtons settingbuttons;
        //
        private void hook_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == settingbuttons)
            {
                string translateValue = c_board.GetClipBoardStr();
                string translateResult = translateEngineBase.GetResult(translateValue);
                c_board.SetClipBoardStr(translateResult);
            }
        }
        //private void hook_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //判断按下的键（Ctrl + T）
        //    if (e.KeyValue == (int)Keys.T && (int)System.Windows.Forms.Control.ModifierKeys == (int)Keys.Control)
        //    {
        //        string translateValue = c_board.GetClipBoardStr();
        //        translateEngineBase = new TranslateEngineBaidu();
        //        string result = translateEngineBase.GetResult(translateValue);
        //    }
        //}
        //这里在窗体上没有拖拽一个NotifyIcon控件，而是在这里定义了一个变量  
        private NotifyIcon notifyIcon = null;



        /// <summary>  

        /// 窗体关闭的单击事件  

        /// </summary>  

        /// <param name="sender"></param>  

        /// <param name="e"></param>  

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            //通过这里可以看出，这里的关闭其实不是真正意义上的“关闭”，而是将窗体隐藏，实现一个“伪关闭”  
            this.Hide();

        }


        private void InitialTray()
        {

            //隐藏主窗体  

            this.Hide();

            //实例化一个NotifyIcon对象  

            notifyIcon = new NotifyIcon();

            //托盘图标气泡显示的内容  

            notifyIcon.BalloonTipText = "正在后台运行";

            //托盘图标显示的内容  

            notifyIcon.Text = "窗体托盘后台运行测试程序";

            //注意：下面的路径可以是绝对路径、相对路径。但是需要注意的是：文件必须是一个.ico格式  

            //相对路径
            //这个是程序根目录下面的一张图片demo.ico
            string path = System.IO.Path.Combine((Application.StartupPath + @"\"), "demo.ico");
            notifyIcon.Icon = new System.Drawing.Icon(path);

            //绝对路径
            // notifyIcon.Icon = new System.Drawing.Icon("E:/ASP项目/images/3.5 inch Floppy.ico");  


            //true表示在托盘区可见，false表示在托盘区不可见  

            notifyIcon.Visible = true;

            //气泡显示的时间（单位是毫秒）  

            notifyIcon.ShowBalloonTip(2000);

            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);


            //设置二级菜单

            //MenuItem setting1 = new MenuItem("二级菜单1");  

            //MenuItem setting2 = new MenuItem("二级菜单2");  

            //MenuItem setting = new MenuItem("一级菜单", new MenuItem[]{setting1,setting2});  



            //帮助选项，这里只是“有名无实”在菜单上只是显示，单击没有效果，可以参照下面的“退出菜单”实现单击事件  

            MenuItem help = new MenuItem("帮助");

            //关于选项  

            MenuItem about = new MenuItem("设置");
            about.Click += new EventHandler(about_Click);
            //退出菜单项  

            MenuItem exit = new MenuItem("退出");

            exit.Click += new EventHandler(exit_Click);


            //关联托盘控件

            //注释的这一行与下一行的区别就是参数不同，setting这个参数是为了实现二级菜单  

            //MenuItem[] childen = new MenuItem[] { setting, help, about, exit };  

            MenuItem[] childen = new MenuItem[] { help, about, exit };

            notifyIcon.ContextMenu = new ContextMenu(childen);


            //窗体关闭时触发  

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);

        }

        private void about_Click(object sender, EventArgs e)
        {
            this.Show();
        }



        /// <summary>  

        /// 鼠标单击  

        /// </summary>  

        /// <param name="sender"></param>  

        /// <param name="e"></param>  

        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            //鼠标左键单击  

            if (e.Button == MouseButtons.Left)

            {

                //如果窗体是可见的，那么鼠标左击托盘区图标后，窗体为不可见  

                if (this.Visible == true)

                {

                    this.Visible = false;

                }

                else

                {

                    this.Visible = true;

                    this.Activate();

                }

            }

        }



        /// <summary>  

        /// 退出选项  

        /// </summary>  

        /// <param name="sender"></param>  

        /// <param name="e"></param>  

        private void exit_Click(object sender, EventArgs e)
        {
            //退出程序  
            System.Environment.Exit(0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        settingbuttons = MouseButtons.Left;
                        break;
                    }
                case 1:
                    {
                        settingbuttons = MouseButtons.Right;
                        break;
                    }
                case 2:
                    {
                        settingbuttons = MouseButtons.Middle;
                        break;
                    }
                case 3:
                    {
                        settingbuttons = MouseButtons.XButton1;
                        break;
                    }
                case 4:
                    {
                        settingbuttons = MouseButtons.XButton2;
                        break;
                    }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            switch (settingbuttons)
            {
                case MouseButtons.Left:
                    {
                        this.comboBox1.SelectedIndex = 0;
                        break;
                    }
                case MouseButtons.Right:
                    {
                        this.comboBox1.SelectedIndex = 1;
                        break;
                    }
                case MouseButtons.Middle:
                    {
                        this.comboBox1.SelectedIndex = 2;
                        break;
                    }
                case MouseButtons.XButton1:
                    {
                        this.comboBox1.SelectedIndex = 3;
                        break;
                    }
                case MouseButtons.XButton2:
                    {
                        this.comboBox1.SelectedIndex = 4;
                        break;
                    }
            }
        }
    }
}
