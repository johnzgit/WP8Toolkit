using System.Windows;
using System.Windows.Controls;

namespace Ayls.WP8Toolkit.Controls
{
    public class ImmediateUpdateSourceTextBox : TextBox
    {
        public static readonly new DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(string), typeof(ImmediateUpdateSourceTextBox), new PropertyMetadata(default(string), OnTextChanged));

        private static void OnTextChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                textBox.Text = (string)e.NewValue;
            }
        }

        public new string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ImmediateUpdateSourceTextBox()
        {
            base.TextChanged += (s, e) => { Text = base.Text; };
        }
    }
}
