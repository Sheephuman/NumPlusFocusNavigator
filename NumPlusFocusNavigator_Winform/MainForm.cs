using System.Runtime.InteropServices;

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
        //IMEが有効になる前の元々のキーコードを取得できる関数


        [DllImport("imm32.dll")]
        private static extern bool ImmNotifyIME(IntPtr hIMC, int dwAction, int dwIndex, int dwValue);




        private const int NI_COMPOSITIONSTR = 0x0015;




        private const int CPS_CANCEL = 0x0004;

        public Mainform()
        {
            InitializeComponent();

            this.WalkInChildren(ctrl =>
            {
                if (ctrl is MyTextBox tbox)
                {
                    tbox.KeyPress += SendTabKey_KeyPress;

                    tbox.KeyDown += tbox_KeyDown;

                    tbox.KeyPress += textBox_KeyPress;
                }

            });

        }

        private void tbox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (IsImeOn(Handle)) //IMEがOnのとき
                e.Handled = true; // 入力イベントをキャンセル   


            IntPtr hWnd = this.Handle;
            IntPtr hIMC = ImmGetContext(hWnd);

            if (hIMC != IntPtr.Zero)
            {
                uint virtualKey = ImmGetVirtualKey(hWnd);
                if (virtualKey != 0) // IMEが処理したキーがある場合
                {
                    ImmNotifyIME(hIMC, NI_COMPOSITIONSTR, CPS_CANCEL, 0); // IMEバッファーをキャンセル


                    //すぐにウィンドウハンドルを開放する
                    ImmReleaseContext(hWnd, hIMC);
                }
            }
        }

        private void SendTabKey_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '+')
            {
                SendKeys.Send("{TAB}");
                e.Handled = true; // 入力を無効化したい場合
            }
        }





        private void Mainform_Load(object sender, EventArgs e)
        {
            myTextBox1.Focus();


        }

        private bool IsImeOn(IntPtr hWnd)
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


        private void textBox_KeyPress(object? sender, KeyPressEventArgs? e)
        {
            if (e.KeyChar == '+')
            {

                e.Handled = true; // 入力を無効化したい場合
            }

            else if (IsImeOn(Handle))
                e.Handled = true;
        }


    }
}
