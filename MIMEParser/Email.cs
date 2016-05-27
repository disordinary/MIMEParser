using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEParser
{
    public delegate Boolean MimeWalker(String headerKey, String headerValue);
    public class Email
    {
        public MIMEParser.MIME email;

        public String from
        {
            get
            {
                try
                {
                    return email.getHeader("From");
                }
                catch
                {
                    return "";
                }
            }
            
        }

        public String[] to
        {
            get
            {
                try
                {
                    return email.getHeader("To").Split(',');
                }
                catch
                {
                    return new String[0];
                }
            }
            
        }

        public String[] CC
        {
            get
            {
                try
                {
                    return email.getHeader("CC").Split(',');
                }
                catch
                {
                    return new String[0];
                }
            }
            
        }

        public String subject
        {
            get
            {
                try
                {
                    return email.getHeader("Subject");
                }
                catch
                {
                    return "";
                }
            }
            
        }

        public String date
        {
            get
            {
                try
                {
                    return email.getHeader("Date");
                }
                catch
                {
                    return "";
                }
            }

        }


        public String body
        {
            get
            {

                MIME b = walk(email, (String headerKey, String headerValue) =>
                {
                    return (headerKey == "Content-Type" && headerValue.Contains("text/plain"));
                });
                try
                {
                    return b.body[0].ToString();
                }
                catch
                {
                    return "";
                }

            }
        }

        public String bodyHTML
        {
            get
            {

                MIME b = walk(email, (String headerKey, String headerValue) =>
                {
                    return (headerKey == "Content-Type" && headerValue.Contains("text/html"));
                });
                try { 
                    return b.body[0].ToString();
                }
                catch
                {
                    return "";
                }

            }
        }

        public String bodyRTF
        {
            get
            {

                MIME b = walk(email, (String headerKey, String headerValue) =>
                {
                    return (headerKey == "Content-Type" && headerValue.Contains("text/rtf"));
                });
                try { 
                    return b.body[0].ToString();
                }
                catch
                {
                    return "";
                }

            }
        }

        public Attachment[] attachments
        {
            get
            {
                List<Attachment> attachments = new List<Attachment>();
                List<MIMEParser.MIME> results = new List<MIMEParser.MIME>();
                walkMatch(email, (String headerKey, String headerValue) =>
                {
                    return (headerKey == "Content-Type" && !headerValue.Contains("text/") && !headerValue.Contains("multipart/"));
                }, ref results);

                foreach (MIME result in results)
                {
                    Attachment attachment = new Attachment( result );
                    attachments.Add(attachment);
                }

                return attachments.ToArray();
            }
            
        }
        public Email(String email)
        {
            this.email = new MIMEParser.MIME( email );
        }

        
        public MIME walk(MIME mimePart, MimeWalker headerMatch)
        {
            for (int i = 0; i < mimePart.body.Count; i++)
            {
                for (int h = 0; h < mimePart.header.Count; h++)
                {
                    if (headerMatch(mimePart.header[h].key, mimePart.header[h].value))
                    {
                        return mimePart;
                    }
                }
               return walk(mimePart.body[i], headerMatch);
            }
            return null;


        }

        public List<MIME> walkMatch(MIME mimePart, MimeWalker headerMatch , ref List<MIME> matchArray)
        {
            if (matchArray == null)
            {
                matchArray = new List<MIME>();
            }
            for (int i = 0; i < mimePart.body.Count; i++)
            {
                for (int h = 0; h < mimePart.header.Count; h++)
                {
                    if (headerMatch(mimePart.header[h].key, mimePart.header[h].value))
                    {
                        matchArray.Add(mimePart);
                    }
                }
                walkMatch(mimePart.body[i], headerMatch , ref matchArray);
            }
            return matchArray;


        }
    }
}
