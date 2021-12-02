using System;
using System.Runtime.InteropServices;

namespace systеm32.exe.Models
{
    public class HotKeyListener
    {
        public const uint MOD_CTRL = 0x0002;
        public const uint VK_S = 0x53;
        public const uint VK_O = 0x4F;
        public const int HOTKEY_ID = 9000;
        public const int VM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    }
}
