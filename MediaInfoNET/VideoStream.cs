using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MediaInfoNET
{
    /// <summary>
    /// Holds informations about a video stream.
    /// </summary>
    /// <remarks>Documented by CFI, 2009-03-27</remarks>
    public class VideoStream
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
        /// Initializes a new instance of the <see cref="VideoStream"/> class.
        /// </summary>
        /// <param name="mediaInfo">The media info.</param>
        /// <param name="id">The id.</param>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        internal VideoStream(MediaInfo mediaInfo, int id) { MediaInfo = mediaInfo; Id = id; }

        #region MediaInfo properties
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        public int Count { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersVideo.Count)); } }
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        public int StreamCount { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersVideo.StreamCount)); } }
        /// <summary>
        /// Gets the kind of the stream.
        /// </summary>
        /// <value>The kind of the stream.</value>
        public StreamKindStruct StreamKind { get { return new StreamKindStruct(Id, StreamKindEnum.Video, MediaInfo); } }
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        /// <value>The inform.</value>
        public string Inform { get { return MediaInfo.Get(Id, ParametersVideo.Inform); } }
        /// <summary>
        /// The unique ID for this stream, should be copied with stream copy
        /// </summary>
        /// <value>The unique ID.</value>
        public string UniqueID { get { return MediaInfo.Get(Id, ParametersVideo.UniqueID); } }
        /// <summary>
        /// The menu ID for this stream in this file
        /// </summary>
        /// <value>The unique ID.</value>
        public int MenuID { get { return Methods.TryParseInt(MediaInfo.Get(Id, ParametersVideo.MenuID)); } }
        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>The format.</value>
        public VideoFormatStruct Format
        {
            get
            {
                return new VideoFormatStruct()
                    {
                        Name = MediaInfo[ParametersVideo.Format],
                        Info = MediaInfo[ParametersVideo.Format_AS_Info],
                        URL = MediaInfo[ParametersVideo.Format_AS_Url],
                        Version = MediaInfo[ParametersVideo.Format_Version],
                        Profile = MediaInfo[ParametersVideo.Format_Profile],
                        SettingsSummary = MediaInfo[ParametersVideo.Format_Settings],
                        Settings = new VideoFormatSettingsStruct()
                            {
                                BVOP = MediaInfo[ParametersVideo.Format_Settings_BVOP],
                                QPel = MediaInfo[ParametersVideo.Format_Settings_QPel],
                                GMC = MediaInfo[ParametersVideo.Format_Settings_GMC],
                                Matrix = MediaInfo[ParametersVideo.Format_Settings_Matrix],
                                CABAC = MediaInfo[ParametersVideo.Format_Settings_CABAC],
                                RefFrames = MediaInfo[ParametersVideo.Format_Settings_RefFrames],
                                Pulldown = MediaInfo[ParametersVideo.Format_Settings_Pulldown]
                            }
                    };
            }
        }
        /// <summary>
        /// How this file is muxed in the container.
        /// </summary>
        /// <value>The muxing mode.</value>
        public int MuxingMode { get { return Methods.TryParseInt(MediaInfo.Get(Id, ParametersVideo.MuxingMode)); } }
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
                    Name = MediaInfo.Get(Id, ParametersVideo.CodecID),
                    Info = MediaInfo.Get(Id, ParametersVideo.CodecID_AS_Info),
                    Hint = MediaInfo.Get(Id, ParametersVideo.CodecID_AS_Hint),
                    URL = MediaInfo.Get(Id, ParametersVideo.CodecID_AS_Url),
                    Description = MediaInfo.Get(Id, ParametersVideo.CodecID_Description)
                };
            }
        }
        /// <summary>
        /// Play time of the stream in ms.
        /// </summary>
        /// <value>The duration.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public long Duration { get { return Methods.TryParseLong(MediaInfo[ParametersVideo.Duration, Id]); } }
        /// <summary>
        /// Bit rate in bps.
        /// </summary>
        /// <value>The overall bit rate.</value>
        public BitRateStruct BitRate
        {
            get
            {
                return new BitRateStruct()
                {
                    Value = Methods.TryParseInt(MediaInfo[ParametersVideo.BitRate, Id]),
                    Minimum = Methods.TryParseInt(MediaInfo[ParametersVideo.BitRate_Minimum, Id]),
                    Nominal = Methods.TryParseInt(MediaInfo[ParametersVideo.BitRate_Nominal, Id]),
                    Maximum = Methods.TryParseInt(MediaInfo[ParametersVideo.BitRate_Maximum, Id]),
                    Mode = Methods.TryParseBitRateMode(MediaInfo[ParametersVideo.BitRate_Mode, Id])
                };
            }
        }
        /// <summary>
        /// Width in pixel.
        /// </summary>
        /// <value>The width.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public int Width { get { return Methods.TryParseInt(MediaInfo[ParametersVideo.Width, Id]); } }
		/// <summary>
		/// Original Width in pixel.
		/// </summary>
		/// <value>The original width - sometimes different from width</value>
		/// <remarks>Documented by jondbell, 2012-01-20</remarks>
		public int OriginalWidth { get { return Methods.TryParseInt(MediaInfo[ParametersVideo.Width_Original, Id]); } }
        /// <summary>
        /// Height in pixel.
        /// </summary>
        /// <value>The height.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public int Height { get { return Methods.TryParseInt(MediaInfo[ParametersVideo.Height, Id]); } }
		/// <summary>
		/// Original Height in pixel.
		/// </summary>
		/// <value>The height.</value>
		/// <remarks>Documented by jondbell, 2012-01-20</remarks>
		public int OriginalHeight { get { return Methods.TryParseInt(MediaInfo[ParametersVideo.Height_Original, Id]); } }
        /// <summary>
        /// Gets the pixel aspect ratio.
        /// </summary>
        /// <value>The pixel aspect ratio.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public double PixelAspectRatio { get { return Methods.TryParseDouble(MediaInfo[ParametersVideo.PixelAspectRatio, Id]); } }
        /// <summary>
        /// Original (in the raw stream) Pixel Aspect ratio.
        /// </summary>
        /// <value>The pixel aspect ratio original.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public double PixelAspectRatioOriginal { get { return Methods.TryParseDouble(MediaInfo[ParametersVideo.PixelAspectRatio_Original, Id]); } }
        /// <summary>
        /// Gets the display aspect ratio.
        /// </summary>
        /// <value>The display aspect ratio.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public double DisplayAspectRatio { get { return Methods.TryParseDouble(MediaInfo[ParametersVideo.DisplayAspectRatio, Id]); } }
        /// <summary>
        /// Original (in the raw stream) Display Aspect ratio.
        /// </summary>
        /// <value>The display aspect ratio original.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public double DisplayAspectRatioOriginal { get { return Methods.TryParseDouble(MediaInfo[ParametersVideo.DisplayAspectRatio_Original, Id]); } }
        /// <summary>
        /// Gets the frame rate.
        /// </summary>
        /// <value>The frame rate.</value>
        public FrameRateStruct FrameRate
        {
            get
            {
                return new FrameRateStruct()
                    {
                        Mode = Methods.TryParseFrameRateMode(MediaInfo[ParametersVideo.FrameRate_Mode, Id]),
                        FPS = Methods.TryParseDouble(MediaInfo[ParametersVideo.FrameRate, Id]),
                        Minimum = Methods.TryParseDouble(MediaInfo[ParametersVideo.FrameRate_Minimum, Id]),
                        Nominal = Methods.TryParseDouble(MediaInfo[ParametersVideo.FrameRate_Nominal, Id]),
                        Maximum = Methods.TryParseDouble(MediaInfo[ParametersVideo.FrameRate_Maximum, Id]),
                        OriginalFrameRate = Methods.TryParseDouble(MediaInfo[ParametersVideo.FrameRate_Original, Id]),
                        FrameCount = Methods.TryParseLong(MediaInfo[ParametersVideo.FrameCount, Id])
                    };
            }
        }
        /// <summary>
        /// Gets the standard (NTSC or PAL).
        /// </summary>
        /// <value>The standard.</value>
        public VideoStandard Standard { get { return Methods.TryParseVideoStandard(MediaInfo[ParametersVideo.Standard, Id]); } }
        /// <summary>
        /// Resolution (16/24/32 bits).
        /// </summary>
        /// <value>The Resolution.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public int Resolution { get { return Methods.TryParseInt(MediaInfo[ParametersVideo.Resolution, Id]); } }
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        /// <value>The Colorimetry.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public int Colorimetry { get { return Methods.TryParseInt(MediaInfo[ParametersVideo.Colorimetry, Id]); } }
        /// <summary>
        /// Gets the Scan Type (p/i).
        /// </summary>
        /// <value>The standard.</value>
        public ScanType ScanType { get { return Methods.TryParseScanType(MediaInfo[ParametersVideo.ScanType, Id]); } }
        /// <summary>
        /// The Scan Order
        /// </summary>
        /// <value>The Scan Order.</value>
        public string ScanOrder { get { return MediaInfo.Get(Id, ParametersVideo.ScanOrder); } }
        /// <summary>
        /// bits/(Pixel*Frame) (~ Gordian Knot)
        /// </summary>
        /// <value>The Gordian Knot.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public double GordianKnot { get { return Methods.TryParseDouble(MediaInfo[ParametersVideo.Bits_IN__OpenBracket_Pixel_x_Frame_CloseBracket_, Id]); } }
        /// <summary>
        /// Delay fixed in the stream (relative) IN MS.
        /// </summary>
        /// <value>The Delay.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public long Delay { get { return Methods.TryParseLong(MediaInfo[ParametersVideo.Delay, Id]); } }
        /// <summary>
        /// Delay fixed in the raw stream (relative) IN MS.
        /// </summary>
        /// <value>The Original Delay.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public long DelayOriginal { get { return Methods.TryParseLong(MediaInfo[ParametersVideo.Delay_Original, Id]); } }
        /// <summary>
        /// Delay settings (in case of timecode for example)
        /// </summary>
        /// <value>The Original Delay Settings .</value>
        public string DelayOriginalSettings { get { return MediaInfo[ParametersVideo.Delay_Original_Settings, Id]; } }
        /// <summary>
        /// Stream size in bytes.
        /// </summary>
        /// <value>The Stream Size.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public double StreamSize { get { return Methods.TryParseDouble(MediaInfo[ParametersVideo.StreamSize, Id]); } }
        /// <summary>
        /// Stream size divided by file size.
        /// </summary>
        /// <value>The Stream Size Proportion.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public double StreamSizeProportion { get { return Methods.TryParseDouble(MediaInfo[ParametersVideo.StreamSize_Proportion, Id]); } }
        /// <summary>
        /// How this stream file is aligned in the container
        /// </summary>
        /// <value>The Alignment.</value>
        public string Alignment { get { return MediaInfo[ParametersVideo.Alignment, Id]; } }
        /// <summary>
        /// Name of the track
        /// </summary>
        /// <value>The Title.</value>
        public string Title { get { return MediaInfo[ParametersVideo.Title, Id]; } }
        /// <summary>
        /// Name of the software package used to create the file, such as Microsoft WaveEdit.
        /// </summary>
        /// <value>The Encoded Application.</value>
        public string EncodedApplication { get { return MediaInfo[ParametersVideo.Encoded_Application, Id]; } }
        /// <summary>
        /// Name of the software package used to create the file, such as Microsoft WaveEdit.
        /// </summary>
        /// <value>The Encoded Application URL.</value>
        public string EncodedApplicationURL { get { return MediaInfo[ParametersVideo.Encoded_Application_AS_Url, Id]; } }
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
                    Title = MediaInfo[ParametersVideo.Encoded_Library, Id],
                    Name = MediaInfo[ParametersVideo.Encoded_Library_AS_Name, Id],
                    Version = MediaInfo[ParametersVideo.Encoded_Library_AS_Version, Id],
                    Date = MediaInfo[ParametersVideo.Encoded_Library_AS_Date, Id],
                    Settings = MediaInfo[ParametersVideo.Encoded_Library_Settings, Id]
                };
            }
        }
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public CultureInfo Language { get { return Methods.TryParseCulture(MediaInfo[ParametersVideo.Language, Id]); } }
        /// <summary>
        /// More info about Language (e.g. Director's Comment).
        /// </summary>
        /// <value>The language details.</value>
        public string LanguageMore { get { return MediaInfo[ParametersVideo.Language_More, Id]; } }
        /// <summary>
        /// The time/date/year that the encoding of this item was completed began.
        /// </summary>
        /// <value>The Encoded Date.</value>
        public string EncodedDate { get { return MediaInfo[ParametersVideo.Encoded_Date, Id]; } }
        /// <summary>
        /// The time/date/year that the tags were done for this item.
        /// </summary>
        /// <value>The Tagged Date.</value>
        public string TaggedDate { get { return MediaInfo[ParametersVideo.Tagged_Date, Id]; } }
        /// <summary>
        /// The Encryption.
        /// </summary>
        /// <value>The Encryption.</value>
        public string Encryption { get { return MediaInfo[ParametersVideo.Encryption, Id]; } }
        #endregion
    }
}
