using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MediaInfoNET
{
    /// <summary>
    /// Holds informations about a menu.
    /// </summary>
    /// <remarks>Documented by CFI, 2009-03-27</remarks>
    public class Menu
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
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="mediaInfo">The media info.</param>
        /// <param name="id">The id.</param>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        internal Menu(MediaInfo mediaInfo, int id) { MediaInfo = mediaInfo; Id = id; }

        #region MediaInfo properties
        /// <summary>
        /// Number of objects available in this stream
        /// </summary>
        public int Count { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersMenu.Count)); } }
        /// <summary>
        /// Number of streams of this kind available
        /// </summary>
        public int StreamCount { get { return Convert.ToInt32(MediaInfo.Get(Id, ParametersMenu.StreamCount)); } }
        /// <summary>
        /// Gets the kind of the stream.
        /// </summary>
        /// <value>The kind of the stream.</value>
        public StreamKindStruct StreamKind { get { return new StreamKindStruct(Id, StreamKindEnum.Menu, MediaInfo); } }
        /// <summary>
        /// Last **Inform** call
        /// </summary>
        /// <value>The inform.</value>
        public string Inform { get { return MediaInfo.Get(Id, ParametersMenu.Inform); } }
        /// <summary>
        /// The unique ID for this stream, should be copied with stream copy
        /// </summary>
        /// <value>The unique ID.</value>
        public string UniqueID { get { return MediaInfo.Get(Id, ParametersMenu.UniqueID); } }
        /// <summary>
        /// The menu ID for this stream in this file
        /// </summary>
        /// <value>The unique ID.</value>
        public int MenuID { get { return Methods.TryParseInt(MediaInfo.Get(Id, ParametersMenu.MenuID)); } }
        /// <summary>
        /// Format used.
        /// </summary>
        /// <value>The Format.</value>
        public string Format { get { return MediaInfo[ParametersMenu.Format, Id]; } }
        /// <summary>
        /// Info about Format.
        /// </summary>
        /// <value>The Format Info.</value>
        public string FormatInfo { get { return MediaInfo[ParametersMenu.Format_AS_Info, Id]; } }
        /// <summary>
        /// Url to infos about the format.
        /// </summary>
        /// <value>The Format Url.</value>
        public string FormatUrl { get { return MediaInfo[ParametersMenu.Format_AS_Url, Id]; } }
        /// <summary>
        /// List of programs available.
        /// </summary>
        /// <value>The StreamKind List.</value>
        public string StreamKindList { get { return MediaInfo[ParametersMenu.List_StreamKind, Id]; } }
        /// <summary>
        /// List of programs available.
        /// </summary>
        /// <value>The StreamPos List.</value>
        public string StreamPosList { get { return MediaInfo[ParametersMenu.List_StreamPos, Id]; } }
        /// <summary>
        /// List of programs available.
        /// </summary>
        /// <value>The List.</value>
        public string List { get { return MediaInfo[ParametersMenu.List, Id]; } }
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public CultureInfo Language { get { return Methods.TryParseCulture(MediaInfo[ParametersMenu.Language, Id]); } }
        /// <summary>
        /// More info about Language (e.g. Director's Comment).
        /// </summary>
        /// <value>The language details.</value>
        public string LanguageMore { get { return MediaInfo[ParametersMenu.Language_More, Id]; } }
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>The service.</value>
        public ServiceStruct Service
        {
            get
            {
                return new ServiceStruct()
                    {
                        Name = MediaInfo[ParametersMenu.ServiceName, Id],
                        Channel = MediaInfo[ParametersMenu.ServiceChannel, Id],
                        URL = MediaInfo[ParametersMenu.Service_AS_Url, Id],
                        Provider = MediaInfo[ParametersMenu.ServiceProvider, Id],
                        Type = MediaInfo[ParametersMenu.ServiceType, Id]
                    };
            }
        }
        /// <summary>
        /// Gets the name of the network.
        /// </summary>
        /// <value>The name of the network.</value>
        public string NetworkName { get { return MediaInfo[ParametersMenu.NetworkName, Id]; } }
        /// <summary>
        /// Gets the name of the original network.
        /// </summary>
        /// <value>The name of the original network.</value>
        public string OriginalNetworkName { get { return MediaInfo[ParametersMenu.Original_AS_NetworkName, Id]; } }
        /// <summary>
        /// Gets the countries.
        /// </summary>
        /// <value>The countries.</value>
        public string Countries { get { return MediaInfo[ParametersMenu.Countries, Id]; } }
        /// <summary>
        /// Gets the time zones.
        /// </summary>
        /// <value>The time zones.</value>
        public string TimeZones { get { return MediaInfo[ParametersMenu.TimeZones, Id]; } }
        /// <summary>
        /// Gets the EPG positions begin.
        /// </summary>
        /// <value>The EPG positions begin.</value>
        public string EPGPositionsBegin { get { return MediaInfo[ParametersMenu.EPG_Positions_Begin, Id]; } }
        /// <summary>
        /// Gets the EPG positions end.
        /// </summary>
        /// <value>The EPG positions end.</value>
        public string EPGPositionsEnd { get { return MediaInfo[ParametersMenu.EPG_Positions_End, Id]; } }
        #endregion
    }
}
