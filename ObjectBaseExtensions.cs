using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// Module Name: ObjectBaseExtensions.cs


namespace System
{
    public static class ObjectBaseExtensions
    {
        private static Random rng = new Random();

        private static object lockShuffle = new object();
        public static void Shuffle<T>(this IList<T> list)
        {
            lock (lockShuffle)
            {
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
        }

        private static object lockDownArrow = new object();
        public static void DownArrow(this WebBrowser ctrl, int numberOfArrows = 1)
        {
            lock (lockDownArrow)
            {
                int x1;

                for (int i = 0; i < numberOfArrows; i++)
                {
                    int scrollTop = ctrl.Document.GetElementsByTagName("HTML")[0].ScrollTop;


                    x1 = scrollTop + 51;
                    ctrl.Document.Window.ScrollTo(0, x1);

                }
            }
        }

        private static object lockUpArrow = new object();
        public static void UpArrow(this WebBrowser ctrl, int numberOfArrows = 1)
        {
            lock (lockUpArrow)
            {
                int x1;

                for (int i = 0; i < numberOfArrows; i++)
                {
                    int scrollTop = ctrl.Document.GetElementsByTagName("HTML")[0].ScrollTop;


                    x1 = scrollTop - 51;
                    if (x1 < 0) x1 = 0;
                    ctrl.Document.Window.ScrollTo(0, x1);

                    if (x1 == 0) return;
                }
            }
        }

        private static object lockPageDown = new object();
        /// <summary>
        /// Requires the Browser to be able to get focus. And sends a page down key.
        /// You should use this on a timer control to allow time beteen call and action.
        /// </summary>
        /// <param name="ctrl"></param>
        public static void PageDown(this WebBrowser ctrl)
        {
            lock (lockPageDown)
            {
                if (ctrl.Document == null)
                    return;

                int scrollTop = ctrl.Document.GetElementsByTagName("HTML")[0].ScrollTop;
                int x1 = scrollTop + 562;

                ctrl.Document.Window.ScrollTo(0, x1);
                /*
            Cursor.Current = Cursors.WaitCursor;
            ctrl.Focus();
            SendKeys.SendWait("{PGDN}");
            Cursor.Current = Cursors.Default;
            */

            }

        }

        private static object lockPageUp = new object();
        /// <summary>
        /// Requires the Browser to be able to get focus. And sends a page down key.
        /// You should use this on a timer control to allow time beteen call and action.
        /// </summary>
        /// <param name="ctrl"></param>
        public static void PageUp(this WebBrowser ctrl)
        {
            lock (lockPageUp)
            {
                Cursor.Current = Cursors.WaitCursor;
                ctrl.Focus();
                SendKeys.SendWait("{PGUP}");
                Cursor.Current = Cursors.Default;
            }
        }

        private static object lockClearEventInvocations = new object();
        public static void ClearEventInvocations(this object obj, string eventName)
        {
            lock (lockClearEventInvocations)
            {
                var fi = obj.GetType().GetEventField(eventName);
                if (fi == null) return;
                fi.SetValue(obj, null);
            }
        }

        private static object lockGetEventField = new object();
        private static FieldInfo GetEventField(this Type type, string eventName)
        {
            lock (lockGetEventField)
            {
                FieldInfo field = null;
                while (type != null)
                {
                    /* Find events defined as field */
                    field = type.GetField(eventName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
                        break;

                    /* Find events defined as property { add; remove; } */
                    field = type.GetField("EVENT_" + eventName.ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field != null)
                        break;
                    type = type.BaseType;
                }
                return field;
            }
        }

        private static object lockToCurrencyText = new object();
        /// <summary>
        /// Format an object (typically a data row value) as a currency without the currency symbol.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToCurrencyText(this object obj)
        {
            lock (lockToCurrencyText)
            {
                string ret = "";

                try
                {
                    if (obj == null)
                        return "0.00";

                    ret = obj.ToString().ToDouble().ToString("0.00");

                    return ret;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Length == 0)
                        return "";
                }

                return "ERROR";
            }
        }

        private static object lockToShortDateText = new object();
        /// <summary>
        /// Returns a string formated as a short date from the supplied object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToShortDateText(this object obj)
        {
            lock (lockToShortDateText)
            {
                string ret = "";

                if (obj == null)
                    return "";

                try
                {
                    DateTime dt = (DateTime)obj;

                    ret = dt.ToString("dd/MM/yyyy");
                    return ret;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Length == 0)
                        return "";
                }

                return "ERROR";
            }
        }

        private static object lockToDouble = new object();
        /// <summary>
        /// Returns a double from the supplied object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ToDouble(this object obj)
        {
            lock (lockToDouble)
            {
                double ret = 0;

                try
                {
                    if (obj == null) return ret;

                    double.TryParse(obj.ToString(), out ret);

                    return ret;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Length == 0)
                        return 0;
                }

                return 0;
            }
        }

        public static string FNBody(this string fullPath)
        {
            string body = "";
            string ext = "";

            body = System.IO.Path.GetFileName(fullPath);
            ext = System.IO.Path.GetExtension(fullPath);

            return body.Replace(ext, "");
        }

        public static string FNName(this string fullPath)
        {
            string name = "";
          

            name = System.IO.Path.GetFileName(fullPath);
            

            return name;
        }
        private static object lockToLong = new object();
        /// <summary>
        /// Convert object to long.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ToLong(this object obj)
        {
            lock (lockToLong)
            {
                long ret = 0;

                try
                {
                    if (obj == null) return ret;

                    long.TryParse(obj.ToString(), out ret);

                    return ret;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Length == 0)
                        return 0;
                }

                return 0;
            }
        }

        private static object lockToSQLBit = new object();
        /// <summary>
        /// Convert object to SQL Bit (1 or 0). ny non zero is returned as 1.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToSQLBit(this object obj)
        {
            lock (lockToSQLBit)
            {
                bool ret = false;

                try
                {
                    if (obj == null) return 0;

                    bool.TryParse(obj.ToString(), out ret);

                    if (ret)
                        return 1;
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Length == 0)
                        return 0;
                }

                return 0;
            }
        }

        private static object lockisDouble = new object();
        public static bool isDouble(this object obj)
        {
            lock (lockisDouble)
            {
                double ret;
                string txt = obj.ToString();

                if (double.TryParse(txt, out ret))
                {
                    return true;
                }
                return false;
            }

        }

        private static object lockToDecimal = new object();
        /// <summary>
        /// Return a decimal from the supplied object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object obj)
        {
            lock (lockToDecimal)
            {
                decimal ret = 0;

                try
                {
                    Decimal.TryParse(obj.ToString(), out ret);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Length == 0)
                        return 0;
                }

                return ret;
            }
        }

        private static object lockToDate = new object();
        public static DateTime ToDate(this object obj)
        {
            lock (lockToDate)
            {
                DateTime ret = DateTime.Now;
                try
                {
                    string str = obj.ToString();

                    if (str.Contains("T"))
                    {
                        str = str.Replace("T", " ");
                        str = str.Substring(0, 19);

                        if (DateTime.TryParse(str, out ret))
                            return ret;
                    }
                    else
                    {
                        if (DateTime.TryParse(str, out ret))
                            return ret;
                    }

                    ret = (DateTime)obj;
                }
                catch
                {
                    //LogTextMessage("clsADOSupport.VarientToDate - " + ex.Message, enuLoggerClassification.Error);
                }

                return ret;

            }
        }


        public static string EmailDomain(this string strValue)
        {
            int i = strValue.IndexOf("@");
            if (i > -1)
            {
                string ret = strValue.Substring(i + 1);
                return ret;
            }
            else
                return strValue;
        }

        private static object lockFormatHTML = new object();
        /// <summary>
        /// This function creates an indented format of HTML with new lines for all elements and text.  Errors result in the original text being returned.
        /// </summary>
        public static string FormatHTML(this string content)
        {
            lock (lockFormatHTML)
            {
                string original = content;
                string open = "<";
                string slash = "/";
                string close = ">";

                int depth = 0; // the indentation
                int adjustment = 0; //adjustment to depth, done after writing text

                int o = 0; // open      <   index of this character
                int c = 0; // close     >   index of this character
                int s = 0; // slash     /   index of this character
                int n = 0; // next      where to start looking for characters in the next iteration
                int b = 0; // begin     resolved start of usable text
                int e = 0; // end       resolved   end of usable test

                string snippet;

                try
                {
                    using (StringWriter writer = new StringWriter())
                    {
                        while (b > -1 && n > -1)
                        {
                            o = content.IndexOf(open, n);
                            s = content.IndexOf(slash, n);
                            c = content.IndexOf(close, n);
                            adjustment = 0;

                            b = n; // begin where we left off in the last iteration
                            if (o > -1 && o < c && o == n)
                            {
                                // starts with "<tag>text"
                                e = c; // end at the next closing tag
                                adjustment = 2; //for after this node
                            }
                            else
                            {
                                // starts with "text<tag>"
                                e = o - 1; // end at the next opening tag
                            }

                            if (b == o && b + 1 == s) // ?Is the 2nd character a slash, this the a closing tag: </div>
                            {
                                depth -= 2;//adjust immediately, not afterward ...for closing tag
                                adjustment = 0;
                            }

                            if ((s + 1) == c && c == e) // don't adjust depth for singletons:  <br/>
                            {
                                adjustment = 0;
                            }



                            //string traceStart = content.Substring(0, b);
                            int length = (e - b + 1);
                            if (length < 0)
                            {
                                snippet = content.Substring(b); // happens on the final iteration
                            }
                            else
                            {
                                snippet = content.Substring(b, (e - b + 1));
                            }
                            //string traceEnd = content.Substring(b);


                            if (snippet == "<br>" || snippet == "<hr>") // don't adjust depth for singletons which lack slashes: <br>
                            {
                                adjustment = 0;
                            }

                            //Write the text
                            if (!string.IsNullOrEmpty(snippet.Trim()))
                            {
                                //Debug.WriteLine(snippet);
                                writer.Write(Environment.NewLine);
                                if (depth > 0) writer.Write(new String(' ', depth)); // add the indentation 
                                writer.Write(snippet);
                            }

                            depth += adjustment; //adjust for the next line which is likely nested

                            n = e + 1; // the next iteration start at the end of this one.

                        }

                        return writer.ToString();
                    }
                }
                catch (Exception ex)
                {
                    FakeReference.FakeMethod(ex);
                    return original;
                }
            }
        }

        private static object lockToNullableDate = new object();
        public static DateTime? ToNullableDate(this object obj)
        {
            lock (lockToNullableDate)
            {
                DateTime? ret = null;
                try
                {
                    if (obj == null)
                        return ret;

                    string str = obj.ToString();

                    if (str.Contains("T"))
                    {
                        str = str.Replace("T", " ");
                        str = str.Substring(0, 19);

                        DateTime dt;
                        if (DateTime.TryParse(str, out dt) == false)
                            return ret;

                        ret = dt;
                        return ret;

                    }
                    else
                    {
                        DateTime dt;
                        if (DateTime.TryParse(str, out dt))
                            return dt;
                    }

                    ret = (DateTime)obj;
                }
                catch
                {
                    //LogTextMessage("clsADOSupport.VarientToDate - " + ex.Message, enuLoggerClassification.Error);
                }

                return ret;
            }
        }


        private static object lockToInt = new object();
        /// <summary>
        /// Return an integer from the supplied object. (Also strips non decimal digits before attempting to parse the number.)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this object obj)
        {
            lock (lockToInt)
            {
                int ret = 0;
                string s;
                try
                {
                    if ( obj == null)
                        return 0;

                    s = obj.ToString().StripNonIntegerDigits();
                    Int32.TryParse(s, out ret);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Length == 0)
                        return 0;
                }

                return ret;
            }
        }
        private static object lockisNumeric = new object();
        /// <summary>
        /// Test to see if this contains only numbers (+,- and decimal point allowed).
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isNumeric(this string str)
        {
            try
            {
                lock (lockisNumeric)
                {
                    foreach (char c in str)
                    {
                        if (char.IsDigit(c) == false && c != '.' && c != '+' && c != '-')
                            return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
              
                if (ex == null ) return false;
                return false;
            }

        }

        private static object lockToBoolean = new object();
        public static bool ToBoolean(this object obj)
        {
            lock (lockToBoolean)
            {
                bool ret = false;
                string s;
                try
                {
                    s = obj.ToString();

                    if (s.isNumeric())
                    {
                        ret = s.ToInt() != 0;
                    }
                    else
                    {
                        Boolean.TryParse(s, out ret);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Length == 0)
                        return false;
                }

                return ret;
            }
        }

        private static object lockTicksToDate = new object();
        public static DateTime TicksToDate(this object obj)
        {
            lock (lockTicksToDate)
            {
                long value;
                DateTime dt = new DateTime();

                dt = DateTime.MinValue;
                try
                {
                    value = obj.ToLong();
                    if (value > 0)
                    {
                        dt = new DateTime(value);
                    }
                }
                catch (Exception ex)
                {
                    dt = DateTime.MinValue;
                    if (ex.Message.Length == 0)
                        return dt;
                }

                return dt;
            }
        }

        private static object lockInsertTextAtCursor = new object();
        public static bool InsertTextAtCursor(this TextBox txt, string insertionText)
        {
            lock (lockInsertTextAtCursor)
            {
                int selectionIndex = txt.SelectionStart;
                txt.Text = txt.Text.Insert(selectionIndex, insertionText);
                txt.SelectionStart = selectionIndex + insertionText.Length;

                return true;
            }
        }

        private static object lockIsNot = new object();
        public static bool IsNot(this bool val)
        {
            lock (lockIsNot)
            {
                return val == false;
            }
        }
    }
}
