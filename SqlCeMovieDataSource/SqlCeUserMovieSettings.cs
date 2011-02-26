using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MovieDataSource
{
    public class SqlCeUserMovieSettings : IUserMovieSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeUserMovieSettings"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public SqlCeUserMovieSettings(int id, SqlCeMovieDataSource store)
        {
            Id = id;
            Store = store;

            Dictionary<string, object> columns = Store.GetRow(Table.UserMovieSettings, id);
            Movie = Store.Movies.ToList().Find(m => m.Id == Store.GetValue<int>(columns["movies_id"]).Value);
            seen = Store.GetValue<bool>(columns["seen"]);
            rating = Store.GetValue<double>(columns["rating"]);
            comment = columns["comment"].ToString();
        }

        /// <summary>
        /// Gets or sets the store.
        /// </summary>
        /// <value>The store.</value>
        public SqlCeMovieDataSource Store { get; private set; }

        #region IUserMovieSettings Members

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the movie.
        /// </summary>
        /// <value>The movie.</value>
        public IMovie Movie { get; private set; }

        private bool? seen;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IUserMovieSettings"/> is seen.
        /// </summary>
        /// <value><c>true</c> if seen; otherwise, <c>false</c>.</value>
        public bool? Seen
        {
            get { return seen; }
            set
            {
                Store.UpdateColumn(Table.UserMovieSettings, Id, "seen", value);
                seen = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Seen"));
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
                Store.UpdateColumn(Table.UserMovieSettings, Id, "rating", val);
                rating = val;
                OnPropetyChanged(new PropertyChangedEventArgs("Rating"));
            }
        }

        private string comment;
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment
        {
            get { return comment; }
            set
            {
                Store.UpdateColumn(Table.UserMovieSettings, Id, "comment", value);
                comment = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Comment"));
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
