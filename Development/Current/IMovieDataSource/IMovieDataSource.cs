using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using IMovieDataSource.Properties;
using RootLibrary.WPF.Localization;
using System.Threading;

namespace MovieDataSource
{
    /// <summary>
    /// Defines a IMovieDataSourcePlugin.
    /// </summary>
    public sealed class DataSourcePlugin : Attribute { }

    /// <summary>
    /// The type of the DataSource.
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// This data source is a File data source --> call the "filename"-Create-Method.
        /// </summary>
        FileDataSource,
        /// <summary>
        /// This data source is a database data source --> call the "connectionstring"-Create-Method.
        /// </summary>
        DataBaseDataSource
    }

    /// <summary>
    /// Connection informations to a database.
    /// </summary>
    public struct ConnectionStringStruct
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public string User { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
    }

    /// <summary>
    /// Interface which creates data sources.
    /// </summary>
    public interface IMovieDataSourceFactory
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>The icon.</value>
        Image Icon { get; }

        /// <summary>
        /// The type of this data source.
        /// </summary>
        DataSourceType Type { get; }
        /// <summary>
        /// Creates the data source with the specified filename (call this for file data sources).
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        IMovieDataSourcePlugin Create(string filename);
        /// <summary>
        /// Creates the data source with the specified connection string (call this for database data sources).
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        IMovieDataSourcePlugin Create(ConnectionStringStruct connectionString);

        /// <summary>
        /// The extension of the file used by this data source (empty for database data sources).
        /// </summary>
        string Extension { get; }
    }

    /// <summary>
    /// A storage for movie informations.
    /// </summary>
    public interface IMovieDataSourcePlugin : INotifyPropertyChanged
    {
        /// <summary>
        /// Closes this data source.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets the filename (empty for database data source).
        /// </summary>
        /// <value>The filename.</value>
        string Filename { get; }
        /// <summary>
        /// Gets the connection (null for file data source).
        /// </summary>
        /// <value>The connection.</value>
        ConnectionStringStruct? Connection { get; }

        /// <summary>
        /// Gets the movies.
        /// </summary>
        /// <value>The movies.</value>
        ObservableCollection<IMovie> Movies { get; }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>The users.</value>
        ObservableCollection<IUserProfile> Users { get; }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        IMovie CreateMovie(string title);
        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IPerson CreatePerson(string name);
        /// <summary>
        /// Creates the genre.
        /// </summary>
        /// <param name="genre">The title.</param>
        /// <returns></returns>
        IGenre CreateGenre(string title);
        /// <summary>
        /// Creates the media file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        IMediaFile CreateMediaFile(string path);
        /// <summary>
        /// Creates the video properties.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        IVideoProperties CreateVideoProperties(long duration);
        /// <summary>
        /// Creates the audio properties.
        /// </summary>
        /// <param name="channels">The channels.</param>
        /// <returns></returns>
        IAudioProperties CreateAudioProperties(int channels);
        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IUserProfile CreateUser(string name);
        /// <summary>
        /// Creates the movie settings.
        /// </summary>
        /// <param name="movie">The movie.</param>
        /// <returns></returns>
        IUserMovieSettings CreateMovieSettings(IMovie movie);

        /// <summary>
        /// Forces the on title property changed event.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        void ForceOnTitlePropertyChanged();
    }

    /// <summary>
    /// Informations about a movie.
    /// </summary>
    public interface IMovie : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }
        /// <summary>
        /// Gets or sets the original title.
        /// </summary>
        /// <value>The original title.</value>
        string OriginalTitle { get; set; }
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        int? Year { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        string URL { get; set; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        string Country { get; set; }
        /// <summary>
        /// Gets or sets the plot.
        /// </summary>
        /// <value>The plot.</value>
        string Plot { get; set; }
        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>The rating.</value>
        double? Rating { get; set; }
        /// <summary>
        /// Gets or sets the cover.
        /// </summary>
        /// <value>The cover.</value>
        Image Cover { get; set; }

        /// <summary>
        /// Gets or sets the directors.
        /// </summary>
        /// <value>The directors.</value>
        ObservableCollection<IPerson> Directors { get; }

        /// <summary>
        /// Gets or sets the cast.
        /// </summary>
        /// <value>The cast.</value>
        ObservableCollection<IPerson> Cast { get; }

        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        /// <value>The genres.</value>
        ObservableCollection<IGenre> Genres { get; }

        /// <summary>
        /// Gets or sets the first media file in the MediaFilesCollection.
        /// </summary>
        /// <value>The media file.</value>
        IMediaFile MediaFile { get; set; }
        /// <summary>
        /// Gets or sets the media files.
        /// </summary>
        /// <value>The media files.</value>
        ObservableCollection<IMediaFile> MediaFiles { get; }

        /// <summary>
        /// Determines whether contains the specified search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>
        /// 	<c>true</c> if contains the specified search text; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(string searchText);

        /// <summary>
        /// Forces the on title property changed.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        void ForceOnTitlePropertyChanged();

        /// <summary>
        /// Gets or sets the tag (NOT STORED IN THE DATA SOURCE).
        /// </summary>
        /// <value>The tag.</value>
        object Tag { get; set; }
    }

    /// <summary>
    /// Infromations about a person.
    /// </summary>
    public interface IPerson : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        /// <remarks>Documented by CFI, 2009-04-16</remarks>
        string Role { get; set; }
        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        /// <value>The picture.</value>
        Image Picture { get; set; }
    }

    /// <summary>
    /// A Genre.
    /// </summary>
    public interface IGenre : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }
    }

    /// <summary>
    /// Informations about a file.
    /// </summary>
    public interface IMediaFile : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; }

        /// <summary>
        /// Gets the path to this file.
        /// </summary>
        /// <value>The path.</value>
        string Path { get; }
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        long? Size { get; set; }

        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>The video.</value>
        IVideoProperties Video { get; set; }
        /// <summary>
        /// Gets the audio.
        /// </summary>
        /// <value>The audio.</value>
        ObservableCollection<IAudioProperties> Audio { get; }
    }

    /// <summary>
    /// Informations about the video of a file.
    /// </summary>
    public interface IVideoProperties : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duratin.</value>
        long? Duration { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        int? Width { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        int? Height { get; set; }
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        string Format { get; set; }
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        string Encoding { get; set; }
        /// <summary>
        /// Gets or sets the bit rate.
        /// </summary>
        /// <value>The bit rate.</value>
        int? BitRate { get; set; }
    }

    /// <summary>
    /// Informations about the audio of a file.
    /// </summary>
    public interface IAudioProperties : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        string Format { get; set; }
        /// <summary>
        /// Gets or sets the bit rate.
        /// </summary>
        /// <value>The bit rate.</value>
        int? BitRate { get; set; }
        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        /// <value>The channels.</value>
        int? Channels { get; set; }
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        string Encoding { get; set; }
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        CultureInfo Language { get; set; }
    }

    /// <summary>
    /// A user.
    /// </summary>
    public interface IUserProfile : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        /// <summary>
        /// Gets the movie settings.
        /// </summary>
        /// <value>The movie settings.</value>
        ObservableCollection<IUserMovieSettings> MovieSettings { get; }
    }

    /// <summary>
    /// Settings of a movie of a user.
    /// </summary>
    public interface IUserMovieSettings : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; }
        /// <summary>
        /// Gets the movie.
        /// </summary>
        /// <value>The movie.</value>
        IMovie Movie { get; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IUserMovieSettings"/> is seen.
        /// </summary>
        /// <value><c>true</c> if seen; otherwise, <c>false</c>.</value>
        bool? Seen { get; set; }
        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>The rating.</value>
        double? Rating { get; set; }
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        string Comment { get; set; }
    }

    public class ToStringMethods
    {
        /// <summary>
        /// Converts IVideoProperties to string.
        /// </summary>
        /// <param name="props">The props.</param>
        /// <returns></returns>
        public static string IVideoPropertiesToString(IVideoProperties props)
        {
            string resPrefix = string.Empty;
            string resSuffix = string.Empty;
            string res = string.Empty;

            if (props.Duration.HasValue && props.Duration.Value > 0)
            {
                TimeSpan dur = (new TimeSpan(0, 0, 0, 0, (int)props.Duration.Value));
                //res += dur.ToString().Split('.')[0];
                res += dur.Hours + "h " + dur.Minutes + "m " + dur.Seconds + "s";
                resPrefix = " (";
                resSuffix = ")";
            }
            if (props.Width.HasValue && props.Height.HasValue)
                res += resPrefix + props.Width.Value + "x" + props.Height.Value + resSuffix;
            if (props.Format != string.Empty)
            {
                res += " - " + props.Format;
                if (props.BitRate.HasValue)
                    res += " (" + (props.Encoding != string.Empty ? props.Encoding + " @ " : string.Empty) + GetBitRateString(props.BitRate.Value) + ")";
            }

            return res;
        }

        /// <summary>
        /// Gets the bit rate string.
        /// </summary>
        /// <param name="bitrate">The bitrate.</param>
        /// <returns></returns>
        private static string GetBitRateString(int bitrate)
        {
            double rate = bitrate;
            if (rate < 1024)
                return rate + " bps";
            else if ((rate /= 1024) < 1024)
                return Math.Round(rate, 2) + " kbps";
            else if ((rate /= 1024) < 1024)
                return Math.Round(rate, 2) + " Mbps";

            return string.Empty;
        }

        /// <summary>
        /// Converts IAudioProperties to string.
        /// </summary>
        /// <param name="props">The props.</param>
        /// <returns></returns>
        public static string IAudioPropertiesToString(IAudioProperties props)
        {
            CultureInfo curCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = LocalizeDictionary.Instance.Culture;

            string res = string.Empty;
            if (props.Language != null)
                res += props.Language.DisplayName + ", ";
            res += string.Format(LocalizeDictionary.Instance.GetLocizedObject<string>("IMovieDataSource", "Resources", "ChannelsText", LocalizeDictionary.Instance.Culture), props.Channels.Value) + ", ";
            if (props.Format != string.Empty)
            {
                res += props.Format;
                if (props.BitRate.HasValue)
                    res += " (" + (props.Encoding != string.Empty ? props.Encoding + " @ " : string.Empty) + (props.BitRate.Value / 1000) + " kbps)";
            }
            res.Trim(',', ' ');

            Thread.CurrentThread.CurrentUICulture = curCulture;

            return res;
        }

        /// <summary>
        /// Converts IPerson to string.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public static string IPersonToString(IPerson person)
        {
            return person.Name + (person.Role != string.Empty ? " (" + person.Role + ")" : string.Empty);
        }

        /// <summary>
        /// Converts IGenre to string.
        /// </summary>
        /// <param name="person">The genre.</param>
        /// <returns></returns>
        public static string IGenreToString(IGenre genre)
        {
            return genre.Title;
        }
    }
}
