using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEParser
{
    public class HeaderField
    {
        public String key;
        public String value;
        public HeaderField(String key, String value)
        {
            this.key = key;
            this.value = value;
        }

        public String ToString()
        {
            return key + " : " + value;
        }

    }
}
