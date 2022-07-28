using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translate
{
    public class KeyboardEvent
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk,byte bScan,uint dwFlags,uint dwExtraInfo);

        public const int KEYEVENTF_KEYUP = 2;
        public void CtrlC()
        {
            keybd_event(Keys.ControlKey, 0, 0, 0);
            keybd_event(Keys.A, 0, 0, 0);
            keybd_event(Keys.ControlKey, 0, KEYEVENTF_KEYUP, 0);
            //keybd_event(Keys.A, 0, 2, 0);
            //A按键为2；
            //keybd_event(Keys.ControlKey, 0, 0, 0);
            //keybd_event(Keys.C, 0, 0, 0);
            //keybd_event(Keys.ControlKey, 0, 2, 0);
            //keybd_event(Keys.C, 0, 2, 0);
        }

        internal void CtrlV()
        {
            keybd_event(Keys.ControlKey, 0, 0, 0);
            keybd_event(Keys.C, 0, 0, 0);
            keybd_event(Keys.ControlKey, 0, 2, 0);
        }
    }
}
