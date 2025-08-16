using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using WindowsInput;
using WindowsInput.Native;

namespace PlusButtonToShiftIndexer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll")]
        private static extern bool ImmGetOpenStatus(IntPtr hIMC);



        [DllImport("imm32.dll")]
        private static extern bool ImmNotifyIME(IntPtr hIMC, int dwAction, int dwIndex, int dwValue);

        [DllImport("imm32.dll")]
        private static extern uint ImmGetVirtualKey(IntPtr hWnd);

        [DllImport("imm32.dll")]
        private static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);


        private const int NI_CLOSECANDIDATE = 0x0011;
        private const int CPS_CANCEL = 0x0004;

        private const int NI_COMPOSITIONSTR = 0x0015;

        public MainWindow()
        {
            InitializeComponent();


            DependencyObjectExtension.WalkInChildren(this, (child) =>
            {
                if (child is TextBox textBox)
                {
                    textBox.PreviewKeyDown += TextBox_PreviewKeyDown;

                }
            });

        }
        IntPtr hIMC;
        private bool IsImeOn()
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            IntPtr hWnd = helper.Handle;
            hIMC = ImmGetContext(hWnd);
            if (hIMC != IntPtr.Zero)
            {
                bool isImeOpen = ImmGetOpenStatus(hIMC);
                return isImeOpen;
            }
            return false;
        }
        private IntPtr winHandle; // ウィンドウハンドルをIntPtrで宣言

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var main = sender as Window;
            inputer.Focus();

            winHandle = new WindowInteropHelper(this).Handle;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            InputSimulator simulator = new();


            if (e.Key == Key.Add)
            {
                simulator.Keyboard.KeyPress(VirtualKeyCode.TAB); // Tabキーを送信
                e.Handled = true; // イベントを処理済みにする
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            IntPtr hWnd = winHandle;
            hIMC = ImmGetContext(hWnd);

            if (hIMC != IntPtr.Zero)
            {
                uint virtualKey = ImmGetVirtualKey(hWnd);
                if (virtualKey != 0) // IMEが処理したキーがある場合
                {
                    ImmNotifyIME(hIMC, NI_COMPOSITIONSTR, CPS_CANCEL, 0); // IMEバッファーをキャンセル

                    if (IsImeOn())
                        e.Handled = true; // 入力イベントをキャンセル

                    //すぐにウィンドウハンドルを開放する
                    ImmReleaseContext(hWnd, hIMC);
                }
            }
        }

        bool DeleteBuffer(HwndSource source)
        {
            var helper = new WindowInteropHelper(this);
            IntPtr hwnd = helper.Handle; // このウィンドウのハンドル

            bool isImeOpen = false;
            if (hwnd == IntPtr.Zero)
                return isImeOpen;

            IntPtr hIMC = ImmGetContext(source.Handle);
            if (hIMC != IntPtr.Zero)
            {
                isImeOpen = ImmNotifyIME(hIMC, NI_CLOSECANDIDATE, CPS_CANCEL, 0);
                //   ImmReleaseContext(hwnd, hIMC);
            }
            return isImeOpen;
        }


        ///以下はTest2タブ用








    }
}