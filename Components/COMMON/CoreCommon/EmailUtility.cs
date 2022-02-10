using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Ropal.CoreCommon
{
    public class EmailMessage
    {
        public MailAddressCollection coll_To { get; set; } // sanjay : need to work on it
        public string To { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }

    }
    public static class EmailUtility
    {
        public static bool SendEmail(EmailMessage o_emailMessage)
        {
            try
            {
                if (String.IsNullOrEmpty(o_emailMessage.From) ||
                    String.IsNullOrEmpty(o_emailMessage.Subject) ||
                    String.IsNullOrEmpty(o_emailMessage.Body)
                    )
                    return false;

                if (CommonUtilities.IsObjectEmpty(o_emailMessage.coll_To))
                    return false;

                MailMessage mailMessage = new MailMessage(o_emailMessage.From, o_emailMessage.To, o_emailMessage.Subject, o_emailMessage.Body);
                mailMessage.Priority = MailPriority.Normal;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(mailMessage);                
            }
            catch (Exception ex)
            {

            }

            return true;
        }


    }
}
