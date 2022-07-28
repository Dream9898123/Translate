using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translate
{
    public class ClipBoard
    {
        /// <summary>
        /// 复制或剪切文件至剪贴板(方法）
        /// </summary>
        /// <param name="files">需要添加到剪切板的文件路径数组</param>
        /// <param name="cut">是否剪切true为剪切，false为复制</param>
        public static void CopyToClipboard(string[] files, bool cut)
        {
            if (files == null) return;
            IDataObject data = new DataObject(DataFormats.FileDrop, files);
            MemoryStream memo = new MemoryStream(4);
            byte[] bytes = new byte[] { (byte)(cut ? 2 : 5), 0, 0, 0 };
            memo.Write(bytes, 0, bytes.Length);
            data.SetData("Preferred DropEffect", memo);
            Clipboard.SetDataObject(data);
        }

        /// <summary>
        /// 获取剪贴板中的文件名（方法）
        /// </summary>
        /// <returns>System.Collections.List<string>返回剪切板中文件名集合</returns>
        public List<string> GetClipboardList()
        {
            List<string> clipboardList = new List<string>();
            System.Collections.Specialized.StringCollection sc = Clipboard.GetFileDropList();
            for (int i = 0; i < sc.Count; i++)
            {
                string listfileName = sc[i];
                clipboardList.Add(listfileName);
            }
            return clipboardList;
        }
        /// <summary>
        /// 获取剪贴板数据
        /// </summary>
        /// <returns>返回剪贴板中的文本数据</returns>
        public string GetClipBoardStr()
        {
            //IDataObject iData = Clipboard.GetDataObject();
            //if (iData.GetDataPresent(DataFormats.Text))
            //{
            //    // Yes it is, so display it in a text box.
            //    return (string)iData.GetData(DataFormats.Text);
            //}
            //return "fail";
            return Clipboard.GetText();
        }
        public void SetClipBoardStr(string value)
        {
            try
            {
                Clipboard.SetDataObject(value, true, 3, 100);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
