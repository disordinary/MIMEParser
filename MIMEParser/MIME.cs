using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEParser
{
    public class MIME
    {

        public List<HeaderField> header = new List<HeaderField>();
        public List<MIME> body = new List<MIME>();
        
        public MIME parent;
        protected List<String> lines = new List<string>();
        enum States
        {
            PARSING_HEADER,
            PARSING_BODY
        }

        enum ContentType
        {
            MIME_BODY,
            STRING_BODY
        }
        public MIME(String email_file = null)
        {
            if( !String.IsNullOrEmpty( email_file)) {
                String[] lines = email_file.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                process(lines);
            }

        }

        public void addLine(String line)
        {
            lines.Add(line);
        }


        public virtual void process()
        {
            process(lines.ToArray());
        }
        public virtual void process( String[ ] lines ) {
            States state = States.PARSING_HEADER;
            ContentType contentType = ContentType.MIME_BODY;

            String boundary = "";

            for (int i = 0; i < lines.Length; i++)
            {
                String line = lines[i];
                //handle state
                if (lines[i] == "" && state == States.PARSING_HEADER)
                {
                    state = States.PARSING_BODY;
                    continue;
                }



                //handle the header
                if (state == States.PARSING_HEADER)
                {

                    if (line.Substring(0, 1) == " " || line.Substring(0, 1) == "\t" || line.Substring(0, 1) == "    ")
                    {
                        header[ header.Count - 1 ].value += Environment.NewLine + line.Substring(1);
                    }
                    else
                    {
                        int colon = line.IndexOf(':');
                        String current_header = line.Substring(0,colon);
                        header.Add(new HeaderField(current_header, line.Substring(colon + 1).Trim()));
                        if (current_header.ToLower() == "content-type" && !line.ToLower().Contains("multipart")) 
                        {
                            contentType = ContentType.STRING_BODY;
                        } 
                        
                    }
                }

                if (state == States.PARSING_BODY)
                {
                    if (line.Length > 1 && line.Substring(0, 2) == "--" && line.Substring(line.Length - 1 , 1 ) == "_")
                    {
                        if (String.IsNullOrEmpty(boundary))
                        {
                            boundary = line;
                            addBody(ref body, contentType);
                        }
                        else if (boundary == line)
                        {
                            addBody(ref body , contentType);
                        }
                        else
                        {
                            body[body.Count - 1].addLine(line);
                        }
                    }
                    else
                    {
                        if (body.Count == 0)
                        {
                            addBody(ref body, contentType);
                        }
                        body[body.Count - 1].addLine(line);
                    }
                }
          
            }
            body[body.Count - 1].process();

        }

        private void addBody(ref List<MIME> body, ContentType contentType )
        {
            if (body.Count > 0)
            {
                body[body.Count - 1].process();
            }
            if (contentType == ContentType.STRING_BODY)
            {
                body.Add(new StringBody() { parent = this });
            }
            else
            {
                body.Add(new MIME() { parent = this });
            }
        }

        public String getHeader(String key)
        {
            for (int i = 0; i < header.Count; i++)
            {
                if (header[i].key == key)
                {
                    return header[i].value;
                }
            }

            return null;
        }
    }
}
