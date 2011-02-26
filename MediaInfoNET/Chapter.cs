using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MediaInfoNET
{
    /// <summary>
    /// Holds informations about a chapter.
    /// </summary>
    /// <remarks>Documented by CFI, 2009-03-27</remarks>
    public class Chapter
    {
        /// <summary>
        /// Gets the id of this stream.
        /// </summary>
        /// <value>The id.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public int Id { get; private set; }
        /// <summary>
        /// Gets or sets the media info to which this informations belong.
        /// </summary>
        /// <value>The media info.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public MediaInfo MediaInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chapter"/> class.
        /// </summary>
        /// <param name="mediaInfo">The media info.</param>
        /// <param name="id">The id.</param>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        internal Chapter(MediaInfo mediaInfo, int id) { MediaInfo = mediaInfo; Id = id; }

        #region MediaInfo properties
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        public int Count { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersChapters.Count)); } }
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        public int StreamCount { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersChapters.StreamCount)); } }
        /// <summary>
        /// Gets the kind of the stream.
        /// </summary>
        /// <value>The kind of the stream.</value>
        public StreamKindStruct StreamKind { get { return new StreamKindStruct(Id, StreamKindEnum.Chapters, MediaInfo); } }
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        /// <value>The inform.</value>
        public string Inform { get { return MediaInfo.Get(Id, ParametersChapters.Inform); } }
        /// <summary>
        /// The unique ID for this stream, should be copied with stream copy
        /// </summary>
        /// <value>The unique ID.</value>
        public string UniqueID { get { return MediaInfo.Get(Id, ParametersChapters.UniqueID); } }
        /// <summary>
        /// Format used.
        /// </summary>
        /// <value>The Format.</value>
        public string Format { get { return MediaInfo[ParametersChapters.Format, Id]; } }
        /// <summary>
        /// Info about Format.
        /// </summary>
        /// <value>The Format Info.</value>
        public string FormatInfo { get { return MediaInfo[ParametersChapters.Format_AS_Info, Id]; } }
        /// <summary>
        /// Url to infos about the format.
        /// </summary>
        /// <value>The Format Url.</value>
        public string FormatUrl { get { return MediaInfo[ParametersChapters.Format_AS_Url, Id]; } }
        /// <summary>
        /// Total number of chapters.
        /// </summary>
        public int Total { get { return Methods.TryParseInt(MediaInfo.Get(Id, ParametersChapters.Total)); } }
        /// <summary>
        /// Name of the track
        /// </summary>
        /// <value>The Title.</value>
        public string Title { get { return MediaInfo[ParametersChapters.Title, Id]; } }
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public CultureInfo Language { get { return Methods.TryParseCulture(MediaInfo[ParametersChapters.Language, Id]); } }
        #endregion
    }
}
