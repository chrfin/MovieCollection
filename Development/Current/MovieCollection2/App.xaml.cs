using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">More than one instance of the <see cref="T:System.Windows.Application"/> class is created per <see cref="T:System.AppDomain"/>.</exception>
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            Startup += new StartupEventHandler(App_Startup);
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Startup event of the App control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.StartupEventArgs"/> instance containing the event data.</param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                System.Windows.Forms.Application.EnableVisualStyles();
            }
            catch (Exception exp) { ShowErrorDialog(exp); }
        }

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by ChrFin00, 2011-06-13</remarks>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowErrorDialog(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// Handles the DispatcherUnhandledException event of the App control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                ShowErrorDialog(e.Exception);
            }
            finally { e.Handled = true; }
        }

        /// <summary>
        /// Shows the error dialog.
        /// </summary>
        /// <param name="e">The e.</param>
        private void ShowErrorDialog(Exception e)
        {
            TaskDialog errorDialog = new TaskDialog();

            errorDialog.Cancelable = true;
            errorDialog.Icon = TaskDialogStandardIcon.Error;

            errorDialog.Caption = MovieCollection.Properties.Resources.TaskDialogUnhandledExceptionTitle;
            errorDialog.InstructionText = MovieCollection.Properties.Resources.TaskDialogUnhandledExceptionMain;
            errorDialog.Text = e.Message;

            errorDialog.DetailsExpanded = false;
            errorDialog.DetailsCollapsedLabel = MovieCollection.Properties.Resources.TaskDialogUnhandledExceptionDetailsLabelCollapsed;
            errorDialog.DetailsExpandedLabel = MovieCollection.Properties.Resources.TaskDialogUnhandledExceptionDetailsLabelExpanded;
            errorDialog.DetailsExpandedText = e.ToString();
            errorDialog.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter;
            errorDialog.StandardButtons = TaskDialogStandardButtons.Close;

            TaskDialogCommandLink buttonSend = new TaskDialogCommandLink("buttonSend", 
                MovieCollection.Properties.Resources.TaskDialogUnhandledExceptionSendButtonText,
                MovieCollection.Properties.Resources.TaskDialogUnhandledExceptionSendButtonInstructions);
            buttonSend.Click += new EventHandler(delegate(object sender, EventArgs eventArgs)
            {
                errorDialog.Close();//ToDo: Send error message!
            });
            errorDialog.Controls.Add(buttonSend);

            errorDialog.Show();
        }

        /// <summary>
        /// Application entry point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            MovieCollection.App app = new App();
            MainWindow window = new MainWindow();
            app.Run(window);
        }
    }
}
