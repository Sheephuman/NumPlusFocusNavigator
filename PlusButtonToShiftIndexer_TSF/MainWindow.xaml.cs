using PlusButtonToShiftIndexer.TSF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlusButtonToShiftIndexer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();



            DependencyObjectExtension.WalkInChildren(this, (child) =>
                        {
                            if (child is TextBox textBox)
                            {
                                textBox.PreviewKeyDown += TextBox_PreviewKeyDown;

                                if (child is UIElement uiElement)
                                {
                                    TextCompositionManager.AddPreviewTextInputHandler(uiElement, OnTextInput);
                                }
                            }
                        });
        }
        FocusNavigationDirection _focusDirection;
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var focused = Keyboard.FocusedElement;

            using (var _tsf = new TsfImeHelper())
            {
                // IMEオン時は無効
                if (e.Key == Key.Add && !_tsf.IsImeActive())
                {
                    e.Handled = true; // 通常の入力をキャンセル


                    var request = new TraversalRequest(FocusNavigationDirection.Next);
                    var element = (FrameworkElement)sender;
                    element.MoveFocus(request);

                    // UIElementの場合
                    if (focused is UIElement uie)
                    {
                        bool moved = uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));


                        if (!moved)
                        {
                            request = new TraversalRequest(FocusNavigationDirection.First);

                            uie.MoveFocus(request);

                        }
                    }

                }

            }

        }



        private void OnTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private bool TryMoveFocus(ContentElement element, FocusNavigationDirection dir)
        {
            var next = element.PredictFocus(dir) as IInputElement;
            if (next is UIElement uie)
            {
                uie.Focus();
                return true;
            }
            return false;
        }

        private void Test2_Button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("execute");
        }


    }
}


// e.Handled = true;




