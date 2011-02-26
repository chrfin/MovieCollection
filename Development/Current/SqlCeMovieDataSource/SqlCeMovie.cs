using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Drawing;
using System.Collections.ObjectModel;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace MovieDataSource
{
    /// <summary>
    /// Stores a IMovie in the Sql CE database.
    /// </summary>
    public class SqlCeMovie : IMovie
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeMovie"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        internal SqlCeMovie(int id, SqlCeMovieDataSource store)
        {
            Id = id;
            Store = store;

            Dictionary<string, object> columns = Store.GetRow(Table.Movies, Id);
            title = columns["title"].ToString();
            originalTitle = columns["title_original"].ToString();
            year = Store.GetValue<int>(columns["year"]);
            url = columns["url"].ToString();
            country = columns["country"].ToString();
            plot = columns["plot"].ToString();
            rating = Store.GetValue<double>(columns["rating"]);
            cover = (columns["cover"] != null && !(columns["cover"] is DBNull)) ? Image.FromStream(new MemoryStream((byte[])columns["cover"])) : null;

            Directors = new ObservableCollection<IPerson>();
            Store.GetReferences(Table.Movies, Table.Directors, Id).ForEach(p => Directors.Add(new SqlCePerson(p, Store, null)));
            Directors.CollectionChanged += new NotifyCollectionChangedEventHandler(Directors_CollectionChanged);

            Cast = new ObservableCollection<IPerson>();
            Store.GetReferences(Table.Movies, Table.Cast, Id).ForEach(p => Cast.Add(new SqlCePerson(p, Store, this)));
            Cast.CollectionChanged += new NotifyCollectionChangedEventHandler(Cast_CollectionChanged);

            Genres = new ObservableCollection<IGenre>();
            Store.GetReferences(Table.Movies, Table.Genres, Id).ForEach(g => Genres.Add(new SqlCeGenre(g, Store)));
            Genres.CollectionChanged += new NotifyCollectionChangedEventHandler(Genres_CollectionChanged);

            MediaFiles = new ObservableCollection<IMediaFile>();
            Store.GetReferences(Table.Movies, Table.MediaFiles, Id).ForEach(m => MediaFiles.Add(new SqlCeMediaFile(m, Store)));
            MediaFiles.CollectionChanged += new NotifyCollectionChangedEventHandler(MediaFiles_CollectionChanged);
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Directors control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Directors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((new List<IPerson>(e.NewItems.Cast<IPerson>())).Find(p => !(p is SqlCePerson)) != null)
                        throw new ArgumentException("Can only add SqlCePersons!");
                    if ((new List<SqlCePerson>(e.NewItems.Cast<SqlCePerson>())).Find(p => p.Store != Store) != null)
                        throw new ArgumentException("Can only add Persons from this source!");
                    (new List<IPerson>(e.NewItems.Cast<IPerson>())).ForEach(p => Store.AddReferences(Table.Movies, Table.Directors, Id, p.Id));
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IPerson person in e.OldItems)
                        Store.RemoveReferences(Table.Movies, Table.Directors, Id, person.Id);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException("Please don't change the whole list at all!");
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Cast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Cast_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((new List<IPerson>(e.NewItems.Cast<IPerson>())).Find(p => !(p is SqlCePerson)) != null)
                        throw new ArgumentException("Can only add SqlCePersons!");
                    if ((new List<SqlCePerson>(e.NewItems.Cast<SqlCePerson>())).Find(p => p.Store != Store) != null)
                        throw new ArgumentException("Can only add Persons from this source!");
                    (new List<IPerson>(e.NewItems.Cast<IPerson>())).ForEach(p => Store.AddReferences(Table.Movies, Table.Cast, Id, p.Id));
                    new List<SqlCePerson>(e.NewItems.Cast<SqlCePerson>()).ForEach(p => p.Movie = this);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IPerson person in e.OldItems)
                        Store.RemoveReferences(Table.Movies, Table.Cast, Id, person.Id);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException("Please don't change the whole list at all!");
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Genres control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Genres_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((new List<IGenre>(e.NewItems.Cast<IGenre>())).Find(p => !(p is SqlCeGenre)) != null)
                        throw new ArgumentException("Can only add SqlCeGenres!");
                    if ((new List<SqlCeGenre>(e.NewItems.Cast<SqlCeGenre>())).Find(g => g.Store != Store) != null)
                        throw new ArgumentException("Can only add Genres from this source!");
                    (new List<IGenre>(e.NewItems.Cast<IGenre>())).ForEach(g => Store.AddReferences(Table.Movies, Table.Genres, Id, g.Id));
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IGenre genre in e.OldItems)
                        Store.RemoveReferences(Table.Movies, Table.Genres, Id, genre.Id);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException("Please don't change the whole list at all!");
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the MediaFiles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void MediaFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((new List<IMediaFile>(e.NewItems.Cast<IMediaFile>())).Find(m => !(m is SqlCeMediaFile)) != null)
                        throw new ArgumentException("Can only add SqlCeMediaFiles!");
                    if ((new List<SqlCeMediaFile>(e.NewItems.Cast<SqlCeMediaFile>())).Find(m => m.Store != Store) != null)
                        throw new ArgumentException("Can only add MediaFiles from this source!");
                    (new List<IMediaFile>(e.NewItems.Cast<IMediaFile>())).ForEach(m => Store.AddReferences(Table.Movies, Table.MediaFiles, Id, m.Id));
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IMediaFile file in e.OldItems)
                    {
                        file.Video = null;
                        while (file.Audio.Count > 0) file.Audio.RemoveAt(0);

                        Store.RemoveReferences(Table.Movies, Table.MediaFiles, Id, file.Id);
                        Store.RemoveRow(Table.MediaFiles, file.Id);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException("Please don't change the whole list at all!");
                default:
                    throw new ArgumentException();
            }
            OnPropetyChanged(new PropertyChangedEventArgs("MediaFile"));
        }

        /// <summary>
        /// Gets or sets the store.
        /// </summary>
        /// <value>The store.</value>
        public SqlCeMovieDataSource Store { get; private set; }

        #region IMovie Members

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; private set; }

        private string title;
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return title; }
            set
            {
                Store.UpdateColumn(Table.Movies, Id, "title", value);
                title = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Title"));
            }
        }

        private string originalTitle;
        /// <summary>
        /// Gets or sets the original title.
        /// </summary>
        /// <value>The original title.</value>
        public string OriginalTitle
        {
            get { return originalTitle; }
            set
            {
                Store.UpdateColumn(Table.Movies, Id, "title_original", value);
                originalTitle = value;
                OnPropetyChanged(new PropertyChangedEventArgs("OriginalTitle"));
            }
        }

        private int? year;
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public int? Year
        {
            get { return year; }
            set
            {
                Store.UpdateColumn(Table.Movies, Id, "year", value);
                year = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Year"));
            }
        }

        private string url;
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string URL
        {
            get { return url; }
            set
            {
                Store.UpdateColumn(Table.Movies, Id, "url", value);
                url = value;
                OnPropetyChanged(new PropertyChangedEventArgs("URL"));
            }
        }

        private string country;
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country
        {
            get { return country; }
            set
            {
                Store.UpdateColumn(Table.Movies, Id, "country", value);
                country = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Country"));
            }
        }

        private string plot;
        /// <summary>
        /// Gets or sets the plot.
        /// </summary>
        /// <value>The plot.</value>
        public string Plot
        {
            get { return plot; }
            set
            {
                Store.UpdateColumn(Table.Movies, Id, "plot", value);
                plot = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Plot"));
            }
        }

        private double? rating;
        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>The rating.</value>
        public double? Rating
        {
            get { return rating; }
            set
            {
                double? val = value;
                if (val.HasValue)
                    val = Math.Round(val.Value, 1);
                Store.UpdateColumn(Table.Movies, Id, "rating", val);
                rating = val;
                OnPropetyChanged(new PropertyChangedEventArgs("Rating"));
            }
        }

        private Image cover;
        /// <summary>
        /// Gets or sets the cover.
        /// </summary>
        /// <value>The cover.</value>
        public Image Cover
        {
            get { return cover; }
            set
            {
                if (value != null)
                {
                    MemoryStream stream = new MemoryStream();
                    value.Save(stream, ImageFormat.Png);
                    Store.UpdateColumn(Table.Movies, Id, "cover", stream.ToArray());
                }
                else
                    Store.UpdateColumn(Table.Movies, Id, "cover", null);
                cover = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Cover"));
            }
        }

        /// <summary>
        /// Gets or sets the directors.
        /// </summary>
        /// <value>The directors.</value>
        public ObservableCollection<IPerson> Directors { get; private set; }

        /// <summary>
        /// Gets or sets the cast.
        /// </summary>
        /// <value>The cast.</value>
        public ObservableCollection<IPerson> Cast { get; private set; }

        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        /// <value>The genres.</value>
        public ObservableCollection<IGenre> Genres { get; private set; }

        /// <summary>
        /// Gets or sets the first media file in the MediaFilesCollection.
        /// </summary>
        /// <value>The media file.</value>
        public IMediaFile MediaFile
        {
            get
            {
                if (MediaFiles.Count > 0)
                    return MediaFiles.First();
                else
                    return null;
            }
            set
            {
                if (MediaFiles.Count > 0)
                    MediaFiles.RemoveAt(0);
                if (value != null)
                    MediaFiles.Insert(0, value);
                OnPropetyChanged(new PropertyChangedEventArgs("MediaFile"));
            }
        }
        /// <summary>
        /// Gets or sets the media files.
        /// </summary>
        /// <value>The media files.</value>
        public ObservableCollection<IMediaFile> MediaFiles { get; private set; }

        /// <summary>
        /// Determines whether contains the specified search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>
        /// 	<c>true</c> if contains the specified search text; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string searchText)
        {
            if (!searchText.Contains(' '))
                return title.ToLower().Contains(searchText) || originalTitle.ToLower().Contains(searchText);
            else
            {
                foreach (string searchWord in searchText.Split(' '))
                    if (!(title.ToLower().Contains(searchText) || originalTitle.ToLower().Contains(searchText)))
                        return false;
                return true;
            }
        }

        /// <summary>
        /// Forces the on title property changed.
        /// </summary>
        public void ForceOnTitlePropertyChanged()
        {
            OnPropetyChanged(new PropertyChangedEventArgs("Title"));
        }

        /// <summary>
        /// Gets or sets the tag (NOT STORED IN THE DATA SOURCE).
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; set; }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the <see cref="E:PropetyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropetyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion
    }
}
