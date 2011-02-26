using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MediaInfoNET
{
    /// <summary>
    /// Holds informations about a audio stream.
    /// </summary>
    /// <remarks>Documented by CFI, 2009-03-27</remarks>
    public class AudioStream
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
        /// Initializes a new instance of the <see cref="AudioStream"/> class.
        /// </summary>
        /// <param name="mediaInfo">The media info.</param>
        /// <param name="id">The id.</param>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        internal AudioStream(MediaInfo mediaInfo, int id) { MediaInfo = mediaInfo; Id = id; }

        #region MediaInfo properties
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        public int Count { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersAudio.Count)); } }
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        public int StreamCount { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersAudio.StreamCount)); } }
        /// <summary>
        /// Gets the kind of the stream.
        /// </summary>
        /// <value>The kind of the stream.</value>
        public StreamKindStruct StreamKind { get { return new StreamKindStruct(Id, StreamKindEnum.Audio, MediaInfo); } }
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        /// <value>The inform.</value>
        public string Inform { get { return MediaInfo.Get(Id, ParametersAudio.Inform); } }
        /// <summary>
        /// The unique ID for this stream, should be copied with stream copy
        /// </summary>
        /// <value>The unique ID.</value>
        public string UniqueID { get { return MediaInfo.Get(Id, ParametersAudio.UniqueID); } }
        /// <summary>
        /// The menu ID for this stream in this file
        /// </summary>
        /// <value>The unique ID.</value>
        public int MenuID { get { return Methods.TryParseInt(MediaInfo.Get(Id, ParametersAudio.MenuID)); } }
        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>The format.</value>
        public AudioFormatStruct Format
        {
            get
            {
                return new AudioFormatStruct()
                {
                    Name = MediaInfo[ParametersAudio.Format],
                    Info = MediaInfo[ParametersAudio.Format_AS_Info],
                    URL = MediaInfo[ParametersAudio.Format_AS_Url],
                    Version = MediaInfo[ParametersAudio.Format_Version],
                    Profile = MediaInfo[ParametersAudio.Format_Profile],
                    SettingsSummary = MediaInfo[ParametersAudio.Format_Settings],
                    Settings = new AudioFormatSettingsStruct()
                    {
                        SBR = MediaInfo[ParametersAudio.Format_Settings_SBR],
                        PS = MediaInfo[ParametersAudio.Format_Settings_PS],
                        Floor = MediaInfo[ParametersAudio.Format_Settings_Floor],
                        Firm = MediaInfo[ParametersAudio.Format_Settings_Firm],
                        Endianness = MediaInfo[ParametersAudio.Format_Settings_Endianness],
                        Sign = MediaInfo[ParametersAudio.Format_Settings_Sign],
                        Law = MediaInfo[ParametersAudio.Format_Settings_Law],
                        ITU = MediaInfo[ParametersAudio.Format_Settings_ITU]
                    }
                };
            }
        }
        /// <summary>
        /// How this file is muxed in the container.
        /// </summary>
        /// <value>The muxing mode.</value>
        public int MuxingMode { get { return Methods.TryParseInt(MediaInfo.Get(Id, ParametersAudio.MuxingMode)); } }
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
                    Name = MediaInfo.Get(Id, ParametersAudio.CodecID),
                    Info = MediaInfo.Get(Id, ParametersAudio.CodecID_AS_Info),
                    Hint = MediaInfo.Get(Id, ParametersAudio.CodecID_AS_Hint),
                    URL = MediaInfo.Get(Id, ParametersAudio.CodecID_AS_Url),
                    Description = MediaInfo.Get(Id, ParametersAudio.CodecID_Description)
                };
            }
        }
        /// <summary>
        /// Play time of the stream in ms.
        /// </summary>
        /// <value>The duration.</value>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public long Duration { get { return Methods.TryParseLong(MediaInfo[ParametersAudio.Duration, Id]); } }
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
                    Value = Methods.TryParseInt(MediaInfo[ParametersAudio.BitRate, Id]),
                    Minimum = Methods.TryParseInt(MediaInfo[ParametersAudio.BitRate_Minimum, Id]),
                    Nominal = Methods.TryParseInt(MediaInfo[ParametersAudio.BitRate_Nominal, Id]),
                    Maximum = Methods.TryParseInt(MediaInfo[ParametersAudio.BitRate_Maximum, Id]),
                    Mode = Methods.TryParseBitRateMode(MediaInfo[ParametersAudio.BitRate_Mode, Id])
                };
            }
        }
        /// <summary>
        /// Gets the channles.
        /// </summary>
        /// <value>The channles.</value>
        public AudioChannels Channles
        {
            get
            {
                return new AudioChannels(Methods.TryParseInt(MediaInfo[ParametersAudio.Channel_OpenBracket_s_CloseBracket_, Id]),
                    MediaInfo[ParametersAudio.ChannelPositions, Id], MediaInfo[ParametersAudio.ChannelPositions_AS_String2, Id]);
            }
        }
        /// <summary>
        /// Sampling rate in Hz.
        /// </summary>
        /// <value>The Sampling Rate.</value>
        public int SamplingRate { get { return Methods.TryParseInt(MediaInfo[ParametersAudio.SamplingRate, Id]); } }
        /// <summary>
        /// Frame count.
        /// </summary>
        /// <value>The Sample count.</value>
        public long SamplingCount { get { return Methods.TryParseLong(MediaInfo[ParametersAudio.SamplingCount, Id]); } }
        /// <summary>
        /// Resolution in bits (8, 16, 20, 24).
        /// </summary>
        /// <value>The Resolution.</value>
        public int Resolution { get { return Methods.TryParseInt(MediaInfo[ParametersAudio.Resolution, Id]); } }
        /// <summary>
        /// Current stream size divided by uncompressed stream size.
        /// </summary>
        /// <value>The Compression Ratio.</value>
        public double CompressionRatio { get { return Methods.TryParseDouble(MediaInfo[ParametersAudio.CompressionRatio, Id]); } }
        /// <summary>
        /// Delay fixed in the stream (relative) in ms.
        /// </summary>
        /// <value>The Delay.</value>
        public long Delay { get { return Methods.TryParseLong(MediaInfo[ParametersAudio.Delay, Id]); } }
        /// <summary>
        /// Delay fixed in the stream (absolute / video) in ms.
        /// </summary>
        /// <value>The Video Delay.</value>
        public long VideoDelay { get { return Methods.TryParseLong(MediaInfo[ParametersAudio.Video_Delay, Id]); } }
        /// <summary>
        /// The gain to apply to reach 89dB SPL on playback in dB.
        /// </summary>
        /// <value>The Replay Gain.</value>
        public double ReplayGain { get { return Methods.TryParseDouble(MediaInfo[ParametersAudio.ReplayGain_Gain, Id]); } }
        /// <summary>
        /// The maximum absolute peak value of the item.
        /// </summary>
        /// <value>The Replay Gain Peak.</value>
        public double ReplayGainPeak { get { return Methods.TryParseDouble(MediaInfo[ParametersAudio.ReplayGain_Peak, Id]); } }
        /// <summary>
        /// Streamsize in bytes.
        /// </summary>
        /// <value>The Stream Size.</value>
        public long StreamSize { get { return Methods.TryParseLong(MediaInfo[ParametersAudio.StreamSize, Id]); } }
        /// <summary>
        /// Stream size divided by file size.
        /// </summary>
        /// <value>The Stream Size Proportion.</value>
        public double StreamSizeProportion { get { return Methods.TryParseDouble(MediaInfo[ParametersAudio.StreamSize_Proportion, Id]); } }
        /// <summary>
        /// How this stream file is aligned in the container
        /// </summary>
        /// <value>The Alignment.</value>
        public string Alignment { get { return MediaInfo[ParametersAudio.Alignment, Id]; } }
        /// <summary>
        /// Between how many video frames the stream is inserted.
        /// </summary>
        /// <value>The Interleave VideoFrames.</value>
        public string InterleaveVideoFrames { get { return MediaInfo[ParametersAudio.Interleave_VideoFrames, Id]; } }
        /// <summary>
        /// Between how much time (ms) the stream is inserted.
        /// </summary>
        /// <value>The Interleave Duration.</value>
        public long InterleaveDuration { get { return Methods.TryParseLong(MediaInfo[ParametersAudio.Interleave_Duration, Id]); } }
        /// <summary>
        /// How much time is buffered before the first video frame.
        /// </summary>
        /// <value>The Interleave Preload.</value>
        public long InterleavePreload { get { return Methods.TryParseLong(MediaInfo[ParametersAudio.Interleave_Preload, Id]); } }
        /// <summary>
        /// Name of the track
        /// </summary>
        /// <value>The Title.</value>
        public string Title { get { return MediaInfo[ParametersAudio.Title, Id]; } }
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
                    Title = MediaInfo[ParametersAudio.Encoded_Library, Id],
                    Name = MediaInfo[ParametersAudio.Encoded_Library_AS_Name, Id],
                    Version = MediaInfo[ParametersAudio.Encoded_Library_AS_Version, Id],
                    Date = MediaInfo[ParametersAudio.Encoded_Library_AS_Date, Id],
                    Settings = MediaInfo[ParametersAudio.Encoded_Library_Settings, Id]
                };
            }
        }
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public CultureInfo Language { get { return Methods.TryParseCulture(MediaInfo[ParametersAudio.Language, Id]); } }
        /// <summary>
        /// More info about Language (e.g. Director's Comment).
        /// </summary>
        /// <value>The language details.</value>
        public string LanguageMore { get { return MediaInfo[ParametersAudio.Language_More, Id]; } }
        /// <summary>
        /// The time/date/year that the encoding of this item was completed began.
        /// </summary>
        /// <value>The Encoded Date.</value>
        public string EncodedDate { get { return MediaInfo[ParametersAudio.Encoded_Date, Id]; } }
        /// <summary>
        /// The time/date/year that the tags were done for this item.
        /// </summary>
        /// <value>The Tagged Date.</value>
        public string TaggedDate { get { return MediaInfo[ParametersAudio.Tagged_Date, Id]; } }
        /// <summary>
        /// The Encryption.
        /// </summary>
        /// <value>The Encryption.</value>
        public string Encryption { get { return MediaInfo[ParametersAudio.Encryption, Id]; } }
        #endregion
    }
}
