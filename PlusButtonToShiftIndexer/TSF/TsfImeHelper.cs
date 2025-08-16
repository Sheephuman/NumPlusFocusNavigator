using System.Runtime.InteropServices;

namespace PlusButtonToShiftIndexer.TSF
{
    public class TsfImeHelper : IDisposable
    {

        [ComImport, Guid("AA80E7F0-2021-11D2-93E0-0060B067B86E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]


        private interface ITfThreadMgr
        {
            void Activate(out int clientId);
            void Deactivate();
            void CreateDocumentMgr(out ITfDocumentMgr docMgr);
            void EnumDocumentMgrs(out object enumDocMgrs);
            int GetFocus(out ITfDocumentMgr docMgr);
            void SetFocus(ITfDocumentMgr docMgr);
            void AssociateFocus(IntPtr hwnd, ITfDocumentMgr newDocMgr, out ITfDocumentMgr prevDocMgr);
            void IsThreadFocus([MarshalAs(UnmanagedType.Bool)] out bool isFocus);


        }


        private interface ITfDocumentMgr
        {
            // 実際には複数メソッドがあるが、今回の用途では不要
        }



        int _tsfInt;

        [DllImport("msctf.dll")]
        static extern int TF_CreateThreadMgr(out ITfThreadMgr pptim);
        ITfThreadMgr _threadMgr { get; set; } = null!;

        public bool IsImeActive()
        {


            ITfThreadMgr threadMgr = null!;

            try
            {

                // ThreadMgr の作成
                if (_tsfInt != 0 || threadMgr == null)
                    return false;

                _threadMgr = threadMgr;
                // フォーカス中の DocumentMgr を取得
                int hr = threadMgr.GetFocus(out ITfDocumentMgr? docMgr);
                if (hr != 0 || docMgr == null)
                    return false;



                // IME判定（必要に応じて docMgr 内の Context も調べられる）
                return true;



            }
            catch
            {
                return false;
            }

            finally
            {
                // 参照カウントを確実に解放

                if (threadMgr != null)
                    Marshal.ReleaseComObject(threadMgr);
            }
        }

        public void Dispose()
        {
            if (Marshal.IsComObject(_threadMgr))
            {
                Marshal.ReleaseComObject(_threadMgr);
            }

        }
    }
}