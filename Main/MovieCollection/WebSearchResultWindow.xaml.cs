using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebSource;
using System.Threading;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for WebSearchResultWindow.xaml
    /// </summary>
    public partial class WebSearchResultWindow : Window
    {
        /// <summary>
        /// Gets or sets the selected details.
        /// </summary>
        /// <value>The selected details.</value>
        public IWebMovieDetails SelectedDetails
        {
            get { return (IWebMovieDetails)GetValue(SelectedDetailsProperty); }
            set { SetValue(SelectedDetailsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected details (DependencyProperty).
        /// </summary>
        public static readonly DependencyProperty SelectedDetailsProperty = DependencyProperty.Register("SelectedDetails", typeof(IWebMovieDetails), typeof(WebSearchResultWindow));

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSearchResultWindow"/> class.
        /// </summary>
        /// <param name="results">The results.</param>
        public WebSearchResultWindow(List<IWebSearchResult> results)
        {
            InitializeComponent();

            checkBoxAutoSelect.IsChecked = Properties.Settings.Default.AutoSelectIfOneResult;

            listBoxResults.ItemsSource = results;
            listBoxResults.SelectedItem = results.First();
        }

        /// <summary>
        /// Handles the Loaded event of the webSearchResultWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void webSearchResultWindow_Loaded(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// Handles the SelectionChanged event of the listBoxResults control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void listBoxResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IWebSearchResult searchResult = (listBoxResults.SelectedItem as IWebSearchResult);

            StatusWindow statusWindow = new StatusWindow();
            try
            {
                statusWindow.Owner = this;
            }
            catch { }
            statusWindow.Line1 = string.Format(Properties.Resources.StatusSearchingOnline, searchResult.Title);
            statusWindow.Line2 = string.Format(Properties.Resources.StatusSearchingContent, searchResult.Title);

            Thread searchThread = new Thread(new ThreadStart(delegate
                {
                    try
                    {
                        Dispatcher.Invoke((Action)delegate { IsEnabled = false; statusWindow.Show(); });
                        IWebMovieDetails details = searchResult.LoadDetails();
                        Dispatcher.Invoke((Action)delegate { SelectedDetails = details; });
                    }
                    catch (Exception exp)
                    {
                        TaskDialog.MessageBox(Properties.Resources.TaskDialogOnlineSearchExceptionTitle, string.Format(Properties.Resources.TaskDialogOnlineSearchExceptionMain, searchResult.Title),
                            Properties.Resources.TaskDialogOnlineSearchExceptionText, exp.ToString(), string.Empty, string.Empty, TaskDialogButtons.OK, TaskDialogIcons.Error, TaskDialogIcons.Error);
                    }
                    finally { Dispatcher.Invoke((Action)delegate { statusWindow.Close(); IsEnabled = true; }); }
                }));
            searchThread.Name = "Searching Online";
            searchThread.IsBackground = true;
            searchThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            searchThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            searchThread.Start();
        }

        /// <summary>
        /// Handles the Click event of the buttonOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Handles the Click event of the buttonCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Handles the Checked event of the checkBoxAutoSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxAutoSelect_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AutoSelectIfOneResult = checkBoxAutoSelect.IsChecked.Value;
            Properties.Settings.Default.Save();
        }
    }
}
