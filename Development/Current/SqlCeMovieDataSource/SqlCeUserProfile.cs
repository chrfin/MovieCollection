using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MovieDataSource
{
    public class SqlCeUserProfile : IUserProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeUserProfile"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public SqlCeUserProfile(int id, SqlCeMovieDataSource store)
        {
            Id = id;
            Store = store;

            Dictionary<string, object> columns = Store.GetRow(Table.UserProfiles, id);
            Name = columns["name"].ToString();

            MovieSettings = new ObservableCollection<IUserMovieSettings>();
            Store.GetReferences(Table.UserProfiles, Table.UserMovieSettings, Id).ForEach(s => MovieSettings.Add(new SqlCeUserMovieSettings(s, Store)));
            MovieSettings.CollectionChanged += new NotifyCollectionChangedEventHandler(MovieSettings_CollectionChanged);
        }

        /// <summary>
        /// Handles the CollectionChanged event of the MovieSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void MovieSettings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if ((new List<IUserMovieSettings>(e.NewItems.Cast<IUserMovieSettings>())).Find(s => !(s is SqlCeUserMovieSettings)) != null)
                        throw new ArgumentException("Can only add SqlCeUserMovieSettings!");
                    if ((new List<SqlCeUserMovieSettings>(e.NewItems.Cast<SqlCeUserMovieSettings>())).Find(s => s.Store != Store) != null)
                        throw new ArgumentException("Can only add User Movie Settings from this source!");
                    (new List<IUserMovieSettings>(e.NewItems.Cast<IUserMovieSettings>())).ForEach(s => Store.AddReferences(Table.UserProfiles, Table.UserMovieSettings, Id, s.Id));
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IUserMovieSettings settings in e.OldItems)
                    {
                        Store.RemoveReferences(Table.UserProfiles, Table.UserMovieSettings, Id, settings.Id);
                        Store.RemoveRow(Table.UserMovieSettings, settings.Id);
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

        #region IUserProfile Members

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the movie settings.
        /// </summary>
        /// <value>The movie settings.</value>
        public ObservableCollection<IUserMovieSettings> MovieSettings { get; private set; }

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

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
