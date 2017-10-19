using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmailSender
/// </summary>
public abstract class EmailSender
{
    private string sFromEmailD = "Jentec DTR <" + ConfigurationManager.AppSettings["emailAdd"].ToString() + ">";
    private string sFromEmail = ConfigurationManager.AppSettings["emailAdd"].ToString();
    private string sEPassword = ConfigurationManager.AppSettings["emailPassword"].ToString();
    private string sSMTPServer = ConfigurationManager.AppSettings["smtpHost"].ToString();
    private int sSMTPPort = int.Parse(ConfigurationManager.AppSettings["smtpPort"].ToString());

    public bool SendEmail(string to, string subject, string body, string cc = "", string bcc = "", string replyTo = "")
    {
        System.Net.Mail.MailAddress eFrom = new System.Net.Mail.MailAddress(this.sFromEmailD);

        System.Net.Mail.MailAddress eTo = new System.Net.Mail.MailAddress(to);

        System.Net.Mail.MailMessage MyMailMessage = new System.Net.Mail.MailMessage(eFrom, eTo);

        if (!string.IsNullOrEmpty(cc))
        {
            MyMailMessage.CC.Add(cc);
        }
        if (!string.IsNullOrEmpty(bcc))
        {
            MyMailMessage.Bcc.Add(bcc);
        }

        MyMailMessage.Subject = subject;
        MyMailMessage.Body = body;

        if (!String.IsNullOrEmpty(replyTo)) {
            MyMailMessage.ReplyToList.Add(replyTo);
        }

        System.Net.NetworkCredential mailAuthentication = new
        System.Net.NetworkCredential(this.sFromEmail, this.sEPassword);

        System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient(this.sSMTPServer, this.sSMTPPort);

        mailClient.EnableSsl = false;
        mailClient.UseDefaultCredentials = false;
        mailClient.Credentials = mailAuthentication;

        MyMailMessage.IsBodyHtml = true;

        try
        {
            mailClient.Send(MyMailMessage);
            return true;
        }
        catch
        {
            return false;
        }
    }
}