using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                System.Windows.Forms.Application.EnableVisualStyles();

                Uri uri = new Uri("PresentationFramework.Aero;V3.0.0.0;31bf3856ad364e35;component\\themes/aero.normalcolor.xaml", UriKind.Relative);

                Resources.MergedDictionaries.Add(Application.LoadComponent(uri) as ResourceDictionary);
            }
            catch { }

        }

        /// <summary>
        /// Handles the DispatcherUnhandledException event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                TaskDialog.MessageBox(MovieCollection.Properties.Resources.TaskDialogUnhandledExceptionTitle, MovieCollection.Properties.Resources.TaskDialogUnhandledExceptionMain,
                    MovieCollection.Properties.Resources.TaskDialogUnhandledExceptionContent, e.Exception.ToString(), string.Empty, string.Empty,
                    TaskDialogButtons.OK, TaskDialogIcons.Error, TaskDialogIcons.Error);

                this.Shutdown(-1);
            }
            finally { e.Handled = true; }
        }
    }
}
