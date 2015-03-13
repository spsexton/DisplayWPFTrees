using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace DisplayWpfTrees
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeTreeviews();
        }

        /// <summary>
        /// DragEnter allows prohibiting disallowed formats on drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// DragEnter allows prohibiting disallowed formats on drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// When file is dropped on window, dump its logical tree to our TreeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string filename = files[0];     // Only deal with first one
            FileInfo fi = new FileInfo(filename);

            if (fi.Extension == ".xaml")
            {
                tvLogical.Items.Clear();
                tvVisual.Items.Clear();

                // Load loose XAML file
                DependencyObject root = (DependencyObject)LoadXAMLObjectGraphFromFile(filename);

                // If window, show it.  This causes visual elements to get properly instantiated.
                Window win = root as Window;
                if (win != null)
                    win.Show();

                // For pages, host them in a NavigationWindow and then show the window.
                // TODO: Even showing the loaded XAML as a page doesn't seem to instantiate
                //   visual objects in the page.  The top-level Page element still doesn't have
                //   any children.  Figure out how to get things properly instantiated.
                Page pg = root as Page;
                NavigationWindow navwin = null;
                if (pg != null)
                {
                    navwin = new NavigationWindow();
                    navwin.NavigationService.Navigate(pg);
                    navwin.Show();
                }

                // Descend through logical tree, adding items to TreeView
                MyLogicalTreeHelper.DumpLogicalTreeToTreeView(root, tvLogical.Items);
                MyVisualTreeHelper.DumpVisualTreeToTreeView(root, tvVisual.Items);

                // If root object is Window, we need to call Close method to avoid process from 
                //   running indefinitely.  ??
                if (win != null)
                    win.Close();

                if (pg != null)
                    navwin.Close();
            }

            e.Handled = true;
        }

        /// <summary>
        /// Given filename, use XamlReader to construct XAML objects by reading the file
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        private static object LoadXAMLObjectGraphFromFile(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return System.Windows.Markup.XamlReader.Load(fs);
            }
        }

        private void InitializeTreeviews()
        {
            TreeViewItem tvi = new TreeViewItem();
            tvi.Header = "Empty";
            tvLogical.Items.Add(tvi);

            TreeViewItem tvi2 = new TreeViewItem();
            tvi2.Header = "Empty";
            tvVisual.Items.Add(tvi2);
        }
    }
}
