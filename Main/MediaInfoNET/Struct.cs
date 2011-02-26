using System;
using System.Collections.Generic;

namespace MediaInfoNET
{
    /// <summary>
    /// Holds information about a stream.
    /// </summary>
    public struct StreamKindStruct
    {
        /// <summary>
        /// Number of the stream (base=0)
        /// </summary>
        /// <value>The id.</value>
        public int Id;
        /// <summary>
        /// When multiple streams, number of the stream (base=1); otherwise -1
        /// </summary>
        /// <value>The position.</value>
        public int Position;
        /// <summary>
        /// Gets or sets the kind of the stream.
        /// </summary>
        /// <value>The kind of the stream.</value>
        public StreamKindEnum StreamKind;

        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamKindStruct"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="streamKind">Kind of the stream.</param>
        /// <param name="mediaInfo">The media info.</param>
        internal StreamKindStruct(int id, StreamKindEnum streamKind, MediaInfo mediaInfo)
        {
            Id = id;
            StreamKind = streamKind;

            string pos = mediaInfo.Get(id, ParametersGeneral.StreamKindPos);
            int position;
            if (Int32.TryParse(pos, out position))
                Position = position;
            else
                Position = -1;

            name = mediaInfo.Get(0, ParametersGeneral.StreamKind_AS_String);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return name;
        }
    }
    /// <summary>
    /// Holds some stream details.
    /// </summary>
    public struct StreamDetailsStruct
    {
        /// <summary>
        /// Get the format
        /// </summary>
        public string Format;
        /// <summary>
        /// Video Codec with popular name (hint)
        /// </summary>
        public string FormatWithHint;
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        public string Codec;
        /// <summary>
        /// Get the language.
        /// </summary>
        public string Language;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamDetailsStruct"/> struct.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatWithHint">The format with hint.</param>
        /// <param name="codec">The codec.</param>
        /// <param name="language">The language.</param>
        internal StreamDetailsStruct(string format, string formatWithHint, string codec, string language)
        {
            Format = format;
            FormatWithHint = formatWithHint;
            Codec = codec;
            Language = language;
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return FormatWithHint;
        }
    }
    /// <summary>
    /// Holds general information about a format.
    /// </summary>
    public struct FormatStruct
    {
        /// <summary>
        /// Format name
        /// </summary>
        public string Name;
        /// <summary>
        /// Info about this Format
        /// </summary>
        public string Info;
        /// <summary>
        /// Link to a description of this format
        /// </summary>
        public string URL;
        /// <summary>
        /// Known extensions of this format
        /// </summary>
        public List<string> Extensions;
        /// <summary>
        /// Version of this format
        /// </summary>
        public string Version;
        /// <summary>
        /// Profile of the Format
        /// </summary>
        public string Profile;
        /// <summary>
        /// Settings needed for decoder used
        /// </summary>
        public string Settings;
    }
    /// <summary>
    /// Codec ID (found in some containers)
    /// </summary>
    public struct CodecIdStruct
    {
        /// <summary>
        /// The name of the codec id.
        /// </summary>
        public string Name;
        /// <summary>
        /// Info about this codec.
        /// </summary>
        public string Info;
        /// <summary>
        /// A hint/popular name for this codec.
        /// </summary>
        public string Hint;
        /// <summary>
        /// A link to more details about this codec ID.
        /// </summary>
        public string URL;
        /// <summary>
        /// Manual description given by the container.
        /// </summary>
        public string Description;
    }
    /// <summary>
    /// Informations about the bitrate of a stream.
    /// </summary>
    public struct BitRateStruct
    {
        /// <summary>
        /// Bit rate of all streams in bps.
        /// </summary>
        public int Value;
        /// <summary>
        /// Minimum Bit rate in bps.
        /// </summary>
        public int Minimum;
        /// <summary>
        /// Nominal Bit rate in bps.
        /// </summary>
        public int Nominal;
        /// <summary>
        /// Maximum Bit rate in bps.
        /// </summary>
        public int Maximum;
        /// <summary>
        /// The mode.
        /// </summary>
        public BitRateMode Mode;
    }
    /// <summary>
    /// Title informations.
    /// </summary>
    public struct TitleStruct
    {
        /// <summary>
        /// (Generic)Title of file.
        /// </summary>
        public string Title;
        /// <summary>
        /// (Generic)More info about the title of file.
        /// </summary>
        public string More;
        /// <summary>
        /// (Generic)Url
        /// </summary>
        public string URL;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }
    }
    /// <summary>
    /// Infromations about a movie.
    /// </summary>
    public struct MovieStruct
    {
        /// <summary>
        /// Name of the movie. Eg : Starwars, a new hope
        /// </summary>
        public string Title;
        /// <summary>
        /// More infos about the movie
        /// </summary>
        public string More;
        /// <summary>
        /// Country, where the movie was procuced
        /// </summary>
        public string Country;
        /// <summary>
        /// Homepage for the movie
        /// </summary>
        public string URL;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }
    }
    /// <summary>
    /// Infroamtions about a album.
    /// </summary>
    public struct AlbumStruct
    {
        /// <summary>
        /// Name of an audio-album. Eg : The joshua tree
        /// </summary>
        public string Title;
        /// <summary>
        /// More infos about the album
        /// </summary>
        public string More;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Sort;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }
    }
    /// <summary>
    /// Informations about a comic.
    /// </summary>
    public struct ComicStruct
    {
        /// <summary>
        /// Name of the comic.
        /// </summary>
        public string Title;
        /// <summary>
        /// More informations.
        /// </summary>
        public string More;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string PositionTotal;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }
    }
    /// <summary>
    /// Informations about a track.
    /// </summary>
    public struct TrackStruct
    {
        /// <summary>
        /// Name of the track. e.g. track1, track 2
        /// </summary>
        public string Title;
        /// <summary>
        /// More infos about the track
        /// </summary>
        public string More;
        /// <summary>
        /// Link to a site about this track
        /// </summary>
        public string URL;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Sort;
        /// <summary>
        /// Number of this track
        /// </summary>
        public int Position;
        /// <summary>
        /// Place of this track, e.g. 3 of 15
        /// </summary>
        public int PositionTotal;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }
    }
    /// <summary>
    /// Informations about a performer.
    /// </summary>
    public struct PerformerStruct
    {
        /// <summary>
        /// Main performer/artist of this file.
        /// </summary>
        public string Title;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Sort;
        /// <summary>
        /// Homepage of the performer/artist.
        /// </summary>
        public string URL;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }
    }
    /// <summary>
    /// Information about the encoder.
    /// </summary>
    public struct EncodedLibraryStruct
    {
        /// <summary>
        /// Software used to create the file
        /// </summary>
        public string Title;
        /// <summary>
        /// Name of the the encoding-software
        /// </summary>
        public string Name;
        /// <summary>
        /// Version of encoding-software
        /// </summary>
        public string Version;
        /// <summary>
        /// Release date of software
        /// </summary>
        public string Date;
        /// <summary>
        /// Parameters used by the software
        /// </summary>
        public string Settings;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }
    }
    /// <summary>
    /// Informations about the original medium.
    /// </summary>
    public struct OriginalSourceStruct
    {
        /// <summary>
        /// Original medium of the material, e.g. vinyl, Audio-CD, Super8 or BetaMax
        /// </summary>
        public string Medium;
        /// <summary>
        /// Original form of the material, e.g. slide, paper, map
        /// </summary>
        public string Form;
        /// <summary>
        /// Number of colors requested when digitizing, e.g. 256 for images or 32 bit RGB for video
        /// </summary>
        public string NumberOfColors;
        /// <summary>
        /// Name of the product the file was originally intended for
        /// </summary>
        public string Name;
        /// <summary>
        /// Describes whether an image has been cropped and, if so, how it was cropped. e.g. 16:9 to 4:3, top and bottom
        /// </summary>
        public string Cropped;
        /// <summary>
        /// Identifies the changes in sharpness for the digitizer requiered to produce the file
        /// </summary>
        public string Sharpness;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Medium;
        }
    }
    /// <summary>
    /// A cover.
    /// </summary>
    public struct CoverStruct
    {
        /// <summary>
        /// short descriptio, e.g. Earth in space
        /// </summary>
        public string Description;
        /// <summary>
        /// Info
        /// </summary>
        public string Type;
        /// <summary>
        /// Info
        /// </summary>
        public string Mime;
        /// <summary>
        /// Cover, in binary format
        /// </summary>
        public byte[] Data;
    }
    /// <summary>
    /// Details about the video format.
    /// </summary>
    public struct VideoFormatStruct
    {
        /// <summary>
        /// Format used
        /// </summary>
        public string Name;
        /// <summary>
        /// Info about Format
        /// </summary>
        public string Info;
        /// <summary>
        /// Link
        /// </summary>
        public string URL;
        /// <summary>
        /// Version of this format
        /// </summary>
        public string Version;
        /// <summary>
        /// Profile of the Format
        /// </summary>
        public string Profile;
        /// <summary>
        /// Settings needed for decoder used, summary
        /// </summary>
        public string SettingsSummary;
        /// <summary>
        /// Settings needed for decoder used
        /// </summary>
        public VideoFormatSettingsStruct Settings;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
    /// <summary>
    /// Settings of the Video format.
    /// </summary>
    public struct VideoFormatSettingsStruct
    {
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string BVOP;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string QPel;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string GMC;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Matrix;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string CABAC;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string RefFrames;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Pulldown;
    }
    /// <summary>
    /// Informations about the Framerate.
    /// </summary>
    public struct FrameRateStruct
    {
        /// <summary>
        /// Frame rate mode (CFR, VFR)
        /// </summary>
        public FrameRateMode Mode;
        /// <summary>
        /// Frames per second
        /// </summary>
        public double FPS;
        /// <summary>
        /// Minimum Frames per second
        /// </summary>
        public double Minimum;
        /// <summary>
        /// Nominal Frames per second
        /// </summary>
        public double Nominal;
        /// <summary>
        /// Maximum Frames per second
        /// </summary>
        public double Maximum;
        /// <summary>
        /// Original (in the raw stream) Frames per second
        /// </summary>
        public double OriginalFrameRate;
        /// <summary>
        /// Number of frames
        /// </summary>
        public long FrameCount;
    }
    /// <summary>
    /// Details about the audio format.
    /// </summary>
    public struct AudioFormatStruct
    {
        /// <summary>
        /// Format used
        /// </summary>
        public string Name;
        /// <summary>
        /// Info about Format
        /// </summary>
        public string Info;
        /// <summary>
        /// Link
        /// </summary>
        public string URL;
        /// <summary>
        /// Version of this format
        /// </summary>
        public string Version;
        /// <summary>
        /// Profile of the Format
        /// </summary>
        public string Profile;
        /// <summary>
        /// Settings needed for decoder used, summary
        /// </summary>
        public string SettingsSummary;
        /// <summary>
        /// Settings needed for decoder used
        /// </summary>
        public AudioFormatSettingsStruct Settings;

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
    /// <summary>
    /// Settings of the audio format.
    /// </summary>
    public struct AudioFormatSettingsStruct
    {
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string SBR;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string PS;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Floor;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Firm;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Endianness;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Sign;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string Law;
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        public string ITU;
    }
    /// <summary>
    /// The audio channles of a stream.
    /// </summary>
    public struct AudioChannels
    {
        /// <summary>
        /// The count of channels.
        /// </summary>
        public int Channels;
        /// <summary>
        /// The positions of the channels.
        /// </summary>
        public AudioChannelsPositions Positions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioChannels"/> struct.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="positionsString">The positions string.</param>
        /// <param name="positions">The positions.</param>
        internal AudioChannels(int count, string positionsString, string positions)
        {
            Channels = count;
            Positions = new AudioChannelsPositions(positionsString, positions);
        }
    }
    /// <summary>
    /// Poisitions of audio channels.
    /// </summary>
    public struct AudioChannelsPositions
    {
        /// <summary>
        /// Left front speaker.
        /// </summary>
        public bool FrontLeft;
        /// <summary>
        /// Center speaker.
        /// </summary>
        public bool Center;
        /// <summary>
        /// Right front speaker.
        /// </summary>
        public bool FrontRight;
        /// <summary>
        /// Subwoofer
        /// </summary>
        public bool LFE;
        /// <summary>
        /// Left surround speaker.
        /// </summary>
        public bool SurroundLeft;
        /// <summary>
        /// Right surround speaker.
        /// </summary>
        public bool SurroundRight;
        /// <summary>
        /// Position of channels (x/y.z format)
        /// </summary>
        public string Positions;
        /// <summary>
        /// Position of channels
        /// </summary>
        public string Locations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioChannelsPositions"/> struct.
        /// </summary>
        private AudioChannelsPositions(string positions)
        {
            FrontLeft = false;
            Center = false;
            FrontRight = false;
            LFE = false;
            SurroundLeft = false;
            SurroundRight = false;
            Positions = positions;
            Locations = string.Empty;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioChannelsPositions"/> struct.
        /// </summary>
        /// <param name="positionsString">The positions string.</param>
        /// <param name="positions">The positions.</param>
        internal AudioChannelsPositions(string positionsString, string positions)
            : this(positions)
        {
            Locations = positionsString;
            string[] pos = positionsString.Split(',');
            foreach (string str in pos)
            {
                if (str.Trim().StartsWith("Front"))
                {
                    string front = str.Replace("Front", string.Empty);
                    FrontLeft = front.Contains("L");
                    FrontRight = front.Contains("R");
                    Center = front.Contains("C");
                }
                else if (str.Trim().StartsWith("Surround"))
                {
                    string rear = str.Replace("Surround", string.Empty);
                    SurroundLeft = rear.Contains("L");
                    SurroundRight = rear.Contains("R");
                }
                else if (str.Trim() == "LFE")
                    LFE = true;
            }
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return Positions;
        }
    }
    /// <summary>
    /// Service informations
    /// </summary>
    public struct ServiceStruct
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the channel.
        /// </summary>
        /// <value>The channel.</value>
        public string Channel { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string URL { get; set; }
        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public string Provider { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }
    }
}
