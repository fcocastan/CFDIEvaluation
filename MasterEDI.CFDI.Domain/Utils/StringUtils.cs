using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEDI.CFDI.Domain.Utils
{
    public class StringUtils
    {
        public static string Normalize(string s)
        {
            if (s == null)
                s = string.Empty;

            s = s.Trim();
            s = s.ToUpperInvariant();

            return s;
        }
    }
}
