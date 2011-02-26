using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace MovieDataSource
{
    /// <summary>
    /// Stores a IPerson in the Sql CE database.
    /// </summary>
    public class SqlCePerson : IPerson
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCePerson"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="store">The store.</param>
        /// <param name="parentMovie">The parent movie.</param>
        /// <remarks>Documented by CFI, 2009-04-16</remarks>
        public SqlCePerson(int id, SqlCeMovieDataSource store, IMovie parentMovie)
        {
            Id = id;
            Store = store;
            Movie = parentMovie;

            Dictionary<string, object> columns = Store.GetRow(Table.Persons, id);
            Name = columns["name"].ToString();
            picture = (columns["picture"] != null && !(columns["picture"] is DBNull)) ?
                Image.FromStream(new MemoryStream((byte[])columns["picture"])) : null;
        }

        /// <summary>
        /// Gets or sets the movie in which this movie is currently loaded (needed to get the role in the movie).
        /// </summary>
        /// <value>The movie.</value>
        /// <remarks>Documented by CFI, 2009-04-16</remarks>
        public IMovie Movie { get; set; }

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
            return ToStringMethods.IPersonToString(this);
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
            if (!typeof(IPerson).IsAssignableFrom(obj.GetType()))
                throw new ArgumentException("Can only compare Persons!");

            IPerson comparer = obj as IPerson;
            if (comparer is SqlCePerson && (comparer as SqlCePerson).Store == Store)
                return Id == comparer.Id;
            else
                return Name == comparer.Name;
        }

        #region IPerson Members

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks>Documented by CFI, 2009-04-16</remarks>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>The role.</value>
        /// <remarks>Documented by CFI, 2009-04-16</remarks>
        public string Role
        {
            get
            {
                if (Movie == null)
                    return string.Empty;
                else
                    return Store.GetField("Movies_Cast", "movies_id", Movie.Id, "cast_id", Id, "role").ToString();
            }
            set
            {
                if (Movie == null)
                    return;

                Store.SetField("Movies_Cast", "movies_id", Movie.Id, "cast_id", Id, "role", value);
                OnPropetyChanged(new PropertyChangedEventArgs("Role"));
            }
        }

        private Image picture;
        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        /// <value>The picture.</value>
        /// <remarks>Documented by CFI, 2009-04-16</remarks>
        public Image Picture
        {
            get { return picture; }
            set
            {
                if (value != null)
                {
                    MemoryStream stream = new MemoryStream();
                    value.Save(stream, ImageFormat.Png);
                    Store.UpdateColumn(Table.Persons, Id, "picture", stream.ToArray());
                }
                else
                    Store.UpdateColumn(Table.Persons, Id, "picture", null);

                picture = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Picture"));
            }
        }

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
