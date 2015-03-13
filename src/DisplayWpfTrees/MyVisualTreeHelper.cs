using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace DisplayWpfTrees
{
    public class MyVisualTreeHelper
    {
        /// <summary>
        /// Add children of specified parent to TreeView, under specified node
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="lvi"></param>
        public static void DumpVisualTreeToTreeView(object parent, ItemCollection tvItems)
        {
            TreeViewItem tvi = new TreeViewItem();
            tvi.Header = XamlElementAsString(parent);
            tvItems.Add(tvi);
            DependencyObject dobj = parent as DependencyObject;
            int test = VisualTreeHelper.GetChildrenCount(dobj);

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent as DependencyObject); i++)
            {
                DumpVisualTreeToTreeView(VisualTreeHelper.GetChild(parent as DependencyObject, i), tvi.Items);
            }
        }

        /// <summary>
        /// Render a XAML element as a string that contains full type name and optional
        ///   element Name.
        /// </summary>
        /// <param name="theElement"></param>
        /// <returns></returns>
        private static string XamlElementAsString(object theElement)
        {
            string asString;

            // Simple elements (e.g. text content), just convert to string
            if (!(theElement is DependencyObject))
                asString = theElement.ToString();

            else
            {
                // Get full type name
                Type objType = theElement.GetType();
                asString = objType.ToString();

                // Optionally, if the element has a name, append it
                PropertyInfo pi = objType.GetProperty("Name");
                if (pi != null)
                {
                    string name = (string)pi.GetValue(theElement, null);
                    if ((name != null) && (name != ""))
                        asString = string.Format("{0}: ({1})", asString, name);
                }
            }

            return asString;
        }
    }
}
