using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TR5TrainerTest
{
    public enum FieldType
    {
        Byte,
        Int16,
        UInt16,
        Int32,
        UInt32,
    }

    public delegate void ValueChangedHandler(long newVal);

    public unsafe partial class FieldCtrl : UserControl
    {
        private string _format;
        private string _address = "0";
        private string _struct;
        private int _bit = -1;

        public event ValueChangedHandler ValueChanged = delegate {  };

        public FieldCtrl()
        {
            InitializeComponent();
            Bit = -1;
        }

        public string FieldName
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        public string Address
        {
            get { return string.IsNullOrWhiteSpace(_address) ? "0" : _address; }
            set
            {
                _address = value;
                Refresh();
            }
        }

        public uint IntAddress => (Address.Contains("0x") 
            ? uint.Parse(Address.Replace("0x", "").Trim(), NumberStyles.HexNumber)
            : uint.Parse(Address.Trim())) + StructOffset;
        public FieldType Type { get; set; }

        public long Value
        {
            get
            {
                try
                {
                    long val;
                    switch (Type)
                    {
                        default:
                        case FieldType.Byte:
                            val = MainWindow.pmr.ReadByte((byte*) IntAddress);
                            break;
                        case FieldType.Int16:
                            val = MainWindow.pmr.ReadInt16((byte*) IntAddress);
                            break;
                        case FieldType.UInt16:
                            val = MainWindow.pmr.ReadUInt16((byte*) IntAddress);
                            break;
                        case FieldType.Int32:
                            val = MainWindow.pmr.ReadInt32((byte*) IntAddress);
                            break;
                        case FieldType.UInt32:
                            val = MainWindow.pmr.ReadUInt32((byte*) IntAddress);
                            break;
                    }
                    if (Bit != -1)
                        val = ((val & (1 << Bit)) != 0 ? 1 : 0);
                    return val;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ValueChanged(value);
                LockedValue = value;
                if (Bit != -1)
                {
                    var tmpBit = Bit;
                    Bit = -1;
                    var val = Value;
                    Bit = tmpBit;
                    if (value != 0)
                        val |= (1 << Bit);
                    else
                        val &= ~(1 << Bit);
                    value = val;
                }
                switch (Type)
                {
                    default:
                    case FieldType.Byte:
                        MainWindow.pmr.WriteByte((byte*) IntAddress, (byte) value);
                        break;
                    case FieldType.Int16:
                        MainWindow.pmr.WriteInt16((byte*) IntAddress, (short) value);
                        break;
                    case FieldType.UInt16:
                        MainWindow.pmr.WriteUInt16((byte*) IntAddress, (ushort) value);
                        break;
                    case FieldType.Int32:
                        MainWindow.pmr.WriteInt32((byte*) IntAddress, (int) value);
                        break;
                    case FieldType.UInt32:
                        MainWindow.pmr.WriteUInt32((byte*) IntAddress, (uint) value);
                        break;
                }
                Refresh();
            }
        }

        public string Format
        {
            get { return _format; }
            set
            {
                _format = value;
                Refresh();
            }
        }

        public string Struct
        {
            get { return _struct ?? ""; }
            set
            {
                _struct = value;
                Refresh();
            }
        }

        public uint StructOffset
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Struct)) return 0;
                else
                {
                    var spl = Struct.Split('+');
                    var str = spl[0];
                    var off = (uint) Marshal.OffsetOf(System.Type.GetType("TR5TrainerTest." + str.Split('.')[0]), str.Split('.')[1]);
                    if (spl.Length == 2) off += uint.Parse(spl[1]);
                    return off;
                }
            }
        }

        public int Bit
        {
            get { return _bit; }
            set
            {
                _bit = value;
                Refresh();
            }
        }

        private bool locking = false;
        private bool changed = false;
        private bool writing = false;

        public void Refresh()
        {
            if (locking)
                return;

            if (chkLock.Checked)
            {
                locking = true;
                Value = LockedValue;
                locking = false;
            }

            if (txtVal.Focused || changed)
            {
                return;
            }

            writing = true;
            txtVal.Text = Value.ToString(Format);
            writing = false;
        }

        private long? getTypedValue()
        {
            try
            {
                long val;
                if (txtVal.Text.Contains("0x"))
                {
                    val = long.Parse(txtVal.Text.Replace("0x", "").Trim(), NumberStyles.HexNumber);
                }
                else
                {
                    val = long.Parse(txtVal.Text.Trim());
                }
                
                txtVal.BackColor = SystemColors.Window;
                return val;
            }
            catch
            {
                txtVal.BackColor = Color.Red;
                return null;
            }
        }

        private long LockedValue = 0;

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtVal.Text.Contains("0x"))
                {
                    Value = long.Parse(txtVal.Text.Replace("0x", "").Trim(), NumberStyles.HexNumber);
                }
                else
                {
                    Value = long.Parse(txtVal.Text.Trim());
                }
                changed = false;
                txtVal.BackColor = SystemColors.Window;
            }
            catch
            {
                txtVal.BackColor = Color.Red;
            }
        }

        private void txtVal_TextChanged(object sender, EventArgs e)
        {
            if (!writing)
                changed = true;
        }

        private void chkLock_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLock.Checked)
                btnSet_Click(sender, e);
        }
    }
}
