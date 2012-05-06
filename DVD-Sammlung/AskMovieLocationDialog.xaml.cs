using System.Windows;
using System.Windows.Controls;
using DvdCollection.Properties;

namespace DvdCollection
{
    /// <summary>
    /// Interaction logic for AskMovieLocationDialog.xaml
    /// </summary>
    public partial class AskMovieLocationDialog : Window
    {
        public string DvdName
        {
            get { return location.Text; }
        }

        public AskMovieLocationDialog ()
        {
            InitializeComponent ();

            location.Text = Settings.Default.LastLocation;
            location.Focus ();
            okButton.IsEnabled = false;
        }

        private void LocationOnTextChanged (object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            okButton.IsEnabled = !string.IsNullOrEmpty (textBox.Text);
        }

        private void OkOnClick (object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Settings.Default.LastLocation = DvdName;
            Close ();
        }
    }
}
