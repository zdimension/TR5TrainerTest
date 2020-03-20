using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TR5TrainerTest.Logger;

namespace TR5TrainerTest
{
    public unsafe partial class MainWindow : Form
    {
        public static ProcessMemoryReader pmr = new ProcessMemoryReader();
        public static globalKeyboardHook hook = new globalKeyboardHook();
        public MainWindow()
        {
            InitializeComponent();

            btnRefresh_Click(this, null);

            Program.wnd = this;

            hook.HookedKeys.Add(Keys.NumPad8);
            hook.HookedKeys.Add(Keys.NumPad2);
            hook.HookedKeys.Add(Keys.NumPad4);
            hook.HookedKeys.Add(Keys.NumPad6);

            hook.HookedKeys.Add(Keys.Add);
            hook.HookedKeys.Add(Keys.Subtract);

            hook.HookedKeys.Add(Keys.Multiply);
            hook.HookedKeys.Add(Keys.Divide);

            hook.HookedKeys.Add(Keys.Decimal);

            hook.KeyDown += HookOnKeyDown;
            hook.KeyUp += Hook_KeyUp;

        }

        private void Hook_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Decimal)
                shiftPressed = false;
        }

        private bool shiftPressed = false;
        private void HookOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Decimal)
                shiftPressed = true;

            var val = (e.Shift || shiftPressed) ? 1024 : 128;
            switch (e.KeyCode)
            {
                case Keys.NumPad8:
                    fcLaraPosX.Value += val;
                    break;
                case Keys.NumPad2:
                    fcLaraPosX.Value -= val;
                    break;

                case Keys.NumPad4:
                    fcLaraPosZ.Value += val;
                    break;
                case Keys.NumPad6:
                    fcLaraPosZ.Value -= val;
                    break;

                case Keys.Add:
                    fcLaraPosY.Value -= val;
                    break;
                case Keys.Subtract:
                    fcLaraPosY.Value += val;
                    break;

                case Keys.Multiply:
                    fieldCtrl19.Value = 3;
                    fieldCtrl17.Value = 445;
                    break;

                case Keys.Divide:
                    fieldCtrl19.Value = fieldCtrl19.Value == 3 ? 0 : 3;
                    break;
            }

            e.Handled = true;
        }

        private bool loadingProcesses = false;

        void RefreshProcesses()
        {
            loadingProcesses = true;
            cbxProc.DataSource = new[] {new {ID = -1, Txt = "Populating..."}};

            new Thread(() =>
            {
                var s = Process.GetProcesses()
                    .Where(x =>
                    {
                        try
                        {
                            var tmp = x.MainModule.FileName;
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .OrderBy(x => Path.GetFileName(x.MainModule.FileName))
                    .Select(x => new
                    {
                        ID = x.Id,
                        Txt = Path.GetFileName(x.MainModule.FileName) + " (ID: " + x.Id + ")"
                    }).ToList();
                var id = s.FindIndex(x => x.Txt.ToLower().Contains("tomb"));
                if (id == -1)
                    id = 0;
                loadingProcesses = false;
                cbxProc.Invoke(new Action(() =>
                {
                    cbxProc.DataSource = s;
                    cbxProc.SelectedIndex = id;
                }));

                
            }).Start();            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshProcesses();
        }

        private void cbxProc_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAttach.Enabled = !loadingProcesses && cbxProc.SelectedIndex != -1;
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            if (btnAttach.Text == "Detach")
            {
                pmr.CloseHandle();
                btnAttach.Enabled = false;
                btnAttach.Text = "Attach";
            }
            else
            {
                try
                {
                    var proc = Process.GetProcessById((int) cbxProc.SelectedValue);
                    pmr.ReadProcess = proc;
                    pmr.OpenProcess();
                    btnAttach.Text = "Detach";
                }
                catch
                {
                    MessageBox.Show("broke");
                    throw;
                }
            }
        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public void FocusGame()
        {
            SetForegroundWindow(pmr.ReadProcess.MainWindowHandle);
            Thread.Sleep(25);
            SetForegroundWindow(this.Handle);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }


        void RefreshData()
        {
            if (btnAttach.Text != "Detach") return;

            var lara = pmr.ReadStruct<lara_info>((byte*) 0xe5bd60);

            foreach (FieldCtrl x in GetAll(this, typeof(FieldCtrl)))
            {
                x.Refresh();
                if (x.Struct.Contains("ITEM_INFO"))
                    x.Address = ((int) ((ITEM_INFO*) pmr.ReadUInt32((byte*) 0xeeeff0) + lara.item_number)).ToString();
            }

            // enable dozy
            var addr = (byte*) pmr.ReadUInt32((byte*) 0xe5c2bc);
            var gf = pmr.ReadStruct<GAMEFLOW>(addr);
            gf.bitfield |= 1;
            pmr.WriteStruct(addr, gf);

            pmr.WriteInt16((byte*) 0xe5bbf8, 1000);

            if (cbxFallSpeed.Checked && fieldCtrl18.Value > 140)
                fieldCtrl18.Value = 140;

            if (outside.Visible)
                (outside.pictureBox1.Image, _, _, outside.label1.Text) = Minimap.GetMinimap((int) fieldCtrl21.Value, outside.mx, outside.my);
            else
                (pbMinimap.Image, _, _, _) = Minimap.GetMinimap((int) fieldCtrl21.Value);

            if (checkBox3.Checked)
            {
                updateFPS();
            }

            if (checkBox4.Checked)
            {
                //pmr.WriteByte((byte*)0xeefa50, 1);
                //pmr.WriteBytes((byte*)0x4030a3, new byte[] { 0x0f, 0x1f, 0x44, 0x00, 0x00 });
             //pmr.WriteBytes((byte*) 0x401d5c, new byte[] {0x0f, 0x1f, 0x44, 0x00, 0x00});
                pmr.WriteBytes((byte*) 0x40f074, new byte[] { 0xE9, 0x03, 0x03, 0x00, 0x00 });
                // pmr.WriteBytes((byte*) 0x40f074, new byte[] {0xE9, 0x7F, 0x03, 0x00, 0x00 });
            }
            else
            {
                //pmr.WriteByte((byte*)0xeefa50, 0);
                //pmr.WriteBytes((byte*) 0x4030a3, new byte[] {0xe9, 0xb8, 0xc6, 0x08, 0x00});
                //pmr.WriteBytes((byte*)0x401d5c, new byte[] { 0xe9, 0xcf, 0xcf, 0x00, 0x00 });
                pmr.WriteBytes((byte*) 0x40f074, new byte[] {0xA1, 0x60, 0xF9, 0xEE, 0x00});
            }
        }

        void updateFPS()
        {
            
            // only update the camera if it's targeted at lara
            if (Math.Abs(fcCamPosX.Value - fcLaraPosX.Value) < 2 * 1024 &&
                Math.Abs(fcCamPosY.Value + 1024 - fcLaraPosY.Value) < 2 * 512 &&
                Math.Abs(fcCamPosZ.Value + 100 - fcLaraPosZ.Value) < 2 * 1024)
            {
                var lara = pmr.ReadStruct<lara_info>((byte*)0xe5bd60);
                var item = pmr.ReadStruct<ITEM_INFO>((byte*)((ITEM_INFO*)pmr.ReadUInt32((byte*)0xeeeff0) + lara.item_number));
                var angle = (65536 - ((item.pos.y_rot + lara.head_y_rot + 16384) % 65536)) * Math.PI / 32768.0;
                //checkBox4.Checked = true;
                /*fcCamPosX.Value = fcLaraPosX.Value;
               
                fcCamPosZ.Value = fcLaraPosZ.Value;*/
                var headoff = 128;
                fcCamPosX.Value = (long)Math.Round(fcLaraPosX.Value - headoff * Math.Cos(angle));
                fcCamPosZ.Value = (long)Math.Round(fcLaraPosZ.Value - headoff * Math.Sin(angle));

                fcCamPosY.Value = fcLaraPosY.Value - 700;
                fcCamTargetY.Value = fcCamPosY.Value;

                var dist = 1024;
               
                fcCamTargetX.Value = (long) Math.Round(fcCamPosX.Value - dist * Math.Cos(angle));
                fcCamTargetZ.Value = (long) Math.Round(fcCamPosZ.Value - dist * Math.Sin(angle));

                pmr.WriteUInt32((byte*) 0xeefa30, (uint) fcCamTargetX.Value);
                pmr.WriteUInt32((byte*) 0xeefa34, (uint) fcCamTargetY.Value);
                pmr.WriteUInt32((byte*) 0xeefa38, (uint) fcCamTargetZ.Value);
            }
            else
            {
                checkBox4.Checked = false;
            }

        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>().ToList();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                .Concat(controls)
                .Where(c => c.GetType() == type);
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void cbxAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            tmrRefresh.Enabled = cbxAutoRefresh.Checked;
        }

        private void fieldCtrl17_ValueChanged(long newVal)
        {
            if (checkBox2.Checked)
            {
                fieldCtrl20.Value = pmr.ReadInt16((byte*) &((ANIM_STRUCT*) pmr.ReadUInt32((byte*) 0x875158) + newVal)->frame_base);
            }
        }

        private void pbMinimap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var (x, y) = (e.Location.X, e.Location.Y);

            double rx = x - (pbMinimap.Width / 2.0 - pbMinimap.Image.Width / 2.0);
            double ry = y - (pbMinimap.Height / 2.0 - pbMinimap.Image.Height / 2.0);

            if (rx >= 0 && rx < pbMinimap.Image.Width &&
                ry >= 0 && ry < pbMinimap.Image.Height)
            {
                rx = (rx * 1024) / 14;
                ry = (ry * 1024) / 14;

                var rptr = (room_info*) MainWindow.pmr.ReadUInt32((byte*) 0x875154) + fieldCtrl21.Value;
                var rm = MainWindow.pmr.ReadStruct<room_info>((byte*)rptr);

                fcLaraPosZ.Value = (long)Math.Round(rm.z + rx);
                fcLaraPosX.Value = (long)Math.Round(rm.x + ry);
            }

            FocusGame();
        }

        OutsideWindow outside = new OutsideWindow();

        private void button1_Click(object sender, EventArgs e)
        {
            outside.Show();
            
        }

        private void pbMinimap_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            fcSmallMedi.Value = fcBigMedi.Value = fcFlares.Value
                = fieldCtrl1.Value = fieldCtrl2.Value
                    = fieldCtrl3.Value = fieldCtrl4.Value
                        = fieldCtrl5.Value = fieldCtrl6.Value
                            = fieldCtrl8.Value = fieldCtrl9.Value = -1;

            fieldCtrl71.Value = fieldCtrl72.Value = fieldCtrl73.Value
                = fieldCtrl74.Value = fieldCtrl75.Value
                    = fieldCtrl76.Value = 9;

            fieldCtrl77.Value = fieldCtrl78.Value = fieldCtrl79.Value
                = fieldCtrl80.Value = fieldCtrl81.Value
                    = fieldCtrl82.Value = fieldCtrl83.Value
                        = fieldCtrl84.Value = fieldCtrl85.Value = 1;

            fieldCtrl86.Value = 0;
        }
    }
}
