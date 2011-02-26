using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MovieDataSource
{
    /// <summary>
    /// Stores a IGenre in the Sql CE database.
    /// </summary>
    public class SqlCeGenre : IGenre
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeGenre"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public SqlCeGenre(int id, SqlCeMovieDataSource store)
        {
            Id = id;
            Store = store;

            Dictionary<string, object> columns = Store.GetRow(Table.Genres, id);
            Title = columns["title"].ToString();
        }

        /// <summary>
        /// Gets or sets the store.
        /// </summary>
        /// <value>The store.</value>
        public SqlCeMovieDataSource Store { get; private set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return ToStringMethods.IGenreToString(this);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        public override bool Equals(object obj)
        {
            if (!typeof(IGenre).IsAssignableFrom(obj.GetType()))
                throw new ArgumentException("Can only compare Genres!");

            IGenre comparer = obj as IGenre;
            if (comparer is SqlCeGenre && (comparer as SqlCeGenre).Store == Store)
                return Id == comparer.Id;
            else
                return Title == comparer.Title;
        }

        #region IGenre Members

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        /// <remarks>Documented by CFI, 2009-04-16</remarks>
        public string Title { get; private set; }

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
