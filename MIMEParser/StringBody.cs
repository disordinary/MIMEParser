using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEParser
{
    class StringBody : MIME
    {
        

        public override void process()
        {
         
        }
        public override void process(String[] lines)
        {

        }

        public override String ToString()
        {

            String encoding;
            if (header.Count == 0)
            {
                encoding = parent.getHeader("Content-Transfer-Encoding");
            }
            else
            {
                encoding = getHeader("Content-Transfer-Encoding");
            }

            if (encoding == "base64")
            {
                byte[] raw = Convert.FromBase64String(String.Join(Environment.NewLine, lines));
                return Encoding.UTF8.GetString(raw);
            }
            return String.Join(Environment.NewLine, lines);
        }


        public Byte[] ToByteArray()
        {

            String encoding;
            if (header.Count == 0)
            {
                encoding = parent.getHeader("Content-Transfer-Encoding");
            }
            else
            {
                encoding = getHeader("Content-Transfer-Encoding");
            }

            if (encoding == "base64")
            {
                String result = "";
                foreach (String line in lines)
                {
                    if (line == "")
                    {
                        break;
                    }
                    if (!String.IsNullOrEmpty(result))
                    {
                        result += Environment.NewLine;
                    }
                    result += line;
                }
                return Convert.FromBase64String(result);
                //return Encoding.UTF8.GetString(raw);
            }
            else
            {
                return new Byte[0];
            }
            
        }
    }
}
