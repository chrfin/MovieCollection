using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.ComponentModel;

namespace MovieDataSource
{
    /// <summary>
    /// Stores a IAudioProperties in the Sql CE database.
    /// </summary>
    public class SqlCeAudioProperties : IAudioProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeAudioProperties"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="store">The store.</param>
        public SqlCeAudioProperties(int id, SqlCeMovieDataSource store)
        {
            Id = id;
            Store = store;

            Dictionary<string, object> columns = Store.GetRow(Table.AudioProperties, Id);
            format = columns["format"].ToString();
            bitrate = Store.GetValue<int>(columns["bitrate"]);
            channels = Store.GetValue<int>(columns["channels"]);

            encoding = columns["encoding"].ToString();
            string langString = columns["language"].ToString();
            language = langString == string.Empty || langString.Length < 1 ? null : langString != "iv" ? new CultureInfo(langString) : null;
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
            return ToStringMethods.IAudioPropertiesToString(this);
        }

        #region IAudioProperties Members

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; private set; }

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
                Store.UpdateColumn(Table.AudioProperties, Id, "format", value);
                format = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Format"));
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
                Store.UpdateColumn(Table.AudioProperties, Id, "bitrate", value);
                bitrate = value;
                OnPropetyChanged(new PropertyChangedEventArgs("BitRate"));
            }
        }

        private int? channels;
        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        /// <value>The channels.</value>
        public int? Channels
        {
            get { return channels; }
            set
            {
                Store.UpdateColumn(Table.AudioProperties, Id, "channels", value);
                channels = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Channels"));
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
                Store.UpdateColumn(Table.AudioProperties, Id, "encoding", value);
                encoding = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Encoding"));
            }
        }

        private CultureInfo language;
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        public CultureInfo Language
        {
            get { return language; }
            set
            {
                if (value != null)
                    Store.UpdateColumn(Table.AudioProperties, Id, "language", value.TwoLetterISOLanguageName);
                else
                    Store.UpdateColumn(Table.AudioProperties, Id, "language", null);
                language = value;
                OnPropetyChanged(new PropertyChangedEventArgs("Language"));
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
