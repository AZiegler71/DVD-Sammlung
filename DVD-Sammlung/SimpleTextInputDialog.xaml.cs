using System.Windows;
using System.Windows.Controls;
using DvdCollection.Properties;

namespace DvdCollection
{
    public partial class SimpleTextInputDialog : Window
    {
        public string Text
        {
            get { return textBox.Text; }
        }

        public SimpleTextInputDialog (string title, string labelText, string defaultText)
        {
            Title = title;
            InitializeComponent ();
            label.Text = labelText;

            textBox.Text = defaultText;
            textBox.Focus ();
            okButton.IsEnabled = !string.IsNullOrEmpty (textBox.Text);
        }

        private void OnTextChanged (object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            okButton.IsEnabled = !string.IsNullOrEmpty (textBox.Text);
        }

        private void OkOnClick (object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close ();
        }
    }
}
