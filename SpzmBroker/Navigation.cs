using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Data;

namespace FDM_GA_Program
{
    class Navigation
    {
        public static string PickFile(string originalPath, string defaultFormat, string defaultFilter) // Get File Path
        {
            Microsoft.Win32.OpenFileDialog openPicker = new Microsoft.Win32.OpenFileDialog();
            openPicker.DefaultExt = defaultFormat;
            openPicker.Filter = defaultFilter;
            Nullable<bool> result = openPicker.ShowDialog();
            if (result == true)
            {
                return openPicker.FileName.ToString();
            }
            else
                return originalPath;
        }

        public static string PickFileNameOnly(string originalPath) // Get File Name only.
        {
            Microsoft.Win32.OpenFileDialog openPicker = new Microsoft.Win32.OpenFileDialog();
            //openPicker.DefaultExt = defaultFormat;
            //openPicker.Filter = defaultFilter;
            Nullable<bool> result = openPicker.ShowDialog();
            if (result == true)
            {
                return openPicker.SafeFileName.ToString();
            }
            else
                return originalPath;
        }

        public static string PickFolder(string originalFolder) // Get Folder Path.
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = originalFolder;
            DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK")
                return folderDialog.SelectedPath;
            else
                return originalFolder;
        }
    }
}
