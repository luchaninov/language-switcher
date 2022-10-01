using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LanguageSwitcher
{
    public partial class FormMain : Form
    {
        private const uint WM_INPUTLANGCHANGEREQUEST = 0x0050;

        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-loadkeyboardlayoutw
        private const uint KLF_ACTIVATE = 1;
        private const uint KLF_SUBSTITUTE_OK = 2;
        private const uint KLF_NOTELLSHELL = 80;

        public FormMain()
        {
            InitializeComponent();
        }

        public void changeLanguageEn()
        {
            changeLanguage("en-US");
        }

        public void changeLanguageRu()
        {
            changeLanguage("ru-RU");
        }
        public void changeLanguageUa()
        {
            changeLanguage("uk-UA");
        }

        private void changeLanguage(string locale)
        {
            // https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-input-locales-for-windows-language-packs?view=windows-11
            var LANG = "00000409"; // en-US
            if (locale == "ru-RU")
                LANG = "00000419";
            else if (locale == "uk-UA")
                LANG = "00020422";

            IntPtr fGWindow = GetForegroundWindow();
            PostMessage(fGWindow, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(LANG, KLF_SUBSTITUTE_OK));
            label1.Text = locale;

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

        private void btnEnClick(object sender, EventArgs e)
        {
            changeLanguageEn();
        }

        private void btnRuClick(object sender, EventArgs e)
        {
            changeLanguageRu();
        }
        private void btnUaClick(object sender, EventArgs e)
        {
            changeLanguageUa();
        }

        private void formLoad(object sender, EventArgs e)
        {
            label1.Text = InputLanguage.CurrentInputLanguage.LayoutName;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
    }
}
