# MIMEParser
Parsers MIME encoded emails

Usage:

```c#
MIMEParser.Email email = new MIMEParser.Email(emailString);

//fields on an email
String from = email.from;
String[ ] to = email.to;
String[ ] cc = email.CC;
String subject = email.subject;
String date = email.date;
String body = email.body; //returns the plain text body
String bodyHTML = email.bodyHTML;
String bodyRTF = email.bodyRTF;
MIMEParser.Attachment[ ] attachments = email.attachments;

//fields on an attachment
String attachmentName = attachments[ 0 ].name;
String attachmentFilename = attachments[ 0 ].filename;
String attachmentEncoding = attachments[ 0 ].encoding;
MIMEParser.MIME attachmentContent = attachments[ 0 ].content; //todo actually get the return the attachment file.
```
