using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Module Name: EnumExtension.cs

namespace System
{
    public static class EnumExtension
    {
        /// <summary>
        /// Create a bound combo with values from the supplied enum.
        /// </summary>
        /// <param name="enu"></param>
        /// <param name="cbo"></param>
        /// <returns></returns>
        public static bool BindCombo(this Enum enu, ComboBox cbo)
        {
            int[] values = (int[])Enum.GetValues(enu.GetType());
            string[] names = (string[])Enum.GetNames(enu.GetType());
            int i;

            DataTable tbl = new DataTable();
            tbl.Columns.Add("ID", typeof(int));
            tbl.Columns.Add("Name", typeof(string));    
            
            for(i=0; i< names.Length; i++)
            {
                DataRow row = tbl.NewRow();
                row["Name"] = names[i];
                row["ID"] = values[i];
                tbl.Rows.Add(row);
            }

            cbo.DisplayMember = "Name";
            cbo.ValueMember = "ID";
            cbo.DataSource = tbl;

            return true;
        }
    }
}
