using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MediaInfoNET
{
    /// <summary>
    /// Holds informations about an image.
    /// </summary>
    /// <remarks>Documented by CFI, 2009-03-27</remarks>
    public class Image
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
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="mediaInfo">The media info.</param>
        /// <param name="id">The id.</param>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        internal Image(MediaInfo mediaInfo, int id) { MediaInfo = mediaInfo; Id = id; }

        #region MediaInfo properties
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        public int Count { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersImage.Count)); } }
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        public int StreamCount { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersImage.StreamCount)); } }
        /// <summary>
        /// Gets the kind of the stream.
        /// </summary>
        /// <value>The kind of the stream.</value>
        public StreamKindStruct StreamKind { get { return new StreamKindStruct(Id, StreamKindEnum.Image, MediaInfo); } }
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        /// <value>The inform.</value>
        public string Inform { get { return MediaInfo.Get(Id, ParametersImage.Inform); } }
        /// <summary>
        /// The unique ID for this stream, should be copied with stream copy
        /// </summary>
        /// <value>The unique ID.</value>
        public string UniqueID { get { return MediaInfo.Get(Id, ParametersImage.UniqueID); } }
        /// <summary>
        /// Name of the track
        /// </summary>
        /// <value>The Title.</value>
        public string Title { get { return MediaInfo[ParametersImage.Title, Id]; } }
        /// <summary>
        /// Format used.
        /// </summary>
        /// <value>The Format.</value>
        public string Format { get { return MediaInfo[ParametersImage.Format, Id]; } }
        /// <summary>
        /// Info about Format.
        /// </summary>
        /// <value>The Format Info.</value>
        public string FormatInfo { get { return MediaInfo[ParametersImage.Format_AS_Info, Id]; } }
        /// <summary>
        /// Url to infos about the format.
        /// </summary>
        /// <value>The Format Url.</value>
        public string FormatUrl { get { return MediaInfo[ParametersImage.Format_AS_Url, Id]; } }
        /// <summary>
        /// Profile of the Format.
        /// </summary>
        /// <value>The Format Profile.</value>
        public string FormatProfile { get { return MediaInfo[ParametersImage.Format_Profile, Id]; } }
        /// <summary>
        /// Codec ID (found in some containers)
        /// </summary>
        /// <value>The codec ID.</value>
        public CodecIdStruct CodecID
        {
            get
            {
                return new CodecIdStruct()
                {
                    Name = MediaInfo.Get(Id, ParametersImage.CodecID),
                    Info = MediaInfo.Get(Id, ParametersImage.CodecID_AS_Info),
                    Hint = MediaInfo.Get(Id, ParametersImage.CodecID_AS_Hint),
                    URL = MediaInfo.Get(Id, ParametersImage.CodecID_AS_Url),
                    Description = MediaInfo.Get(Id, ParametersImage.CodecID_Description)
                };
            }
        }
        /// <summary>
        /// Width in pixel.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get { return Methods.TryParseInt(MediaInfo[ParametersImage.Width, Id]); } }
        /// <summary>
        /// Height in pixel.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get { return Methods.TryParseInt(MediaInfo[ParametersImage.Height, Id]); } }
        /// <summary>
        /// Gets the resolution.
        /// </summary>
        /// <value>The resolution.</value>
        public string Resolution { get { return MediaInfo[ParametersImage.Resolution, Id]; } }
        /// <summary>
        /// Stream size in bytes.
        /// </summary>
        /// <value>The Stream Size.</value>
        public double StreamSize { get { return Methods.TryParseDouble(MediaInfo[ParametersImage.StreamSize, Id]); } }
        /// <summary>
        /// Stream size divided by file size.
        /// </summary>
        /// <value>The Stream Size Proportion.</value>
        public double StreamSizeProportion { get { return Methods.TryParseDouble(MediaInfo[ParametersImage.StreamSize_Proportion, Id]); } }
        /// <summary>
        /// Software used to create the file
        /// </summary>
        /// <value>The Encoded Library.</value>
        public EncodedLibraryStruct EncodedLibrary
        {
            get
            {
                return new EncodedLibraryStruct()
                {
                    Title = MediaInfo[ParametersImage.Encoded_Library, Id],
                    Name = MediaInfo[ParametersImage.Encoded_Library_AS_Name, Id],
                    Version = MediaInfo[ParametersImage.Encoded_Library_AS_Version, Id],
                    Date = MediaInfo[ParametersImage.Encoded_Library_AS_Date, Id],
                    Settings = MediaInfo[ParametersImage.Encoded_Library_Settings, Id]
                };
            }
        }
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public CultureInfo Language { get { return Methods.TryParseCulture(MediaInfo[ParametersImage.Language, Id]); } }
        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary { get { return MediaInfo[ParametersImage.Summary, Id]; } }
        /// <summary>
        /// The time/date/year that the encoding of this item was completed began.
        /// </summary>
        /// <value>The Encoded Date.</value>
        public string EncodedDate { get { return MediaInfo[ParametersImage.Encoded_Date, Id]; } }
        /// <summary>
        /// The time/date/year that the tags were done for this item.
        /// </summary>
        /// <value>The Tagged Date.</value>
        public string TaggedDate { get { return MediaInfo[ParametersImage.Tagged_Date, Id]; } }
        /// <summary>
        /// The Encryption.
        /// </summary>
        /// <value>The Encryption.</value>
        public string Encryption { get { return MediaInfo[ParametersImage.Encryption, Id]; } }
        #endregion
    }
}
