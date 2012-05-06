using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DvdCollection
{
    public class SortableGridView : ListView
    {
        public static readonly DependencyProperty ColumnHeaderSortedAscendingTemplateProperty =
            DependencyProperty.Register ("ColumnHeaderSortedAscendingTemplate", typeof (string),
            typeof (SortableGridView), new UIPropertyMetadata (""));

        public string ColumnHeaderSortedAscendingTemplate
        {
            get { return (string) GetValue (ColumnHeaderSortedAscendingTemplateProperty); }
            set { SetValue (ColumnHeaderSortedAscendingTemplateProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeaderSortedDescendingTemplateProperty =
            DependencyProperty.Register ("ColumnHeaderSortedDescendingTemplate", typeof (string), typeof (SortableGridView),
            new UIPropertyMetadata (""));

        public string ColumnHeaderSortedDescendingTemplate
        {
            get { return (string) GetValue (ColumnHeaderSortedDescendingTemplateProperty); }
            set { SetValue (ColumnHeaderSortedDescendingTemplateProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeaderNotSortedTemplateProperty =
            DependencyProperty.Register ("ColumnHeaderNotSortedTemplate", typeof (string), typeof (SortableGridView),
            new UIPropertyMetadata (""));

        public string ColumnHeaderNotSortedTemplate
        {
            get { return (string) GetValue (ColumnHeaderNotSortedTemplateProperty); }
            set { SetValue (ColumnHeaderNotSortedTemplateProperty, value); }
        }

        protected override void OnInitialized (EventArgs e)
        {
            this.AddHandler (GridViewColumnHeader.ClickEvent, new RoutedEventHandler (GridViewColumnHeaderClickedHandler));

            GridView gridView = this.View as GridView;
            if (gridView != null)
            {
                SortableGridViewColumn sortableGridViewColumn = null;
                foreach (GridViewColumn gridViewColumn in gridView.Columns)
                {
                    sortableGridViewColumn = gridViewColumn as SortableGridViewColumn;
                    if (sortableGridViewColumn != null)
                    {
                        if (sortableGridViewColumn.IsDefaultSortColumn)
                        {
                            break;
                        }
                        sortableGridViewColumn = null;
                    }
                }

                if (sortableGridViewColumn != null)
                {
                    m_lastSortedOnColumn = sortableGridViewColumn;
                    Sort (sortableGridViewColumn.SortPropertyName, ListSortDirection.Ascending);

                    if (!String.IsNullOrEmpty (this.ColumnHeaderSortedAscendingTemplate))
                    {
                        sortableGridViewColumn.HeaderTemplate = this.TryFindResource (ColumnHeaderSortedAscendingTemplate) as DataTemplate;
                    }
                    this.SelectedIndex = 0;
                }
            }

            base.OnInitialized (e);
        }

        private void GridViewColumnHeaderClickedHandler (object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {
                SortableGridViewColumn sortableGridViewColumn = (headerClicked.Column) as SortableGridViewColumn;
                if (sortableGridViewColumn != null && !String.IsNullOrEmpty (sortableGridViewColumn.SortPropertyName))
                {
                    ListSortDirection direction;
                    bool newSortColumn = false;

                    if (m_lastSortedOnColumn == null
                        || String.IsNullOrEmpty (m_lastSortedOnColumn.SortPropertyName)
                        || !String.Equals (sortableGridViewColumn.SortPropertyName, m_lastSortedOnColumn.SortPropertyName,
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        newSortColumn = true;
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (m_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string sortPropertyName = sortableGridViewColumn.SortPropertyName;
                    Sort (sortPropertyName, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        if (!String.IsNullOrEmpty (this.ColumnHeaderSortedAscendingTemplate))
                        {
                            sortableGridViewColumn.HeaderTemplate = this.TryFindResource (ColumnHeaderSortedAscendingTemplate) as DataTemplate;
                        }
                        else
                        {
                            sortableGridViewColumn.HeaderTemplate = null;
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty (this.ColumnHeaderSortedDescendingTemplate))
                        {
                            sortableGridViewColumn.HeaderTemplate = this.TryFindResource (ColumnHeaderSortedDescendingTemplate) as DataTemplate;
                        }
                        else
                        {
                            sortableGridViewColumn.HeaderTemplate = null;
                        }
                    }

                    if (newSortColumn && m_lastSortedOnColumn != null)
                    {
                        if (!String.IsNullOrEmpty (this.ColumnHeaderNotSortedTemplate))
                        {
                            m_lastSortedOnColumn.HeaderTemplate = this.TryFindResource (ColumnHeaderNotSortedTemplate) as DataTemplate;
                        }
                        else
                        {
                            m_lastSortedOnColumn.HeaderTemplate = null;
                        }
                    }
                    m_lastSortedOnColumn = sortableGridViewColumn;
                }
            }
        }

        private void Sort (string sortBy, ListSortDirection direction)
        {
            if (ItemsSource == null)
            {
                return;
            }

            m_lastDirection = direction;
            ICollectionView dataView = CollectionViewSource.GetDefaultView (ItemsSource);

            dataView.SortDescriptions.Clear ();
            SortDescription sd = new SortDescription (sortBy, direction);
            dataView.SortDescriptions.Add (sd);
            dataView.Refresh ();
        }

        private SortableGridViewColumn m_lastSortedOnColumn = null;
        private ListSortDirection m_lastDirection = ListSortDirection.Ascending;
    }
}