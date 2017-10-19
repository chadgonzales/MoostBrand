using System;
using System.Configuration;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

public class EmailSenderClient : EmailSender
{
    public void SendErrorNotification(string errMessage)
    {
        string emailBody = "Error details: " + errMessage;

        string emailSubject = "Jentec DTR Error Notification";

        base.SendEmail("arman.paulo.me@gmail.com", emailSubject, emailBody);
    }
}