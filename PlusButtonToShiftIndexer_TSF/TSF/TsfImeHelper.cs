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

        private interface ITfDocumentMgr { /* 今回は空でOK */ }

        [DllImport("msctf.dll")]
        private static extern int TF_CreateThreadMgr(out ITfThreadMgr pptim);

        private ITfThreadMgr? _threadMgr;
        private int _tsfHResult;
        private bool _disposed;

        public TsfImeHelper()
        {
            // 二重初期化防止
            if (_threadMgr != null) return;

            _tsfHResult = TF_CreateThreadMgr(out _threadMgr);

            // HRESULT 成功判定
            if (_tsfHResult != 0 || _threadMgr == null)
            {
                // 初期化失敗時は解放
                _threadMgr = null;
            }
        }

        public bool IsImeActive()
        {
            if (_threadMgr == null || _tsfHResult != 0)
                return false;

            ITfDocumentMgr? docMgr = null;

            try
            {
                int hr = _threadMgr.GetFocus(out docMgr);
                if (hr != 0 || docMgr == null)
                    return false;

                // ここでIMEの詳細チェックを入れる場合は docMgr のContextを確認
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (docMgr != null)
                    Marshal.ReleaseComObject(docMgr);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_threadMgr != null && Marshal.IsComObject(_threadMgr))
            {
                Marshal.ReleaseComObject(_threadMgr);
                _threadMgr = null;
            }
        }
    }
}
