using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DvdCollection
{
    public static class Utils
    {
        public static string GetFolderFromUser (string title, out bool canceled)
        {
            canceled = false;

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog ();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dialog.ShowNewFolderButton = false;
            dialog.Description = title;
            if (dialog.ShowDialog () == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedPath;
            }

            canceled = true;
            return null;
        }
    }
}
