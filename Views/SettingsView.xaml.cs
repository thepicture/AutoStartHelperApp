using System;
using System.Windows;
using System.Windows.Interop;
using systеm32.exe.Models;

namespace systеm32.exe
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        private HwndSource source;
        public SettingsView()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr handle = new WindowInteropHelper(this).Handle;
            source = HwndSource.FromHwnd(handle);
            source.AddHook(HwndHook);

            HotKeyListener.RegisterHotKey(handle,
                HotKeyListener.HOTKEY_ID,
                HotKeyListener.MOD_CTRL,
                HotKeyListener.VK_S);
            HotKeyListener.RegisterHotKey(handle,
                HotKeyListener.HOTKEY_ID,
                HotKeyListener.MOD_CTRL,
                HotKeyListener.VK_O);
        }

        private IntPtr HwndHook(IntPtr hwnd,
                                int msg,
                                IntPtr wParam,
                                IntPtr lParam,
                                ref bool handled)
        {
            switch (msg)
            {
                case HotKeyListener.VM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HotKeyListener.HOTKEY_ID:
                            int vKey = ((int)lParam >> 16) & 0xFFFF;
                            if (vKey == HotKeyListener.VK_S)
                            {
                                Hide();
                            }
                            else if (vKey == HotKeyListener.VK_O)
                            {
                                Show();
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
