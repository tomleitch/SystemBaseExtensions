using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Module Name: FakeReference.cs

namespace System
{
    public class FakeReference
    {
        public static void FakeMethod(object x)
        {
            if ( x == null)
              return;
        }
    }
}
