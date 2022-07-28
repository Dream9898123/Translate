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
    public partial class TipForm : Form
    {
        public TipForm()
        {
            InitializeComponent();
        }
        public void Show(int x, int y)
        {
            this.label1.Text = Clipboard.GetText();
            point = new Point(x, y - this.Height);
            this.Show();
        }
        private Point point;
        private void TipForm_Paint(object sender, PaintEventArgs e)
        {
            this.Location = point;
        }
    }
}
