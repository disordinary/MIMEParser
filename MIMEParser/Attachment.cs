using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEParser
{
    public class Attachment
    {
        public String name;
        public String filename;
        public String encoding;
        public MIME content;

        public Attachment(MIME attachment = null)
        {
            if (attachment == null)
            {
                return;
            }

            foreach (HeaderField header in attachment.header)
            {
                if (header.key == "Content-Type")
                {
                    String _name = lookup_header_val(header.value, "name");
                    if (!String.IsNullOrWhiteSpace(_name))
                    {
                        name = _name;
                        if (String.IsNullOrWhiteSpace(filename))
                        {
                            filename = name;
                        }
                    }
                }

                if (header.key == "Content-Transfer-Encoding")
                {
                    encoding = header.value;
                }

                if (header.key == "Content-Disposition")
                {
                    String _filename = lookup_header_val(header.value, "filename");
                    if (!String.IsNullOrWhiteSpace(_filename))
                    {
                        filename = _filename;
                        if (String.IsNullOrWhiteSpace(name))
                        {
                            name = _filename;
                        }
                    }
                }

                content = attachment;

            }
            /* {Content-Type : application/pdf; name="attachment_inv_7180452.pdf"}
    [1]: {Content-Transfer-Encoding : base64}
    [2]: {Content-Disposition : attachment; filename="attachment_inv_7180452.pdf"}
*/

        }

        private String lookup_header_val( String header_value , String lookup ) {
            String[] header_val = header_value.Split(';');
            for( int i = 0; i < header_val.Length; i++ ) {
                if (header_val[i].Trim().StartsWith(lookup))
                {
                    return header_val[i].Split('=')[1];
                }
            }

            return null;
        }

        public byte[] get_attachment()
        {
            return ((StringBody)content.body[0]).ToByteArray();
        }
    }
}
