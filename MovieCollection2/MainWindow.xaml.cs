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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Windows.Controls.Ribbon;
using Microsoft.WindowsAPICodePack.Dialogs;
using MovieCollection.Properties;
using System.Globalization;
using System.Threading;
using RootLibrary.WPF.Localization;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            PrepareLocalisation();
        }

        /// <summary>
        /// Prepares the localisation.
        /// </summary>
        private void PrepareLocalisation()
        {
            foreach (string lang in Settings.Default.AvailableLanguages.Split(';'))
            {
                CultureInfo culture = new CultureInfo(lang);
                RibbonGalleryItem item = new RibbonGalleryItem();
                item.Content = culture.Parent.NativeName + " (" + culture.Parent.EnglishName + ")";
                item.Tag = culture;
                item.Selected += new RoutedEventHandler(language_Selected);
                ribbonGalleryCategoryLanguages.Items.Add(item);

                if (culture.TwoLetterISOLanguageName == Settings.Default.SelectedLanguage)
                    SetUICulture(culture);
            }
        }

        /// <summary>
        /// Handles the Selected event of the language control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void language_Selected(object sender, RoutedEventArgs e)
        {
            SetUICulture((sender as RibbonGalleryItem).Tag as CultureInfo);
        }

        /// <summary>
        /// Sets the UI culture.
        /// </summary>
        /// <param name="culture">The culture.</param>
        private void SetUICulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            LocalizeDictionary.Instance.Culture = culture;

            foreach (RibbonGalleryItem item in ribbonGalleryCategoryLanguages.Items)
            {
                if (item.Tag == culture)
                    item.IsSelected = true;
            }

            Settings.Default.SelectedLanguage = culture.TwoLetterISOLanguageName;
            Settings.Default.Save();
        }

        private void buttonException_Click(object sender, RoutedEventArgs e)
        {
            throw new Exception("bla");
        }

        /// <summary>
        /// Handles the Click event of the buttonExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
