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
using RootLibrary.WPF.Localization;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for StatusWindow.xaml
    /// </summary>
    public partial class StatusWindow : Window
    {
        /// <summary>
        /// Gets or sets the line1.
        /// </summary>
        /// <value>The line1.</value>
        public string Line1
        {
            get { return (string)GetValue(Line1Property); }
            set { SetValue(Line1Property, value); }
        }
        /// <summary>
        /// Gets or sets the line 1 (DependencyProperty).
        /// </summary>
        public static readonly DependencyProperty Line1Property = DependencyProperty.Register("Line1", typeof(string), typeof(StatusWindow));

        /// <summary>
        /// Gets or sets the line 2.
        /// </summary>
        /// <value>The line 2.</value>
        public string Line2
        {
            get { return (string)GetValue(Line2Property); }
            set { SetValue(Line2Property, value); }
        }
        /// <summary>
        /// Gets or sets the line 2 (DependencyProperty).
        /// </summary>
        public static readonly DependencyProperty Line2Property = DependencyProperty.Register("Line2", typeof(string), typeof(StatusWindow));

        private StatusMode mode;
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public StatusMode Mode
        {
            get { return mode; }
            set
            {
                if (value == mode)
                    return;

                mode = value;
                Title = LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources",
                    mode == StatusMode.Searching ? "StatusWindowTitleSearching" : mode == StatusMode.Importing ? "StatusWindowTitleImporting" : "StatusWindowTitleOpening",
                    LocalizeDictionary.Instance.Culture);
                labelMode.Text = LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources",
                    mode == StatusMode.Searching ? "LabelSearching" : mode == StatusMode.Importing ? "LabelImporting" : "LabelOpening", LocalizeDictionary.Instance.Culture);

                progressBarProgress.IsIndeterminate = (mode != StatusMode.Importing);
            }
        }

        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>The progress.</value>
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        /// <summary>
        /// Gets or sets the progress (DependencyProperty).
        /// </summary>
        /// <value>The progress.</value>
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(StatusWindow));

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusWindow"/> class.
        /// </summary>
        public StatusWindow()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// Mode of the Status Display.
    /// </summary>
    public enum StatusMode
    {
        /// <summary>
        /// Searching (like searching a websorce).
        /// </summary>
        Searching,
        /// <summary>
        /// Importing (like importing an folder).
        /// </summary>
        Importing,
        /// <summary>
        /// Opening a data source
        /// </summary>
        Opening
    }
}
