using System.Runtime.InteropServices;

namespace NumPlusFocusNavigator_Winform
{
    internal class DetectIME
    {
        public DetectIME(IntPtr handle)
        {

            _WinHandle = handle;
        }

        IntPtr _WinHandle;


        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);
        //指定されたウィンドウに関連付けられている入力コンテキストを取得します。
        [DllImport("imm32.dll")]
        private static extern bool ImmGetOpenStatus(IntPtr hIMC);
        ///IME が開いているかどうかを調べます。
        [DllImport("imm32.dll")]
        private static extern uint ImmGetVirtualKey(IntPtr hWnd);
        //IMEが有効になる前の元々のキーコードを取得できる関数
        [DllImport("imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);
        //ImmGetContextで使用したhWnd、hIMCを解放(解放しないと不安定になる)



        private const int NI_CLOSECANDIDATE = 0x0011;
        private const int CPS_CANCEL = 0x0004;
        private const int NI_COMPOSITIONSTR = 0x0015;

        public bool IsImeOn()
        {
            IntPtr hWnd = _WinHandle;                 // ← WinFormsはこれでHWNDを取れる
            IntPtr hIMC = ImmGetContext(hWnd);
            if (hIMC != IntPtr.Zero)
            {
                bool isImeOpen = ImmGetOpenStatus(hIMC);
                ImmReleaseContext(hWnd, hIMC);         // ← 忘れずに解放
                return isImeOpen;
            }
            return false;
        }


    }
}
