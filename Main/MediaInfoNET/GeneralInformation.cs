using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaInfoNET
{
    /// <summary>
    /// Holds the general informations about a file.
    /// </summary>
    /// <remarks>Documented by CFI, 2009-03-27</remarks>
    public class GeneralInformation
    {
        /// <summary>
        /// Gets or sets the media info to which this informations belong.
        /// </summary>
        /// <value>The media info.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public MediaInfo MediaInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInformation"/> class.
        /// </summary>
        /// <param name="mediaInfo">The media info.</param>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        internal GeneralInformation(MediaInfo mediaInfo) { MediaInfo = mediaInfo; }

        #region MediaInfo properties
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        public int Count { get { return Convert.ToInt32(MediaInfo.Get(0, ParametersGeneral.Count)); } }
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        public int StreamCount { get { return Convert.ToInt32(MediaInfo.Get(0, ParametersGeneral.StreamCount)); } }
        /// <summary>
        /// Gets the kind of the stream.
        /// </summary>
        /// <value>The kind of the stream.</value>
        public StreamKindStruct StreamKind { get { return new StreamKindStruct(0, StreamKindEnum.General, MediaInfo); } }
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        /// <value>The inform.</value>
        public string Inform { get { return MediaInfo.Get(0, ParametersGeneral.Inform); } }
        /// <summary>
        /// The unique ID for this stream, should be copied with stream copy
        /// </summary>
        /// <value>The unique ID.</value>
        public string UniqueID { get { return MediaInfo.Get(0, ParametersGeneral.UniqueID); } }
        /// <summary>
        /// Number of general streams.
        /// </summary>
        /// <value>The general count.</value>
        public int GeneralCount
        {
            get
            {
                string cnt = MediaInfo.Get(0, ParametersGeneral.GeneralCount);
                int count;
                if (Int32.TryParse(cnt, out count))
                    return count;
                else
                    return 1;
            }
        }
        /// <summary>
        /// Number of video streams.
        /// </summary>
        /// <value>The video count.</value>
        public int VideoCount
        {
            get
            {
                string cnt = MediaInfo.Get(0, ParametersGeneral.VideoCount);
                int count;
                if (Int32.TryParse(cnt, out count))
                    return count;
                else
                    return 0;
            }
        }
        /// <summary>
        /// Number of audio streams.
        /// </summary>
        /// <value>The audio count.</value>
        public int AudioCount
        {
            get
            {
                string cnt = MediaInfo.Get(0, ParametersGeneral.AudioCount);
                int count;
                if (Int32.TryParse(cnt, out count))
                    return count;
                else
                    return 0;
            }
        }
        /// <summary>
        /// Number of text streams.
        /// </summary>
        /// <value>The text count.</value>
        public int TextCount
        {
            get
            {
                string cnt = MediaInfo.Get(0, ParametersGeneral.TextCount);
                int count;
                if (Int32.TryParse(cnt, out count))
                    return count;
                else
                    return 0;
            }
        }
        /// <summary>
        /// Number of chapters streams.
        /// </summary>
        /// <value>The chapters count.</value>
        public int ChaptersCount
        {
            get
            {
                string cnt = MediaInfo.Get(0, ParametersGeneral.ChaptersCount);
                int count;
                if (Int32.TryParse(cnt, out count))
                    return count;
                else
                    return 0;
            }
        }
        /// <summary>
        /// Number of image streams.
        /// </summary>
        /// <value>The image count.</value>
        public int ImageCount
        {
            get
            {
                string cnt = MediaInfo.Get(0, ParametersGeneral.ImageCount);
                int count;
                if (Int32.TryParse(cnt, out count))
                    return count;
                else
                    return 0;
            }
        }
        /// <summary>
        /// Number of menu streams.
        /// </summary>
        /// <value>The menu count.</value>
        public int MenuCount
        {
            get
            {
                string cnt = MediaInfo.Get(0, ParametersGeneral.MenuCount);
                int count;
                if (Int32.TryParse(cnt, out count))
                    return count;
                else
                    return 0;
            }
        }
        /// <summary>
        /// Gets the details of the video streams.
        /// </summary>
        /// <value>The video stream details.</value>
        public List<StreamDetailsStruct> VideoStreams
        {
            get
            {
                List<StreamDetailsStruct> details = new List<StreamDetailsStruct>();

                string[] formats = MediaInfo.Get(0, ParametersGeneral.Video_Format_List).Split('/');
                string[] hints = MediaInfo.Get(0, ParametersGeneral.Video_Format_WithHint_List).Split('/');
                string[] codecs = MediaInfo.Get(0, ParametersGeneral.Video_Codec_List).Split('/');
                string[] langs = MediaInfo.Get(0, ParametersGeneral.Video_Language_List).Split('/');

                for (int i = 0; i < formats.Length; i++)
                    details.Add(new StreamDetailsStruct(formats[i].Trim(), hints[i].Trim(), codecs[i].Trim(), langs[i].Trim()));

                return details;
            }
        }
        /// <summary>
        /// Gets the details of the audio streams.
        /// </summary>
        /// <value>The audio stream details.</value>
        public List<StreamDetailsStruct> AudioStreams
        {
            get
            {
                List<StreamDetailsStruct> details = new List<StreamDetailsStruct>();

                string[] formats = MediaInfo.Get(0, ParametersGeneral.Audio_Format_List).Split('/');
                string[] hints = MediaInfo.Get(0, ParametersGeneral.Audio_Format_WithHint_List).Split('/');
                string[] codecs = MediaInfo.Get(0, ParametersGeneral.Audio_Codec_List).Split('/');
                string[] langs = MediaInfo.Get(0, ParametersGeneral.Audio_Language_List).Split('/');

                for (int i = 0; i < formats.Length; i++)
                    details.Add(new StreamDetailsStruct(formats[i].Trim(), hints[i].Trim(), codecs[i].Trim(), langs[i].Trim()));

                return details;
            }
        }
        /// <summary>
        /// Gets the details of the text streams.
        /// </summary>
        /// <value>The text stream details.</value>
        public List<StreamDetailsStruct> TextStreams
        {
            get
            {
                List<StreamDetailsStruct> details = new List<StreamDetailsStruct>();

                string[] formats = MediaInfo.Get(0, ParametersGeneral.Text_Format_List).Split('/');
                string[] hints = MediaInfo.Get(0, ParametersGeneral.Text_Format_WithHint_List).Split('/');
                string[] codecs = MediaInfo.Get(0, ParametersGeneral.Text_Codec_List).Split('/');
                string[] langs = MediaInfo.Get(0, ParametersGeneral.Text_Language_List).Split('/');

                for (int i = 0; i < formats.Length; i++)
                    details.Add(new StreamDetailsStruct(formats[i].Trim(), hints[i].Trim(), codecs[i].Trim(), langs[i].Trim()));

                return details;
            }
        }
        /// <summary>
        /// Gets the details of the Chapters.
        /// </summary>
        /// <value>The Chapters details.</value>
        public List<StreamDetailsStruct> Chapters
        {
            get
            {
                List<StreamDetailsStruct> details = new List<StreamDetailsStruct>();

                string[] formats = MediaInfo.Get(0, ParametersGeneral.Chapters_Format_List).Split('/');
                string[] hints = MediaInfo.Get(0, ParametersGeneral.Chapters_Format_WithHint_List).Split('/');
                string[] codecs = MediaInfo.Get(0, ParametersGeneral.Chapters_Codec_List).Split('/');
                string[] langs = MediaInfo.Get(0, ParametersGeneral.Chapters_Language_List).Split('/');

                for (int i = 0; i < formats.Length; i++)
                    details.Add(new StreamDetailsStruct(formats[i].Trim(), hints[i].Trim(), codecs[i].Trim(), langs[i].Trim()));

                return details;
            }
        }
        /// <summary>
        /// Gets the details of the image streams.
        /// </summary>
        /// <value>The image stream details.</value>
        public List<StreamDetailsStruct> ImageStreams
        {
            get
            {
                List<StreamDetailsStruct> details = new List<StreamDetailsStruct>();

                string[] formats = MediaInfo.Get(0, ParametersGeneral.Image_Format_List).Split('/');
                string[] hints = MediaInfo.Get(0, ParametersGeneral.Image_Format_WithHint_List).Split('/');
                string[] codecs = MediaInfo.Get(0, ParametersGeneral.Image_Codec_List).Split('/');
                string[] langs = MediaInfo.Get(0, ParametersGeneral.Image_Language_List).Split('/');

                for (int i = 0; i < formats.Length; i++)
                    details.Add(new StreamDetailsStruct(formats[i].Trim(), hints[i].Trim(), codecs[i].Trim(), langs[i].Trim()));

                return details;
            }
        }
        /// <summary>
        /// Gets the details of the menu streams.
        /// </summary>
        /// <value>The menu stream details.</value>
        public List<StreamDetailsStruct> MenuStreams
        {
            get
            {
                List<StreamDetailsStruct> details = new List<StreamDetailsStruct>();

                string[] formats = MediaInfo.Get(0, ParametersGeneral.Menu_Format_List).Split('/');
                string[] hints = MediaInfo.Get(0, ParametersGeneral.Menu_Format_WithHint_List).Split('/');
                string[] codecs = MediaInfo.Get(0, ParametersGeneral.Menu_Codec_List).Split('/');
                string[] langs = MediaInfo.Get(0, ParametersGeneral.Menu_Language_List).Split('/');

                for (int i = 0; i < formats.Length; i++)
                    details.Add(new StreamDetailsStruct(formats[i].Trim(), hints[i].Trim(), codecs[i].Trim(), langs[i].Trim()));

                return details;
            }
        }
        /// <summary>
        /// Complete name (Folder+Name+Extension)
        /// </summary>
        /// <value>The name of the complete.</value>
        public string CompleteName { get { return MediaInfo.Get(0, ParametersGeneral.CompleteName); } }
        /// <summary>
        /// Folder name only
        /// </summary>
        /// <value>The name of the Folder.</value>
        public string FolderName { get { return MediaInfo.Get(0, ParametersGeneral.FolderName); } }
        /// <summary>
        /// File name only
        /// </summary>
        /// <value>The File name.</value>
        public string FileName { get { return MediaInfo.Get(0, ParametersGeneral.FileName); } }
        /// <summary>
        /// File extension only
        /// </summary>
        /// <value>The name of the extension.</value>
        public string FileExtension { get { return MediaInfo.Get(0, ParametersGeneral.FileExtension); } }
        /// <summary>
        /// Gets the format used.
        /// </summary>
        /// <value>The format.</value>
        public FormatStruct Format
        {
            get
            {
                return new FormatStruct()
                    {
                        Name = MediaInfo.Get(0, ParametersGeneral.Format),
                        Info = MediaInfo.Get(0, ParametersGeneral.Format_AS_Info),
                        URL = MediaInfo.Get(0, ParametersGeneral.Format_AS_Url),
                        Extensions = new List<string>(MediaInfo.Get(0, ParametersGeneral.Format_AS_Extensions).Split(' ')),
                        Version = MediaInfo.Get(0, ParametersGeneral.Format_Version),
                        Profile = MediaInfo.Get(0, ParametersGeneral.Format_Profile),
                        Settings = MediaInfo.Get(0, ParametersGeneral.Format_Settings)
                    };
            }
        }
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
                        Name = MediaInfo.Get(0, ParametersGeneral.CodecID),
                        Info = MediaInfo.Get(0, ParametersGeneral.CodecID_AS_Info),
                        Hint = MediaInfo.Get(0, ParametersGeneral.CodecID_AS_Hint),
                        URL = MediaInfo.Get(0, ParametersGeneral.CodecID_AS_Url),
                        Description = MediaInfo.Get(0, ParametersGeneral.CodecID_Description)
                    };
            }
        }
        /// <summary>
        /// If Audio and video are muxed.
        /// </summary>
        /// <value><c>true</c> if interleaved; otherwise, <c>false</c>.</value>
        public bool Interleaved
        {
            get
            {
                bool il;
                if (bool.TryParse(MediaInfo.Get(0, ParametersGeneral.Interleaved), out il))
                    return il;
                else
                    return false;
            }
        }
        /// <summary>
        /// File size in bytes.
        /// </summary>
        /// <value>The size of the file.</value>
        public long FileSize { get { return Convert.ToInt64(MediaInfo.Get(0, ParametersGeneral.FileSize)); } }
        /// <summary>
        /// Play time of the stream in ms.
        /// </summary>
        /// <value>The duration.</value>
        public long Duration { get { return Convert.ToInt64(MediaInfo.Get(0, ParametersGeneral.Duration)); } }
        /// <summary>
        /// Bit rate of all streams in bps.
        /// </summary>
        /// <value>The overall bit rate.</value>
        public BitRateStruct OverallBitRate
        {
            get
            {
                int val, min, max, nom;
                if (!Int32.TryParse(MediaInfo.Get(0, ParametersGeneral.OverallBitRate), out val))
                    val = -1;
                if (!Int32.TryParse(MediaInfo.Get(0, ParametersGeneral.OverallBitRate_Minimum), out min))
                    min = -1;
                if (!Int32.TryParse(MediaInfo.Get(0, ParametersGeneral.OverallBitRate_Nominal), out nom))
                    nom = -1;
                if (!Int32.TryParse(MediaInfo.Get(0, ParametersGeneral.OverallBitRate_Maximum), out max))
                    max = -1;

                return new BitRateStruct()
                    {
                        Value = val,
                        Minimum = min,
                        Nominal = nom,
                        Maximum = max,
                        Mode = BitRateMode.NA
                    };
            }
        }
        /// <summary>
        /// Gets the size of the header.
        /// </summary>
        /// <value>The size of the header.</value>
        public long HeaderSize { get { return Methods.TryParseLong(MediaInfo.Get(0, ParametersGeneral.HeaderSize)); } }
        /// <summary>
        /// Gets the size of the data.
        /// </summary>
        /// <value>The size of the data.</value>
        public long DataSize { get { return Methods.TryParseLong(MediaInfo.Get(0, ParametersGeneral.DataSize)); } }
        /// <summary>
        /// Gets the size of the footer.
        /// </summary>
        /// <value>The size of the footer.</value>
        public long FooterSize { get { return Methods.TryParseLong(MediaInfo.Get(0, ParametersGeneral.FooterSize)); } }
        /// <summary>
        /// The gain to apply to reach 89dB SPL on playback.
        /// </summary>
        /// <value>The album replay gain.</value>
        public double AlbumReplayGain { get { return Methods.TryParseDouble(MediaInfo.Get(0, ParametersGeneral.Album_ReplayGain_Gain)); } }
        /// <summary>
        /// The maximum absolute peak value of the item.
        /// </summary>
        /// <value>The album replay gain peak.</value>
        public double AlbumReplayGainPeak { get { return Methods.TryParseDouble(MediaInfo.Get(0, ParametersGeneral.Album_ReplayGain_Peak)); } }
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public TitleStruct Title
        {
            get
            {
                return new TitleStruct()
                    {
                        Title = MediaInfo.Get(0, ParametersGeneral.Title),
                        More = MediaInfo.Get(0, ParametersGeneral.Title_AS_More),
                        URL = MediaInfo.Get(0, ParametersGeneral.Title_AS_Url)
                    };
            }
        }
        /// <summary>
        /// Univers movies belong to, e.g. Starwars, Stargate, Buffy, Dragonballs.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain { get { return MediaInfo.Get(0, ParametersGeneral.Domain); } }
        /// <summary>
        /// Name of the series, e.g. Starwars movies, Stargate SG-1, Stargate Atlantis, Buffy, Angel.
        /// </summary>
        /// <value>The collection.</value>
        public string Collection { get { return MediaInfo.Get(0, ParametersGeneral.Collection); } }
        /// <summary>
        /// Name of the season, e.g. Strawars first Trilogy, Season 1.
        /// </summary>
        /// <value>The season.</value>
        public string Season { get { return MediaInfo.Get(0, ParametersGeneral.Season); } }
        /// <summary>
        /// Number of the Season.
        /// </summary>
        /// <value>The season position.</value>
        public int SeasonPosition { get { return Methods.TryParseInt(MediaInfo.Get(0, ParametersGeneral.Season_Position)); } }
        /// <summary>
        /// Place of the season e.g. 2 of 7.
        /// </summary>
        /// <value>The season positions total.</value>
        public int SeasonPositionsTotal { get { return Methods.TryParseInt(MediaInfo.Get(0, ParametersGeneral.Season_Position_Total)); } }
        /// <summary>
        /// Gets the movie.
        /// </summary>
        /// <value>The movie.</value>
        public MovieStruct Movie
        {
            get
            {
                return new MovieStruct()
                    {
                        Title = MediaInfo.Get(0, ParametersGeneral.Movie),
                        More = MediaInfo.Get(0, ParametersGeneral.Movie_AS_More),
                        Country = MediaInfo.Get(0, ParametersGeneral.Movie_AS_Country),
                        URL = MediaInfo.Get(0, ParametersGeneral.Movie_AS_Url)
                    };
            }
        }
        /// <summary>
        /// Gets the album.
        /// </summary>
        /// <value>The album.</value>
        public AlbumStruct Album
        {
            get
            {
                return new AlbumStruct()
                    {
                        Title = MediaInfo.Get(0, ParametersGeneral.Album),
                        More = MediaInfo.Get(0, ParametersGeneral.Album_AS_More),
                        Sort = MediaInfo.Get(0, ParametersGeneral.Album_AS_Sort)
                    };
            }
        }
        /// <summary>
        /// Gets the comic.
        /// </summary>
        /// <value>The comic.</value>
        public ComicStruct Comic
        {
            get
            {
                return new ComicStruct()
                    {
                        Title = MediaInfo.Get(0, ParametersGeneral.Comic),
                        More = MediaInfo.Get(0, ParametersGeneral.Comic_AS_More),
                        PositionTotal = MediaInfo.Get(0, ParametersGeneral.Comic_AS_Position_Total)
                    };
            }
        }
        /// <summary>
        /// Name of the part. e.g. CD1, CD2.
        /// </summary>
        /// <value>The part.</value>
        public string Part { get { return MediaInfo.Get(0, ParametersGeneral.Part); } }
        /// <summary>
        /// Number of the part.
        /// </summary>
        /// <value>The part position.</value>
        public int PartPosition { get { return Methods.TryParseInt(MediaInfo.Get(0, ParametersGeneral.Part_AS_Position)); } }
        /// <summary>
        /// Place of the part e.g. 2 of 3.
        /// </summary>
        /// <value>The part positions total.</value>
        public int PartPositionsTotal { get { return Methods.TryParseInt(MediaInfo.Get(0, ParametersGeneral.Part_AS_Position_Total)); } }
        /// <summary>
        /// Gets the track.
        /// </summary>
        /// <value>The track.</value>
        public TrackStruct Track
        {
            get
            {
                return new TrackStruct()
                    {
                        Title = MediaInfo.Get(0, ParametersGeneral.Track),
                        More = MediaInfo.Get(0, ParametersGeneral.Track_AS_More),
                        URL = MediaInfo.Get(0, ParametersGeneral.Track_AS_Url),
                        Sort = MediaInfo.Get(0, ParametersGeneral.Track_AS_Sort),
                        Position = Methods.TryParseInt(MediaInfo.Get(0, ParametersGeneral.Track_AS_Position)),
                        PositionTotal = Methods.TryParseInt(MediaInfo.Get(0, ParametersGeneral.Track_AS_Position_Total))
                    };
            }
        }
        /// <summary>
        /// Name of the chapter.
        /// </summary>
        /// <value>The chapter.</value>
        public string Chapter { get { return MediaInfo[ParametersGeneral.Chapter]; } }
        /// <summary>
        /// Name of the subtrack.
        /// </summary>
        /// <value>The sub track.</value>
        public string SubTrack { get { return MediaInfo[ParametersGeneral.SubTrack]; } }
        /// <summary>
        /// Original name of album, serie...
        /// </summary>
        /// <value>The orginal album.</value>
        public string OrginalAlbum { get { return MediaInfo[ParametersGeneral.Original_AS_Album]; } }
        /// <summary>
        /// Original lyricist(s)/text writer(s).
        /// </summary>
        /// <value>The orginal lyricist.</value>
        public string OrginalLyricist { get { return MediaInfo[ParametersGeneral.Original_AS_Lyricist]; } }
        /// <summary>
        /// Original name of the movie.
        /// </summary>
        /// <value>The orginal movie.</value>
        public string OrginalMovie { get { return MediaInfo[ParametersGeneral.Original_AS_Movie]; } }
        /// <summary>
        /// Original name of the part in the original support.
        /// </summary>
        /// <value>The orginal part.</value>
        public string OrginalPart { get { return MediaInfo[ParametersGeneral.Original_AS_Part]; } }
        /// <summary>
        /// Original artist(s)/performer(s).
        /// </summary>
        /// <value>The orginal performer.</value>
        public string OrginalPerformer { get { return MediaInfo[ParametersGeneral.Original_AS_Performer]; } }
        /// <summary>
        /// The date/year that the item was originaly released.
        /// </summary>
        /// <value>The orginal released date.</value>
        public string OrginalReleasedDate { get { return MediaInfo[ParametersGeneral.Original_AS_Released_Date]; } }
        /// <summary>
        /// Original name of the track in the original support.
        /// </summary>
        /// <value>The orginal track.</value>
        public string OrginalTrack { get { return MediaInfo[ParametersGeneral.Original_AS_Track]; } }
        /// <summary>
        /// Gets the performer.
        /// </summary>
        /// <value>The performer.</value>
        public PerformerStruct Performer
        {
            get
            {
                return new PerformerStruct()
                    {
                        Title = MediaInfo[ParametersGeneral.Performer],
                        Sort = MediaInfo[ParametersGeneral.Performer_AS_Sort],
                        URL = MediaInfo[ParametersGeneral.Performer_AS_Url]
                    };
            }
        }
        /// <summary>
        /// Band/orchestra/accompaniment/musician.
        /// </summary>
        /// <value>The accompaniment.</value>
        public string Accompaniment { get { return MediaInfo[ParametersGeneral.Accompaniment]; } }
        /// <summary>
        /// Name of the original composer.
        /// </summary>
        /// <value>The Composer.</value>
        public string Composer { get { return MediaInfo[ParametersGeneral.Composer]; } }
        /// <summary>
        /// Nationality of the main composer of the item, mostly for classical music.
        /// </summary>
        /// <value>The Composer Nationality.</value>
        public string ComposerNationality { get { return MediaInfo[ParametersGeneral.Composer_AS_Nationality]; } }
        /// <summary>
        /// The person who arranged the piece. e.g. Ravel.
        /// </summary>
        /// <value>The Arranger.</value>
        public string Arranger { get { return MediaInfo[ParametersGeneral.Arranger]; } }
        /// <summary>
        /// The person who wrote the lyrics for a musical item.
        /// </summary>
        /// <value>The Lyricist.</value>
        public string Lyricist { get { return MediaInfo[ParametersGeneral.Lyricist]; } }
        /// <summary>
        /// The artist(s) who performed the work. In classical music this would be the conductor, orchestra, soloists.
        /// </summary>
        /// <value>The Conductor.</value>
        public string Conductor { get { return MediaInfo[ParametersGeneral.Conductor]; } }
        /// <summary>
        /// Name of the director.
        /// </summary>
        /// <value>The Director.</value>
        public string Director { get { return MediaInfo[ParametersGeneral.Director]; } }
        /// <summary>
        /// Name of the assistant director.
        /// </summary>
        /// <value>The AssistantDirector.</value>
        public string AssistantDirector { get { return MediaInfo[ParametersGeneral.AssistantDirector]; } }
        /// <summary>
        /// The name of the director of photography, also known as cinematographer.
        /// </summary>
        /// <value>The DirectorOfPhotography.</value>
        public string DirectorOfPhotography { get { return MediaInfo[ParametersGeneral.DirectorOfPhotography]; } }
        /// <summary>
        /// The name of the sound engineer or sound recordist.
        /// </summary>
        /// <value>The SoundEngineer.</value>
        public string SoundEngineer { get { return MediaInfo[ParametersGeneral.SoundEngineer]; } }
        /// <summary>
        /// The person who oversees the artists and craftspeople who build the sets.
        /// </summary>
        /// <value>The ArtDirector.</value>
        public string ArtDirector { get { return MediaInfo[ParametersGeneral.ArtDirector]; } }
        /// <summary>
        /// The person responsible for designing the Overall visual appearance of a movie.
        /// </summary>
        /// <value>The ProductionDesigner.</value>
        public string ProductionDesigner { get { return MediaInfo[ParametersGeneral.ProductionDesigner]; } }
        /// <summary>
        /// The name of the choregrapher.
        /// </summary>
        /// <value>The Choregrapher.</value>
        public string Choregrapher { get { return MediaInfo[ParametersGeneral.Choregrapher]; } }
        /// <summary>
        /// The name of the costume designer.
        /// </summary>
        /// <value>The CostumeDesigner.</value>
        public string CostumeDesigner { get { return MediaInfo[ParametersGeneral.CostumeDesigner]; } }
        /// <summary>
        /// Real name of an actor or actress playing a role in the movie.
        /// </summary>
        /// <value>The Actor.</value>
        public string Actor { get { return MediaInfo[ParametersGeneral.Actor]; } }
        /// <summary>
        /// Name of the character an actor or actress plays in this movie.
        /// </summary>
        /// <value>The Actor Character.</value>
        public string Actor_Character { get { return MediaInfo[ParametersGeneral.Actor_Character]; } }
        /// <summary>
        /// The author of the story or script.
        /// </summary>
        /// <value>The person who written the plot.</value>
        public string WrittenBy { get { return MediaInfo[ParametersGeneral.WrittenBy]; } }
        /// <summary>
        /// The author of the screenplay or scenario (used for movies and TV shows).
        /// </summary>
        /// <value>The ScreenplayBy.</value>
        public string ScreenplayBy { get { return MediaInfo[ParametersGeneral.ScreenplayBy]; } }
        /// <summary>
        /// Editors name
        /// </summary>
        /// <value>The Editors name.</value>
        public string Editor { get { return MediaInfo[ParametersGeneral.EditedBy]; } }
        /// <summary>
        /// Name of the person or organization that commissioned the subject of the file
        /// </summary>
        /// <value>The Commissioner.</value>
        public string Commissioner { get { return MediaInfo[ParametersGeneral.CommissionedBy]; } }
        /// <summary>
        /// Name of the producer of the movie.
        /// </summary>
        /// <value>The Producer.</value>
        public string Producer { get { return MediaInfo[ParametersGeneral.Producer]; } }
        /// <summary>
        /// The name of a co-producer.
        /// </summary>
        /// <value>The CoProducer.</value>
        public string CoProducer { get { return MediaInfo[ParametersGeneral.CoProducer]; } }
        /// <summary>
        /// The name of an executive producer.
        /// </summary>
        /// <value>The Executive Producer.</value>
        public string ExecutiveProducer { get { return MediaInfo[ParametersGeneral.ExecutiveProducer]; } }
        /// <summary>
        /// Main music-artist for a movie
        /// </summary>
        /// <value>The MusicBy.</value>
        public string MusicBy { get { return MediaInfo[ParametersGeneral.MusicBy]; } }
        /// <summary>
        /// Company the item is mainly distributed by
        /// </summary>
        /// <value>The Distributor.</value>
        public string Distributor { get { return MediaInfo[ParametersGeneral.DistributedBy]; } }
        /// <summary>
        /// Name of the person or organization who supplied the original subject
        /// </summary>
        /// <value>The Original Source Distributor.</value>
        public string OriginalSourceFormDistributor { get { return MediaInfo[ParametersGeneral.OriginalSourceForm_AS_DistributedBy]; } }
        /// <summary>
        /// The engineer who mastered the content for a physical medium or for digital distribution.
        /// </summary>
        /// <value>The MasteredBy.</value>
        public string MasteredBy { get { return MediaInfo[ParametersGeneral.MasteredBy]; } }
        /// <summary>
        /// Name of the person or organisation that encoded/ripped the audio file.
        /// </summary>
        /// <value>The EncodedBy.</value>
        public string EncodedBy { get { return MediaInfo[ParametersGeneral.EncodedBy]; } }
        /// <summary>
        /// Name of the artist(s), that interpreted, remixed, or otherwise modified the item.
        /// </summary>
        /// <value>The Remixer.</value>
        public string RemixedBy { get { return MediaInfo[ParametersGeneral.RemixedBy]; } }
        /// <summary>
        /// Main production studio
        /// </summary>
        /// <value>The Production Studio.</value>
        public string ProductionStudio { get { return MediaInfo[ParametersGeneral.ProductionStudio]; } }
        /// <summary>
        /// A very general tag for everyone else that wants to be listed.
        /// </summary>
        /// <value>The ThanksTo.</value>
        public string ThanksTo { get { return MediaInfo[ParametersGeneral.ThanksTo]; } }
        /// <summary>
        /// Name of the organization publishing the album (i.e. the 'record label') or movie.
        /// </summary>
        /// <value>The Publisher.</value>
        public string Publisher { get { return MediaInfo[ParametersGeneral.Publisher]; } }
        /// <summary>
        /// Publishers official webpage.
        /// </summary>
        /// <value>The PublisherURL.</value>
        public string PublisherURL { get { return MediaInfo[ParametersGeneral.Publisher_AS_URL]; } }
        /// <summary>
        /// Brand or trademark associated with the marketing of music recordings and music videos.
        /// </summary>
        /// <value>The Label.</value>
        public string Label { get { return MediaInfo[ParametersGeneral.Label]; } }
        /// <summary>
        /// The main genre of the audio or video. e.g. classical, ambient-house, synthpop, sci-fi, drama, etc.
        /// </summary>
        /// <value>The Genre.</value>
        public string Genre { get { return MediaInfo[ParametersGeneral.Genre]; } }
        /// <summary>
        /// Intended to reflect the mood of the item with a few keywords, e.g. Romantic, Sad, Uplifting, etc.
        /// </summary>
        /// <value>The Mood.</value>
        public string Mood { get { return MediaInfo[ParametersGeneral.Mood]; } }
        /// <summary>
        /// The type of the item. e.g. Documentary, Feature Film, Cartoon, Music Video, Music, Sound FX, etc.
        /// </summary>
        /// <value>The ContentType.</value>
        public string ContentType { get { return MediaInfo[ParametersGeneral.ContentType]; } }
        /// <summary>
        /// Describes the topic of the file, such as Aerial view of Seattle...
        /// </summary>
        /// <value>The Subject.</value>
        public string Subject { get { return MediaInfo[ParametersGeneral.Subject]; } }
        /// <summary>
        /// A short description of the contents, such as Two birds flying.
        /// </summary>
        /// <value>The Description.</value>
        public string Description { get { return MediaInfo[ParametersGeneral.Description]; } }
        /// <summary>
        /// Keywords to the item separated by a comma, used for searching.
        /// </summary>
        /// <value>The Keywords.</value>
        public string Keywords { get { return MediaInfo[ParametersGeneral.Keywords]; } }
        /// <summary>
        /// A plot outline or a summary of the story.
        /// </summary>
        /// <value>The Summary.</value>
        public string Summary { get { return MediaInfo[ParametersGeneral.Summary]; } }
        /// <summary>
        /// A description of the story line of the item.
        /// </summary>
        /// <value>The Synopsys.</value>
        public string Synopsys { get { return MediaInfo[ParametersGeneral.Synopsys]; } }
        /// <summary>
        /// Describes the period that the piece is from or about. e.g. Renaissance.
        /// </summary>
        /// <value>The Period.</value>
        public string Period { get { return MediaInfo[ParametersGeneral.Period]; } }
        /// <summary>
        /// Depending on the country it's the format of the rating of a movie (P, R, X in the USA, an age in other countries or a URI defining a logo).
        /// </summary>
        /// <value>The LawRating.</value>
        public string LawRating { get { return MediaInfo[ParametersGeneral.LawRating]; } }
        /// <summary>
        /// Reason for the law rating
        /// </summary>
        /// <value>The LawRating Reason.</value>
        public string LawRatingReason { get { return MediaInfo[ParametersGeneral.LawRating_Reason]; } }
        /// <summary>
        /// The ICRA rating. (Previously RSACi)
        /// </summary>
        /// <value>The ICRA.</value>
        public string ICRA { get { return MediaInfo[ParametersGeneral.ICRA]; } }
        /// <summary>
        /// The date/year that the item was released.
        /// </summary>
        /// <value>The Released Date.</value>
        public string ReleasedDate { get { return MediaInfo[ParametersGeneral.Released_Date]; } }
        /// <summary>
        /// The time/date/year that the recording began.
        /// </summary>
        /// <value>The Recorded Date.</value>
        public string RecordedDate { get { return MediaInfo[ParametersGeneral.Recorded_Date]; } }
        /// <summary>
        /// The time/date/year that the encoding of this item was completed began.
        /// </summary>
        /// <value>The Encoded Date.</value>
        public string EncodedDate { get { return MediaInfo[ParametersGeneral.Encoded_Date]; } }
        /// <summary>
        /// The time/date/year that the tags were done for this item.
        /// </summary>
        /// <value>The TaggedDate.</value>
        public string TaggedDate { get { return MediaInfo[ParametersGeneral.Tagged_Date]; } }
        /// <summary>
        /// The time/date/year that the composition of the music/script began.
        /// </summary>
        /// <value>The Written Date.</value>
        public string WrittenDate { get { return MediaInfo[ParametersGeneral.Written_Date]; } }
        /// <summary>
        /// The time/date/year that the item was tranfered to a digitalmedium.
        /// </summary>
        /// <value>The Mastered Date.</value>
        public string MasteredDate { get { return MediaInfo[ParametersGeneral.Mastered_Date]; } }
        /// <summary>
        /// The time that the file was created on the file system.
        /// </summary>
        /// <value>The File Created Date.</value>
        [Obsolete("Use FileInfo instead!")]
        public string FileCreatedDate { get { return MediaInfo[ParametersGeneral.File_Created_Date]; } }
        /// <summary>
        /// The time that the file was modified on the file system.
        /// </summary>
        /// <value>The File Modified Date.</value>
        [Obsolete("Use FileInfo instead!")]
        public string FileModifiedDate { get { return MediaInfo[ParametersGeneral.File_Modified_Date]; } }
        /// <summary>
        /// Location where track was recorded. (See COMPOSITION_LOCATION for format)
        /// </summary>
        /// <value>The Recorded Location.</value>
        public string RecordedLocation { get { return MediaInfo[ParametersGeneral.Recorded_Location]; } }
        /// <summary>
        /// Location that the item was originaly designed/written. Information should be stored in the following format: country code, state/province, city where the coutry code is the same 2 octets as in Internet domains, or possibly ISO-3166. e.g. US, Texas, Austin or US, , Austin.
        /// </summary>
        /// <value>The Written Location.</value>
        public string WrittenLocation { get { return MediaInfo[ParametersGeneral.Written_Location]; } }
        /// <summary>
        /// Location, where an item is archived, e.eg. Louvre, Paris, France
        /// </summary>
        /// <value>The Archival Location.</value>
        public string ArchivalLocation { get { return MediaInfo[ParametersGeneral.Archival_Location]; } }
        /// <summary>
        /// Name of the software package used to create the file, such as Microsoft WaveEdit.
        /// </summary>
        /// <value>The Encoded Application.</value>
        public string EncodedApplication { get { return MediaInfo[ParametersGeneral.Encoded_Application]; } }
        /// <summary>
        /// Name of the software package used to create the file, such as Microsoft WaveEdit.
        /// </summary>
        /// <value>The Encoded Application URL.</value>
        public string EncodedApplicationURL { get { return MediaInfo[ParametersGeneral.Encoded_Application_AS_Url]; } }
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
                        Title = MediaInfo[ParametersGeneral.Encoded_Library],
                        Name = MediaInfo[ParametersGeneral.Encoded_Library_AS_Name],
                        Version = MediaInfo[ParametersGeneral.Encoded_Library_AS_Version],
                        Date = MediaInfo[ParametersGeneral.Encoded_Library_AS_Date],
                        Settings = MediaInfo[ParametersGeneral.Encoded_Library_Settings]
                    };
            }
        }
        /// <summary>
        /// Describes whether an image has been cropped and, if so, how it was cropped.
        /// </summary>
        /// <value>The Cropped.</value>
        public string Cropped { get { return MediaInfo[ParametersGeneral.Cropped]; } }
        /// <summary>
        /// Specifies the size of the original subject of the file. eg 8.5 in h, 11 in w
        /// </summary>
        /// <value>The Dimensions.</value>
        public string Dimensions { get { return MediaInfo[ParametersGeneral.Dimensions]; } }
        /// <summary>
        /// Stores dots per inch setting of the digitizer used to produce the file
        /// </summary>
        /// <value>The Dots Per Inch.</value>
        public string DotsPerInch { get { return MediaInfo[ParametersGeneral.DotsPerInch]; } }
        /// <summary>
        /// Describes the changes in lightness settings on the digitizer required to produce the file
        /// </summary>
        /// <value>The Lightness.</value>
        public string Lightness { get { return MediaInfo[ParametersGeneral.Lightness]; } }
        /// <summary>
        /// Gets the original source.
        /// </summary>
        /// <value>The original source.</value>
        public OriginalSourceStruct OriginalSource
        {
            get
            {
                return new OriginalSourceStruct()
                    {
                        Medium = MediaInfo[ParametersGeneral.OriginalSourceMedium],
                        Form = MediaInfo[ParametersGeneral.OriginalSourceForm],
                        NumberOfColors = MediaInfo[ParametersGeneral.OriginalSourceForm_AS_NumColors],
                        Name = MediaInfo[ParametersGeneral.OriginalSourceForm_AS_Name],
                        Cropped = MediaInfo[ParametersGeneral.OriginalSourceForm_AS_Cropped],
                        Sharpness = MediaInfo[ParametersGeneral.OriginalSourceForm_AS_Sharpness]
                    };
            }
        }
        /// <summary>
        /// Software used to tag this file
        /// </summary>
        /// <value>The Tagged Application.</value>
        public string TaggedApplication { get { return MediaInfo[ParametersGeneral.Tagged_Application]; } }
        /// <summary>
        /// Average number of beats per minute
        /// </summary>
        /// <value>The BPM.</value>
        public double BPM { get { return Methods.TryParseDouble(MediaInfo[ParametersGeneral.BPM]); } }
        /// <summary>
        /// International Standard Recording Code, excluding the ISRC prefix and including hyphens.
        /// </summary>
        /// <value>The ISRC.</value>
        public string ISRC { get { return MediaInfo[ParametersGeneral.ISRC]; } }
        /// <summary>
        /// International Standard Book Number.
        /// </summary>
        /// <value>The ISBN.</value>
        public string ISBN { get { return MediaInfo[ParametersGeneral.ISBN]; } }
        /// <summary>
        /// EAN-13 (13-digit European Article Numbering) or UPC-A (12-digit Universal Product Code) bar code identifier.
        /// </summary>
        /// <value>The BarCode.</value>
        public string BarCode { get { return MediaInfo[ParametersGeneral.BarCode]; } }
        /// <summary>
        /// Library of Congress Control Number.
        /// </summary>
        /// <value>The LCCN.</value>
        public string LCCN { get { return MediaInfo[ParametersGeneral.LCCN]; } }
        /// <summary>
        /// A label-specific catalogue number used to identify the release. e.g. TIC 01.
        /// </summary>
        /// <value>The Catalog Number.</value>
        public string CatalogNumber { get { return MediaInfo[ParametersGeneral.CatalogNumber]; } }
        /// <summary>
        /// A 4-digit or 5-digit number to identify the record label, typically printed as (LC) xxxx or (LC) 0xxxx on CDs medias or covers, with only the number being stored.
        /// </summary>
        /// <value>The LabelCode.</value>
        public string LabelCode { get { return MediaInfo[ParametersGeneral.LabelCode]; } }
        /// <summary>
        /// Owner of the file
        /// </summary>
        /// <value>The Owner.</value>
        public string Owner { get { return MediaInfo[ParametersGeneral.Owner]; } }
        /// <summary>
        /// Copyright attribution.
        /// </summary>
        /// <value>The Copyright.</value>
        public string Copyright { get { return MediaInfo[ParametersGeneral.Copyright]; } }
        /// <summary>
        /// Link to a site with copyright/legal information.
        /// </summary>
        /// <value>The Copyright URL.</value>
        public string CopyrightURL { get { return MediaInfo[ParametersGeneral.Copyright_AS_Url]; } }
        /// <summary>
        /// The copyright information as per the productioncopyright holder.
        /// </summary>
        /// <value>The Producer Copyright.</value>
        public string ProducerCopyright { get { return MediaInfo[ParametersGeneral.Producer_Copyright]; } }
        /// <summary>
        /// License information, e.g., All Rights Reserved,Any Use Permitted.
        /// </summary>
        /// <value>The Terms Of Use.</value>
        public string TermsOfUse { get { return MediaInfo[ParametersGeneral.TermsOfUse]; } }
        /// <summary>
        /// Get the Service Name
        /// </summary>
        /// <value>The Service Name.</value>
        public string ServiceName { get { return MediaInfo[ParametersGeneral.ServiceName]; } }
        /// <summary>
        /// The Service Channel.
        /// </summary>
        /// <value>The Service Channel.</value>
        public string ServiceChannel { get { return MediaInfo[ParametersGeneral.ServiceChannel]; } }
        /// <summary>
        /// The Service Url.
        /// </summary>
        /// <value>The Service Url.</value>
        public string ServiceURL { get { return MediaInfo[ParametersGeneral.Service_AS_Url]; } }
        /// <summary>
        /// The Service Provider.
        /// </summary>
        /// <value>The Service Provider.</value>
        public string ServiceProvider { get { return MediaInfo[ParametersGeneral.ServiceProvider]; } }
        /// <summary>
        /// The Service Provider Url.
        /// </summary>
        /// <value>The Service Provider Url.</value>
        public string ServiceProviderURL { get { return MediaInfo[ParametersGeneral.ServiceProviderr_AS_Url]; } }
        /// <summary>
        /// The Service Type.
        /// </summary>
        /// <value>The Service Type.</value>
        public string ServiceType { get { return MediaInfo[ParametersGeneral.ServiceType]; } }
        /// <summary>
        /// The Network Name.
        /// </summary>
        /// <value>The Network Name.</value>
        public string NetworkName { get { return MediaInfo[ParametersGeneral.NetworkName]; } }
        /// <summary>
        /// The Original Network Name.
        /// </summary>
        /// <value>The Original Network Name.</value>
        public string OriginalNetworkName { get { return MediaInfo[ParametersGeneral.OriginalNetworkName]; } }
        /// <summary>
        /// The Country.
        /// </summary>
        /// <value>The Country.</value>
        public string Country { get { return MediaInfo[ParametersGeneral.Country]; } }
        /// <summary>
        /// The Time Zone.
        /// </summary>
        /// <value>The Time Zone.</value>
        public string TimeZone { get { return MediaInfo[ParametersGeneral.TimeZone]; } }
        /// <summary>
        /// The Cover.
        /// </summary>
        /// <value>The Cover.</value>
        public CoverStruct? Cover
        {
            get
            {
                bool isAvailable;
                if (!bool.TryParse(MediaInfo[ParametersGeneral.Cover], out isAvailable))
                    return null;
                return new CoverStruct()
                    {
                        Description = MediaInfo[ParametersGeneral.Cover_Description],
                        Type = MediaInfo[ParametersGeneral.Cover_Type],
                        Mime = MediaInfo[ParametersGeneral.Cover_Mime],
                        Data = Convert.FromBase64String(MediaInfo[ParametersGeneral.Cover_Data])
                    };
            }
        }
        /// <summary>
        /// Text of a song
        /// </summary>
        /// <value>The Lyrics.</value>
        public string Lyrics { get { return MediaInfo[ParametersGeneral.Lyrics]; } }
        /// <summary>
        /// Any comment related to the content.
        /// </summary>
        /// <value>The Comment.</value>
        public string Comment { get { return MediaInfo[ParametersGeneral.Comment]; } }
        /// <summary>
        /// A numeric value defining how much a person likes the song/movie. The number is between 0 and 5 with decimal values possible (e.g. 2.7), 5(.0) being the highest possible rating.
        /// </summary>
        /// <value>The Rating.</value>
        public double Rating { get { return Methods.TryParseDouble(MediaInfo[ParametersGeneral.Rating]); } }
        /// <summary>
        /// Date/year the item was added to the owners collection
        /// </summary>
        /// <value>The Added Date.</value>
        public string AddedDate { get { return MediaInfo[ParametersGeneral.Added_Date]; } }
        /// <summary>
        /// The date, the owner first played an item
        /// </summary>
        /// <value>The Played First Date.</value>
        public string PlayedFirstDate { get { return MediaInfo[ParametersGeneral.Played_First_Date]; } }
        /// <summary>
        /// The date, the owner last played an item
        /// </summary>
        /// <value>The Played Last Date.</value>
        public string PlayedLastDate { get { return MediaInfo[ParametersGeneral.Played_Last_Date]; } }
        /// <summary>
        /// Number of times an item was played
        /// </summary>
        /// <value>The Played Count.</value>
        public int PlayedCount { get { return Methods.TryParseInt(MediaInfo[ParametersGeneral.Played_Count]); } }
        /// <summary>
        /// The EPG Positions Begin.
        /// </summary>
        /// <value>The EPG Positions Begin.</value>
        public string EPGPositionsBegin { get { return MediaInfo[ParametersGeneral.EPG_Positions_Begin]; } }
        /// <summary>
        /// The EPG Positions End.
        /// </summary>
        /// <value>The EPG Positions End.</value>
        public string EPGPositionsEnd { get { return MediaInfo[ParametersGeneral.EPG_Positions_End]; } }
        #endregion
    }
}
