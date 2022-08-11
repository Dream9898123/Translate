using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translate
{
    class MouseHook
    {
        public event System.Windows.Forms.MouseEventHandler MouseDownEvent;
        public event System.Windows.Forms.MouseEventHandler MouseClickEvent;
        public event System.Windows.Forms.MouseEventHandler MouseUpEvent;


        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        static int hMouseHook = 0; //声明鼠标钩子处理的初始值
        public const int WH_MOUSE_LL = 14;   //线程键盘钩子监听鼠标消息设为2，全局键盘监听鼠标消息设为13
        HookProc MouseHookProcedure; //声明MouseHookProcedure作为HookProc类型

        //使用此功能，安装了一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);


        //调用此函数卸载钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);


        //使用此功能，通过信息钩子继续下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        // 取得当前线程编号（线程钩子需要用到）
        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();

        //使用WINDOWS API函数代替获取当前实例的函数,防止钩子失效
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        public void Start()
        {
            // 安装键盘钩子
            if (hMouseHook == 0)
            {
                MouseHookProcedure = new HookProc(MouseHookProc);
                hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                
                //如果SetWindowsHookEx失败
                if (hMouseHook == 0)
                {
                    Stop();
                    throw new Exception("安装键盘钩子失败");
                }
            }
        }
        public void Stop()
        {
            bool retMouse = true;


            if (hMouseHook != 0)
            {
                retMouse = UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
            }

            if (!(retMouse)) throw new Exception("卸载钩子失败！");
        }
        //鼠标结构体
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public Point pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDBLCLK = 0x209;

        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // 侦听键盘事件
            if ((nCode >= 0) && (MouseDownEvent != null || MouseUpEvent != null || MouseClickEvent != null))
            {
                MouseHookStruct mouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                //鼠标单击
                //if (MouseClickEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                //{
                //    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                //    System.Windows.Forms.KeyEventArgs e = new System.Windows.Forms.KeyEventArgs(keyData);
                //    KeyDownEvent(this, e);
                //}

                //鼠标按下
                if (MouseDownEvent != null && (wParam == WM_LBUTTONDOWN || wParam == WM_RBUTTONDOWN || wParam == WM_MBUTTONDOWN))
                {
                    MouseButtons btn = MouseButtons.None;
                    switch (wParam)
                    {
                        case WM_LBUTTONDOWN:
                            {
                                btn = MouseButtons.Left;
                                break;
                            }
                        case WM_RBUTTONDOWN:
                            {
                                btn = MouseButtons.Right;
                                break;
                            }
                        case WM_MBUTTONDOWN:
                            {
                                btn = MouseButtons.Middle;
                                break;
                            }
                    }
                    MouseEventArgs e = new MouseEventArgs(btn, 1, mouseHookStruct.pt.X, mouseHookStruct.pt.Y, 0);
                    MouseDownEvent(this, e);
                }


            }
            //如果返回1，则结束消息，这个消息到此为止，不再传递。
            //如果返回0或调用CallNextHookEx函数则消息出了这个钩子继续往下传递，也就是传给消息真正的接受者
            return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }
        ~MouseHook()
        {
            Stop();
        }
    }
}
