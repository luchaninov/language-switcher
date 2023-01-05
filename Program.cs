using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection.Emit;
using LanguageSwitcher.Properties;

namespace LanguageSwitcher
{
    internal static class Program
    {
        private const uint WM_INPUTLANGCHANGEREQUEST = 0x0050;

        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-loadkeyboardlayoutw
        private const uint KLF_ACTIVATE = 1;
        private const uint KLF_SUBSTITUTE_OK = 2;
        private const uint KLF_NOTELLSHELL = 80;

        private const int WH_CALLWNDPROC = 4;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static FormMain formMain = null;

        private static int lastKeyCodeDown = 0;
        private static DateTime lastControlKeyDate = DateTime.MinValue;

        [STAThread]
        static void Main()
        {
            _hookID = SetHook(_proc);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            //formMain = new FormMain();
            //Application.Run(formMain);

            Application.Run(new MyCustomApplicationContext(_hookID));

            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            // TODO
            // check RegisterHotKey
            // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerhotkey
            // https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool handled = false;

            // vkCode:
            // 19 - pause
            // 93 - context menu
            // 161 - right shift
            // 162 - left ctrl
            // 163 - right ctrl
            // 164 - left alt
            // 165 - right alt

            if (nCode >= 0) {
                if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN) {
                    int vkCode = Marshal.ReadInt32(lParam);

                    if (vkCode == 165 || vkCode == 162 || vkCode == 163) {
                        if (vkCode != lastKeyCodeDown) {
                            lastControlKeyDate = DateTime.Now;
                        }
                    }

                    lastKeyCodeDown = vkCode;
                    handled = (vkCode == 165);
                } else if (wParam == (IntPtr)WM_KEYUP|| wParam == (IntPtr)WM_SYSKEYUP) {
                    int vkCode = Marshal.ReadInt32(lParam);
                    handled = false;

                    if (vkCode == lastKeyCodeDown) {
                        TimeSpan elapsed = DateTime.Now - lastControlKeyDate;
                        //Console.WriteLine("{0} - {1} = {2}", DateTime.Now, lastControlKeyDate, elapsed.ToString());
                        //Console.WriteLine(elapsed.TotalMilliseconds);
                        if (elapsed.TotalMilliseconds < 500) {
                            if (vkCode == 165) {
                                changeLanguageUa();
                                handled = true;
                            } else if (vkCode == 162) {
                                changeLanguageEn();
                                handled = false;
                            } else if (vkCode == 163) {
                                changeLanguageRu();
                                handled = false;
                            }
                        }

                        lastKeyCodeDown = 0;
                    }

                    if (formMain != null)
                        formMain.label2.Text = vkCode.ToString() + " - " + handled.ToString();
                    //Console.WriteLine((Keys)vkCode);
                }
            }

            

            if (handled)
                return (System.IntPtr)1;

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void changeLanguage(string locale)
        {
            // https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-input-locales-for-windows-language-packs?view=windows-11
            var LANG = "00000409"; // en-US
            if (locale == "ru-RU")
                LANG = "00000419";
            else if (locale == "uk-UA")
                LANG = "00020422";

            IntPtr fGWindow = GetForegroundWindow();
            PostMessage(fGWindow, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(LANG, KLF_SUBSTITUTE_OK));

            /*
            // https://stackoverflow.com/questions/68843918/loadkeyboardlayout-fails-to-change-keyboard-layout-if-the-windows-taskbar-has
            IntPtr shell_TrayWnd = FindWindow("Shell_TrayWnd", null);
            if (fGWindow == shell_TrayWnd) {
                IntPtr vHandle = FindWindow("Progman", "Program Manager"); // Desktop Handler.
                SetForegroundWindow(vHandle);
                label1.Text = vHandle.ToString();
                //PostMessage(vHandle, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(LANG, KLF_SUBSTITUTE_OK));
                PostMessage(vHandle, WM_INPUTLANGCHANGEREQUEST, new IntPtr(-1), LoadKeyboardLayout(LANG, KLF_NOTELLSHELL));
            }
            else {
                
                PostMessage(fGWindow, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(LANG, KLF_SUBSTITUTE_OK));
                label1.Text = locale;
            }
            */

            /*
            IntPtr activeWindow = GetForegroundWindow();

            this.Focus();

            var original = InputLanguage.CurrentInputLanguage;
            var culture = System.Globalization.CultureInfo.GetCultureInfo(locale);
            var language = InputLanguage.FromCulture(culture);
            if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0) {
                InputLanguage.CurrentInputLanguage = language;
                label1.Text = locale;
            } else {
                InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
                label1.Text = "Not found";
            }
            */

            //SetForegroundWindow(activeWindow);
        }

        public static void changeLanguageEn()
        {
            changeLanguage("en-US");
        }

        public static void changeLanguageRu()
        {
            changeLanguage("ru-RU");
        }
        public static void changeLanguageUa()
        {
            changeLanguage("uk-UA");
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
    }

    public class MyCustomApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        private IntPtr _hookID = IntPtr.Zero;

        public MyCustomApplicationContext(IntPtr hookID)
        {
            _hookID = hookID;

            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit)
            }),
                Visible = true
            };
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            UnhookWindowsHookEx(_hookID);

            Application.Exit();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
    }
}
