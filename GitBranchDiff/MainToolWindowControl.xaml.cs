//------------------------------------------------------------------------------
// <copyright file="MainToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace GitBranchDiff
{
    using Model;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainToolWindowControl.
    /// </summary>
    public partial class MainToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainToolWindowControl"/> class.
        /// </summary>
        public MainToolWindowControl()
        {
            this.InitializeComponent();
        }

        private void OnItemMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var node = (TreeViewItem)sender;
            if (node.DataContext.GetType() == typeof(FileItem))
            {
                var file = (FileItem)node.DataContext;
                var model = (MainToolWindowViewModel)DataContext;
                model.OpenFile(file);
            }
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            var model = (MainToolWindowViewModel)DataContext;
            model.Reload();
        }

        private void Branch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = (MainToolWindowViewModel)DataContext;
            model.Reload(false);
        }
    }
}