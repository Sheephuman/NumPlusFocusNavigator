using System.Runtime.InteropServices;
using System.Text;

namespace NumPlusFocusNavigator_Winform
{
    public partial class Mainform : Form
    {
        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll")]
        private static extern bool ImmGetOpenStatus(IntPtr hIMC);

        [DllImport("imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("imm32.dll")]
        private static extern uint ImmGetVirtualKey(IntPtr hWnd);

        [DllImport("imm32.dll")]
        private static extern bool ImmNotifyIME(IntPtr hIMC, int dwAction, int dwIndex, int dwValue);


        [DllImport("imm32.dll", CharSet = CharSet.Unicode)]
        private static extern int ImmGetCompositionStringW(
     IntPtr hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);


        private const int NI_COMPOSITIONSTR = 0x0015;


        /// <summary>
        /// Windows の IME がアプリに送る通知メッセージのひとつで、
        //        主に「未確定文字列」や「変換中の情報」が更新されたときに飛んできます。
        /// </summary>
        private const int WM_IME_COMPOSITION = 0x010F;


        private const int CPS_CANCEL = 0x0004;

        private const int NI_CLOSECANDIDATE = 0x0011;  // 候補ウィンドウを閉じる

        private const int GCS_COMPSTR = 0x0008;   // 未確定文字列


        public Mainform()
        {
            InitializeComponent();

            this.WalkInChildren(ctrl =>
            {
                if (ctrl is TextBox tbox)
                {
                    tbox.KeyDown += SendTabKey;
                    tbox.KeyDown += textBox_Keydowm_DetectIME;

                    tbox.TextChanged += TextBox_CleaText;
                }

            });

            this.KeyPreview = true;
            //KeydownイベントをMainFormで拾う
        }

        private void TextBox_CleaText(object? sender, EventArgs e)
        {
            var tbox2 = sender as TextBox;

            if (tbox2 is null && hWnd == IntPtr.Zero)
                return;

            if (IsImeOn(hWnd))
                tbox2!.Clear();


        }
        IntPtr hWnd;
        private void textBox_Keydowm_DetectIME(object? sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            hWnd = textBox.Handle;   // WinFormsでは Handle が HWND
            IntPtr hIMC = ImmGetContext(hWnd);

            if (hIMC != IntPtr.Zero)
            {
                uint virtualKey = ImmGetVirtualKey(hWnd);
                if (virtualKey != 0) // IME が処理したキーがある場合
                {



                    if (IsImeOn(hWnd)) // IMEがONなら
                    {


                        ImmNotifyIME(hIMC, NI_CLOSECANDIDATE, 0, 0);
                        // 候補ウィンドウを閉じる




                        ImmNotifyIME(hIMC, NI_COMPOSITIONSTR, CPS_CANCEL, 0); // IMEバッファをキャンセル


                        e.Handled = true;  // WinFormsで KeyDown をキャンセルする方法
                    }
                }
                ImmReleaseContext(hWnd, hIMC); // 必ず解放
            }
        }

        private void SendTabKey(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                SendKeys.Send("{TAB}");
        }

        private void Mainform_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            IntPtr hWnd = textBox.Handle;   // WinFormsでは Handle が HWND
            IntPtr hIMC = ImmGetContext(hWnd);

            if (hIMC != IntPtr.Zero)
            {
                uint virtualKey = ImmGetVirtualKey(hWnd);
                if (virtualKey != 0) // IME が処理したキーがある場合
                {
                    ImmNotifyIME(hIMC, NI_COMPOSITIONSTR, CPS_CANCEL, 0); // IMEバッファをキャンセル

                    if (IsImeOn(hWnd)) // IMEがONなら
                    {
                        e.Handled = true;  // WinFormsで KeyDown をキャンセルする方法
                    }
                }
                ImmReleaseContext(hWnd, hIMC); // 必ず解放
            }
        }

        private static bool IsImeOn(IntPtr hWnd)
        {
            IntPtr hIMC = ImmGetContext(hWnd);
            if (hIMC != IntPtr.Zero)
            {
                bool isImeOpen = ImmGetOpenStatus(hIMC);
                ImmReleaseContext(hWnd, hIMC);
                return isImeOpen;
            }
            return false;
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_IME_COMPOSITION)
            {
                IntPtr hIMC = ImmGetContext(this.Handle);
                if (hIMC != IntPtr.Zero)
                {
                    // 未確定文字列の長さを取得
                    int size = ImmGetCompositionStringW(hIMC, GCS_COMPSTR, null, 0);
                    if (size > 0)
                    {
                        byte[] buffer = new byte[size];
                        ImmGetCompositionStringW(hIMC, GCS_COMPSTR, buffer, size);

                        string comp = Encoding.Unicode.GetString(buffer);
                        Console.WriteLine("未確定文字列: " + comp);
                    }
                    ImmReleaseContext(this.Handle, hIMC);
                }
            }
            base.WndProc(ref m);
        }
    }
}
