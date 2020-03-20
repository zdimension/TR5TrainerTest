using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TR5TrainerTest
{
    public unsafe partial class OutsideWindow : Form
    {
        public OutsideWindow()
        {
            InitializeComponent();
        }

        public int mx = -1;
        public int my = -1;

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var (x, y) = (e.Location.X, e.Location.Y);
            if (pictureBox1.Image == null) return;
            double rx = x - (pictureBox1.Width / 2.0 - pictureBox1.Image.Width / 2.0);
            double ry = y - (pictureBox1.Height / 2.0 - pictureBox1.Image.Height / 2.0);

            if (rx >= 0 && rx < pictureBox1.Image.Width &&
                ry >= 0 && ry < pictureBox1.Image.Height)
            {
                rx /= 14;
                ry /= 14;

                mx = (int)(rx);
                my = (int) (ry);
            }
            else
            {
                mx = my = -1;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var (x, y) = (e.Location.X, e.Location.Y);

            double rx = x - (pictureBox1.Width / 2.0 - pictureBox1.Image.Width / 2.0);
            double ry = y - (pictureBox1.Height / 2.0 - pictureBox1.Image.Height / 2.0);

            if (rx >= 0 && rx < pictureBox1.Image.Width &&
                ry >= 0 && ry < pictureBox1.Image.Height)
            {
                rx = (rx * 1024) / 14;
                ry = (ry * 1024) / 14;

                var rptr = (room_info*)MainWindow.pmr.ReadUInt32((byte*)0x875154) + Program.wnd.fieldCtrl21.Value;
                var rm = MainWindow.pmr.ReadStruct<room_info>((byte*)rptr);

                Program.wnd.fcLaraPosZ.Value = (long)Math.Round(rm.z + rx);
                Program.wnd.fcLaraPosX.Value = (long)Math.Round(rm.x + ry);
            }

            Program.wnd.FocusGame();
        }
    }
}
