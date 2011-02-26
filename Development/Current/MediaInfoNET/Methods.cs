using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MediaInfoNET
{
    /// <summary>
    /// Some Helper Methods.
    /// </summary>
    public class Methods
    {
        /// <summary>
        /// Tries the parse int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int TryParseInt(string value)
        {
            int val;
            if (Int32.TryParse(value, out val))
                return val;
            else
                return -1;
        }

        /// <summary>
        /// Tries the parse long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long TryParseLong(string value)
        {
            long val;
            if (Int64.TryParse(value, out val))
                return val;
            else
                return -1;
        }

        /// <summary>
        /// Tries the parse double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double TryParseDouble(string value)
        {
            double val;
            if (Double.TryParse(value, out val))
                return val;
            else
                return -1;
        }

        /// <summary>
        /// Tries the parse bit rate mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public static BitRateMode TryParseBitRateMode(string value)
        {
            try
            {
                return (BitRateMode)Enum.Parse(typeof(BitRateMode), value, true);
            }
            catch { return BitRateMode.NA; }
        }

        /// <summary>
        /// Tries the parse frame rate mode.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public static FrameRateMode TryParseFrameRateMode(string value)
        {
            try
            {
                return (FrameRateMode)Enum.Parse(typeof(FrameRateMode), value, true);
            }
            catch { return FrameRateMode.NA; }
        }

        /// <summary>
        /// Tries the parse video standard.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public static VideoStandard TryParseVideoStandard(string value)
        {
            try
            {
                return (VideoStandard)Enum.Parse(typeof(VideoStandard), value, true);
            }
            catch { return VideoStandard.Other; }
        }

        /// <summary>
        /// Tries the parse Scan Type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2009-03-30</remarks>
        public static ScanType TryParseScanType(string value)
        {
            try
            {
                return (ScanType)Enum.Parse(typeof(ScanType), value, true);
            }
            catch { return ScanType.Other; }
        }

        /// <summary>
        /// Tries the parse culture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static CultureInfo TryParseCulture(string value)
        {
            try
            {
                return new CultureInfo(value);
            }
            catch { return null; }
        }
    }
}
