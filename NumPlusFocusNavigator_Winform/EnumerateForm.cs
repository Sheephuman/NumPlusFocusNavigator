namespace NumPlusFocusNavigator_Winform
{
    internal static class EnumerateForm
    {
        /// <summary>
        /// 内部的に再帰で子コントロールを探索するメソッド
        /// </summary>
        private static void Walk(Control parent, Action<Control> act)
        {
            foreach (Control child in parent.Controls)
            {
                if (child == null) continue;
                if (act == null) return;

                act(child);
                Walk(child, act);  // 再帰
            }
        }

        /// <summary>
        /// 子コントロールに対してデリゲートを実行する拡張メソッド
        /// </summary>
        public static void WalkInChildren(this Control parent, Action<Control> act)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (act == null) throw new ArgumentNullException(nameof(act));

            Walk(parent, act);
        }
    }
}
