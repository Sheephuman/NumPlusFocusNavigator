using System.Windows;

namespace PlusButtonToShiftIndexer
{
    static class DependencyObjectExtension
    {

        /// <summary>
        /// WalkInChildrenメソッドの本体
        /// </summary>
        /// <param name="obj">DependencyObject</param>
        /// <param name="act">Action</param>
        private static void Walk(DependencyObject obj, System.Action<DependencyObject> act)
        {
            foreach (var child in LogicalTreeHelper.GetChildren(obj))
            {


                if (child is DependencyObject)
                {
                    var childobj = child as DependencyObject;

                    if (childobj is null || act is null)
                        return;


                    act(childobj);
                    Walk(childobj, act);
                }
            }
        }

        /// <summary>
        /// 子オブジェクトに対してデリゲートを実行する
        /// </summary>
        /// <param name="obj">this : DependencyObject</param>
        /// <param name="act">デリゲート : Action</param>
        public static void WalkInChildren(this DependencyObject obj, Action<DependencyObject> act)
        {
            if (act == null)
                throw new ArgumentNullException(obj.Dispatcher.ToString());

            Walk(obj, act);
        }
    }
}
