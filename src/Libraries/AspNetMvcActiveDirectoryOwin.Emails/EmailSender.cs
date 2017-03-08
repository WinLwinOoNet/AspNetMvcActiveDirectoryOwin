using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace AspNetMvcActiveDirectoryOwin.Emails
{
    public class EmailSender : IEmailSender
    {
        public void SendEmail(EmailAccount emailAccount, 
            string subject, string body, 
            string fromAddress, 
            IEnumerable<string> toAddresses, 
            IEnumerable<string> replyToAddresses = null, 
            IEnumerable<string> bccAddresses = null, 
            IEnumerable<string> ccAddresses = null, 
            string attachmentFilePath = null, 
            string attachmentFileName = null)
        {
            var fromMailAddress = new MailAddress(fromAddress);

            var toMailAddresses = new List<MailAddress>();
            foreach (var toAddress in toAddresses.Where(x => !string.IsNullOrWhiteSpace(x)))
                toMailAddresses.Add(new MailAddress(toAddress));

            var replyToMailAddresses = new List<MailAddress>();
            if (replyToAddresses != null)
            {
                foreach (var replyToAddress in replyToAddresses.Where(x => !string.IsNullOrWhiteSpace(x)))
                    replyToMailAddresses.Add(new MailAddress(replyToAddress));
            }

            var bccMailAddresses = new List<MailAddress>();
            if (bccAddresses != null)
            {
                foreach (var address in bccAddresses.Where(x => !string.IsNullOrWhiteSpace(x)))
                    bccMailAddresses.Add(new MailAddress(address.Trim()));
            }

            var ccMailAddresses = new List<MailAddress>();
            if (ccAddresses != null)
            {
                foreach (var address in ccAddresses.Where(x => !string.IsNullOrWhiteSpace(x)))
                    ccMailAddresses.Add(new MailAddress(address.Trim()));
            }

            SendEmail(emailAccount, subject, body, 
                fromMailAddress, toMailAddresses, 
                replyToMailAddresses,
                bccMailAddresses, ccMailAddresses, 
                attachmentFilePath: attachmentFilePath, attachmentFileName: attachmentFileName);
        }

        public void SendEmail(EmailAccount emailAccount, string subject, string body, 
            MailAddress fromAddress, 
            IEnumerable<MailAddress> toAddresses, 
            IEnumerable<MailAddress> replyToAddresses, 
            IEnumerable<MailAddress> bccMailAddresses, 
            IEnumerable<MailAddress> ccMailAddresses, 
            string attachmentFilePath = null, string attachmentFileName = null)
        {
            var message = new MailMessage {From = fromAddress};

            foreach (var toAddress in toAddresses)
                message.To.Add(toAddress);

            foreach (var replayToAddress in replyToAddresses)
                message.ReplyToList.Add(replayToAddress);

            foreach (var address in bccMailAddresses)
                message.Bcc.Add(address);

            foreach (var address in ccMailAddresses)
                message.CC.Add(address);

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            if (!string.IsNullOrWhiteSpace(attachmentFilePath) && File.Exists(attachmentFilePath))
            {
                var attachment = new Attachment(attachmentFilePath);
                attachment.ContentDisposition.CreationDate = File.GetCreationTime(attachmentFilePath);
                attachment.ContentDisposition.ModificationDate = File.GetLastWriteTime(attachmentFilePath);
                attachment.ContentDisposition.ReadDate = File.GetLastAccessTime(attachmentFilePath);
                if (!string.IsNullOrEmpty(attachmentFileName))
                {
                    attachment.Name = attachmentFileName;
                }
                message.Attachments.Add(attachment);
            }

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = emailAccount.Host;
                smtpClient.Send(message);
            }
        }
    }
}
