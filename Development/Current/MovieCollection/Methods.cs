using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.ObjectModel;
using MovieDataSource;
using System.Globalization;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using RootLibrary.WPF.Localization;

namespace MovieCollection
{
    public class Methods
    {
        /// <summary>
        /// Gets the image source from image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static ImageSource GetImageSourceFromImage(Image image)
        {
            Stream fileStream = new MemoryStream();
            image.Save(fileStream, ImageFormat.Png);
            if (fileStream != null)
            {
                PngBitmapDecoder bitmapDecoder = new PngBitmapDecoder(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                ImageSource imageSource = bitmapDecoder.Frames[0];
                return imageSource;
            }
            else return null;
        }

        /// <summary>
        /// Gets the size of the file in a readable string.
        /// </summary>
        /// <param name="byteCount">The byte count.</param>
        /// <returns></returns>
        public static string GetFileSize(long byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824)
                size = String.Format("{0:##.##}", byteCount * 1.0 / 1073741824) + " GB";
            else if (byteCount >= 1048576)
                size = String.Format("{0:##.##}", byteCount * 1.0 / 1048576) + " MB";
            else if (byteCount >= 1024)
                size = String.Format("{0:##.##}", byteCount * 1.0 / 1024) + " KB";
            else if (byteCount > 0 && byteCount < 1024)
                size = byteCount.ToString() + " Bytes";

            return size;
        }

        private static Thread sizeThread = null;
        /// <summary>
        /// Loads the media file into movie.
        /// </summary>
        public static void LoadMediaFileIntoMovie(IMovie movie, string filename, IMovieDataSourcePlugin dataSource)
        {
            if (filename.Length < 3)
                return;

            movie.MediaFile = dataSource.CreateMediaFile(filename);
            if (File.Exists(filename))
            {
                movie.MediaFile.Size = (new FileInfo(filename)).Length;

                MediaInfoNET.MediaInfo info = new MediaInfoNET.MediaInfo(filename);
                if (info.VideoStreams.Count > 0)
                {
                    movie.MediaFile.Video = dataSource.CreateVideoProperties(info.VideoStreams[0].Duration);
                    movie.MediaFile.Video.BitRate = info.VideoStreams[0].BitRate.Value;
                    movie.MediaFile.Video.Encoding = info.VideoStreams[0].EncodedLibrary.Name;
                    movie.MediaFile.Video.Format = info.VideoStreams[0].Format.Name;
                    movie.MediaFile.Video.Height = info.VideoStreams[0].Height;
                    movie.MediaFile.Video.Width = info.VideoStreams[0].Width;
                }

                foreach (MediaInfoNET.AudioStream audio in info.AudioStreams)
                {
                    IAudioProperties props = dataSource.CreateAudioProperties(audio.Channles.Channels);
                    props.BitRate = audio.BitRate.Value;
                    props.Format = audio.Format.Name;
                    props.Encoding = audio.CodecID.Hint;
                    props.Language = (audio.Language == null || audio.Language.TwoLetterISOLanguageName == "iv") ? null : audio.Language;
                    movie.MediaFile.Audio.Add(props);
                }

                foreach (string file in Directory.GetFiles(System.IO.Path.GetDirectoryName(filename),
                    System.IO.Path.GetFileNameWithoutExtension(filename) + "*", SearchOption.TopDirectoryOnly))
                {
                    if (System.IO.Path.GetExtension(file) == ".ac3" || System.IO.Path.GetExtension(file) == ".dts" || System.IO.Path.GetExtension(file) == ".mka")
                    {
                        MediaInfoNET.MediaInfo audioInfo = new MediaInfoNET.MediaInfo(file);

                        foreach (MediaInfoNET.AudioStream audio in audioInfo.AudioStreams)
                        {
                            IAudioProperties props = dataSource.CreateAudioProperties(audio.Channles.Channels);
                            props.BitRate = audio.BitRate.Value;
                            props.Format = audio.Format.Name;
                            props.Language = audio.Language ?? TryGetCulture(file);
                            movie.MediaFile.Audio.Add(props);
                            movie.MediaFiles.Add(dataSource.CreateMediaFile(file));
                        }
                    }
                }
            }
            else if (Directory.Exists(filename))
            {
                movie.MediaFile.Size = 0;

                if (sizeThread != null)
                    sizeThread.Abort();

                sizeThread = new Thread(new ThreadStart(delegate
                    {
                        try
                        {
                            foreach (FileInfo fileInfo in (new DirectoryInfo(filename)).GetFiles("*.*", SearchOption.AllDirectories))
                            {
                                try
                                {
                                    long size = fileInfo.Length;
                                    movie.MediaFile.Size += size;
                                }
                                catch (Exception exp) { Trace.WriteLine(exp.ToString()); }
                            }
                        }
                        catch (Exception exp) { Trace.WriteLine(exp.ToString()); }
                        finally { sizeThread = null; }
                    }));
                sizeThread.IsBackground = true;
                sizeThread.Priority = ThreadPriority.Lowest;
                sizeThread.Name = "Size Thread";
                sizeThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                sizeThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                sizeThread.Start();
            }
        }

        /// <summary>
        /// Tries to get the culture.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private static CultureInfo TryGetCulture(string filename)
        {
            try
            {
                string file = System.IO.Path.GetFileName(filename);
                int start = filename.IndexOf(".") + 1;
                int length = filename.IndexOf(".", start) - start;
                return new CultureInfo(file.Substring(start, length));
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Gets the title from filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public static string GetTitleFromFilename(string file)
        {
            string filename;
            if (File.Exists(file))
            {
                filename = Path.GetFileNameWithoutExtension(file);
            }
            else if (Directory.Exists(file))
            {
                string[] folders = file.Split('\\', '/');
                filename = folders[folders.Length - 1];
            }
            else
                return Properties.Resources.Unknown;

            string title = string.Empty;

            if (filename.EndsWith(".720p"))
                title = filename.Substring(0, filename.Length - 5);
            else if (filename.EndsWith(" - 720p"))
                title = filename.Substring(0, filename.Length - 7);
            else if (filename.EndsWith(".1080p"))
                title = filename.Substring(0, filename.Length - 6);
            else if (filename.EndsWith(" - 1080p"))
                title = filename.Substring(0, filename.Length - 8);
            else
                title = filename;

            if (title.ToLower().Contains("dvdrip"))
                title = title.Substring(0, title.ToLower().IndexOf("dvdrip"));
            if (title.ToLower().Contains("xvid"))
                title = title.Substring(0, title.ToLower().IndexOf("xvid"));
            else if (title.ToLower().Contains("divx"))
                title = title.Substring(0, title.ToLower().IndexOf("divx"));

            title = title.Trim(',', ' ', '.', '-');

            try
            {
                int year = Convert.ToInt32(title.Substring(title.Length - 4, 4));
                if (year > 1900 || year < 2020)
                    title = title.Substring(0, title.Length - 4);
            }
            catch { }

            return title.Trim(',', ' ', '.', '-').Replace('.', ' ');
        }
    }

    public class MediaFileToMediaFileSizeText : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Properties.Resources.GroupBoxMediaFile;

            if (!(value is long?))
                throw new ArgumentException("Can only convert long?");

            if (!(value as long?).HasValue)
                return Properties.Resources.GroupBoxMediaFile;

            return LocalizeDictionary.Instance.GetLocizedObject<string>("MovieCollection", "Resources", "GroupBoxMediaFile", LocalizeDictionary.Instance.Culture) +
                                    " (" + Methods.GetFileSize((value as long?).Value) + ")";
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class ToolTipDisplayConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Properties.Settings.Default.ListViewShowToolTip)
                return value;
            else
                return null;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class MovieToTitleConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IMovie))
                return "ERROR: Can only convert IMovie!";

            IMovie movie = value as IMovie;

            if (movie.Year.HasValue && Properties.Settings.Default.SimpleListViewShowYear)
                return string.Format("{0} ({1})", movie.Title, movie.Year.Value);
            else
                return movie.Title;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <remarks>Documented by CFI, 2009-05-02</remarks>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class DictionaryToStringConverter : IValueConverter
    {
        private const int MAX_LENGTH = 15;

        #region IValueConverter Members

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!typeof(IDictionary).IsAssignableFrom(value.GetType()))
                throw new ArgumentException("Can only Convert IDictionary's!");

            int count = 0;
            string res = string.Empty;
            foreach (object obj in (value as IDictionary).Keys)
            {
                if (count >= MAX_LENGTH)
                    break;

                res += obj.ToString() + (((value as IDictionary)[obj] == null || (value as IDictionary)[obj] as string == string.Empty) ? string.Empty : " (" + (value as IDictionary)[obj].ToString() + ")") + ", ";
                count++;
            }

            return res.Trim(',', ' ') + (count >= MAX_LENGTH ? ",..." : string.Empty);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ListToStringConverter : IValueConverter
    {
        private const int MAX_LENGTH = 5;

        #region IValueConverter Members

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                throw new ArgumentException("Can only Convert IEnumerable's!");

            int count = 0;
            string res = string.Empty;
            foreach (object obj in (value as IEnumerable))
            {
                if (count >= MAX_LENGTH)
                    break;

                res += obj.ToString() + ", ";
                count++;
            }

            return res.Trim(',', ' ') + (count >= MAX_LENGTH ? ",..." : string.Empty);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ImageToImageSourceConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                if (parameter is System.Windows.Controls.Image)
                    return (parameter as System.Windows.Controls.Image).Source;
                else
                    return null;
            }

            if (!(value is Image))
                throw new ArgumentException("Can only convert Image's!");

            return ConverImage(value as Image);
        }

        private ImageSource ConverImage(Image value)
        {
            Stream fileStream = new MemoryStream();
            (value as Image).Save(fileStream, ImageFormat.Png);
            if (fileStream != null)
            {
                PngBitmapDecoder bitmapDecoder = new PngBitmapDecoder(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                ImageSource imageSource = bitmapDecoder.Frames[0];
                return imageSource;
            }

            return null;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
