using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MediaInfoNET
{
    /// <summary>
    /// Holds informations about a text stream.
    /// </summary>
    /// <remarks>Documented by CFI, 2009-03-27</remarks>
    public class TextStream
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
        /// Initializes a new instance of the <see cref="TextStream"/> class.
        /// </summary>
        /// <param name="mediaInfo">The media info.</param>
        /// <param name="id">The id.</param>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        internal TextStream(MediaInfo mediaInfo, int id) { MediaInfo = mediaInfo; Id = id; }

        #region MediaInfo properties
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        public int Count { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersText.Count)); } }
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        public int StreamCount { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersText.StreamCount)); } }
        /// <summary>
        /// Gets the kind of the stream.
        /// </summary>
        /// <value>The kind of the stream.</value>
        public StreamKindStruct StreamKind { get { return new StreamKindStruct(Id, StreamKindEnum.Text, MediaInfo); } }
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        /// <value>The inform.</value>
        public string Inform { get { return MediaInfo.Get(Id, ParametersText.Inform); } }
        /// <summary>
        /// The unique ID for this stream, should be copied with stream copy
        /// </summary>
        /// <value>The unique ID.</value>
        public string UniqueID { get { return MediaInfo.Get(Id, ParametersText.UniqueID); } }
        /// <summary>
        /// The menu ID for this stream in this file
        /// </summary>
        /// <value>The unique ID.</value>
        public int MenuID { get { return Methods.TryParseInt(MediaInfo.Get(Id, ParametersText.MenuID)); } }
        /// <summary>
        /// Format used.
        /// </summary>
        /// <value>The Format.</value>
        public string Format { get { return MediaInfo[ParametersText.Format, Id]; } }
        /// <summary>
        /// Info about Format.
        /// </summary>
        /// <value>The Format Info.</value>
        public string FormatInfo { get { return MediaInfo[ParametersText.Format_AS_Info, Id]; } }
        /// <summary>
        /// Url to infos about the format.
        /// </summary>
        /// <value>The Format Url.</value>
        public string FormatUrl { get { return MediaInfo[ParametersText.Format_AS_Url, Id]; } }
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
                    Name = MediaInfo.Get(Id, ParametersText.CodecID),
                    Info = MediaInfo.Get(Id, ParametersText.CodecID_AS_Info),
                    Hint = MediaInfo.Get(Id, ParametersText.CodecID_AS_Hint),
                    URL = MediaInfo.Get(Id, ParametersText.CodecID_AS_Url),
                    Description = MediaInfo.Get(Id, ParametersText.CodecID_Description)
                };
            }
        }
        /// <summary>
        /// Play time of the stream in ms.
        /// </summary>
        /// <value>The duration.</value>
        public long Duration { get { return Methods.TryParseLong(MediaInfo[ParametersText.Duration, Id]); } }
        /// <summary>
        /// Bit rate in bps.
        /// </summary>
        /// <value>The bit rate.</value>
        public BitRateStruct BitRate
        {
            get
            {
                return new BitRateStruct()
                {
                    Value = Methods.TryParseInt(MediaInfo[ParametersText.BitRate, Id]),
                    Minimum = -1,
                    Nominal = -1,
                    Maximum = -1,
                    Mode = Methods.TryParseBitRateMode(MediaInfo[ParametersText.BitRate_Mode, Id])
                };
            }
        }
        /// <summary>
        /// Width in pixel.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get { return Methods.TryParseInt(MediaInfo[ParametersText.Width, Id]); } }
        /// <summary>
        /// Height in pixel.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get { return Methods.TryParseInt(MediaInfo[ParametersText.Height, Id]); } }
        /// <summary>
        /// Gets the frame count.
        /// </summary>
        /// <value>The frame count.</value>
        public long FrameCount { get { return Methods.TryParseLong(MediaInfo[ParametersText.FrameCount, Id]); } }
        /// <summary>
        /// Gets the resolution.
        /// </summary>
        /// <value>The resolution.</value>
        public string Resolution { get { return MediaInfo[ParametersText.Resolution, Id]; } }
        /// <summary>
        /// Delay fixed in the stream (relative) in ms.
        /// </summary>
        /// <value>The Delay.</value>
        public long Delay { get { return Methods.TryParseLong(MediaInfo[ParametersText.Delay, Id]); } }
        /// <summary>
        /// Delay fixed in the stream (absolute / video) in ms.
        /// </summary>
        /// <value>The Video Delay.</value>
        public long VideoDelay { get { return Methods.TryParseLong(MediaInfo[ParametersText.Video_Delay, Id]); } }
        /// <summary>
        /// Stream size in bytes.
        /// </summary>
        /// <value>The Stream Size.</value>
        public double StreamSize { get { return Methods.TryParseDouble(MediaInfo[ParametersText.StreamSize, Id]); } }
        /// <summary>
        /// Stream size divided by file size.
        /// </summary>
        /// <value>The Stream Size Proportion.</value>
        public double StreamSizeProportion { get { return Methods.TryParseDouble(MediaInfo[ParametersText.StreamSize_Proportion, Id]); } }
        /// <summary>
        /// Name of the track
        /// </summary>
        /// <value>The Title.</value>
        public string Title { get { return MediaInfo[ParametersText.Title, Id]; } }
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
                    Title = MediaInfo[ParametersText.Encoded_Library, Id],
                    Name = MediaInfo[ParametersText.Encoded_Library_AS_Name, Id],
                    Version = MediaInfo[ParametersText.Encoded_Library_AS_Version, Id],
                    Date = MediaInfo[ParametersText.Encoded_Library_AS_Date, Id],
                    Settings = MediaInfo[ParametersText.Encoded_Library_Settings, Id]
                };
            }
        }
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public CultureInfo Language { get { return Methods.TryParseCulture(MediaInfo[ParametersText.Language, Id]); } }
        /// <summary>
        /// More info about Language (e.g. Director's Comment).
        /// </summary>
        /// <value>The language details.</value>
        public string LanguageMore { get { return MediaInfo[ParametersText.Language_More, Id]; } }
        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary { get { return MediaInfo[ParametersText.Summary, Id]; } }
        /// <summary>
        /// The time/date/year that the encoding of this item was completed began.
        /// </summary>
        /// <value>The Encoded Date.</value>
        public string EncodedDate { get { return MediaInfo[ParametersText.Encoded_Date, Id]; } }
        /// <summary>
        /// The time/date/year that the tags were done for this item.
        /// </summary>
        /// <value>The Tagged Date.</value>
        public string TaggedDate { get { return MediaInfo[ParametersText.Tagged_Date, Id]; } }
        /// <summary>
        /// The Encryption.
        /// </summary>
        /// <value>The Encryption.</value>
        public string Encryption { get { return MediaInfo[ParametersText.Encryption, Id]; } }
        #endregion
    }
}
