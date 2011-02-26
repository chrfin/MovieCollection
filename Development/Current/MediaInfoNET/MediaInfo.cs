using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MediaInfoNET
{
    /// <summary>
    /// Provides all available information about a media file.
    /// </summary>
    /// <remarks>Documented by CFI, 2009-03-27</remarks>
    public class MediaInfo
    {
        #region Import of DLL functions
        //DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_New();
        [DllImport("MediaInfo.dll")]
        private static extern void MediaInfo_Delete(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Open(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string FileName);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Open(IntPtr Handle, IntPtr FileName);
        [DllImport("MediaInfo.dll")]
        private static extern void MediaInfo_Close(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Inform(IntPtr Handle, IntPtr Reserved);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Inform(IntPtr Handle, IntPtr Reserved);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_GetI(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_GetI(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, [MarshalAs(UnmanagedType.LPWStr)] string Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Option(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string Option, [MarshalAs(UnmanagedType.LPWStr)] string Value);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Option(IntPtr Handle, IntPtr Option, IntPtr Value);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_State_Get(IntPtr Handle);
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Count_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber);
        #endregion

        #region private variables
        private bool MustUseAnsi;
        private IntPtr Handle;
        #endregion

        #region Con-/Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaInfo"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public MediaInfo(string filename)
        {
            Handle = MediaInfo_New();

            if (Environment.OSVersion.ToString().IndexOf("Windows") == -1)
                MustUseAnsi = true;
            else
                MustUseAnsi = false;

            Open(filename);
        }
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="MediaInfo"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        ~MediaInfo()
        {
            MediaInfo_Delete(Handle);
        }
        #endregion

        #region Open/Close
        /// <summary>
        /// Opens the specified file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public int Open(string filename)
        {
            this.filename = filename;
            if (MustUseAnsi)
            {
                IntPtr FileName_Ptr = Marshal.StringToHGlobalAnsi(filename);
                int result = (int)MediaInfoA_Open(Handle, FileName_Ptr);
                Marshal.FreeHGlobal(FileName_Ptr);

                return result;
            }
            else
                return (int)MediaInfo_Open(Handle, filename);
        }
        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public void Close()
        {
            MediaInfo_Close(Handle);
            filename = string.Empty;
        }
        #endregion

        #region "native" Methods
        /// <summary>
        /// Get the complete list of the properties of the opened file.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Inform()
        {
            if (MustUseAnsi)
                return Marshal.PtrToStringAnsi(MediaInfoA_Inform(Handle, (IntPtr)0));
            else
                return Marshal.PtrToStringUni(MediaInfo_Inform(Handle, (IntPtr)0));
        }

        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamKind">Kind of the stream.</param>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(StreamKindEnum streamKind, int streamNumber, int parameter)
        {
            return Get(streamKind, streamNumber, parameter, InfoKind.Text);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamKind">Kind of the stream.</param>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="kindOfInfo">The kind of info.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(StreamKindEnum streamKind, int streamNumber, int parameter, InfoKind kindOfInfo)
        {
            if (MustUseAnsi)
                return Marshal.PtrToStringAnsi(MediaInfoA_GetI(Handle, (IntPtr)streamKind, (IntPtr)streamNumber, (IntPtr)parameter, (IntPtr)kindOfInfo));
            else
                return Marshal.PtrToStringUni(MediaInfo_GetI(Handle, (IntPtr)streamKind, (IntPtr)streamNumber, (IntPtr)parameter, (IntPtr)kindOfInfo));
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(int streamNumber, ParametersGeneral parameter)
        {
            return Get(StreamKindEnum.General, streamNumber, parameter as Enum);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(int streamNumber, ParametersVideo parameter)
        {
            return Get(StreamKindEnum.Video, streamNumber, parameter as Enum);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(int streamNumber, ParametersAudio parameter)
        {
            return Get(StreamKindEnum.Audio, streamNumber, parameter as Enum);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(int streamNumber, ParametersChapters parameter)
        {
            return Get(StreamKindEnum.Chapters, streamNumber, parameter as Enum);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(int streamNumber, ParametersText parameter)
        {
            return Get(StreamKindEnum.Text, streamNumber, parameter as Enum);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(int streamNumber, ParametersMenu parameter)
        {
            return Get(StreamKindEnum.Menu, streamNumber, parameter as Enum);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(int streamNumber, ParametersImage parameter)
        {
            return Get(StreamKindEnum.Image, streamNumber, parameter as Enum);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamKind">Kind of the stream.</param>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        internal string Get(StreamKindEnum streamKind, int streamNumber, Enum parameter)
        {
            return Get(streamKind, streamNumber, parameter.ToString().Replace("_AS_", "/").Replace("_IN_", "-").Replace("_OpenBracket_", "(").Replace("_CloseBracket_", ")").Replace("_x_", "*"));
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamKind">Kind of the stream.</param>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(StreamKindEnum streamKind, int streamNumber, string parameter)
        {
            return Get(streamKind, streamNumber, parameter, InfoKind.Text, InfoKind.Name);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamKind">Kind of the stream.</param>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="kindOfInfo">The kind of info.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(StreamKindEnum streamKind, int streamNumber, string parameter, InfoKind kindOfInfo)
        {
            return Get(streamKind, streamNumber, parameter, kindOfInfo, InfoKind.Name);
        }
        /// <summary>
        /// Gets the specified parameter of the specified stream kind.
        /// </summary>
        /// <param name="streamKind">Kind of the stream.</param>
        /// <param name="streamNumber">The stream number.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="kindOfInfo">The kind of info.</param>
        /// <param name="kindOfSearch">The kind of search.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Get(StreamKindEnum streamKind, int streamNumber, string parameter, InfoKind kindOfInfo, InfoKind kindOfSearch)
        {
            if (MustUseAnsi)
            {
                IntPtr Parameter_Ptr = Marshal.StringToHGlobalAnsi(parameter);
                string result = Marshal.PtrToStringAnsi(MediaInfoA_Get(Handle, (IntPtr)streamKind, (IntPtr)streamNumber, Parameter_Ptr, (IntPtr)kindOfInfo, (IntPtr)kindOfSearch));
                Marshal.FreeHGlobal(Parameter_Ptr);

                return result;
            }
            else
                return Marshal.PtrToStringUni(MediaInfo_Get(Handle, (IntPtr)streamKind, (IntPtr)streamNumber, parameter, (IntPtr)kindOfInfo, (IntPtr)kindOfSearch));
        }

        /// <summary>
        /// Get the specified options.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Option(string option)
        {
            return Option(option, string.Empty);
        }
        /// <summary>
        /// Get the specified options.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Option(string option, string value)
        {
            if (MustUseAnsi)
            {
                IntPtr Option_Ptr = Marshal.StringToHGlobalAnsi(option);
                IntPtr Value_Ptr = Marshal.StringToHGlobalAnsi(value);
                string result = Marshal.PtrToStringAnsi(MediaInfoA_Option(Handle, Option_Ptr, Value_Ptr));
                Marshal.FreeHGlobal(Option_Ptr);
                Marshal.FreeHGlobal(Value_Ptr);

                return result;
            }
            else
                return Marshal.PtrToStringUni(MediaInfo_Option(Handle, option, value));
        }

        /// <summary>
        /// Get the count of streams of the specified stream kind.
        /// </summary>
        /// <param name="streamKind">Kind of the stream.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public int Count(StreamKindEnum streamKind)
        {
            return Count(streamKind, -1);
        }
        /// <summary>
        /// Get the count of streams of the specified stream kind.
        /// </summary>
        /// <param name="streamKind">Kind of the stream.</param>
        /// <param name="streamNumber">The stream number.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public int Count(StreamKindEnum streamKind, int streamNumber)
        {
            return (int)MediaInfo_Count_Get(Handle, (IntPtr)streamKind, (IntPtr)streamNumber);
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public int GetState()
        {
            return (int)MediaInfo_State_Get(Handle);
        }
        #endregion

        #region Properties
        private string filename = string.Empty;
        /// <summary>
        /// Gets or sets the file currently opened.
        /// </summary>
        /// <value>The filename.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public string Filename
        {
            get { return filename; }
            set { Open(filename); }
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersGeneral parameter]
        {
            get
            {
                return Get(0, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersAudio parameter]
        {
            get
            {
                return Get(0, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersAudio parameter, int streamNumber]
        {
            get
            {
                return Get(streamNumber, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersChapters parameter]
        {
            get
            {
                return Get(0, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersChapters parameter, int streamNumber]
        {
            get
            {
                return Get(streamNumber, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersImage parameter]
        {
            get
            {
                return Get(0, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersImage parameter, int streamNumber]
        {
            get
            {
                return Get(streamNumber, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersMenu parameter]
        {
            get
            {
                return Get(0, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersMenu parameter, int streamNumber]
        {
            get
            {
                return Get(streamNumber, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersText parameter]
        {
            get
            {
                return Get(0, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersText parameter, int streamNumber]
        {
            get
            {
                return Get(streamNumber, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersVideo parameter]
        {
            get
            {
                return Get(0, parameter);
            }
        }
        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified parameter.
        /// </summary>
        /// <value></value>
        public string this[ParametersVideo parameter, int streamNumber]
        {
            get
            {
                return Get(streamNumber, parameter);
            }
        }

        /// <summary>
        /// Gets the general informations about this file.
        /// </summary>
        /// <value>The general.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public GeneralInformation General { get { return new GeneralInformation(this); } }
        /// <summary>
        /// Gets the video streams.
        /// </summary>
        /// <value>The video streams.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public List<VideoStream> VideoStreams
        {
            get
            {
                List<VideoStream> streams = new List<VideoStream>();
                for (int i = 0; i < Count(StreamKindEnum.Video); i++)
                    streams.Add(new VideoStream(this, i));
                return streams;
            }
        }
        /// <summary>
        /// Gets the audio streams.
        /// </summary>
        /// <value>The audio streams.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public List<AudioStream> AudioStreams
        {
            get
            {
                List<AudioStream> streams = new List<AudioStream>();
                for (int i = 0; i < Count(StreamKindEnum.Audio); i++)
                    streams.Add(new AudioStream(this, i));
                return streams;
            }
        }
        /// <summary>
        /// Gets the chapters.
        /// </summary>
        /// <value>The chapters.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public List<Chapter> Chapters
        {
            get
            {
                List<Chapter> chapters = new List<Chapter>();
                for (int i = 0; i < Count(StreamKindEnum.Chapters); i++)
                    chapters.Add(new Chapter(this, i));
                return chapters;
            }
        }
        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <value>The images.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public List<Image> Images
        {
            get
            {
                List<Image> images = new List<Image>();
                for (int i = 0; i < Count(StreamKindEnum.Image); i++)
                    images.Add(new Image(this, i));
                return images;
            }
        }
        /// <summary>
        /// Gets the menus.
        /// </summary>
        /// <value>The menus.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public List<Menu> Menus
        {
            get
            {
                List<Menu> menus = new List<Menu>();
                for (int i = 0; i < Count(StreamKindEnum.Menu); i++)
                    menus.Add(new Menu(this, i));
                return menus;
            }
        }
        /// <summary>
        /// Gets the text streams.
        /// </summary>
        /// <value>The text streams.</value>
        /// <remarks>Documented by CFI, 2009-03-27</remarks>
        public List<TextStream> TextStreams
        {
            get
            {
                List<TextStream> streams = new List<TextStream>();
                for (int i = 0; i < Count(StreamKindEnum.Text); i++)
                    streams.Add(new TextStream(this, i));
                return streams;
            }
        }
        #endregion
    }
}