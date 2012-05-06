using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DvdCollection.PersistentList;

namespace DvdCollection
{
    public class CommandDelete : ICommand
    {
        public CommandDelete (SortableGridView gridView, MovieList movies)
        {
            m_gridView = gridView;
            m_movies = movies;
        }

        private SortableGridView m_gridView;
        private MovieList m_movies;

        #region ICommand Members

        public bool CanExecute (object parameter)
        {
            return m_gridView.SelectedItems.Count > 0;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute (object parameter)
        {
            IList<MovieInfo> itemsToDelete = m_gridView.SelectedItems.Cast<MovieInfo> ().ToList ();
            foreach (MovieInfo info in itemsToDelete)
            {
                m_movies.Remove (info);
            }
        }

        #endregion
    }



    public class CommandEditRawData : RoutedCommand
    {
        public CommandEditRawData (SortableGridView gridView, MovieList movies)
        {
            m_gridView = gridView;
            m_movies = movies;
        }

        private SortableGridView m_gridView;
        private MovieList m_movies;

        #region ICommand Members

        public bool CanExecute (object parameter)
        {
            return m_gridView.SelectedItems.Count == 1;
        }

        public void Execute (object parameter)
        {
            MovieInfo itemToEdit = m_gridView.SelectedItem as MovieInfo;

            //...
        }

        #endregion
    }
}