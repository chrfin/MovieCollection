using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MovieDataSource
{
    /// <summary>
    /// Stores a IMediaFile in the Sql CE database.
    /// </summary>
    public class SqlCeMediaFile : IMediaFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeMediaFile"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="store">The store.</param>
        public SqlCeMediaFile(int id, SqlCeMovieDataSource store)
        {
            Id = id;
            Store = store;

            Dictionary<string, object> columns = Store.GetRow(Table.MediaFiles, Id);
            Path = columns["path"].ToString();
            size = Store.GetValue<long>(columns["size"]);

            List<int> videoRefs = Store.GetReferences(Table.MediaFiles, Table.VideoProperties, Id);
            if (videoRefs.Count > 0)
                video = new SqlCeVideoProperties(videoRefs[0], Store);

            List<int> audioRefs = Store.GetReferences(Table.MediaFiles, Table.AudioProperties, Id);
            Audio = new ObservableCollection<IAudioProperties>();
            audioRefs.ForEach(a => Audio.Add(new SqlCeAudioProperties(a, Store)));
            Audio.CollectionChanged += new NotifyCollectionChangedEventHandler(Audio_CollectionChanged);
        }

        /// <summary>
        /// Handles the CollectionChanged event of the Audio control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void Audio_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((new List<IAudioProperties>(e.NewItems.Cast<IAudioProperties>())).Find(p => !(p is SqlCeAudioProperties)) != null)
                        throw new ArgumentException("Can only add SqlCeAudioProperties!");
                    if ((new List<SqlCeAudioProperties>(e.NewItems.Cast<SqlCeAudioProperties>())).Find(p => p.Store != Store) != null)
                        throw new ArgumentException("Can only add Audio Properties from this source!");
                    (new List<IAudioProperties>(e.NewItems.Cast<IAudioProperties>())).ForEach(p => Store.AddReferences(Table.MediaFiles, Table.AudioProperties, Id, p.Id));
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IAudioProperties prop in e.OldItems)
                    {
                        Store.RemoveReferences(Table.MediaFiles, Table.AudioProperties, Id, prop.Id);
                        Store.RemoveRow(Table.AudioProperties, prop.Id);
                    }
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
        /// Gets or sets the store.
        /// </summary>
        /// <value>The store.</value>
        public SqlCeMovieDataSource Store { get; private set; }

        #region IMediaFile Members

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the path to this file.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; private set; }

        private long? size;
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public long? Size
        {
            get { return size; }
            set
            {
                if (!value.HasValue)
                    Store.UpdateColumn(Table.MediaFiles, Id, "size", value);
                else
                    Store.UpdateColumn(Table.MediaFiles, Id, "size", value.Value);
                size = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Size"));
            }
        }

        private IVideoProperties video;
        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>The video.</value>
        public IVideoProperties Video
        {
            get { return video; }
            set
            {
                if (value == null)
                {
                    if (video != null)
                    {
                        Store.RemoveReferences(Table.MediaFiles, Table.VideoProperties, Id, video.Id);
                        Store.RemoveRow(Table.VideoProperties, video.Id);
                    }
                }
                else if (value is SqlCeVideoProperties && (value as SqlCeVideoProperties).Store == Store)
                {
                    if (video != null)
                    {
                        Store.RemoveReferences(Table.MediaFiles, Table.VideoProperties, Id, video.Id);
                        Store.RemoveRow(Table.VideoProperties, video.Id);
                    }
                    Store.AddReferences(Table.MediaFiles, Table.VideoProperties, Id, value.Id);
                    video = value;
                    OnPropetyChanged(new PropertyChangedEventArgs("Video"));
                }
                else
                    throw new ArgumentException("Can only add SqlCeVideoProperties from this Store!");
            }
        }

        /// <summary>
        /// Gets the audio.
        /// </summary>
        /// <value>The audio.</value>
        public ObservableCollection<IAudioProperties> Audio { get; private set; }

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
