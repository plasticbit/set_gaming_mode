using System.Runtime.InteropServices;
using System;
using System.Diagnostics;

namespace set_gaming_mode
{
    public partial class Form1 : Form
    {
        [DllImport(@"..\..\..\..\x64\Debug\ddcci_lib.dll")]
        private static extern IntPtr GetMonitor(IntPtr hwnd);

        [DllImport(@"..\..\..\..\x64\Debug\ddcci_lib.dll")]
        private static extern ulong GetNumMonitors(IntPtr hmonitor);

        [DllImport(@"..\..\..\..\x64\Debug\ddcci_lib.dll")]
        private static extern IntPtr AllocMonitorStruct(IntPtr hmonitor, ulong size);

        [DllImport(@"..\..\..\..\x64\Debug\ddcci_lib.dll")]
        private static extern bool GetPhysicalMonitor(IntPtr hmonitor, ulong size, IntPtr lppms);

        [DllImport(@"..\..\..\..\x64\Debug\ddcci_lib.dll")]
        private static extern IntPtr GetDescriptionFromStruct(IntPtr lppms, int idx);

        [DllImport(@"..\..\..\..\x64\Debug\ddcci_lib.dll")]
        private static extern bool SetVCPFeatureToMonitor(IntPtr lppms, byte key, ulong value);

        [DllImport(@"..\..\..\..\x64\Debug\ddcci_lib.dll")]
        private static extern bool GetVCPFeatureCurrentValueFromMonitor(IntPtr lppms, int idx, byte code, out ulong currentValue);

        [DllImport(@"..\..\..\..\x64\Debug\ddcci_lib.dll")]
        private static extern bool DestroyMonitorStruct(ulong size, IntPtr lppms);


        private IntPtr Monitor;
        private IntPtr LPPMS;

        public Form1()
        {
            InitializeComponent();

            Load += Form1_Load;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            Monitor = GetMonitor(Handle);
            if (Monitor == IntPtr.Zero) ShowErrorExit("Could not get the monitor.");

            var size = GetNumMonitors(Monitor);
            if (size <= 0) ShowErrorExit("Monitor not found.");

            LPPMS = AllocMonitorStruct(Monitor, size);
            if (LPPMS == IntPtr.Zero) ShowErrorExit("Failed to allocate memory");

            if (!GetPhysicalMonitor(Monitor, size, LPPMS)) ShowErrorExit("Could not get monitor information.");

            var description = Marshal.PtrToStringUni(GetDescriptionFromStruct(LPPMS, 0));
            label1.Text = (description == null ? "不明なモニター" : description.ToString());

            button3.Click += (o, e) => 
            {
                if (!GetVCPFeatureCurrentValueFromMonitor(LPPMS, 0, 0x15, out ulong cval))
                {
                    MessageBox.Show("Failed to get the current value.");
                    return;
                }

                var rb = ModeValueToRadioButton(cval);
                if (rb == null)
                {
                    MessageBox.Show("Unknown value returned.");
                    return;
                }

                rb.Focus();
            };

            button2.Click += (o, e) =>
            {
                var checkedButton = groupBox1.Controls.OfType<RadioButton>().SingleOrDefault(rb => rb.Checked);
                if (checkedButton == null) return;

                ulong? mode = RadioButtonToModeValue(checkedButton);
                if (mode == null) return;

                if (!SetVCPFeatureToMonitor(LPPMS, 0x15, (ulong)mode)) MessageBox.Show("Failed to set.", "Error");
                Close();
            };

            FormClosed += (o, e) =>
            {
                if (LPPMS != IntPtr.Zero) DestroyMonitorStruct(size, LPPMS);
            };
        }

        private void ShowErrorExit(string errMessage)
        {
            MessageBox.Show(errMessage, "Error");
            Close();
        }

        private ulong? RadioButtonToModeValue(RadioButton rb)
        {
            if (rb.Equals(radioButton1)) return 0x2d;
            if (rb.Equals(radioButton2)) return 0x2e;
            if (rb.Equals(radioButton3)) return 0x1e;
            if (rb.Equals(radioButton4)) return 0x1f;
            if (rb.Equals(radioButton5)) return 0x31;
            if (rb.Equals(radioButton6)) return 0x1;
            if (rb.Equals(radioButton7)) return 0x27;
            if (rb.Equals(radioButton8)) return 0xf;

            return null;
        }

        private RadioButton? ModeValueToRadioButton(ulong val)
        {
            if (val == 0x2d) return radioButton1;
            if (val == 0x2e) return radioButton2;
            if (val == 0x1e) return radioButton3;
            if (val == 0x1f) return radioButton4;
            if (val == 0x31) return radioButton5;
            if (val == 0x1) return radioButton6;
            if (val == 0x27) return radioButton7;
            if (val == 0xf) return radioButton8;

            return null;
        }
    }
}
/*
    ゲーマー1: 0x2d
    ゲーマー2: 0x2e
    FPS: 0x1e
    RTS: 0x1f
    鮮やか: 0x31
    ブルーライト低減モード: 0x1
    HDR効果: 0x27
    sRGB: 0xf
*/