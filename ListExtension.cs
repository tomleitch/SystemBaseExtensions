using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Module Name: ListExtension.cs

namespace System
{
    public class ListExtension<T> : List<T>
    {
        /// <summary>
        /// Load Records = a generic function to populate the public list.
        /// </summary>
        /// <returns></returns>
        public virtual bool LoadRecords(DataTable tblData)
        {
            return true;
        }
        private int currentOffset = 0;
        public  T GetFirst()
        {
            currentOffset = 0;
            if (Count > 0)
                return this.ElementAt<T>(currentOffset++);
            else
                return default(T);
        }

        public T GetNext()
        {
            if ( Count > currentOffset)
                return this.ElementAt<T>(currentOffset++);
            else
                return default(T);
        }

        public bool hasAvailable
        {
            get
            {
                return currentOffset < (Count - 1);
            }
        }
    }
}
