using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaInfoNET
{
    /// <summary>
    /// The type of stream you wish to get the information about.
    /// </summary>
    public enum StreamKindEnum
    {
        /// <summary>
        /// General information about the file.
        /// </summary>
        General,
        /// <summary>
        /// Informations about the video stream.
        /// </summary>
        Video,
        /// <summary>
        /// Informations about the audio stream.
        /// </summary>
        Audio,
        /// <summary>
        /// Informations about the texts in the file (subtitles).
        /// </summary>
        Text,
        /// <summary>
        /// Informations about the chapters.
        /// </summary>
        Chapters,
        /// <summary>
        /// Informations about the images in the file.
        /// </summary>
        Image,
        /// <summary>
        /// Informations about the menu in the file.
        /// </summary>
        Menu
    }

    /// <summary>
    /// To define which kind of info you wish to get.
    /// </summary>
    public enum InfoKind
    {
        /// <summary>
        /// Get the name.
        /// </summary>
        Name,
        /// <summary>
        /// Get the text.
        /// </summary>
        Text,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Measure,
        /// <summary>
        /// Get the options.
        /// </summary>
        Options,
        /// <summary>
        /// Get the name and the text.
        /// </summary>
        NameText,
        /// <summary>
        /// Get the measure value and the text.
        /// </summary>
        MeasureText,
        /// <summary>
        /// Get the info.
        /// </summary>
        Info,
        /// <summary>
        /// Get a how to.
        /// </summary>
        HowTo
    }

    /// <summary>
    /// Available options of a parameter.
    /// </summary>
    public enum InfoOptions
    {
        /// <summary>
        /// Show it in the inform output.
        /// </summary>
        ShowInInform,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Support,
        /// <summary>
        /// Show it in the supported output.
        /// </summary>
        ShowInSupported,
        /// <summary>
        /// Get the type of the value.
        /// </summary>
        TypeOfValue
    }

    /// <summary>
    /// The diverent file options available.
    /// </summary>
    public enum InfoFileOptions
    {
        /// <summary>
        /// Do nothing at all.
        /// </summary>
        FileOption_Nothing = 0x00,
        /// <summary>
        /// Search recursively.
        /// </summary>
        FileOption_Recursive = 0x01,
        /// <summary>
        /// Close all.
        /// </summary>
        FileOption_CloseAll = 0x02,
        /// <summary>
        /// Use the maximum.
        /// </summary>
        FileOption_Max = 0x04
    }

    /// <summary>
    /// The mode how the bitrate is calculated.
    /// </summary>
    public enum BitRateMode
    {
        /// <summary>
        /// Variable
        /// </summary>
        VBR,
        /// <summary>
        /// Constant
        /// </summary>
        CBR,
        /// <summary>
        /// Not available (e.g. overall bitrate)
        /// </summary>
        NA
    }

    /// <summary>
    /// Type of framerate.
    /// </summary>
    public enum FrameRateMode
    {
        /// <summary>
        /// Variable
        /// </summary>
        VFR,
        /// <summary>
        /// Constant
        /// </summary>
        CFR,
        /// <summary>
        /// Not available
        /// </summary>
        NA
    }

    /// <summary>
    /// The video standard
    /// </summary>
    public enum VideoStandard
    {
        /// <summary>
        /// NTSC
        /// </summary>
        NTSC,
        /// <summary>
        /// PAL
        /// </summary>
        PAL,
        /// <summary>
        /// Something other than PAL/NTSC
        /// </summary>
        Other
    }

    /// <summary>
    /// The scan type.
    /// </summary>
    public enum ScanType
    {
        /// <summary>
        /// full pictures
        /// </summary>
        Progressive,
        /// <summary>
        /// row by row
        /// </summary>
        Interleaved,
        /// <summary>
        /// something other
        /// </summary>
        Other
    }

    #region parameters
    /// <summary>
    /// Parameters for the general section.
    /// </summary>
    public enum ParametersGeneral
    {
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        Count,
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        StreamCount,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind_AS_String,
        /// <summary>
        /// Number of the stream (base=0)
        /// </summary>
        StreamKindID,
        /// <summary>
        /// When multiple streams, number of the stream (base=1)
        /// </summary>
        StreamKindPos,
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        Inform,
        /// <summary>
        /// The ID for this stream in this file
        /// </summary>
        ID,
        /// <summary>
        /// The ID for this stream in this file
        /// </summary>
        ID_AS_String,
        /// <summary>
        /// The unique ID for this stream, should be copied with stream copy
        /// </summary>
        UniqueID,
        /// <summary>
        /// Number of general streams
        /// </summary>
        GeneralCount,
        /// <summary>
        /// Number of video streams
        /// </summary>
        VideoCount,
        /// <summary>
        /// Number of audio streams
        /// </summary>
        AudioCount,
        /// <summary>
        /// Number of text streams
        /// </summary>
        TextCount,
        /// <summary>
        /// Number of chapters streams
        /// </summary>
        ChaptersCount,
        /// <summary>
        /// Number of image streams
        /// </summary>
        ImageCount,
        /// <summary>
        /// Number of menu streams
        /// </summary>
        MenuCount,
        /// <summary>
        /// Video Codecs in this file, separated by /
        /// </summary>
        Video_Format_List,
        /// <summary>
        /// Video Codecs in this file with popular name (hint), separated by /
        /// </summary>
        Video_Format_WithHint_List,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Video_Codec_List,
        /// <summary>
        /// Video languagesin this file, full names, separated by /
        /// </summary>
        Video_Language_List,
        /// <summary>
        /// Audio Codecs in this file,separated by /
        /// </summary>
        Audio_Format_List,
        /// <summary>
        /// Audio Codecs in this file with popular name (hint), separated by /
        /// </summary>
        Audio_Format_WithHint_List,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Audio_Codec_List,
        /// <summary>
        /// Audio languages in this file separated by /
        /// </summary>
        Audio_Language_List,
        /// <summary>
        /// Text Codecs in this file, separated by /
        /// </summary>
        Text_Format_List,
        /// <summary>
        /// Text Codecs in this file with popular name (hint),separated by /
        /// </summary>
        Text_Format_WithHint_List,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Text_Codec_List,
        /// <summary>
        /// Text languages in this file, separated by /
        /// </summary>
        Text_Language_List,
        /// <summary>
        /// Chapters Codecs in this file, separated by /
        /// </summary>
        Chapters_Format_List,
        /// <summary>
        /// Chapters Codecs in this file with popular name (hint), separated by /
        /// </summary>
        Chapters_Format_WithHint_List,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Chapters_Codec_List,
        /// <summary>
        /// Chapters languages in this file, separated by /
        /// </summary>
        Chapters_Language_List,
        /// <summary>
        /// Image Codecs in this file, separated by /
        /// </summary>
        Image_Format_List,
        /// <summary>
        /// Image Codecs in this file with popular name (hint), separated by /
        /// </summary>
        Image_Format_WithHint_List,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Image_Codec_List,
        /// <summary>
        /// Image languages in this file, separated by /
        /// </summary>
        Image_Language_List,
        /// <summary>
        /// Menu Codecsin this file, separated by /
        /// </summary>
        Menu_Format_List,
        /// <summary>
        /// Menu Codecs in this file with popular name (hint),separated by /
        /// </summary>
        Menu_Format_WithHint_List,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Menu_Codec_List,
        /// <summary>
        /// Menu languages in this file, separated by /
        /// </summary>
        Menu_Language_List,
        /// <summary>
        /// Complete name (Folder+Name+Extension)
        /// </summary>
        CompleteName,
        /// <summary>
        /// Folder name only
        /// </summary>
        FolderName,
        /// <summary>
        /// File name only
        /// </summary>
        FileName,
        /// <summary>
        /// File extension only
        /// </summary>
        FileExtension,
        /// <summary>
        /// Format used
        /// </summary>
        Format,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Format_AS_String,
        /// <summary>
        /// Info about this Format
        /// </summary>
        Format_AS_Info,
        /// <summary>
        /// Link to a description of this format
        /// </summary>
        Format_AS_Url,
        /// <summary>
        /// Known extensions of this format
        /// </summary>
        Format_AS_Extensions,
        /// <summary>
        /// Version of this format
        /// </summary>
        Format_Version,
        /// <summary>
        /// Profile of the Format
        /// </summary>
        Format_Profile,
        /// <summary>
        /// Settings needed for decoder used
        /// </summary>
        Format_Settings,
        /// <summary>
        /// Codec ID (found in some containers)
        /// </summary>
        CodecID,
        /// <summary>
        /// Info about this codec
        /// </summary>
        CodecID_AS_Info,
        /// <summary>
        /// A hint/popular name for this codec
        /// </summary>
        CodecID_AS_Hint,
        /// <summary>
        /// A link to more details about this codec ID
        /// </summary>
        CodecID_AS_Url,
        /// <summary>
        /// Manual description given by the container
        /// </summary>
        CodecID_Description,
        /// <summary>
        /// If Audio and video are muxed
        /// </summary>
        Interleaved,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_String,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_Info,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_Url,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_Extensions,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_Automatic,
        /// <summary>
        /// File size in bytes
        /// </summary>
        FileSize,
        /// <summary>
        /// File size (with measure)
        /// </summary>
        FileSize_AS_String,
        /// <summary>
        /// File size (with measure, 1 digit mini)
        /// </summary>
        FileSize_AS_String1,
        /// <summary>
        /// File size (with measure, 2 digit mini)
        /// </summary>
        FileSize_AS_String2,
        /// <summary>
        /// File size (with measure, 3 digit mini)
        /// </summary>
        FileSize_AS_String3,
        /// <summary>
        /// File size (with measure, 4 digit mini)
        /// </summary>
        FileSize_AS_String4,
        /// <summary>
        /// Play time of the stream in ms
        /// </summary>
        Duration,
        /// <summary>
        /// Play time in format : XXx YYy only, YYy omited if zero
        /// </summary>
        Duration_AS_String,
        /// <summary>
        /// Play time in format : HHh MMmn SSs MMMms, XX omited if zero
        /// </summary>
        Duration_AS_String1,
        /// <summary>
        /// Play time in format : XXx YYy only, YYy omited if zero
        /// </summary>
        Duration_AS_String2,
        /// <summary>
        /// Play time in format : HH:MM:SS.MMM
        /// </summary>
        Duration_AS_String3,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Duration_Start,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Duration_End,
        /// <summary>
        /// Bit rate mode of all streams (VBR, CBR)
        /// </summary>
        OverallBitRate_Mode,
        /// <summary>
        /// Bit rate mode of all streams (Variable, Constant)
        /// </summary>
        OverallBitRate_Mode_AS_String,
        /// <summary>
        /// Bit rate of all streams in bps
        /// </summary>
        OverallBitRate,
        /// <summary>
        /// Bit rate of all streams (with measure)
        /// </summary>
        OverallBitRate_AS_String,
        /// <summary>
        /// Minimum Bit rate in bps
        /// </summary>
        OverallBitRate_Minimum,
        /// <summary>
        /// Minimum Bit rate (with measurement)
        /// </summary>
        OverallBitRate_Minimum_AS_String,
        /// <summary>
        /// Nominal Bit rate in bps
        /// </summary>
        OverallBitRate_Nominal,
        /// <summary>
        /// Nominal Bit rate (with measurement)
        /// </summary>
        OverallBitRate_Nominal_AS_String,
        /// <summary>
        /// Maximum Bit rate in bps
        /// </summary>
        OverallBitRate_Maximum,
        /// <summary>
        /// Maximum Bit rate (with measurement)
        /// </summary>
        OverallBitRate_Maximum_AS_String,
        /// <summary>
        /// Stream size in bytes
        /// </summary>
        StreamSize,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String1,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String2,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String3,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String4,
        /// <summary>
        /// With proportion
        /// </summary>
        StreamSize_AS_String5,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_Proportion,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        HeaderSize,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        DataSize,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        FooterSize,
        /// <summary>
        /// The gain to apply to reach 89dB SPL on playback.
        /// </summary>
        Album_ReplayGain_Gain,
        /// <summary>
        /// The gain to apply to reach 89dB SPL on playback.
        /// </summary>
        Album_ReplayGain_Gain_AS_String,
        /// <summary>
        /// The maximum absolute peak value of the item.
        /// </summary>
        Album_ReplayGain_Peak,
        /// <summary>
        /// Title
        /// </summary>
        Title,
        /// <summary>
        /// Title
        /// </summary>
        Title_AS_More,
        /// <summary>
        /// Title
        /// </summary>
        Title_AS_Url,
        /// <summary>
        /// Title
        /// </summary>
        Domain,
        /// <summary>
        /// Title
        /// </summary>
        Collection,
        /// <summary>
        /// Title
        /// </summary>
        Season,
        /// <summary>
        /// Title
        /// </summary>
        Season_Position,
        /// <summary>
        /// Title
        /// </summary>
        Season_Position_Total,
        /// <summary>
        /// Title
        /// </summary>
        Movie,
        /// <summary>
        /// Title
        /// </summary>
        Movie_AS_More,
        /// <summary>
        /// Title
        /// </summary>
        Movie_AS_Country,
        /// <summary>
        /// Title
        /// </summary>
        Movie_AS_Url,
        /// <summary>
        /// Title
        /// </summary>
        Album,
        /// <summary>
        /// Title
        /// </summary>
        Album_AS_More,
        /// <summary>
        /// Title
        /// </summary>
        Album_AS_Sort,
        /// <summary>
        /// Title
        /// </summary>
        Comic,
        /// <summary>
        /// Title
        /// </summary>
        Comic_AS_More,
        /// <summary>
        /// Title
        /// </summary>
        Comic_AS_Position_Total,
        /// <summary>
        /// Title
        /// </summary>
        Part,
        /// <summary>
        /// Title
        /// </summary>
        Part_AS_Position,
        /// <summary>
        /// Title
        /// </summary>
        Part_AS_Position_Total,
        /// <summary>
        /// Title
        /// </summary>
        Track,
        /// <summary>
        /// Title
        /// </summary>
        Track_AS_More,
        /// <summary>
        /// Title
        /// </summary>
        Track_AS_Url,
        /// <summary>
        /// Title
        /// </summary>
        Track_AS_Sort,
        /// <summary>
        /// Title
        /// </summary>
        Track_AS_Position,
        /// <summary>
        /// Title
        /// </summary>
        Track_AS_Position_Total,
        /// <summary>
        /// Title
        /// </summary>
        Chapter,
        /// <summary>
        /// Title
        /// </summary>
        SubTrack,
        /// <summary>
        /// Title
        /// </summary>
        Original_AS_Album,
        /// <summary>
        /// Title
        /// </summary>
        Original_AS_Movie,
        /// <summary>
        /// Title
        /// </summary>
        Original_AS_Part,
        /// <summary>
        /// Title
        /// </summary>
        Original_AS_Track,
        /// <summary>
        /// Entity
        /// </summary>
        Performer,
        /// <summary>
        /// Entity
        /// </summary>
        Performer_AS_Sort,
        /// <summary>
        /// Entity
        /// </summary>
        Performer_AS_Url,
        /// <summary>
        /// Entity
        /// </summary>
        Original_AS_Performer,
        /// <summary>
        /// Entity
        /// </summary>
        Accompaniment,
        /// <summary>
        /// Entity
        /// </summary>
        Composer,
        /// <summary>
        /// Entity
        /// </summary>
        Composer_AS_Nationality,
        /// <summary>
        /// Entity
        /// </summary>
        Arranger,
        /// <summary>
        /// Entity
        /// </summary>
        Lyricist,
        /// <summary>
        /// Entity
        /// </summary>
        Original_AS_Lyricist,
        /// <summary>
        /// Entity
        /// </summary>
        Conductor,
        /// <summary>
        /// Entity
        /// </summary>
        Director,
        /// <summary>
        /// Entity
        /// </summary>
        AssistantDirector,
        /// <summary>
        /// Entity
        /// </summary>
        DirectorOfPhotography,
        /// <summary>
        /// Entity
        /// </summary>
        SoundEngineer,
        /// <summary>
        /// Entity
        /// </summary>
        ArtDirector,
        /// <summary>
        /// Entity
        /// </summary>
        ProductionDesigner,
        /// <summary>
        /// Entity
        /// </summary>
        Choregrapher,
        /// <summary>
        /// Entity
        /// </summary>
        CostumeDesigner,
        /// <summary>
        /// Entity
        /// </summary>
        Actor,
        /// <summary>
        /// Entity
        /// </summary>
        Actor_Character,
        /// <summary>
        /// Entity
        /// </summary>
        WrittenBy,
        /// <summary>
        /// Entity
        /// </summary>
        ScreenplayBy,
        /// <summary>
        /// Entity
        /// </summary>
        EditedBy,
        /// <summary>
        /// Entity
        /// </summary>
        CommissionedBy,
        /// <summary>
        /// Entity
        /// </summary>
        Producer,
        /// <summary>
        /// Entity
        /// </summary>
        CoProducer,
        /// <summary>
        /// Entity
        /// </summary>
        ExecutiveProducer,
        /// <summary>
        /// Entity
        /// </summary>
        MusicBy,
        /// <summary>
        /// Entity
        /// </summary>
        DistributedBy,
        /// <summary>
        /// Entity
        /// </summary>
        OriginalSourceForm_AS_DistributedBy,
        /// <summary>
        /// Entity
        /// </summary>
        MasteredBy,
        /// <summary>
        /// Entity
        /// </summary>
        EncodedBy,
        /// <summary>
        /// Entity
        /// </summary>
        RemixedBy,
        /// <summary>
        /// Entity
        /// </summary>
        ProductionStudio,
        /// <summary>
        /// Entity
        /// </summary>
        ThanksTo,
        /// <summary>
        /// Entity
        /// </summary>
        Publisher,
        /// <summary>
        /// Entity
        /// </summary>
        Publisher_AS_URL,
        /// <summary>
        /// Entity
        /// </summary>
        Label,
        /// <summary>
        /// Classification
        /// </summary>
        Genre,
        /// <summary>
        /// Classification
        /// </summary>
        Mood,
        /// <summary>
        /// Classification
        /// </summary>
        ContentType,
        /// <summary>
        /// Classification
        /// </summary>
        Subject,
        /// <summary>
        /// Classification
        /// </summary>
        Description,
        /// <summary>
        /// Classification
        /// </summary>
        Keywords,
        /// <summary>
        /// Classification
        /// </summary>
        Summary,
        /// <summary>
        /// Classification
        /// </summary>
        Synopsys,
        /// <summary>
        /// Classification
        /// </summary>
        Period,
        /// <summary>
        /// Classification
        /// </summary>
        LawRating,
        /// <summary>
        /// Classification
        /// </summary>
        LawRating_Reason,
        /// <summary>
        /// Classification
        /// </summary>
        ICRA,
        /// <summary>
        /// Temporal
        /// </summary>
        Released_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Original_AS_Released_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Recorded_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Encoded_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Tagged_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Written_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Mastered_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        File_Created_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        File_Modified_Date,
        /// <summary>
        /// Spatial
        /// </summary>
        Recorded_Location,
        /// <summary>
        /// Spatial
        /// </summary>
        Written_Location,
        /// <summary>
        /// Spatial
        /// </summary>
        Archival_Location,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Application,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Application_AS_Url,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Library,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Library_AS_String,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Library_AS_Name,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Library_AS_Version,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Library_AS_Date,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Library_Settings,
        /// <summary>
        /// Technical
        /// </summary>
        Cropped,
        /// <summary>
        /// Technical
        /// </summary>
        Dimensions,
        /// <summary>
        /// Technical
        /// </summary>
        DotsPerInch,
        /// <summary>
        /// Technical
        /// </summary>
        Lightness,
        /// <summary>
        /// Technical
        /// </summary>
        OriginalSourceMedium,
        /// <summary>
        /// Technical
        /// </summary>
        OriginalSourceForm,
        /// <summary>
        /// Technical
        /// </summary>
        OriginalSourceForm_AS_NumColors,
        /// <summary>
        /// Technical
        /// </summary>
        OriginalSourceForm_AS_Name,
        /// <summary>
        /// Technical
        /// </summary>
        OriginalSourceForm_AS_Cropped,
        /// <summary>
        /// Technical
        /// </summary>
        OriginalSourceForm_AS_Sharpness,
        /// <summary>
        /// Technical
        /// </summary>
        Tagged_Application,
        /// <summary>
        /// Technical
        /// </summary>
        BPM,
        /// <summary>
        /// Identifier
        /// </summary>
        ISRC,
        /// <summary>
        /// Identifier
        /// </summary>
        ISBN,
        /// <summary>
        /// Identifier
        /// </summary>
        BarCode,
        /// <summary>
        /// Identifier
        /// </summary>
        LCCN,
        /// <summary>
        /// Identifier
        /// </summary>
        CatalogNumber,
        /// <summary>
        /// Identifier
        /// </summary>
        LabelCode,
        /// <summary>
        /// Legal
        /// </summary>
        Owner,
        /// <summary>
        /// Legal
        /// </summary>
        Copyright,
        /// <summary>
        /// Legal
        /// </summary>
        Copyright_AS_Url,
        /// <summary>
        /// Legal
        /// </summary>
        Producer_Copyright,
        /// <summary>
        /// Legal
        /// </summary>
        TermsOfUse,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceName,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceChannel,
        /// <summary>
        /// Legal
        /// </summary>
        Service_AS_Url,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceProvider,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceProviderr_AS_Url,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceType,
        /// <summary>
        /// Legal
        /// </summary>
        NetworkName,
        /// <summary>
        /// Legal
        /// </summary>
        OriginalNetworkName,
        /// <summary>
        /// Legal
        /// </summary>
        Country,
        /// <summary>
        /// Legal
        /// </summary>
        TimeZone,
        /// <summary>
        /// Info
        /// </summary>
        Cover,
        /// <summary>
        /// Info
        /// </summary>
        Cover_Description,
        /// <summary>
        /// Info
        /// </summary>
        Cover_Type,
        /// <summary>
        /// Info
        /// </summary>
        Cover_Mime,
        /// <summary>
        /// Info
        /// </summary>
        Cover_Data,
        /// <summary>
        /// Info
        /// </summary>
        Lyrics,
        /// <summary>
        /// Personal
        /// </summary>
        Comment,
        /// <summary>
        /// Personal
        /// </summary>
        Rating,
        /// <summary>
        /// Personal
        /// </summary>
        Added_Date,
        /// <summary>
        /// Personal
        /// </summary>
        Played_First_Date,
        /// <summary>
        /// Personal
        /// </summary>
        Played_Last_Date,
        /// <summary>
        /// Personal
        /// </summary>
        Played_Count,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        EPG_Positions_Begin,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        EPG_Positions_End
    }

    /// <summary>
    /// Paramters for the video section.
    /// </summary>
    public enum ParametersVideo
    {
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        Count,
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        StreamCount,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind_AS_String,
        /// <summary>
        /// Number of the stream (base=0)
        /// </summary>
        StreamKindID,
        /// <summary>
        /// When multiple streams, number of the stream (base=1)
        /// </summary>
        StreamKindPos,
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        Inform,
        /// <summary>
        /// The ID for this stream in this file
        /// </summary>
        ID,
        /// <summary>
        /// The ID for this stream in this file
        /// </summary>
        ID_AS_String,
        /// <summary>
        /// The unique ID for this stream, should be copied with stream copy
        /// </summary>
        UniqueID,
        /// <summary>
        /// The menu ID for this stream in this file
        /// </summary>
        MenuID,
        /// <summary>
        /// The menu ID for this stream in this file
        /// </summary>
        MenuID_AS_String,
        /// <summary>
        /// Format used
        /// </summary>
        Format,
        /// <summary>
        /// Info about Format
        /// </summary>
        Format_AS_Info,
        /// <summary>
        /// Link
        /// </summary>
        Format_AS_Url,
        /// <summary>
        /// Version of this format
        /// </summary>
        Format_Version,
        /// <summary>
        /// Profile of the Format
        /// </summary>
        Format_Profile,
        /// <summary>
        /// Settings needed for decoder used, summary
        /// </summary>
        Format_Settings,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_BVOP,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_BVOP_AS_String,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_QPel,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_QPel_AS_String,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_GMC,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_GMC_AS_String,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_Matrix,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_Matrix_AS_String,
        /// <summary>
        /// Matrix, in binary format encoded BASE64. Order = intra, non-intra, gray intra, gray non-intra
        /// </summary>
        Format_Settings_Matrix_Data,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_CABAC,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_CABAC_AS_String,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_RefFrames,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_RefFrames_AS_String,
        /// <summary>
        /// Settings needed for decoder used, detailled
        /// </summary>
        Format_Settings_Pulldown,
        /// <summary>
        /// How this file is muxed in the container
        /// </summary>
        MuxingMode,
        /// <summary>
        /// Codec ID (found in some containers)
        /// </summary>
        CodecID,
        /// <summary>
        /// Info on the codec
        /// </summary>
        CodecID_AS_Info,
        /// <summary>
        /// Hint/popular name for this codec
        /// </summary>
        CodecID_AS_Hint,
        /// <summary>
        /// Homepage for more details about this codec
        /// </summary>
        CodecID_AS_Url,
        /// <summary>
        /// Manual description given by the container
        /// </summary>
        CodecID_Description,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_String,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_Family,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_Info,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_Url,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_CC,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Profile,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Description,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_PacketBitStream,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_BVOP,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_QPel,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_GMC,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_GMC_AS_String,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_Matrix,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_Matrix_Data,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_CABAC,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_RefFrames,
        /// <summary>
        /// Play time of the stream in ms
        /// </summary>
        Duration,
        /// <summary>
        /// Play time in format : XXx YYy only, YYy omited if zero
        /// </summary>
        Duration_AS_String,
        /// <summary>
        /// Play time in format : HHh MMmn SSs MMMms, XX omited if zero
        /// </summary>
        Duration_AS_String1,
        /// <summary>
        /// Play time in format : XXx YYy only, YYy omited if zero
        /// </summary>
        Duration_AS_String2,
        /// <summary>
        /// Play time in format : HH:MM:SS.MMM
        /// </summary>
        Duration_AS_String3,
        /// <summary>
        /// Bit rate mode (VBR, CBR)
        /// </summary>
        BitRate_Mode,
        /// <summary>
        /// Bit rate mode (Variable, Cconstant)
        /// </summary>
        BitRate_Mode_AS_String,
        /// <summary>
        /// Bit rate in bps
        /// </summary>
        BitRate,
        /// <summary>
        /// Bit rate (with measurement)
        /// </summary>
        BitRate_AS_String,
        /// <summary>
        /// Minimum Bit rate in bps
        /// </summary>
        BitRate_Minimum,
        /// <summary>
        /// Minimum Bit rate (with measurement)
        /// </summary>
        BitRate_Minimum_AS_String,
        /// <summary>
        /// Nominal Bit rate in bps
        /// </summary>
        BitRate_Nominal,
        /// <summary>
        /// Nominal Bit rate (with measurement)
        /// </summary>
        BitRate_Nominal_AS_String,
        /// <summary>
        /// Maximum Bit rate in bps
        /// </summary>
        BitRate_Maximum,
        /// <summary>
        /// Maximum Bit rate (with measurement)
        /// </summary>
        BitRate_Maximum_AS_String,
        /// <summary>
        /// Width in pixel
        /// </summary>
        Width,
		/// <summary>
		/// Original Width in pixel
		/// </summary>
		Width_Original,
        /// <summary>
        /// Width with measurement (pixel)
        /// </summary>
        Width_AS_String,
        /// <summary>
        /// Height in pixel
        /// </summary>
        Height,
		/// <summary>
		/// Original Height in pixel
		/// </summary>
		Height_Original,
        /// <summary>
        /// Width with measurement (pixel)
        /// </summary>
        Height_AS_String,
        /// <summary>
        /// Pixel Aspect ratio
        /// </summary>
        PixelAspectRatio,
        /// <summary>
        /// Pixel Aspect ratio
        /// </summary>
        PixelAspectRatio_AS_String,
        /// <summary>
        /// Original (in the raw stream) Pixel Aspect ratio
        /// </summary>
        PixelAspectRatio_Original,
        /// <summary>
        /// Original (in the raw stream) Pixel Aspect ratio
        /// </summary>
        PixelAspectRatio_Original_AS_String,
        /// <summary>
        /// Display Aspect ratio
        /// </summary>
        DisplayAspectRatio,
        /// <summary>
        /// Display Aspect ratio
        /// </summary>
        DisplayAspectRatio_AS_String,
        /// <summary>
        /// Original (in the raw stream) Display Aspect ratio
        /// </summary>
        DisplayAspectRatio_Original,
        /// <summary>
        /// Original (in the raw stream) Display Aspect ratio
        /// </summary>
        DisplayAspectRatio_Original_AS_String,
        /// <summary>
        /// Frame rate mode (CFR, VFR)
        /// </summary>
        FrameRate_Mode,
        /// <summary>
        /// Frame rate mode (Constant, Variable)
        /// </summary>
        FrameRate_Mode_AS_String,
        /// <summary>
        /// Frames per second
        /// </summary>
        FrameRate,
        /// <summary>
        /// Frames per second (with measurement)
        /// </summary>
        FrameRate_AS_String,
        /// <summary>
        /// Minimum Frames per second
        /// </summary>
        FrameRate_Minimum,
        /// <summary>
        /// Minimum Frames per second (with measurement)
        /// </summary>
        FrameRate_Minimum_AS_String,
        /// <summary>
        /// Nominal Frames per second
        /// </summary>
        FrameRate_Nominal,
        /// <summary>
        /// Nominal Frames per second (with measurement)
        /// </summary>
        FrameRate_Nominal_AS_String,
        /// <summary>
        /// Maximum Frames per second
        /// </summary>
        FrameRate_Maximum,
        /// <summary>
        /// Maximum Frames per second (with measurement)
        /// </summary>
        FrameRate_Maximum_AS_String,
        /// <summary>
        /// Original (in the raw stream) Frames per second
        /// </summary>
        FrameRate_Original,
        /// <summary>
        /// Original (in the raw stream) Frames per second (with measurement)
        /// </summary>
        FrameRate_Original_AS_String,
        /// <summary>
        /// Number of frames
        /// </summary>
        FrameCount,
        /// <summary>
        /// NTSC or PAL
        /// </summary>
        Standard,
        /// <summary>
        /// 16/24/32 
        /// </summary>
        Resolution,
        /// <summary>
        /// 16/24/32 bits
        /// </summary>
        Resolution_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Colorimetry,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        ScanType,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        ScanType_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        ScanOrder,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        ScanOrder_AS_String,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Interlacement,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Interlacement_AS_String,
        /// <summary>
        /// bits/(Pixel*Frame) (like Gordian Knot)
        /// </summary>
        Bits_IN__OpenBracket_Pixel_x_Frame_CloseBracket_,
        /// <summary>
        /// Delay fixed in the stream (relative) IN MS
        /// </summary>
        Delay,
        /// <summary>
        /// Delay with measurement
        /// </summary>
        Delay_AS_String,
        /// <summary>
        /// Delay with measurement
        /// </summary>
        Delay_AS_String1,
        /// <summary>
        /// Delay with measurement
        /// </summary>
        Delay_AS_String2,
        /// <summary>
        /// format : HH:MM:SS.MMM
        /// </summary>
        Delay_AS_String3,
        /// <summary>
        /// Delay settings (in case of timecode for example)
        /// </summary>
        Delay_Settings,
        /// <summary>
        /// Delay fixed in the raw stream (relative) IN MS
        /// </summary>
        Delay_Original,
        /// <summary>
        /// Delay with measurement
        /// </summary>
        Delay_Original_AS_String,
        /// <summary>
        /// Delay with measurement
        /// </summary>
        Delay_Original_AS_String1,
        /// <summary>
        /// Delay with measurement
        /// </summary>
        Delay_Original_AS_String2,
        /// <summary>
        /// format : HH:MM:SS.MMM
        /// </summary>
        Delay_Original_AS_String3,
        /// <summary>
        /// Delay settings (in case of timecode for example)
        /// </summary>
        Delay_Original_Settings,
        /// <summary>
        /// Stream size in bytes
        /// </summary>
        StreamSize,
        /// <summary>
        /// Streamsize in with percentage value
        /// </summary>
        StreamSize_AS_String,
        /// <summary>
        /// Streamsize with measurement
        /// </summary>
        StreamSize_AS_String1,
        /// <summary>
        /// Streamsize with measurement
        /// </summary>
        StreamSize_AS_String2,
        /// <summary>
        /// Streamsize with measurement
        /// </summary>
        StreamSize_AS_String3,
        /// <summary>
        /// Streamsize with measurement
        /// </summary>
        StreamSize_AS_String4,
        /// <summary>
        /// Streamsize in with percentage value
        /// </summary>
        StreamSize_AS_String5,
        /// <summary>
        /// Stream size divided by file size
        /// </summary>
        StreamSize_Proportion,
        /// <summary>
        /// How this stream file is aligned in the container
        /// </summary>
        Alignment,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Alignment_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Title,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Application,
        /// <summary>
        /// Technical
        /// </summary>
        Encoded_Application_AS_Url,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Name,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Version,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Date,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_Settings,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language_More,
        /// <summary>
        /// Temporal
        /// </summary>
        Encoded_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Tagged_Date,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encryption
    }

    /// <summary>
    /// Parameters for the audio section.
    /// </summary>
    public enum ParametersAudio
    {
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        Count,
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        StreamCount,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind_AS_String,
        /// <summary>
        /// Number of the stream (base=0)
        /// </summary>
        StreamKindID,
        /// <summary>
        /// When multiple streams, number of the stream (base=1)
        /// </summary>
        StreamKindPos,
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        Inform,
        /// <summary>
        /// The ID of this stream in this file
        /// </summary>
        ID,
        /// <summary>
        /// The ID of this stream in this file
        /// </summary>
        ID_AS_String,
        /// <summary>
        /// A unique ID for this stream, should be copied with stream copy
        /// </summary>
        UniqueID,
        /// <summary>
        /// The menu ID for this stream in this file
        /// </summary>
        MenuID,
        /// <summary>
        /// The menu ID for this stream in this file
        /// </summary>
        MenuID_AS_String,
        /// <summary>
        /// Format used
        /// </summary>
        Format,
        /// <summary>
        /// Info about the format
        /// </summary>
        Format_AS_Info,
        /// <summary>
        /// Homepage of this format
        /// </summary>
        Format_AS_Url,
        /// <summary>
        /// Version of this format
        /// </summary>
        Format_Version,
        /// <summary>
        /// Profile of this Format
        /// </summary>
        Format_Profile,
        /// <summary>
        /// Settings needed for decoder used, summary
        /// </summary>
        Format_Settings,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_SBR,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_SBR_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_PS,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_PS_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_Floor,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_Firm,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_Endianness,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_Sign,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_Law,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Format_Settings_ITU,
        /// <summary>
        /// How this stream is muxed in the container
        /// </summary>
        MuxingMode,
        /// <summary>
        /// Codec ID (found in some containers)
        /// </summary>
        CodecID,
        /// <summary>
        /// Info about codec ID
        /// </summary>
        CodecID_AS_Info,
        /// <summary>
        /// Hint/popular name for this codec ID
        /// </summary>
        CodecID_AS_Hint,
        /// <summary>
        /// Homepage for more details about this codec ID
        /// </summary>
        CodecID_AS_Url,
        /// <summary>
        /// Manual description given by the container
        /// </summary>
        CodecID_Description,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_String,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_Family,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_Info,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_Url,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_AS_CC,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Description,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Profile,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_Automatic,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_Floor,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_Firm,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_Endianness,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_Sign,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_Law,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Codec_Settings_ITU,
        /// <summary>
        /// Play time of the stream
        /// </summary>
        Duration,
        /// <summary>
        /// Play time in format : XXx YYy only, YYy omited if zero
        /// </summary>
        Duration_AS_String,
        /// <summary>
        /// Play time in format : HHh MMmn SSs MMMms, XX omited if zero
        /// </summary>
        Duration_AS_String1,
        /// <summary>
        /// Play time in format : XXx YYy only, YYy omited if zero
        /// </summary>
        Duration_AS_String2,
        /// <summary>
        /// Play time in format : HH:MM:SS.MMM
        /// </summary>
        Duration_AS_String3,
        /// <summary>
        /// Bit rate mode (VBR, CBR)
        /// </summary>
        BitRate_Mode,
        /// <summary>
        /// Bit rate mode (Constant, Variable)
        /// </summary>
        BitRate_Mode_AS_String,
        /// <summary>
        /// Bit rate in bps
        /// </summary>
        BitRate,
        /// <summary>
        /// Bit rate (with measurement)
        /// </summary>
        BitRate_AS_String,
        /// <summary>
        /// Minimum Bit rate in bps
        /// </summary>
        BitRate_Minimum,
        /// <summary>
        /// Minimum Bit rate (with measurement)
        /// </summary>
        BitRate_Minimum_AS_String,
        /// <summary>
        /// Nominal Bit rate in bps
        /// </summary>
        BitRate_Nominal,
        /// <summary>
        /// Nominal Bit rate (with measurement)
        /// </summary>
        BitRate_Nominal_AS_String,
        /// <summary>
        /// Maximum Bit rate in bps
        /// </summary>
        BitRate_Maximum,
        /// <summary>
        /// Maximum Bit rate (with measurement)
        /// </summary>
        BitRate_Maximum_AS_String,
        /// <summary>
        /// Number of channels
        /// </summary>
        Channel_OpenBracket_s_CloseBracket_,
        /// <summary>
        /// Number of channels (with measurement)
        /// </summary>
        Channel_OpenBracket_s_CloseBracket__AS_String,
        /// <summary>
        /// Position of channels
        /// </summary>
        ChannelPositions,
        /// <summary>
        /// Position of channels (x/y.z format)
        /// </summary>
        ChannelPositions_AS_String2,
        /// <summary>
        /// Sampling rate
        /// </summary>
        SamplingRate,
        /// <summary>
        /// in KHz
        /// </summary>
        SamplingRate_AS_String,
        /// <summary>
        /// Frame count
        /// </summary>
        SamplingCount,
        /// <summary>
        /// Resolution in bits (8, 16, 20, 24)
        /// </summary>
        Resolution,
        /// <summary>
        ///  n bits
        /// </summary>
        Resolution_AS_String,
        /// <summary>
        /// Current stream size divided by uncompressed stream size
        /// </summary>
        CompressionRatio,
        /// <summary>
        /// Delay fixed in the stream (relative)
        /// </summary>
        Delay,
        /// <summary>
        /// Delay in format : XXx YYy only, YYy omited if zero
        /// </summary>
        Delay_AS_String,
        /// <summary>
        /// Delay in format : HHh MMmn SSs MMMms, XX omited if zero
        /// </summary>
        Delay_AS_String1,
        /// <summary>
        /// Delay in format : XXx YYy only, YYy omited if zero
        /// </summary>
        Delay_AS_String2,
        /// <summary>
        /// Delay in format : HH:MM:SS.MMM
        /// </summary>
        Delay_AS_String3,
        /// <summary>
        /// Delay fixed in the stream (absolute / video)
        /// </summary>
        Video_Delay,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Video_Delay_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Video_Delay_AS_String1,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Video_Delay_AS_String2,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Video_Delay_AS_String3,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Video0_Delay,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Video0_Delay_AS_String,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Video0_Delay_AS_String1,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Video0_Delay_AS_String2,
        /// <summary>
        /// Deprecated, do not use in new projects
        /// </summary>
        Video0_Delay_AS_String3,
        /// <summary>
        /// The gain to apply to reach 89dB SPL on playback
        /// </summary>
        ReplayGain_Gain,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        ReplayGain_Gain_AS_String,
        /// <summary>
        /// The maximum absolute peak value of the item
        /// </summary>
        ReplayGain_Peak,
        /// <summary>
        /// Streamsize in bytes
        /// </summary>
        StreamSize,
        /// <summary>
        /// Streamsize in with percentage value
        /// </summary>
        StreamSize_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String1,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String2,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String3,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String4,
        /// <summary>
        /// Streamsize in with percentage value
        /// </summary>
        StreamSize_AS_String5,
        /// <summary>
        /// Stream size divided by file size
        /// </summary>
        StreamSize_Proportion,
        /// <summary>
        /// How this stream file is aligned in the container
        /// </summary>
        Alignment,
        /// <summary>
        /// Where this stream file is aligned in the container
        /// </summary>
        Alignment_AS_String,
        /// <summary>
        /// Between how many video frames the stream is inserted
        /// </summary>
        Interleave_VideoFrames,
        /// <summary>
        /// Between how much time (ms) the stream is inserted
        /// </summary>
        Interleave_Duration,
        /// <summary>
        /// Between how much time and video frames the stream is inserted (with measurement)
        /// </summary>
        Interleave_Duration_AS_String,
        /// <summary>
        /// How much time is buffered before the first video frame
        /// </summary>
        Interleave_Preload,
        /// <summary>
        /// How much time is buffered before the first video frame (with measurement)
        /// </summary>
        Interleave_Preload_AS_String,
        /// <summary>
        /// Name of the track
        /// </summary>
        Title,
        /// <summary>
        /// Software used to create the file
        /// </summary>
        Encoded_Library,
        /// <summary>
        /// Software used to create the file
        /// </summary>
        Encoded_Library_AS_String,
        /// <summary>
        /// Info from the software
        /// </summary>
        Encoded_Library_AS_Name,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Version,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Date,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_Settings,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language_More,
        /// <summary>
        /// Temporal
        /// </summary>
        Encoded_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Tagged_Date,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encryption,
    }

    /// <summary>
    /// Parameters for the chapters section
    /// </summary>
    public enum ParametersChapters
    {
        /// <summary>
        /// Count of objects available in this stream
        /// </summary>
        Count,
        /// <summary>
        /// Count of streams of that kind available
        /// </summary>
        StreamCount,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind_AS_String,
        /// <summary>
        /// Number of the stream (base=0)
        /// </summary>
        StreamKindID,
        /// <summary>
        /// When multiple streams, number of the stream (base=1)
        /// </summary>
        StreamKindPos,
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        Inform,
        /// <summary>
        /// A ID for this stream in this file
        /// </summary>
        ID,
        /// <summary>
        /// A ID for this stream in this file
        /// </summary>
        ID_AS_String,
        /// <summary>
        /// A unique ID for this stream, should be copied with stream copy
        /// </summary>
        UniqueID,
        /// <summary>
        /// Format used
        /// </summary>
        Format,
        /// <summary>
        /// Info about Format
        /// </summary>
        Format_AS_Info,
        /// <summary>
        /// Link
        /// </summary>
        Format_AS_Url,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_String,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_Info,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_Url,
        /// <summary>
        /// Total number of chapters
        /// </summary>
        Total,
        /// <summary>
        /// Name of the track
        /// </summary>
        Title,
        /// <summary>
        /// Language (2-letter ISO 639-1 if exists, else 3-letter ISO 639-2, and with optional ISO 3166-1 country separated by a dash if available, e.g. en, en-us, zh-cn)
        /// </summary>
        Language,
        /// <summary>
        /// Language (full)
        /// </summary>
        Language_AS_String,
    }

    /// <summary>
    /// Parameters for the text section.
    /// </summary>
    public enum ParametersText
    {
        /// <summary>
        /// Count of objects available in this stream
        /// </summary>
        Count,
        /// <summary>
        /// Count of streams of that kind available
        /// </summary>
        StreamCount,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind_AS_String,
        /// <summary>
        /// Number of the stream (base=0)
        /// </summary>
        StreamKindID,
        /// <summary>
        /// When multiple streams, number of the stream (base=1)
        /// </summary>
        StreamKindPos,
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        Inform,
        /// <summary>
        /// A ID for this stream in this file
        /// </summary>
        ID,
        /// <summary>
        /// A ID for this stream in this file
        /// </summary>
        ID_AS_String,
        /// <summary>
        /// A unique ID for this stream, should be copied with stream copy
        /// </summary>
        UniqueID,
        /// <summary>
        /// A menu ID for this stream in this file
        /// </summary>
        MenuID,
        /// <summary>
        /// A menu ID for this stream in this file
        /// </summary>
        MenuID_AS_String,
        /// <summary>
        /// Format used
        /// </summary>
        Format,
        /// <summary>
        /// Info about Format
        /// </summary>
        Format_AS_Info,
        /// <summary>
        /// Link
        /// </summary>
        Format_AS_Url,
        /// <summary>
        /// Codec ID (found in some containers)
        /// </summary>
        CodecID,
        /// <summary>
        /// Info about codec ID
        /// </summary>
        CodecID_AS_Info,
        /// <summary>
        /// A hint for this codec ID
        /// </summary>
        CodecID_AS_Hint,
        /// <summary>
        /// A link for more details about this codec ID
        /// </summary>
        CodecID_AS_Url,
        /// <summary>
        /// Manual description given by the container
        /// </summary>
        CodecID_Description,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_String,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_Info,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_Url,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_CC,
        /// <summary>
        /// Play time of the stream
        /// </summary>
        Duration,
        /// <summary>
        /// Play time (formated)
        /// </summary>
        Duration_AS_String,
        /// <summary>
        /// Play time in format : HHh MMmn SSs MMMms, XX omited if zero
        /// </summary>
        Duration_AS_String1,
        /// <summary>
        /// Play time in format : XXx YYy only, YYy omited if zero
        /// </summary>
        Duration_AS_String2,
        /// <summary>
        /// Play time in format : HH:MM:SS.MMM
        /// </summary>
        Duration_AS_String3,
        /// <summary>
        /// Bit rate mode (VBR, CBR)
        /// </summary>
        BitRate_Mode,
        /// <summary>
        /// Bit rate mode (VBR, CBR)
        /// </summary>
        BitRate_Mode_AS_String,
        /// <summary>
        /// Bit rate in bps
        /// </summary>
        BitRate,
        /// <summary>
        /// Bit rate (with measurement)
        /// </summary>
        BitRate_AS_String,
        /// <summary>
        /// Width
        /// </summary>
        Width,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Width_AS_String,
        /// <summary>
        /// Height
        /// </summary>
        Height,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Height_AS_String,
        /// <summary>
        /// Frame count
        /// </summary>
        FrameCount,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Resolution,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Resolution_AS_String,
        /// <summary>
        /// Delay fixed in the stream (relative)
        /// </summary>
        Delay,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Delay_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Delay_AS_String1,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Delay_AS_String2,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Delay_AS_String3,
        /// <summary>
        /// Delay fixed in the stream (absolute / video)
        /// </summary>
        Video_Delay,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Video_Delay_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Video_Delay_AS_String1,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Video_Delay_AS_String2,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Video_Delay_AS_String3,
        /// <summary>
        /// Deprecated
        /// </summary>
        Video0_Delay,
        /// <summary>
        /// Deprecated
        /// </summary>
        Video0_Delay_AS_String,
        /// <summary>
        /// Deprecated
        /// </summary>
        Video0_Delay_AS_String1,
        /// <summary>
        /// Deprecated
        /// </summary>
        Video0_Delay_AS_String2,
        /// <summary>
        /// Deprecated
        /// </summary>
        Video0_Delay_AS_String3,
        /// <summary>
        /// Stream size in bytes
        /// </summary>
        StreamSize,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String1,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String2,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String3,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String4,
        /// <summary>
        /// With proportion
        /// </summary>
        StreamSize_AS_String5,
        /// <summary>
        /// Stream size divided by file size
        /// </summary>
        StreamSize_Proportion,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Title,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Name,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Version,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Date,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_Settings,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language_More,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Summary,
        /// <summary>
        /// Temporal
        /// </summary>
        Encoded_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Tagged_Date,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encryption,
    }

    /// <summary>
    /// Parameters for the menu section.
    /// </summary>
    public enum ParametersMenu
    {
        /// <summary>
        /// Count of objects available in this stream
        /// </summary>
        Count,
        /// <summary>
        /// Count of streams of that kind available
        /// </summary>
        StreamCount,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind_AS_String,
        /// <summary>
        /// Number of the stream (base=0)
        /// </summary>
        StreamKindID,
        /// <summary>
        /// When multiple streams, number of the stream (base=1)
        /// </summary>
        StreamKindPos,
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        Inform,
        /// <summary>
        /// A ID for this stream in this file
        /// </summary>
        ID,
        /// <summary>
        /// A ID for this stream in this file
        /// </summary>
        ID_AS_String,
        /// <summary>
        /// A unique ID for this stream, should be copied with stream copy
        /// </summary>
        UniqueID,
        /// <summary>
        /// A menu ID for this stream in this file
        /// </summary>
        MenuID,
        /// <summary>
        /// A menu ID for this stream in this file
        /// </summary>
        MenuID_AS_String,
        /// <summary>
        /// Format used
        /// </summary>
        Format,
        /// <summary>
        /// Info about Format
        /// </summary>
        Format_AS_Info,
        /// <summary>
        /// Link
        /// </summary>
        Format_AS_Url,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Codec_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Codec_AS_Info,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Codec_AS_Url,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        List_StreamKind,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        List_StreamPos,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        List,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        List_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Title,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language_More,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceName,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceChannel,
        /// <summary>
        /// Legal
        /// </summary>
        Service_AS_Url,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceProvider,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceProviderr_AS_Url,
        /// <summary>
        /// Legal
        /// </summary>
        ServiceType,
        /// <summary>
        /// Legal
        /// </summary>
        NetworkName,
        /// <summary>
        /// Legal
        /// </summary>
        Original_AS_NetworkName,
        /// <summary>
        /// Legal
        /// </summary>
        Countries,
        /// <summary>
        /// Legal
        /// </summary>
        TimeZones,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        EPG_Positions_Begin,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        EPG_Positions_End,
    }

    /// <summary>
    /// Parameters for the image section.
    /// </summary>
    public enum ParametersImage
    {
        /// <summary>
        /// Count of objects available in this stream
        /// </summary>
        Count,
        /// <summary>
        /// Count of streams of that kind available
        /// </summary>
        StreamCount,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind,
        /// <summary>
        /// Stream type name
        /// </summary>
        StreamKind_AS_String,
        /// <summary>
        /// Number of the stream (base=0)
        /// </summary>
        StreamKindID,
        /// <summary>
        /// When multiple streams, number of the stream (base=1)
        /// </summary>
        StreamKindPos,
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        Inform,
        /// <summary>
        /// A ID for this stream in this file
        /// </summary>
        ID,
        /// <summary>
        /// A unique ID for this stream, should be copied with stream copy
        /// </summary>
        UniqueID,
        /// <summary>
        /// Name of the track
        /// </summary>
        Title,
        /// <summary>
        /// Format used
        /// </summary>
        Format,
        /// <summary>
        /// Info about Format
        /// </summary>
        Format_AS_Info,
        /// <summary>
        /// Link
        /// </summary>
        Format_AS_Url,
        /// <summary>
        /// Profile of the Format
        /// </summary>
        Format_Profile,
        /// <summary>
        /// Codec ID (found in some containers)
        /// </summary>
        CodecID,
        /// <summary>
        /// Info about codec ID
        /// </summary>
        CodecID_AS_Info,
        /// <summary>
        /// A hint for this codec ID
        /// </summary>
        CodecID_AS_Hint,
        /// <summary>
        /// A link for more details about this codec ID
        /// </summary>
        CodecID_AS_Url,
        /// <summary>
        /// Manual description given by the container
        /// </summary>
        CodecID_Description,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_String,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_Family,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_Info,
        /// <summary>
        /// Deprecated
        /// </summary>
        Codec_AS_Url,
        /// <summary>
        /// Width
        /// </summary>
        Width,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Width_AS_String,
        /// <summary>
        /// Height
        /// </summary>
        Height,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Height_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Resolution,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Resolution_AS_String,
        /// <summary>
        /// Stream size in bytes
        /// </summary>
        StreamSize,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String1,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String2,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String3,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String4,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_AS_String5,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        StreamSize_Proportion,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Name,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Version,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_AS_Date,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encoded_Library_Settings,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Language_AS_String,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Summary,
        /// <summary>
        /// Temporal
        /// </summary>
        Encoded_Date,
        /// <summary>
        /// Temporal
        /// </summary>
        Tagged_Date,
        /// <summary>
        /// ToDo: Whats that?
        /// </summary>
        Encryption
    }
    #endregion
}
