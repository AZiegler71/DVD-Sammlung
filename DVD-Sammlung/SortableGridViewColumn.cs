using System.Windows;
using System.Windows.Controls;

namespace DvdCollection
{
    public class SortableGridViewColumn : GridViewColumn
    {
        public static readonly DependencyProperty SortPropertyNameProperty =
            DependencyProperty.Register ("SortPropertyName", typeof (string), typeof (SortableGridViewColumn),
            new UIPropertyMetadata (""));

        public static readonly DependencyProperty IsDefaultSortColumnProperty =
            DependencyProperty.Register ("IsDefaultSortColumn", typeof (bool), typeof (SortableGridViewColumn),
            new UIPropertyMetadata (false));

        public string SortPropertyName
        {
            get { return (string) GetValue (SortPropertyNameProperty); }
            set { SetValue (SortPropertyNameProperty, value); }
        }

        public bool IsDefaultSortColumn
        {
            get { return (bool) GetValue (IsDefaultSortColumnProperty); }
            set { SetValue (IsDefaultSortColumnProperty, value); }
        }
    }
}