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
using MovieDataSource;
using System.ComponentModel;
using System.IO;
using MediaInfoNET;
using System.Globalization;
using System.Diagnostics;
using RootLibrary.WPF.Localization;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for MovieControl.xaml
    /// </summary>
    public partial class MovieControl : UserControl
    {
        /// <summary>
        /// Gets or sets the movie.
        /// </summary>
        /// <value>The movie.</value>
        public IMovie Movie
        {
            get { return (IMovie)GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }
        /// <summary>
        /// Gets or sets the movie(DependencyProperty).
        /// </summary>
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(IMovie), typeof(MovieControl));

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public IUserProfile User
        {
            get { return (IUserProfile)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        /// <summary>
        /// Gets or sets the user (DependencyProperty).
        /// </summary>
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(IUserProfile), typeof(MovieControl));

        /// <summary>
        /// Gets or sets the user settings.
        /// </summary>
        /// <value>The user settings.</value>
        public IUserMovieSettings UserSettings
        {
            get { return (IUserMovieSettings)GetValue(UserSettingsProperty); }
            set { SetValue(UserSettingsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the user settings (DependencyProperty).
        /// </summary>
        public static readonly DependencyProperty UserSettingsProperty = DependencyProperty.Register("UserSettings", typeof(IUserMovieSettings), typeof(MovieControl));

        /// <summary>
        /// Occurs when "search online" is pressed.
        /// </summary>
        public event EventHandler SearchOnline;
        /// <summary>
        /// Raises the <see cref="E:SearchOnline"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSearchOnline(EventArgs e)
        {
            if (SearchOnline != null)
                SearchOnline(this, e);
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public IMovieDataSourcePlugin DataSource { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieControl"/> class.
        /// </summary>
        public MovieControl()
        {
            InitializeComponent();

            UpdateCastHeader();
            UpdateGenreHeader();

            DependencyPropertyDescriptor movieProp = DependencyPropertyDescriptor.FromProperty(MovieControl.MovieProperty, typeof(MovieControl));
            movieProp.AddValueChanged(this, delegate { UpdateDetails(); });

            DependencyPropertyDescriptor userProp = DependencyPropertyDescriptor.FromProperty(MovieControl.UserProperty, typeof(MovieControl));
            userProp.AddValueChanged(this, delegate { UpdateDetails(); });
        }

        private bool updating = false;
        /// <summary>
        /// Updates the details.
        /// </summary>
        public void UpdateDetails()
        {
            updating = true;

            if (Movie == null)
                IsEnabled = false;
            else
                IsEnabled = true;

            if (User == null)
                groupBoxUserSettings.Visibility = Visibility.Collapsed;
            else
                groupBoxUserSettings.Visibility = Visibility.Visible;

            UpdateCastHeader();
            UpdateGenreHeader();

            if (Movie != null)
            {
                if (User != null)
                {
                    if (User.MovieSettings.ToList().Find(s => s.Movie == Movie) == null)
                        User.MovieSettings.Add(DataSource.CreateMovieSettings(Movie));

                    UserSettings = User.MovieSettings.ToList().Find(s => s.Movie == Movie);
                }

                if (Movie.MediaFile != null)
                    textBoxMediaFile.Text = Movie.MediaFile.Path;
                else
                    textBoxMediaFile.Text = string.Empty;
            }

            updating = false;
        }

        /// <summary>
        /// Updates the localisation.
        /// </summary>
        public void UpdateLocalisation()
        {
            if (Movie != null)
            {
                UpdateCastHeader();
                UpdateGenreHeader();
                UpdateMediaHeader();

                listBoxAudio.ItemsSource = null;
                Binding audioBinding = new Binding();
                audioBinding.Source = this;
                audioBinding.Path = new PropertyPath("Movie.MediaFile.Audio");
                listBoxAudio.SetBinding(ListBox.ItemsSourceProperty, audioBinding);
            }
        }

        /// <summary>
        /// Begins to edit the title.
        /// </summary>
        public void BeginEditTitle()
        {
            textBoxTitle.Focus();
            textBoxTitle.SelectionStart = 0;
            textBoxTitle.SelectionLength = textBoxTitle.Text.Length;
        }

        /// <summary>
        /// Handles the Click event of the buttonCover control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonCover_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = Properties.Resources.ImagesFilterText + "|" + Properties.Resources.ImagesFilter;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Image img = System.Drawing.Bitmap.FromFile(ofd.FileName);
                Movie.Cover = img;
            }
        }

        /// <summary>
        /// Handles the Click event of the buttonWebSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonWebSearch_Click(object sender, RoutedEventArgs e)
        {
            OnSearchOnline(EventArgs.Empty);
        }

        /// <summary>
        /// Handles the KeyUp event of the textBoxTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void textBoxTitle_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBoxTitle.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                OnSearchOnline(EventArgs.Empty);
            }
        }

        private static bool isMediaOffline = false;
        public bool IsMediaOffline { get { return isMediaOffline; } set { isMediaOffline = value; groupBoxMediaFile.IsEnabled = !isMediaOffline; } }
        /// <summary>
        /// Handles the TextChanged event of the textBoxMediaFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void textBoxMediaFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsMediaOffline)
            {
                if (textBoxMediaFile.Text.Length <= 0 || (!File.Exists(textBoxMediaFile.Text) && !Directory.Exists(textBoxMediaFile.Text)))
                {
                    if (textBoxMediaFile.Text.Length > 0 && !textBoxMediaFile.IsFocused)
                    {
                        TaskDialog.ShowCommandBox(Properties.Resources.TaskDialogMediaOfflineTitle, Properties.Resources.TaskDialogMediaOfflineMain,
                            Properties.Resources.TaskDialogMediaOfflineText, Properties.Resources.TaskDialogMediaOfflineButtons, false);

                        if (TaskDialog.CommandButtonResult == 0)
                            IsMediaOffline = true;
                        else
                            Movie.MediaFile = null;
                    }
                    else
                        Movie.MediaFile = null;

                    groupBoxMediaFile.Header = new TextBlock()
                    {
                        Text = Properties.Resources.GroupBoxMediaFile,
                        FontWeight = FontWeights.Bold
                    };
                }
                else
                {
                    if (Movie.MediaFile == null || Movie.MediaFile.Path != textBoxMediaFile.Text)
                    {
                        Methods.LoadMediaFileIntoMovie(Movie, textBoxMediaFile.Text, DataSource);
                    }
                }
            }
            else
                groupBoxMediaFile.Header = new TextBlock()
                {
                    Text = Properties.Resources.GroupBoxMediaFile,
                    FontWeight = FontWeights.Bold
                };

            stackPanelLogos.Children.Clear();

            if (Movie.MediaFile != null)
            {
                UpdateMediaHeader();

                if (Movie.MediaFile.Video != null)
                {
                    if (Movie.MediaFile.Video.Width >= 1920)
                        stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.FullHD));
                    else if (Movie.MediaFile.Video.Width >= 1280)
                        stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.HD));

                    if (Movie.MediaFile.Video.Encoding.ToLower() == "x264")
                        stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.x264));
                    else if (Movie.MediaFile.Video.Encoding.ToLower().Replace(".", string.Empty) == "h264")
                        stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.h264));
                    else if (Movie.MediaFile.Video.Encoding.ToLower() == "xvid")
                        stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.xvid));
                    else if (Movie.MediaFile.Video.Encoding.ToLower() == "divx")
                        stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.xvid));
                    else if (Movie.MediaFile.Video.Format.ToLower().Contains("mpeg"))
                        stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.mpeg));

                    if (Movie.MediaFile.Path.ToLower().EndsWith("wmv"))
                    {
                        if (Movie.MediaFile.Video.Width >= 1280)
                            stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.wmvhd));
                        else
                            stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.wmv));
                    }
                }

                if (Movie.MediaFile.Path.ToLower().EndsWith("mkv"))
                    stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.MKV));

                if (Movie.MediaFile.Audio.ToList().Find(a => a.Format == "DTS") != null)
                    stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.DTS));
                if (Movie.MediaFile.Audio.ToList().Find(a => a.Format == "AC-3") != null)
                    stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.dolby_digital));
                if (Movie.MediaFile.Audio.ToList().Find(a => a.Encoding == "MP3") != null)
                    stackPanelLogos.Children.Add(GetLogoImage(Properties.Resources.mp3));
            }
        }
        /// <summary>
        /// Gets the logo image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        private System.Windows.Controls.Image GetLogoImage(System.Drawing.Image image)
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Source = Methods.GetImageSourceFromImage(image);
            //img.Width = 48;
            img.MaxWidth = 75;
            img.Height = 48;
            img.Margin = new Thickness(5);
            img.Stretch = Stretch.Uniform;

            return img;
        }

        /// <summary>
        /// Handles the Click event of the buttonBrowseMediaFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonBrowseMediaFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = Properties.Resources.MediaFilterText + "|" + Properties.Resources.MediaFilter;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                textBoxMediaFile.Text = ofd.FileName;
        }

        /// <summary>
        /// Handles the Click event of the buttonPlay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(textBoxMediaFile.Text) || Directory.Exists(textBoxMediaFile.Text))
                Process.Start(textBoxMediaFile.Text);
        }

        /// <summary>
        /// Handles the SelectionChanged event of the listBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void listBoxAudio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBoxAudio.SelectedItem = null;
        }

        /// <summary>
        /// Handles the SelectionChanged event of the listBoxGenre control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void listBoxGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonDeleteGenre.IsEnabled = listBoxGenre.SelectedItem != null;
        }

        /// <summary>
        /// Handles the SelectionChanged event of the listBoxCast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void listBoxCast_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonDeleteCast.IsEnabled = listBoxCast.SelectedItem != null;
        }

        /// <summary>
        /// Handles the Click event of the buttonAddGenre control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonAddGenre_Click(object sender, RoutedEventArgs e)
        {
            if (Movie == null) return;

            InputBoxResult res = InputBox.Show(Properties.Resources.InputBoxAddGenrePromt, Properties.Resources.InputBoxAddGenreTitle, Properties.Resources.InputBoxAddGenreDefault);

            if (res.ReturnCode == System.Windows.Forms.DialogResult.OK && res.Text.Length > 0)
                Movie.Genres.Add(DataSource.CreateGenre(res.Text));
        }

        /// <summary>
        /// Handles the Click event of the buttonDeleteGenre control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonDeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxGenre.SelectedItem == null || Movie == null)
                return;

            Movie.Genres.Remove(listBoxGenre.SelectedItem as IGenre);
        }

        /// <summary>
        /// Handles the Click event of the buttonAddCast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonAddCast_Click(object sender, RoutedEventArgs e)
        {
            if (Movie == null) return;

            InputBoxResult res = InputBox.Show(Properties.Resources.InputBoxAddCastPromtPerson, Properties.Resources.InputBoxAddCastTitlePerson, Properties.Resources.InputBoxAddCastDefaultPerson);

            if (res.ReturnCode == System.Windows.Forms.DialogResult.OK && res.Text.Length > 0)
            {
                string name = res.Text;
                InputBoxResult res2 = InputBox.Show(Properties.Resources.InputBoxAddCastPromtRole, Properties.Resources.InputBoxAddCastTitleRole, Properties.Resources.InputBoxAddCastDefaultRole);
                IPerson pers = DataSource.CreatePerson(name);
                Movie.Cast.Add(pers);
                pers.Role = res2.Text;
            }
        }

        /// <summary>
        /// Handles the Click event of the buttonDeleteCast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonDeleteCast_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxCast.SelectedItem == null || Movie == null)
                return;

            Movie.Cast.Remove(listBoxCast.SelectedItem as IPerson);
        }

        /// <summary>
        /// Handles the Expanded event of the expanderCast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void expanderCast_Expanded(object sender, RoutedEventArgs e)
        {
            UpdateCastHeader();
        }

        /// <summary>
        /// Handles the Collapsed event of the expanderCast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void expanderCast_Collapsed(object sender, RoutedEventArgs e)
        {
            UpdateCastHeader();
        }

        /// <summary>
        /// Updates the cast header.
        /// </summary>
        private void UpdateCastHeader()
        {
            if (expanderCast.IsExpanded || Movie == null)
            {
                expanderCast.Header = new TextBlock()
                    {
                        Text = LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources", "ExpanderCast", LocalizeDictionary.Instance.Culture),
                        FontWeight = FontWeights.Bold
                    };
            }
            else
            {
                ListToStringConverter converter = new ListToStringConverter();

                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;
                stack.Children.Add(new TextBlock()
                    {
                        Text = LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources", "ExpanderCast", LocalizeDictionary.Instance.Culture) + ": ",
                        FontWeight = FontWeights.Bold
                    });
                stack.Children.Add(new TextBlock()
                    {
                        Text = converter.Convert(Movie.Cast, typeof(string), null, LocalizeDictionary.Instance.Culture).ToString()
                    });
                expanderCast.Header = stack;
            }
        }

        /// <summary>
        /// Handles the Expanded event of the expanderGenre control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void expanderGenre_Expanded(object sender, RoutedEventArgs e)
        {
            UpdateGenreHeader();
        }

        /// <summary>
        /// Handles the Collapsed event of the expanderGenre control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void expanderGenre_Collapsed(object sender, RoutedEventArgs e)
        {
            UpdateGenreHeader();
        }

        /// <summary>
        /// Updates the genre header.
        /// </summary>
        private void UpdateGenreHeader()
        {
            if (expanderGenre.IsExpanded || Movie == null)
            {
                expanderGenre.Header = new TextBlock()
                    {
                        Text = LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources", "ExpanderGenre", LocalizeDictionary.Instance.Culture),
                        FontWeight = FontWeights.Bold
                    };
            }
            else
            {
                ListToStringConverter converter = new ListToStringConverter();

                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;
                stack.Children.Add(new TextBlock()
                {
                    Text = LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources", "ExpanderGenre", LocalizeDictionary.Instance.Culture) + ": ",
                    FontWeight = FontWeights.Bold
                });
                stack.Children.Add(new TextBlock()
                {
                    Text = converter.Convert(Movie.Genres, typeof(string), null, LocalizeDictionary.Instance.Culture).ToString()
                });
                expanderGenre.Header = stack;
            }
        }

        /// <summary>
        /// Updates the media header.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void UpdateMediaHeader()
        {
            groupBoxMediaFile.Header = new TextBlock()
            {
                Text = Properties.Resources.GroupBoxMediaFile,
                FontWeight = FontWeights.Bold
            };

            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Movie.MediaFile.Size");
            binding.Converter = new MediaFileToMediaFileSizeText();
            (groupBoxMediaFile.Header as TextBlock).SetBinding(TextBlock.TextProperty, binding);
        }

        /// <summary>
        /// Handles the MouseRightButtonUp event of the buttonCover control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void buttonCover_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Movie != null)
                Movie.Cover = null;
        }

        /// <summary>
        /// Handles the LostFocus event of the textBoxDirectors control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void textBoxDirectors_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Movie == null)
                return;

            string[] directors = textBoxDirectors.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            while (Movie.Directors.Count > 0) Movie.Directors.RemoveAt(0);
            directors.ToList().ForEach(d => Movie.Directors.Add(DataSource.CreatePerson(d.Trim())));

            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Movie.Directors");
            binding.Converter = new ListToStringConverter();
            binding.Mode = BindingMode.OneWay;
            textBoxDirectors.SetBinding(TextBox.TextProperty, binding);
        }
    }
}
