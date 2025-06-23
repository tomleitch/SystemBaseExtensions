using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Module Name: FormBaseExtension.cs

namespace System
{
    public static class FormBaseExtension
    {
        public static SqlParameter AddOutputWithNullValue(this SqlParameterCollection col, string name, object objValue)
        {
            SqlParameter p = AddWithNullValue(col, name, objValue);

            p.Direction = System.Data.ParameterDirection.InputOutput;
            return p;
        }
        public static SqlParameter AddReturnValue(this SqlParameterCollection col)
        {
            SqlParameter p = col.Add("RetVal", SqlDbType.Int);

            p.Direction = System.Data.ParameterDirection.ReturnValue;
            return p;
        }

        public static int ReturnValue(this SqlParameterCollection col)
        {
            return col["RetVal"].Value.ToInt();
        }
        public static SqlParameter AddWithNullValue(this SqlParameterCollection col, string name, object objValue)
        {
            if (objValue == null || (typeof(string) == objValue.GetType() && objValue.ToString().Length == 0))
                return col.AddWithValue(name, DBNull.Value);
            else
                return col.AddWithValue(name, objValue);
        }
        public static void Center(this Form frm)
        {
            int lft;
            int top;
            int width = Screen.PrimaryScreen.WorkingArea.Width;
            int height = Screen.PrimaryScreen.WorkingArea.Height;

            lft = (int)(width - frm.Width) / 2;
            if (lft < 0)
                lft = 0;

            top = (int)(height - frm.Height) / 2;
            if (top < 0)
                top = 0;

            frm.Top = top;
            frm.Left = lft;
        }

        public static void ResizeAndCenter(this Form frm, double HeightRatio, double widthRatio)
        {
            int width = Screen.PrimaryScreen.WorkingArea.Width;
            int height = Screen.PrimaryScreen.WorkingArea.Height;

            frm.Height = (int)(height * HeightRatio);
            frm.Width = (int)(width * widthRatio);

            frm.Center();
        }

        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;
        private const int HWND_TOP = 0;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;

        // Import SetWindowPos from user32.dll
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private static async Task SetTempTopmost(this Form frm)
        {
            // Set window to system topmost
            SetWindowPos(frm.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
            Console.WriteLine("Set to system topmost");

            // Wait for 1 second
            await Task.Delay(1000);

            // Set window to app topmost (bring to top of app's z-order, not system-wide)
            SetWindowPos(frm.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
            SetWindowPos(frm.Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
            Console.WriteLine("Set to app topmost");
        }
    }


}
