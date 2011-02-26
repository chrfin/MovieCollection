using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MediaInfoNET;
using MovieCollection.Properties;
using MovieDataSource;
using RootLibrary.WPF.Localization;
using WebSource;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region UICommands
        public readonly static RoutedUICommand SourceLoadedCommand = new RoutedUICommand("SourceLoaded", "SourceLoaded", typeof(MainWindow));
        private void CanExecuteSourceLoadedCommand(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = dataSource != null; }
        #endregion

        /// <summary>
        /// Gets or sets the current movie.
        /// </summary>
        /// <value>The current movie.</value>
        public IMovie CurrentMovie
        {
            get { return (IMovie)GetValue(CurrentMovieProperty); }
            set { SetValue(CurrentMovieProperty, value); }
        }
        /// <summary>
        /// Gets or sets the current movie (DependencyProperty).
        /// </summary>
        public static readonly DependencyProperty CurrentMovieProperty = DependencyProperty.Register("CurrentMovie", typeof(IMovie), typeof(MainWindow));

        /// <summary>
        /// Gets or sets the current user settings.
        /// </summary>
        /// <value>The current user settings.</value>
        public IUserMovieSettings CurrentUserSettings
        {
            get { return (IUserMovieSettings)GetValue(CurrentUserSettingsProperty); }
            set { SetValue(CurrentUserSettingsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the current user settings (DependencyProperty).
        /// </summary>
        public static readonly DependencyProperty CurrentUserSettingsProperty = DependencyProperty.Register("CurrentUserSettings", typeof(IUserMovieSettings), typeof(MainWindow));

        private List<IWebSourcePlugin> webSourcePlugins = new List<IWebSourcePlugin>();
        private List<IMovieDataSourceFactory> dataSourceFactories = new List<IMovieDataSourceFactory>();

        private IMovieDataSourcePlugin dataSource;
        private IUserProfile currentUser;
        internal IUserProfile User { get { return currentUser; } }
        private IWebSourcePlugin currentWebPlugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            TaskDialog.ForceEmulationMode = true;

            InitializeComponent();

            Height = Settings.Default.MainWindowHeight;
            Width = Settings.Default.MainWindowWidth;

            PrepareLocalisation();
            InitializeListViewView();

            LoadPlugins<IWebSourcePlugin>(Path.Combine(System.Windows.Forms.Application.StartupPath, @"Plugins\WebSources"),
                webSourcePlugins, typeof(WebSourcePlugin));
            webSourcePlugins.ForEach(delegate(IWebSourcePlugin plugin)
                                        {
                                            MenuItem pluginItem = new MenuItem();
                                            pluginItem.Header = plugin.Name;

                                            Stream fileStream = new MemoryStream();
                                            plugin.Icon.Save(fileStream, ImageFormat.Png);
                                            if (fileStream != null)
                                            {
                                                PngBitmapDecoder bitmapDecoder = new PngBitmapDecoder(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                                                ImageSource imageSource = bitmapDecoder.Frames[0];
                                                pluginItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 22, Height = 22 };
                                            }

                                            pluginItem.Tag = plugin;
                                            pluginItem.IsCheckable = true;
                                            if (plugin.Name == Settings.Default.SelectedWebSource)
                                            {
                                                pluginItem.IsChecked = true;
                                                currentWebPlugin = plugin;
                                            }
                                            pluginItem.Click += new RoutedEventHandler(webSourcePluginItem_Click);
                                            menuItemPlugins.Items.Add(pluginItem);
                                        });

            LoadPlugins<IMovieDataSourceFactory>(Path.Combine(System.Windows.Forms.Application.StartupPath, @"Plugins\DataSources"),
                dataSourceFactories, typeof(DataSourcePlugin));
            dataSourceFactories.ForEach(delegate(IMovieDataSourceFactory factory)
                                        {
                                            MenuItem pluginItem = new MenuItem();
                                            pluginItem.Header = factory.Name;

                                            Stream fileStream = new MemoryStream();
                                            factory.Icon.Save(fileStream, ImageFormat.Png);
                                            if (fileStream != null)
                                            {
                                                PngBitmapDecoder bitmapDecoder = new PngBitmapDecoder(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                                                ImageSource imageSource = bitmapDecoder.Frames[0];
                                                pluginItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 22, Height = 22 };
                                            }

                                            pluginItem.Tag = factory;
                                            pluginItem.Click += new RoutedEventHandler(dataSourcePluginItem_Click);
                                            menuItemNew.Items.Add(pluginItem);
                                        });
        }
        /// <summary>
        /// Handles the Loaded event of the mainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                try
                {
                    statusBarItemInfo.Text = args[1];
                    IMovieDataSourceFactory factory = dataSourceFactories.Find(f => f.Extension == Path.GetExtension(args[1]));
                    if (factory != null)
                        OpenFileSource(args[1], factory);
                }
                catch (Exception exp) { Trace.WriteLine(exp.ToString()); }
            }
            else if (File.Exists(Settings.Default.LastFile))
                OpenFileSource(Settings.Default.LastFile, dataSourceFactories.Find(f => f.Extension == Path.GetExtension(Settings.Default.LastFile)));
        }
        /// <summary>
        /// Handles the Closed event of the mainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void mainWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(-1);
        }

        /// <summary>
        /// Handles the Click event of the webSourcePluginItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void webSourcePluginItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (MenuItem item in menuItemPlugins.Items)
                if (item != sender)
                    item.IsChecked = false;

            currentWebPlugin = (sender as MenuItem).Tag as IWebSourcePlugin;
            Settings.Default.SelectedWebSource = currentWebPlugin.Name;
            Settings.Default.Save();
        }

        /// <summary>
        /// Prepares the localisation.
        /// </summary>
        private void PrepareLocalisation()
        {
            foreach (string lang in Settings.Default.AvailableLanguages.Split(';'))
            {
                CultureInfo culture = new CultureInfo(lang);
                MenuItem item = new MenuItem();
                item.Header = culture.Parent.NativeName + " (" + culture.Parent.EnglishName + ")";
                item.Tag = culture;
                if (Properties.Resources.ResourceManager.GetObject("lang_icon", culture) != null)
                {
                    Stream fileStream = new MemoryStream();
                    (Properties.Resources.ResourceManager.GetObject("lang_icon", culture) as Bitmap).Save(fileStream, ImageFormat.Png);
                    if (fileStream != null)
                    {
                        PngBitmapDecoder bitmapDecoder = new PngBitmapDecoder(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        ImageSource imageSource = bitmapDecoder.Frames[0];
                        item.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 22, Height = 22 };
                    }
                }
                item.IsCheckable = true;
                item.Click += new RoutedEventHandler(languageItem_Click);
                menuItemLanguage.Items.Add(item);

                if (culture.TwoLetterISOLanguageName == Settings.Default.SelectedLanguage)
                    SetUICulture(culture);
            }
        }
        /// <summary>
        /// Handles the Click event of the languageItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void languageItem_Click(object sender, RoutedEventArgs e)
        {
            SetUICulture((sender as MenuItem).Tag as CultureInfo);
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
            movieControlMain.UpdateLocalisation();
            foreach (MenuItem item in menuItemLanguage.Items)
            {
                if (item.Tag == culture)
                    item.IsChecked = true;
                else
                    item.IsChecked = false;
            }
            Settings.Default.SelectedLanguage = culture.TwoLetterISOLanguageName;
            Settings.Default.Save();

            UpdateFilter();
        }

        /// <summary>
        /// Loads the plugins.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="pluginList">The plugin list.</param>
        /// <param name="attribute">The attribute to identify the plugin.</param>
        private void LoadPlugins<T>(string path, List<T> pluginList, Type attribute) where T : class
        {
            string[] plugins = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
            foreach (string pluginFile in plugins)
            {
                try
                {
                    Assembly assembly = null;
                    string typeName = string.Empty;
                    Type pluginType = null;

                    if (File.Exists(pluginFile))
                        assembly = Assembly.LoadFile(pluginFile);
                    else
                        continue;

                    if (assembly != null)
                    {
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (type.IsAbstract) continue;
                            if (type.IsDefined(attribute, true))
                            {
                                pluginType = type;
                                break;
                            }
                        }

                        if (pluginType != null)
                        {
                            T plugin = Activator.CreateInstance(pluginType) as T;
                            pluginList.Add(plugin);
                        }
                    }

                }
                catch (Exception e) { Trace.WriteLine(e.ToString()); }
            }
        }

        /// <summary>
        /// Handles the Click event of the dataSourcePluginItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void dataSourcePluginItem_Click(object sender, RoutedEventArgs e)
        {
            IMovieDataSourceFactory factory = (sender as MenuItem).Tag as IMovieDataSourceFactory;

            switch (factory.Type)
            {
                case DataSourceType.FileDataSource:
                    System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                    sfd.Filter = factory.Name + "|*" + factory.Extension;

                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        CloseDataSource();

                        if (File.Exists(sfd.FileName))
                            File.Delete(sfd.FileName);

                        OpenFileSource(sfd.FileName, factory);
                    }
                    break;
                case DataSourceType.DataBaseDataSource:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Handles the Click event of the menuItemOpen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            dataSourceFactories.FindAll(f => f.Type == DataSourceType.FileDataSource).ForEach(f => ofd.Filter += f.Name + "|*" + f.Extension);

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                OpenFileSource(ofd.FileName, dataSourceFactories.Find(f => f.Extension == Path.GetExtension(ofd.FileName)));
        }

        private string titleString = string.Empty;
        /// <summary>
        /// Opens the file source.
        /// </summary>
        /// <param name="filename">The filename.</param>
        private void OpenFileSource(string filename, IMovieDataSourceFactory factory)
        {
            CloseDataSource();
            movieControlMain.IsMediaOffline = false;

            if (titleString == string.Empty)
                titleString = Title.Replace("-", " ({0}) - ");

            Title = string.Format(titleString, filename);

            StatusWindow statusWindow = new StatusWindow();
            statusWindow.Owner = this;
            statusWindow.Mode = StatusMode.Opening;
            statusWindow.Line1 = string.Format(Properties.Resources.OpeningLine1, factory.Name);
            statusWindow.Line2 = string.Format(Properties.Resources.OpeningLine2, filename);

            IsEnabled = false;
            Thread openThread = new Thread(new ThreadStart(delegate
                {
                    try
                    {
                        Dispatcher.Invoke((Action)delegate { statusWindow.Show(); });

                        dataSource = factory.Create(filename);

                        Dispatcher.Invoke((Action)delegate
                        {
                            statusWindow.Close();

                            movieControlMain.IsMediaOffline = false;
                            movieControlMain.DataSource = dataSource;

                            listBoxMovies.ItemsSource = dataSource.Movies;

                            checkBoxFilter.IsChecked = false;
                            UpdateSorting();
                            UpdateFilterLists();

                            statusBarItemInfo.Text = string.Format(Properties.Resources.StatusBarInfoText, dataSource.Movies.Count);
                            UpdateFilter();

                            dataSource.Movies.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Movies_CollectionChanged);
                            dataSource.PropertyChanged += new PropertyChangedEventHandler(dataSource_PropertyChanged);
                            dataSource.Movies.ToList().ForEach(m => m.PropertyChanged += new PropertyChangedEventHandler(movie_PropertyChanged));

                            if (dataSource.Users.Count == 0)
                            {
                                IUserProfile newUser;
                                InputBoxResult res = InputBox.Show(Properties.Resources.AddUserText, Properties.Resources.AddUserHeader, Environment.UserName);
                                if (res.ReturnCode == System.Windows.Forms.DialogResult.OK)
                                    newUser = dataSource.CreateUser(res.Text);
                                else
                                    newUser = dataSource.CreateUser(Environment.UserName);

                                AddUserItem(newUser);
                                SelectUser(newUser);
                            }
                            else
                            {
                                dataSource.Users.ToList().ForEach(u => AddUserItem(u));
                                SelectUser(dataSource.Users.ToList().Exists(u => u.Name == Settings.Default.SelectedUser) ?
                                    dataSource.Users.ToList().Find(u => u.Name == Settings.Default.SelectedUser) : dataSource.Users[0]);
                            }

                            gridList.IsEnabled = true;
                            movieControlMain.IsEnabled = true;

                            if (dataSource.Movies.Count > 0)
                            {
                                listBoxMovies.SelectedIndex = listBoxMovies.Items.IndexOf(listBoxMovies.Items.GetItemAt(0));
                                movieControlMain.Movie = listBoxMovies.Items.GetItemAt(0) as IMovie;
                            }
                        });
                    }
                    finally { Dispatcher.Invoke((Action)delegate { IsEnabled = true; }); }
                }));
            openThread.Name = "Opening Thread";
            openThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            openThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            openThread.Start();
        }
        /// <summary>
        /// Closes the data source.
        /// </summary>
        private void CloseDataSource()
        {
            if (dataSource == null)
                return;

            dataSource.Close();
        }

        /// <summary>
        /// Handles the PropertyChanged event of the movie control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void movie_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSorting();
        }

        /// <summary>
        /// Handles the PropertyChanged event of the dataSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void dataSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSorting();
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Movies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Movies_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            checkBoxFilter.IsChecked = false;
            UpdateSorting();
            UpdateFilterLists();
        }

        /// <summary>
        /// Updates the sorting.
        /// </summary>
        private void UpdateSorting()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(dataSource.Movies);
            if (view != null)
                view.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
        }

        /// <summary>
        /// Handles the Click event of the menuItemExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the menuItemAddUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemAddUser_Click(object sender, RoutedEventArgs e)
        {
            IUserProfile newUser;
            InputBoxResult res = InputBox.Show(Properties.Resources.AddUserText, Properties.Resources.AddUserHeader, Environment.UserName);
            if (res.ReturnCode == System.Windows.Forms.DialogResult.OK)
            {
                if (dataSource.Users.ToList().Find(u => u.Name == res.Text) == null)
                {
                    newUser = dataSource.CreateUser(res.Text);
                    AddUserItem(newUser);
                    SelectUser(newUser);
                }
                else
                    TaskDialog.MessageBox(Properties.Resources.AddUserAlreadExistsTitle, Properties.Resources.AddUserAlreadExistsCaption, Properties.Resources.AddUserAlreadExistsText, TaskDialogButtons.OK, TaskDialogIcons.Error);
            }
        }

        /// <summary>
        /// Adds the user item.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private MenuItem AddUserItem(IUserProfile user)
        {
            MenuItem item = new MenuItem();
            item.Header = user.Name;
            item.IsCheckable = true;
            item.Tag = user;
            item.Click += new RoutedEventHandler(userItem_Click);
            menuItemUser.Items.Insert(menuItemUser.Items.Count > 1 ? menuItemUser.Items.Count - 1 : 0, item);

            return item;
        }

        /// <summary>
        /// Handles the Click event of the userItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void userItem_Click(object sender, RoutedEventArgs e)
        {
            SelectUser((sender as MenuItem).Tag as IUserProfile);
        }

        /// <summary>
        /// Selects the user.
        /// </summary>
        /// <param name="user">The user.</param>
        private void SelectUser(IUserProfile user)
        {
            foreach (MenuItem item in menuItemUser.Items)
            {
                if (item.Tag == user)
                    item.IsChecked = true;
                else
                    item.IsChecked = false;
            }

            currentUser = user;
            movieControlMain.User = user;

            statusBarItemUserName.Text = user.Name;

            List<IUserMovieSettings> settings = currentUser.MovieSettings.ToList();
            foreach (IMovie movie in dataSource.Movies)
            {
                if (!settings.Exists(s => s.Movie == movie))
                    currentUser.MovieSettings.Add(dataSource.CreateMovieSettings(movie));
            }

            settings.ForEach(s => s.PropertyChanged += new PropertyChangedEventHandler(userSettings_PropertyChanged));

            Settings.Default.SelectedUser = user.Name;
            Settings.Default.Save();
        }

        void userSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Seen")
                UpdateFilter();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the listBoxMovies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void listBoxMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object item in e.RemovedItems)
                (item as IMovie).PropertyChanged -= selectedMovie_PropertyChanged;
            foreach (object item in e.AddedItems)
                (item as IMovie).PropertyChanged += new PropertyChangedEventHandler(selectedMovie_PropertyChanged);

            listBoxMovies.ScrollIntoView(listBoxMovies.SelectedItem);
            buttonDeleteMovie.IsEnabled = listBoxMovies.SelectedItem != null;

            CurrentMovie = listBoxMovies.SelectedItem as IMovie;
            CurrentUserSettings = CurrentMovie == null ? null : currentUser.MovieSettings.ToList().Find(s => s.Movie == CurrentMovie);
        }

        /// <summary>
        /// Handles the PropertyChanged event of the selectedMovie control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void selectedMovie_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            listBoxMovies.ScrollIntoView(sender);
        }

        /// <summary>
        /// Handles the GotFocus event of the textBoxSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void textBoxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxSearch.Text == LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources", "TextBoxSearch", LocalizeDictionary.Instance.Culture))
            {
                textBoxSearch.Text = string.Empty;
                textBoxSearch.Foreground = Foreground;
            }
        }

        /// <summary>
        /// Handles the LostFocus event of the textBoxSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void textBoxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxSearch.Text == string.Empty)
            {
                textBoxSearch.Text = LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources", "TextBoxSearch", LocalizeDictionary.Instance.Culture);
                textBoxSearch.Foreground = System.Windows.Media.Brushes.LightGray;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the textBoxSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        /// <summary>
        /// Handles the Click event of the buttonDeleteMovie control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonDeleteMovie_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedMovies(false);
        }

        /// <summary>
        /// Deletes the selected movies.
        /// </summary>
        private void DeleteSelectedMovies(bool deleteMedia)
        {
            if (listBoxMovies.SelectedItems.Count <= 0)
                return;

            if (TaskDialog.MessageBox(Properties.Resources.DeleteMoviesTitle, Properties.Resources.DeleteMoviesCaption, string.Format(Properties.Resources.DeleteMoviesText, listBoxMovies.SelectedItems.Count), string.Empty,
                Properties.Resources.DeleteMoviesFooter, deleteMedia ? Properties.Resources.DeleteMoviesCheckBox : string.Empty,
                TaskDialogButtons.YesNo, TaskDialogIcons.Question, TaskDialogIcons.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                List<IMovie> moviesToRemove = new List<IMovie>(listBoxMovies.SelectedItems.Cast<IMovie>());
                int index = listBoxMovies.Items.IndexOf(moviesToRemove[0]) - 1;

                foreach (IMovie movie in moviesToRemove)
                {
                    if (deleteMedia && TaskDialog.VerificationChecked)
                    {
                        foreach (IMediaFile mFile in movie.MediaFiles)
                        {
                            try
                            {
                                if (File.Exists(mFile.Path))
                                    File.Delete(mFile.Path);
                                else if (Directory.Exists(mFile.Path))
                                    DeleteFolder(mFile.Path);
                            }
                            catch (Exception exp)
                            {
                                TaskDialog.MessageBox(Properties.Resources.DeleteMovieErrorTitle, Properties.Resources.DeleteMovieErrorMain, string.Format(Properties.Resources.DeleteMovieErrorText, mFile.Path),
                                    exp.ToString(), string.Empty, string.Empty, TaskDialogButtons.OK, TaskDialogIcons.Error, TaskDialogIcons.Error);
                            }
                        }
                    }

                    dataSource.Movies.Remove(movie);
                }

                listBoxMovies.SelectedIndex = index > 0 ? index : 0;
            }
        }

        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="path">The path.</param>
        private void DeleteFolder(string path)
        {
            foreach (string folder in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
                DeleteFolder(folder);
            foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                File.Delete(file);

            Directory.Delete(path);
        }

        /// <summary>
        /// Handles the Click event of the buttonAddMovie control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonAddMovie_Click(object sender, RoutedEventArgs e)
        {
            IMovie movie = dataSource.CreateMovie(Properties.Resources.NewMovieDefaultTitle);
            listBoxMovies.SelectedItem = movie;
            movieControlMain.BeginEditTitle();
        }

        /// <summary>
        /// Handles the SearchOnline event of the movieControlMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void movieControlMain_SearchOnline(object sender, EventArgs e)
        {
            SearchOnline();
        }

        /// <summary>
        /// Searches the online.
        /// </summary>
        private void SearchOnline()
        {
            if (string.IsNullOrEmpty(movieControlMain.Movie.Title))
                return;

            IWebSourcePlugin webPlugin = null;

            foreach (MenuItem item in menuItemPlugins.Items)
                if (item.IsChecked)
                    webPlugin = item.Tag as IWebSourcePlugin;

            selectedPlugin = webPlugin;
            SearchOnline(webPlugin);
        }

        private bool searching = false;
        private IWebSourcePlugin selectedPlugin = null;
        private List<IWebSourcePlugin> alternativeSearchPlugins = null;
        /// <summary>
        /// Searches the online.
        /// </summary>
        /// <param name="webPlugin">The web plugin.</param>
        private void SearchOnline(IWebSourcePlugin webPlugin)
        {
            if (webPlugin == null)
                return;

            StatusWindow statusWindow = new StatusWindow();
            statusWindow.Owner = this;
            statusWindow.Line1 = string.Format(Properties.Resources.StatusSearchingOnline, webPlugin.Name);
            statusWindow.Line2 = string.Format(Properties.Resources.StatusSearchingContent, movieControlMain.Movie.Title);
            string searchString = movieControlMain.Movie.Title;

            if (importing)
            {
                if (importingStatusWindow.WindowStartupLocation == WindowStartupLocation.CenterScreen)
                {
                    importingStatusWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                    importingStatusWindow.Top -= importingStatusWindow.Height / 2 - 5;
                }

                statusWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                statusWindow.Left = importingStatusWindow.Left;
                statusWindow.Top = importingStatusWindow.Top + importingStatusWindow.Height + 5;
            }

            StatusWindow statusWindowDetails = new StatusWindow();
            statusWindowDetails.Owner = this;

            searching = true;
            Thread searchThread = new Thread(new ThreadStart(delegate
                {
                    Dispatcher.Invoke((Action)delegate { IsEnabled = false; });

                    try
                    {
                        Dispatcher.Invoke((Action)delegate { statusWindow.Show(); });

                        List<IWebSearchResult> results = webPlugin.Search(searchString);

                        Dispatcher.Invoke((Action)delegate { statusWindow.Close(); });

                        if (results.Count > 0)
                        {
                            alternativeSearchPlugins = null;

                            if (results.Count == 1 && Properties.Settings.Default.AutoSelectIfOneResult || importing && autoSelectFirstResult)
                            {
                                Dispatcher.Invoke((Action)delegate
                                    {
                                        statusWindowDetails.Line1 = string.Format(Properties.Resources.StatusSearchingOnline, results[0].Title);
                                        statusWindowDetails.Line2 = string.Format(Properties.Resources.StatusSearchingContent, results[0].Title);
                                        if (importing)
                                        {
                                            statusWindowDetails.WindowStartupLocation = WindowStartupLocation.Manual;
                                            statusWindowDetails.Left = importingStatusWindow.Left;
                                            statusWindowDetails.Top = importingStatusWindow.Top + importingStatusWindow.Height + 5;
                                        }
                                        statusWindowDetails.Show();

                                        LoadResultIntoMovie(results[0].LoadDetails());
                                    });

                                Dispatcher.Invoke((Action)delegate { statusWindowDetails.Close(); });
                            }
                            else
                            {
                                Dispatcher.Invoke((Action)delegate
                                {
                                    WebSearchResultWindow resultsWindow = new WebSearchResultWindow(results);
                                    resultsWindow.Title = string.Format(Properties.Resources.WebSearchResultTitle, searchString);
                                    if (resultsWindow.ShowDialog().Value)
                                        LoadResultIntoMovie(resultsWindow.SelectedDetails);
                                });
                            }
                        }
                        else if (!(importing && autoSelectFirstResult))
                        {
                            if (webPlugin == selectedPlugin)
                                TaskDialog.MessageBox(Properties.Resources.TaskDialogNoOnlineResultsTitle, string.Format(Properties.Resources.TaskDialogNoOnlineResultsCaption, webPlugin.Name),
                                    Properties.Resources.TaskDialogNoOnlineResultsMain, string.Empty, Properties.Resources.TaskDialogNoOnlineResultsFooter, Properties.Resources.TaskDialogNoOnlineResultsCheckbox,
                                    TaskDialogButtons.OK, TaskDialogIcons.Error, TaskDialogIcons.Information);

                            if (webPlugin != selectedPlugin || TaskDialog.VerificationChecked)
                            {
                                if (alternativeSearchPlugins == null)
                                {
                                    IWebSourcePlugin[] array = new IWebSourcePlugin[webSourcePlugins.Count];
                                    webSourcePlugins.CopyTo(array);
                                    alternativeSearchPlugins = new List<IWebSourcePlugin>(array);
                                }

                                alternativeSearchPlugins.Remove(webPlugin);

                                if (alternativeSearchPlugins.Count <= 0)
                                {
                                    alternativeSearchPlugins = null;
                                    TaskDialog.MessageBox(Properties.Resources.TaskDialogNoOnlineResultsTitle, string.Format(Properties.Resources.TaskDialogNoOnlineResultsCaption,
                                        Properties.Resources.TaskDialogNoOnlineResultsAllSources), Properties.Resources.TaskDialogNoOnlineResultsMain, TaskDialogButtons.OK, TaskDialogIcons.Error);
                                }
                                else
                                    Dispatcher.Invoke((Action)delegate { SearchOnline(alternativeSearchPlugins[0]); });
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        TaskDialog.MessageBox(Properties.Resources.TaskDialogOnlineSearchExceptionTitle, string.Format(Properties.Resources.TaskDialogOnlineSearchExceptionMain, webPlugin.Name),
                            Properties.Resources.TaskDialogOnlineSearchExceptionText, exp.ToString(), string.Empty, string.Empty, TaskDialogButtons.OK, TaskDialogIcons.Error, TaskDialogIcons.Error);
                    }
                    finally
                    {
                        searching = false;
                        Dispatcher.Invoke((Action)delegate
                            {
                                if (!importing)
                                    IsEnabled = true;
                            });
                    }
                }));
            searchThread.Name = "Searching Online";
            searchThread.IsBackground = true;
            searchThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            searchThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            searchThread.Start();
        }

        protected delegate IMovie IMovieReturnDelegate();
        /// <summary>
        /// Loads the result into movie.
        /// </summary>
        /// <param name="details">The details.</param>
        private void LoadResultIntoMovie(IWebMovieDetails details)
        {
            IMovie movie = Dispatcher.Invoke((IMovieReturnDelegate)delegate { return movieControlMain.Movie; }) as IMovie;

            movie.URL = details.URL;
            movie.Title = details.Title;
            movie.OriginalTitle = details.OriginalTitle;
            movie.Country = details.Country;
            movie.Year = details.Year;

            Dispatcher.Invoke((Action)delegate
            {
                while (movie.Genres.Count > 0) movie.Genres.RemoveAt(0);
                details.Genres.ForEach(g => movie.Genres.Add(dataSource.CreateGenre(g.Title)));
            });

            if (details.ImageURL != string.Empty)
                movie.Cover = Bitmap.FromStream(new MemoryStream((new WebClient()).DownloadData(details.ImageURL)));

            Dispatcher.Invoke((Action)delegate
            {
                while (movie.Directors.Count > 0) movie.Directors.RemoveAt(0);
                if (details.Director != null && !String.IsNullOrEmpty(details.Director.Name))
                    movie.Directors.Add(dataSource.CreatePerson(details.Director.Name));
            });

            Dispatcher.Invoke((Action)delegate
            {
                while (movie.Cast.Count > 0) movie.Cast.RemoveAt(0);
                foreach (KeyValuePair<IWebPerson, string> pair in details.Cast)
                {
                    IPerson newPerson = dataSource.CreatePerson(pair.Key.Name);
                    movie.Cast.Add(newPerson);
                    newPerson.Role = pair.Value;
                }
            });

            movie.Plot = details.Plot;
            movie.Rating = details.Rating;

            Dispatcher.Invoke((Action)delegate { movieControlMain.UpdateDetails(); });
        }

        /// <summary>
        /// Handles the Closing event of the mainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void mainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                Settings.Default.MainWindowHeight = Height;
                Settings.Default.MainWindowWidth = Width;
                Settings.Default.LastFile = dataSource.Filename;
            }
            catch (Exception exp) { Trace.WriteLine(exp.ToString()); }
            finally { Settings.Default.Save(); }
        }

        /// <summary>
        /// Handles the Click event of the menuItemImportFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemImportFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = Properties.Resources.MediaFilterText + "|" + Properties.Resources.MediaFilter;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ofd.FileNames.ToList().ForEach(f => ImportFile(f));
        }

        /// <summary>
        /// Imports the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        private void ImportFile(string filename)
        {
            IMovie movie = dataSource.CreateMovie(Methods.GetTitleFromFilename(filename));

            listBoxMovies.SelectedItem = movie;
            Methods.LoadMediaFileIntoMovie(movie, filename, dataSource);
            SearchOnline();
            while (searching)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(10);
            }
        }

        private bool importing = false;
        private bool autoSelectFirstResult = false;
        private StatusWindow importingStatusWindow = new StatusWindow();
        private enum FolderImportMode
        {
            AsMovie,
            FirstLevel,
            SecondLevel,
            Ignore
        }
        /// <summary>
        /// Handles the Click event of the menuItemImportFolder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemImportFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TaskDialog.ShowCommandBox(Properties.Resources.TaskDialogImportSubFoldersTitle,
                    Properties.Resources.TaskDialogImportSubFoldersMain, Properties.Resources.TaskDialogImportSubFoldersText, Properties.Resources.TaskDialogImportSubFoldersButtons, false);

                FolderImportMode importSubFolders;
                switch (TaskDialog.CommandButtonResult)
                {
                    case 0:
                        importSubFolders = FolderImportMode.AsMovie;
                        break;
                    case 1:
                        importSubFolders = FolderImportMode.FirstLevel;
                        break;
                    case 2:
                        importSubFolders = FolderImportMode.SecondLevel;
                        break;
                    case 3:
                    default:
                        importSubFolders = FolderImportMode.Ignore;
                        break;
                }

                System.Windows.Forms.DialogResult res = TaskDialog.MessageBox(Properties.Resources.TaskDialogImportingAutoSelectResultTitle,
                    Properties.Resources.TaskDialogImportingAutoSelectResultMain, Properties.Resources.TaskDialogImportingAutoSelectResultText, TaskDialogButtons.YesNo, TaskDialogIcons.Question);

                autoSelectFirstResult = (res == System.Windows.Forms.DialogResult.Yes);

                importingStatusWindow = new StatusWindow();
                importingStatusWindow.Owner = this;
                string searchFolder = fbd.SelectedPath;

                Thread importThread = new Thread(new ThreadStart(delegate
                    {
                        importing = true;
                        string currentFile = string.Empty;

                        try
                        {
                            Dispatcher.Invoke((Action)delegate
                            {
                                IsEnabled = false;

                                importingStatusWindow.Line1 = string.Format(Properties.Resources.StatusSearchingFolder, searchFolder);
                                importingStatusWindow.Line2 = Properties.Resources.StatusSearchingFolderContent;
                                importingStatusWindow.Show();
                            });

                            List<string> files = new List<string>();
                            foreach (string extension in Properties.Resources.MediaFilter.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                foreach (string file in Directory.GetFiles(searchFolder, "*" + extension, SearchOption.TopDirectoryOnly))
                                {
                                    files.Add(file);
                                }
                            }

                            if (importSubFolders == FolderImportMode.AsMovie)
                            {
                                foreach (string folder in Directory.GetDirectories(searchFolder))
                                    files.Add(folder);
                            }
                            else if (importSubFolders != FolderImportMode.Ignore)
                            {
                                foreach (string folder in Directory.GetDirectories(searchFolder))
                                {
                                    foreach (string extension in Properties.Resources.MediaFilter.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        foreach (string file in Directory.GetFiles(folder, "*" + extension, SearchOption.TopDirectoryOnly))
                                        {
                                            files.Add(file);
                                        }
                                    }

                                    if (importSubFolders == FolderImportMode.FirstLevel)
                                    {
                                        foreach (string subfolder in Directory.GetDirectories(folder))
                                            files.Add(subfolder);
                                    }
                                    else
                                    {
                                        foreach (string subfolder in Directory.GetDirectories(folder))
                                        {
                                            foreach (string extension in Properties.Resources.MediaFilter.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                                            {
                                                foreach (string file in Directory.GetFiles(subfolder, "*" + extension, SearchOption.TopDirectoryOnly))
                                                {
                                                    files.Add(file);
                                                }
                                            }

                                            foreach (string subsubfolder in Directory.GetDirectories(subfolder))
                                                files.Add(subsubfolder);
                                        }
                                    }
                                }
                            }

                            int pos = 0;
                            foreach (string file in files)
                            {
                                pos++;
                                currentFile = file;

                                Dispatcher.Invoke((Action)delegate
                                {
                                    importingStatusWindow.Mode = StatusMode.Importing;
                                    importingStatusWindow.Line1 = string.Format(Properties.Resources.StatusImportingFile, pos, files.Count);
                                    importingStatusWindow.Line2 = string.Format(Properties.Resources.StatusImportingFileContent, currentFile);
                                    importingStatusWindow.Progress = pos * 1.0 / files.Count * 100;
                                });

                                if (dataSource.Movies.ToList().Find(m => m.MediaFile != null && m.MediaFile.Path == file) == null)
                                {
                                    Dispatcher.Invoke((Action)delegate
                                        {
                                            IsEnabled = false;
                                            ImportFile(file);
                                        });
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            TaskDialog.MessageBox(Properties.Resources.TaskDialogImportFolderExceptionTitle, string.Format(Properties.Resources.TaskDialogImportFolderExceptionMain, currentFile),
                                Properties.Resources.TaskDialogImportFolderExceptionText, exp.ToString(), string.Empty, string.Empty, TaskDialogButtons.OK, TaskDialogIcons.Error, TaskDialogIcons.Error);
                        }
                        finally { importing = false; Dispatcher.Invoke((Action)delegate { importingStatusWindow.Close(); IsEnabled = true; }); }
                    }));
                importThread.Name = "Importing";
                importThread.IsBackground = true;
                importThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                importThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                importThread.Start();
            }
        }

        /// <summary>
        /// Handles the Click event of the menuItemSimpleView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemSimpleView_Click(object sender, RoutedEventArgs e)
        {
            UpdateListViewView(false);
        }

        /// <summary>
        /// Handles the Click event of the menuItemAdvancedView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemAdvancedView_Click(object sender, RoutedEventArgs e)
        {
            UpdateListViewView(true);
        }

        /// <summary>
        /// Initializes the list view view.
        /// </summary>
        private void InitializeListViewView()
        {
            UpdateListViewView(Properties.Settings.Default.AdvancedListView);
        }

        /// <summary>
        /// Updates the list view.
        /// </summary>
        private void UpdateListViewView(bool advancedView)
        {
            if (advancedView)
                listBoxMovies.ItemTemplate = Resources["AdvancedViewTemplate"] as DataTemplate;
            else
                listBoxMovies.ItemTemplate = Resources["SimpleViewTemplate"] as DataTemplate;

            menuItemSimpleView.IsChecked = !advancedView;
            menuItemAdvancedView.IsChecked = advancedView;
            menuItemSimpleViewYear.IsChecked = Properties.Settings.Default.SimpleListViewShowYear;
            menuItemViewToolTip.IsChecked = Properties.Settings.Default.ListViewShowToolTip;
            Properties.Settings.Default.AdvancedListView = advancedView;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Handles the KeyUp event of the listBoxMovies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void listBoxMovies_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DeleteSelectedMovies(false);
        }

        /// <summary>
        /// Handles the Click event of the menuItemSimpleViewYear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void menuItemSimpleViewYear_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.SimpleListViewShowYear = menuItemSimpleViewYear.IsChecked;
            Properties.Settings.Default.Save();

            bool advView = Properties.Settings.Default.AdvancedListView;
            UpdateListViewView(!advView);
            UpdateListViewView(advView);
        }

        /// <summary>
        /// Handles the Click event of the menuItemViewToolTip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void menuItemViewToolTip_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ListViewShowToolTip = menuItemViewToolTip.IsChecked;
            Properties.Settings.Default.Save();

            bool advView = Properties.Settings.Default.AdvancedListView;
            UpdateListViewView(!advView);
            UpdateListViewView(advView);
        }

        /// <summary>
        /// Updates the filter.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void UpdateFilter()
        {
            if (dataSource == null)
                return;

            ICollectionView view = CollectionViewSource.GetDefaultView(dataSource.Movies);

            if (view != null)
            {
                int currentCount = 0;
                long currentSize = 0;
                long totalSize = 0;

                Predicate<object> filter = (textBoxSearch.Text == string.Empty ||
                    textBoxSearch.Text == LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources", "TextBoxSearch", LocalizeDictionary.Instance.Culture) &&
                    !checkBoxFilter.IsChecked.Value) ? null :
                    new Predicate<object>(delegate(object o)
                        {
                            if (!(o is IMovie))
                                return false;

                            IMovie movie = o as IMovie;

                            movie.MediaFiles.ToList().ForEach(m => totalSize += m.Size.HasValue ? m.Size.Value : 0);

                            if (textBoxSearch.Text != string.Empty &&
                                textBoxSearch.Text != LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources", "TextBoxSearch", LocalizeDictionary.Instance.Culture))
                                if (!movie.Contains(textBoxSearch.Text.ToLower()))
                                    return false;

                            if (checkBoxFilter.IsChecked.Value)
                            {
                                if (checkBoxFilterSeen.IsChecked.HasValue && currentUser.MovieSettings.ToList().Find(s => s.Movie == movie).Seen != checkBoxFilterSeen.IsChecked.Value)
                                    return false;

                                if (checkBoxFilterResolution.IsChecked.Value)
                                {
                                    if (movie.MediaFile == null || movie.MediaFile.Video == null || !movie.MediaFile.Video.Width.HasValue || movie.MediaFile.Video.Width.Value < 1280)
                                    {
                                        if (!checkBoxFilterNonHD.IsChecked.Value)
                                            return false;
                                    }
                                    else if (movie.MediaFile.Video.Width < 1920)
                                    {
                                        if (!checkBoxFilter720p.IsChecked.Value)
                                            return false;
                                    }
                                    else
                                    {
                                        if (!checkBoxFilter1080p.IsChecked.Value)
                                            return false;
                                    }
                                }

                                if (checkBoxFilterGenre.IsChecked.Value)
                                {
                                    bool matching = false;

                                    foreach (object obj in comboBoxFilterGenre.Items)
                                    {
                                        if (!(obj as CheckBox).IsChecked.Value)
                                            continue;

                                        if (movie.Genres.ToList().Find(g => g.Equals((obj as CheckBox).Tag as IGenre)) != null)
                                        {
                                            matching = true;
                                            break;
                                        }
                                    }

                                    if (!matching)
                                        return false;
                                }

                                if (checkBoxFilterLanguage.IsChecked.Value)
                                {
                                    if (movie.MediaFile == null)
                                        return false;

                                    bool matching = false;

                                    foreach (object obj in comboBoxFilterLanguage.Items)
                                    {
                                        if (!(obj as CheckBox).IsChecked.Value)
                                            continue;

                                        CultureInfo searchInfo = (obj as CheckBox).Tag as CultureInfo;
                                        foreach (IAudioProperties props in movie.MediaFile.Audio)
                                        {
                                            if (props.Language != null && props.Language.DisplayName == searchInfo.DisplayName)
                                            {
                                                matching = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!matching)
                                        return false;
                                }
                            }

                            currentCount++;
                            movie.MediaFiles.ToList().ForEach(m => currentSize += m.Size.HasValue ? m.Size.Value : 0);

                            return true;
                        });

                if (view.Filter == null && filter == null)
                    return;

                view.Filter = filter;
                statusBarItemInfo.Text = filter == null ? string.Format(Properties.Resources.StatusBarInfoText, dataSource.Movies.Count) :
                    string.Format(Properties.Resources.StatusBarInfoTextFiltered, currentCount, Methods.GetFileSize(currentSize), dataSource.Movies.Count, Methods.GetFileSize(totalSize));
            }
            else
                statusBarItemInfo.Text = string.Format(Properties.Resources.StatusBarInfoText, dataSource.Movies.Count);
        }

        /// <summary>
        /// Handles the Click event of the checkBoxFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void checkBoxFilter_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxFilter.IsChecked.Value)
                expanderFilter.IsExpanded = true;

            UpdateFilter();
        }

        /// <summary>
        /// Handles the Click event of the checkBoxFilterSeen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void checkBoxFilterSeen_Click(object sender, RoutedEventArgs e)
        {
            checkBoxFilter.IsChecked = true;

            UpdateFilter();
        }
        /// <summary>
        /// Handles the MouseRightButtonUp event of the checkBoxFilterSeen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void checkBoxFilterSeen_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            checkBoxFilterSeen.IsChecked = null;

            UpdateFilter();
        }

        /// <summary>
        /// Handles the Click event of the checkBoxFilterResolution control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void checkBoxFilterResolution_Click(object sender, RoutedEventArgs e)
        {
            checkBoxFilter.IsChecked = true;

            UpdateFilter();
        }
        /// <summary>
        /// Handles the Click event of the checkBoxFilterNonHD control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void checkBoxFilterNonHD_Click(object sender, RoutedEventArgs e)
        {
            checkBoxFilter.IsChecked = true;
            checkBoxFilterResolution.IsChecked = true;

            UpdateFilter();
        }
        /// <summary>
        /// Handles the Click event of the checkBoxFilter720p control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void checkBoxFilter720p_Click(object sender, RoutedEventArgs e)
        {
            checkBoxFilter.IsChecked = true;
            checkBoxFilterResolution.IsChecked = true;

            UpdateFilter();
        }
        /// <summary>
        /// Handles the Click event of the checkBoxFilter1080p control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void checkBoxFilter1080p_Click(object sender, RoutedEventArgs e)
        {
            checkBoxFilter.IsChecked = true;
            checkBoxFilterResolution.IsChecked = true;

            UpdateFilter();
        }

        /// <summary>
        /// Updates the filter lists.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void UpdateFilterLists()
        {
            UpdateLanguageFilterList();
            UpdateGenreFilterList();
        }

        /// <summary>
        /// Updates the language filter list.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void UpdateLanguageFilterList()
        {
            List<CultureInfo> langs = new List<CultureInfo>();
            foreach (IMovie movie in dataSource.Movies)
            {
                if (movie.MediaFile != null)
                {
                    foreach (IAudioProperties props in movie.MediaFile.Audio)
                        if (props.Language != null && !langs.Contains(props.Language))
                            langs.Add(props.Language);
                }
            }
            comboBoxFilterLanguage.Items.Clear();
            foreach (CultureInfo info in langs)
            {
                CheckBox languageFilterCheckBox = new CheckBox();
                languageFilterCheckBox.Content = info.DisplayName;
                languageFilterCheckBox.Tag = info;
                languageFilterCheckBox.Click += new RoutedEventHandler(languageFilterCheckBox_Click);
                comboBoxFilterLanguage.Items.Add(languageFilterCheckBox);
            }
            //comboBoxFilterLanguage.SelectedIndex = 0;
        }
        /// <summary>
        /// Handles the Click event of the languageFilterCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void languageFilterCheckBox_Click(object sender, RoutedEventArgs e)
        {
            checkBoxFilter.IsChecked = true;
            checkBoxFilterLanguage.IsChecked = true;

            UpdateFilter();
        }
        /// <summary>
        /// Handles the Click event of the checkBoxFilterLanguage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void checkBoxFilterLanguage_Click(object sender, RoutedEventArgs e)
        {
            checkBoxFilter.IsChecked = true;

            UpdateFilter();
        }

        /// <summary>
        /// Updates the genre filter list.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void UpdateGenreFilterList()
        {
            List<IGenre> genres = new List<IGenre>();
            foreach (IMovie movie in dataSource.Movies)
            {
                foreach (IGenre genre in movie.Genres)
                    if (!genres.Contains(genre))
                        genres.Add(genre);
            }
            comboBoxFilterGenre.Items.Clear();
            foreach (IGenre genre in genres)
            {
                CheckBox genreFilterCheckBox = new CheckBox();
                genreFilterCheckBox.Content = genre.Title;
                genreFilterCheckBox.Tag = genre;
                genreFilterCheckBox.Click += new RoutedEventHandler(genreFilterCheckBox_Click);
                comboBoxFilterGenre.Items.Add(genreFilterCheckBox);
            }
            //comboBoxFilterGenre.SelectedIndex = 0;
        }
        /// <summary>
        /// Handles the Click event of the genreFilterCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void genreFilterCheckBox_Click(object sender, RoutedEventArgs e)
        {
            checkBoxFilter.IsChecked = true;
            checkBoxFilterGenre.IsChecked = true;

            UpdateFilter();
        }
        /// <summary>
        /// Handles the Click event of the checkBoxFilterGenre control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        private void checkBoxFilterGenre_Click(object sender, RoutedEventArgs e)
        {
            checkBoxFilter.IsChecked = true;

            UpdateFilter();
        }

        /// <summary>
        /// Handles the Click event of the menuItemDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedMovies(false);
        }

        /// <summary>
        /// Handles the Click event of the menuItemSeen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemSeen_Click(object sender, RoutedEventArgs e)
        {
            UpdateSeenOfSelectedItems((sender as MenuItem).IsChecked);
        }

        /// <summary>
        /// Updates the seen of selected items.
        /// </summary>
        /// <param name="seen">if set to <c>true</c> [seen].</param>
        private void UpdateSeenOfSelectedItems(bool seen)
        {
            foreach (IMovie movie in listBoxMovies.SelectedItems.OfType<IMovie>().ToArray())
                currentUser.MovieSettings.ToList().Find(s => s.Movie == movie).Seen = seen;
        }

        /// <summary>
        /// Handles the Opened event of the ContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ContextMenu menu = sender as System.Windows.Controls.ContextMenu;

            (menu.Items[0] as MenuItem).IsChecked = CurrentUserSettings.Seen.Value;
        }

        /// <summary>
        /// Handles the Click event of the menuItemRemove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void menuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedMovies(true);
        }

        /// <summary>
        /// Handles the Drop event of the mainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        private void mainWindow_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string file in files)
                    ImportFile(file);
            }
        }
    }
}