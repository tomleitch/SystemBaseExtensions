using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// Module Name: StringBaseExyensions.cs

namespace System
{

    public static class StringBaseExtensions
    {
        private static object lockSafeSQL = new object();
        public static string SafeSQL(this string strSQL)
        {
            lock (lockSafeSQL)
            {
                    string ret = strSQL.Replace("'", "''");
                    return ret;
            }
        }

        private static object lockDecodeHTMLAndURL = new object();
        public static string DecodeHTMLAndURL(this string value)
        {
            lock (lockDecodeHTMLAndURL)
            {
                string ret = value.HTMLDecode();
                ret = System.Net.WebUtility.UrlDecode(ret);
                return ret;
            }
        }

        private static object lockAppendPath = new object();
        /// <summary>
        /// Appends to a path or drive ensuring there is a backslash between the source and new value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="appendStr"></param>
        /// <returns></returns>
        public static string AppendPath(this string value, string appendStr)
        {
            lock (lockAppendPath)
            {
                int l = value.Length;

                if (l == 0)
                    return appendStr;
                if (value.Substring(l - 1) == "\\")
                    return value + appendStr;

                return value + "\\" + appendStr;
            }
        }

        private static object lockStripUnicodeCharactersFromString = new object();
        public static String StripUnicodeCharactersFromString(this string inputValue)
        {
            lock (lockStripUnicodeCharactersFromString)
            {
                return new string(inputValue.Where(c => c <= sbyte.MaxValue).ToArray());
            }
        }

        private static object lockToHexString = new object();
        public static string ToHexString(this int intValue, int hexLength)
        {
            lock (lockToHexString)
            {
                string ret = Convert.ToString(intValue, 16);
                string zeros = "000000000";
                int pad = hexLength - ret.Length;

                if (pad > 0)
                    return zeros.Substring(0, pad) + ret;

                return ret;
            }
        }

        public static string FromHexString(this string strValue)
        {
            return strValue.HexEncodedToString();
        }
        private static object lockToHexString2 = new object();
        public static string ToHexString(this string strValue)
        {
            lock (lockToHexString2)
            {
                StringBuilder hex = new StringBuilder(strValue.Length * 2);
                foreach (char c in strValue)
                {
                    hex.AppendFormat("{0:x2}", (int)c);
                }
                return hex.ToString();
            }
        }

        private static object lockHexEncodedToInt = new object();
        public static int HexEncodedToInt(this string strValue)
        {
            lock (lockHexEncodedToInt)
            {
                return Convert.ToInt32(strValue, 16);
            }
        }

        private static object lockHexEncodedToString = new object();
        public static string HexEncodedToString(this string strValue)
        {
            if (strValue == null || strValue.Length == 0)
                return "";

            lock (lockHexEncodedToString)
            {

                StringBuilder ret = new StringBuilder((int)(strValue.Length / 2));
                int offset = 0;
                string hexV = "";
                string v;
                while (offset < strValue.Length - 1)
                {
                    hexV = strValue[offset].ToString() + strValue[offset + 1].ToString();

                    v = ((char)Convert.ToInt32(hexV, 16)).ToString();
                    ret.Append(v);
                    offset += 2;
                }
                return ret.ToString();
            }
        }

        private static object lockBestMatch = new object();
        /// <summary>
        /// Get the best match for the supplied string from the supplied list.  The best distance is a reliability figure. Less than 10 is usually a good match.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <param name="bestDistance"></param>
        /// <returns></returns>
        public static string BestMatch(this string value, List<string> list, out int bestDistance)
        {
            lock (lockBestMatch)
            {
                string bestString = "";
                int bestMatch = 999999;
                string compare = value.ToUpper();
                int curMatch = 0;

                foreach (string s in list)
                {
                    curMatch = compare.ComputeDistance(s.ToUpper());
                    if (curMatch < bestMatch)
                    {
                        bestString = s;
                        bestMatch = curMatch;
                    }
                }

                bestDistance = bestMatch;
                return bestString;
            }
        }


        private static object lockComputeDistance = new object();
        /// <summary>
        /// Compute the number of edits required to change one string to another using LevenshteinDistance.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        public static int ComputeDistance(this string s, string compareTo)
        {
            lock (lockComputeDistance)
            {
                return LevenshteinDistance.Compute(s, compareTo);
            }
        }

        private static object lockReplaceIgnoringCase = new object();
        /// <summary>
        /// Replace using ignore case.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceIgnoringCase(this string s, string oldValue, string newValue)
        {
            lock (lockReplaceIgnoringCase)
            {
                StringComparison comparisonType = StringComparison.OrdinalIgnoreCase;

                try
                {
                    if (s == null)
                        return null;

                    if (String.IsNullOrEmpty(oldValue))
                        return s;

                    StringBuilder result = new StringBuilder(Math.Min(4096, s.Length));
                    int pos = 0;

                    while (true)
                    {
                        int i = s.IndexOf(oldValue, pos, comparisonType);
                        if (i < 0)
                            break;

                        result.Append(s, pos, i - pos);
                        result.Append(newValue);

                        pos = i + oldValue.Length;
                    }
                    result.Append(s, pos, s.Length - pos);

                    return result.ToString();
                }
                catch (Exception ex)
                {
                    if (ex == null)
                        return "";
                }

                return "ERROR";
            }

        }

        private static object lockStripNonDecimalDigits = new object();
        /// <summary>
        /// Remove all non decimal digits from the supplied string. (Allows - and . only.)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StripNonDecimalDigits(this string value)
        {
            lock (lockStripNonDecimalDigits)
            {
                string ret = "";

                foreach (char c in value)
                {
                    if (Char.IsDigit(c) || c == '.' || c == '-')
                        ret += c;
                }

                return ret;
            }

        }

        private static object lockCopyUpTo = new object();
        /// <summary>
        /// Returns a substring of the supplied string upto the supplied match string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static string CopyUpTo(this string value, string match)
        {
            lock (lockCopyUpTo)
            {
                int i;
                string ret = "";

                i = value.IndexOf(match);
                if (i > -1)
                    ret = value.Substring(0, i);

                return ret;
            }
        }

        private static object lockCopyUpTo2 = new object();
        /// <summary>
        /// Returns a substring of the supplied string upto the supplied maximum length.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string CopyUpTo(this string value, int maxLength)
        {
            lock (lockCopyUpTo2)
            {
                string ret = "";

                if (value.Length > maxLength)
                    ret = value.Substring(0, maxLength);
                else
                    ret = value;

                return ret;
            }
        }

        private static object lockCopyFrom = new object();
        /// <summary>
        /// Copy a string from the supplied match value. Returns empty string if no match is found.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static string CopyFrom(this string value, string match)
        {
            lock (lockCopyFrom)
            {
                int i;
                string ret = "";

                i = value.IndexOf(match);
                if (i > -1)
                    ret = value.Substring(i);

                return ret;
            }
        }

        private static object lockHTMLDecode = new object();
        /// <summary>
        /// Convert &amp; to & and other encoded symbols.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HTMLDecode(this string value)
        {
            lock (lockHTMLDecode)
            {
                if (value == null)
                    return value;


                string s = value.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&nbsp;", " ");

                s = s.Replace("&pound;", "£").Replace("&#8211;", "-").Replace("&#x27;", "'");

                System.Web.HttpUtility.HtmlDecode(s);

                return s;
            }
        }

        private static object lockStripNonIntegerDigits = new object();
        /// <summary>
        /// Strinp non integer characters stops at position of decimal point (if found).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StripNonIntegerDigits(this string value)
        {
            lock (lockStripNonIntegerDigits)
            {
                string ret = "";

                foreach (char c in value)
                {
                    if (Char.IsDigit(c) || c == '-')
                        ret += c;
                    else
                    {
                        if (c == '.')
                            return ret;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// Case sensitive Like supports % and _ search terms.
        /// </summary>
        /// <param name="toSearch"></param>
        /// <param name="toFind"></param>
        /// <returns></returns>
        public static bool Like(this string toSearch, string toFind)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\")
                .Replace(toFind, ch => @"\" + ch)
                .Replace('_', '.')
                .Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }

        private static object lockStripControlChars = new object();
        /// <summary>
        /// Strip and characters with a value less than space.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StripControlChars(this string value)
        {
            lock (lockStripControlChars)
            {
                string ret = "";

                foreach (char c in value)
                {
                    if (c >= ' ')
                        ret += c;
                }

                return ret;
            }
        }

        private static object lockRemoveCompleteJSComments = new object();
        public static string RemoveCompleteJSComments(this string value)
        {
            lock (lockRemoveCompleteJSComments)
            {
                string ret;
                int i;
                int j = 0;
                string s = value;

                ret = value;
                i = ret.IndexOf("<!--");
                if (i > -1)
                {
                    j = ret.IndexOf("-->", i);
                    while (i > -1 && j > -1)
                    {
                        s = ret.Substring(i, j - i + 3);

                        ret = ret.Substring(0, i) + ret.Substring(j + 3);
                        i = ret.IndexOf("<!--");
                        if (i > -1)
                            j = ret.IndexOf("-->", i);
                        else
                            j = -1;
                    }
                }
                return ret;
            }
        }

        private static object lockRemoveJSComments = new object();
        public static string RemoveJSComments(this string value)
        {
            lock (lockRemoveJSComments)
            {
                string ret;

                ret = value.Replace("-->", "");
                ret = ret.Replace("<!--", "");

                return ret;
            }
        }

        private static object lockSTrim = new object();
        public static string STrim(this string value)
        {
            lock (lockSTrim)
            {
                return value.StripControlChars().Trim();
            }
        }

        private static object cmdAddReturnParameter = new object();

        /// <summary>
        /// Append an int ReturnValue parameter named @ReturnValue to the parameter collection.
        /// Tis does not have to be specified in the called stored proc. It needs only return an int value.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static SqlParameter AddReturnParameter(this SqlCommand cmd)
        {
            SqlParameter returnParameter = null;

            lock (cmdAddReturnParameter)
            {
                returnParameter = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
            }
            return returnParameter;
        }

        public static int ReturnParameterValue(this SqlCommand cmd)
        {
            lock (cmdAddReturnParameter)
            {
                return cmd.Parameters["@ReturnValue"].Value.ToInt();
            }
        }

        private static object cmdAddConfirmationParameter = new object();

        /// <summary>
        /// Add an int output parameter @ConfirmAction. Non zero equals success.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static SqlParameter AddConfirmationParameter(this SqlCommand cmd)
        {
            lock (cmdAddConfirmationParameter)
            {
                var returnParameter = cmd.Parameters.Add("@ConfirmAction", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.Output;

                return returnParameter;
            }
        }

        public static int ConfirmationParameterValue(this SqlCommand cmd)
        {
            lock (cmdAddConfirmationParameter)
            {
                return cmd.Parameters["@ConfirmAction"].Value.ToInt();
            }
        }

        private static object lockTrimDoubleSpaces = new object();
        /// <summary>
        /// Converts anything below Ascii(32) to a space then replaces spaces with a single space
        /// </summary>
        /// <returns></returns>
        public static string TrimDoubleSpaces(this string value)
        {
            lock (lockTrimDoubleSpaces)
            {
                int i;
                string ret;
                string comp;

                ret = value;
                comp = "";
                for (i = 0; i < ret.Length; i++)
                {
                    if (ret[i] < ' ')
                        comp += ' ';
                    else
                        comp += ret[i];
                }

                ret = comp;

                ret = ret.Replace("  ", " ");
                while (ret != comp)
                {
                    comp = ret;
                    ret = ret.Replace("  ", " ");
                }

                return ret;
            }
        }

        private static object lockisIn = new object();
        public static bool isIn(this char c, string match)
        {
            lock (lockisIn)
            {
                return match.Contains(c.ToString());
            }
        }

        private static object lockReplaceOddText = new object();
        public static string ReplaceOddText(this string s)
        {
            lock (lockReplaceOddText)
            {
                string ret = s.Replace("’", "'");

                ret = ret.Replace("“", "\"");
                ret = ret.Replace("'", "'");
                ret = ret.Replace("…", "...");
                ret = ret.Replace(((Char)8230).ToString(), "");
                ret = ret.Replace("‘", "'");
                ret = ret.Replace("–", "-");

                return ret;
            }
        }

        public static string AppendTo(this string value, char charToRepeat, int length)
        {
            if ( value.Length >= length)
                return value;

            int repLength = length - value.Length;
            string ret = value + new string(charToRepeat, repLength);
            return ret;
        }

        private static object lockStripNonDigits = new object();
        /// <summary>
        /// Strip non decimal characters from the supplied string. Allows 0-9 and minus.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StripNonDigits(this string value)
        {
            lock (lockStripNonDigits)
            {
                string ret = "";

                foreach (char c in value)
                {
                    if (Char.IsDigit(c) || c == '-')
                        ret += c;
                }

                return ret;
            }
        }

        private static object lockRemoveRoundBrackets = new object();
        /// <summary>
        /// .Replace("(","").Replace(")","").Trim()
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveRoundBrackets(this string value)
        {
            string s;
            lock (lockRemoveRoundBrackets)
            {
                if (value.Contains("("))
                {
                    s = value.Trim();
                    if (s.StartsWith("(") && s.EndsWith(")"))
                        return s.Substring(1, s.Length - 2).Trim();
                }

                return value;
            }
        }

     

        private static object lockRemoveSquareBrackets = new object();
        /// <summary>
        /// .Replace("(","").Replace(")","").Trim()
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveSquareBrackets(this string value)
        {
            string s;
            lock (lockRemoveSquareBrackets)
            {
                if (value.Contains("["))
                {
                    s = value.Trim();
                    if (s.StartsWith("[") && s.EndsWith("]"))
                        return s.Substring(1, s.Length - 2).Trim();
                }

                return value;
            }
        }

        private static object lockContains = new object();
        /// <summary>
        /// Returns if any items of the supplied list are contained it the string. The function also supports case or ignore case.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool Contains(this string value, List<string> values, bool ignoreCase = false)
        {
            lock (lockContains)
            {
                string lcaseValue = "";

                if (ignoreCase)
                    lcaseValue = value.ToLower();

                foreach (string text in values)
                {
                    if (ignoreCase == false)
                    {
                        if (value.Contains(text))
                            return true;
                    }
                    else
                    {
                        if (lcaseValue.Contains(text.ToLower()))
                            return true;
                    }
                }

                return false;
            }
        }

        private static object lockSplit = new object();
        /// <summary>
        /// Split using a string rather than a single character as the delimiter.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string[] Split(this string value, string target)
        {
            lock (lockSplit)
            {
                char c = (char)254;

                string test = value.Replace(target, c.ToString());
                return test.Split(c);
            }
        }


        private static object lockIsNullOrEmpty = new object();
        /// <summary>
        /// Returns true if string is null or of zero length.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            lock (lockIsNullOrEmpty)
            {
                return string.IsNullOrEmpty(value);
            }
        }

        private static object lockReplaceIgnoreCase = new object();
        /// <summary>
        /// Replace while ignoring case.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="oldVale"></param>
        /// <param name="newVale"></param>
        /// <returns></returns>
        public static string ReplaceIgnoreCase(this string source, string oldVale, string newVale)
        {
            lock (lockReplaceIgnoreCase)
            {
                if (source.IsNullOrEmpty() || oldVale.IsNullOrEmpty())
                    return source;

                var stringBuilder = new StringBuilder();
                string result = source;

                int index = result.IndexOf(oldVale, StringComparison.InvariantCultureIgnoreCase);

                while (index >= 0)
                {
                    if (index > 0)
                        stringBuilder.Append(result.Substring(0, index));

                    if (newVale.IsNullOrEmpty().IsNot())
                        stringBuilder.Append(newVale);

                    stringBuilder.Append(result.Substring(index + oldVale.Length));

                    result = stringBuilder.ToString();

                    index = result.IndexOf(oldVale, index + newVale.Length, StringComparison.InvariantCultureIgnoreCase);
                }

                return result;
            }
        }

        private static object lockCharBetween = new object();

        public static bool Between(this char value, char minValue, char maxValue)
        {
            lock (lockCharBetween)
            {
                if (minValue <= value && value <= maxValue)
                    return true;
                else
                    return false;
            }
        }


        private static object lockBetween = new object();
        /// <summary>
        /// Return a string between the supplied delimiter.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Between(this string input, char delimiter)
        {
            lock (lockBetween)
            {
                int i;
                int start;
                int fin;
                bool escaped = false;

                start = input.IndexOf(delimiter);
                fin = -1;
                for (i = start + 1; i < input.Length && fin < 0; i++)
                {
                    if (input[i] == delimiter)
                    {
                        if (escaped == false)
                            fin = i + 1; ;
                    }
                    if (input[i] == '\\')
                        escaped = true;
                    else
                        escaped = false;
                }

                return input.Substring(start, fin - start);
            }
        }

        private static object lockItemStartsWith = new object();
        /// <summary>
        /// Return the first item in the array which starts with the supplied string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startSequence"></param>
        /// <returns></returns>
        public static string ItemStartsWith(this string[] value, string startSequence)
        {
            lock (lockItemStartsWith)
            {
                foreach (string s in value)
                {
                    if (s.StartsWith(startSequence, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return s;
                    }
                }

                return null;
            }
        }
    }
}
