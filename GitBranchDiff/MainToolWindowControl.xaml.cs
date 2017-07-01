//------------------------------------------------------------------------------
// <copyright file="MainToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace GitBranchDiff
{
    using Model;
    using System.Diagnostics.CodeAnalysis;
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

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "MainToolWindow");
        }

        private void OnItemMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var node = (TreeViewItem)sender;
            if (node.DataContext.GetType() == typeof(FileItem))
            {
                var file = (FileItem)node.DataContext;
                var model = (MainToolWindowViewModel)DataContext;
                model.OpenFile(file.Path);
            }
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            var model = (MainToolWindowViewModel)DataContext;
            model.Reload();
        }
    }
}