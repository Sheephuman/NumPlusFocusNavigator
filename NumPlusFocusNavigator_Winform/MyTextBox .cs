using System.Runtime.InteropServices;

namespace NumPlusFocusNavigator_Winform
{
    class MyTextBox : TextBox
    {


        public MyTextBox()
        {



            this.KeyDown += textBox_Keydown;
        }


        private void textBox_Keydown(object? sender, KeyEventArgs e)
        {

        }







        /// <summary>
        /// IME の未確定文字列のバイト長を取得する
        /// </summary>
        /// <returns>未確定文字列の文字数</returns>
        public int GetImeCompositionLength()
        {
            IntPtr hIMC = ImmGetContext(this.Handle);
            if (hIMC != IntPtr.Zero)
            {
                try
                {
                    int sizeInBytes = ImmGetCompositionStringW(hIMC, GCS_COMPSTR, null, 0);
                    // Unicode なので 2 バイト単位で文字数に換算
                    return sizeInBytes / 2;
                }
                finally
                {
                    ImmReleaseContext(this.Handle, hIMC);
                }
            }
            return 0;
        }



        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("imm32.dll")] private static extern bool ImmGetOpenStatus(IntPtr hIMC);

        [DllImport("imm32.dll", CharSet = CharSet.Unicode)]
        private static extern int ImmGetCompositionStringW(
     IntPtr hIMC, int dwIndex, byte[]? lpBuf, int dwBufLen);


        private const int NI_COMPOSITIONSTR = 0x0015;


        /// <summary>
        /// Windows の IME がアプリに送る通知メッセージのひとつで、
        //        主に「未確定文字列」や「変換中の情報」が更新されたときに飛んできます。
        /// </summary>
        private const int WM_IME_COMPOSITION = 0x010F;


        private const int CPS_CANCEL = 0x0004;

        private const int NI_CLOSECANDIDATE = 0x0011;  // 候補ウィンドウを閉じる

        private const int GCS_COMPSTR = 0x0008;   // 未確定文字列




        private const int WM_KEYDOWN = 0x0100;


    }
}
