using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MovieDataSource
{
    /// <summary>
    /// Stores a IVideoProperties in the Sql CE database.
    /// </summary>
    public class SqlCeVideoProperties : IVideoProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeVideoProperties"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="store">The store.</param>
        public SqlCeVideoProperties(int id, SqlCeMovieDataSource store)
        {
            Id = id;
            Store = store;

            Dictionary<string, object> columns = Store.GetRow(Table.VideoProperties, Id);
            duration = Store.GetValue<int>(columns["duration"]);
            width = Store.GetValue<int>(columns["width"]);
            height = Store.GetValue<int>(columns["height"]);
            format = columns["format"].ToString();
            encoding = columns["encoding"].ToString();
            bitrate = Store.GetValue<int>(columns["bitrate"]);
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
            return ToStringMethods.IVideoPropertiesToString(this);
        }

        #region IVideoProperties Members

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; private set; }

        private long? duration;
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duratin.</value>
        public long? Duration
        {
            get { return duration; }
            set
            {
                if (!value.HasValue)
                    throw new ArgumentNullException("Duration must not be null!");

                Store.UpdateColumn(Table.VideoProperties, Id, "duration", value);
                duration = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Duration"));
            }
        }

        private int? width;
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int? Width
        {
            get { return width; }
            set
            {
                Store.UpdateColumn(Table.VideoProperties, Id, "width", value);
                width = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Width"));
            }
        }

        private int? height;
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int? Height
        {
            get { return height; }
            set
            {
                Store.UpdateColumn(Table.VideoProperties, Id, "height", value);
                height = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Height"));
            }
        }

        private string format;
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public string Format
        {
            get { return format; }
            set
            {
                Store.UpdateColumn(Table.VideoProperties, Id, "format", value);
                format = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Format"));
            }
        }

        private string encoding;
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        public string Encoding
        {
            get { return encoding; }
            set
            {
                Store.UpdateColumn(Table.VideoProperties, Id, "encoding", value);
                encoding = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Encoding"));
            }
        }

        private int? bitrate;
        /// <summary>
        /// Gets or sets the bit rate.
        /// </summary>
        /// <value>The bit rate.</value>
        public int? BitRate
        {
            get { return bitrate; }
            set
            {
                Store.UpdateColumn(Table.VideoProperties, Id, "bitrate", value);
                bitrate = value;
                OnPropetyChanged(new PropertyChangedEventArgs("BitRate"));
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
