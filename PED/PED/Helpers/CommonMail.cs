using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PED.Helpers
{
    public class CommonMail
    {
        public static bool SendMail(string SMTPServer, int SMTPPort, bool SMTPSSL, string From, string password, string To, string Subject, string Body, string CC, string Attachment)
        {
            //To = "vanithac@medicount.com";
            //CC = "";
            //To = "AKing@medicount.com,SSiebenthaler@medicount.com";
            //CC = "arengasamy@medicount.com,vanithac@medicount.com";
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string Password = string.Empty;
            string msg = string.Empty;
            try
            {
                SmtpClient SmtpServer = new SmtpClient(SMTPServer);
                message.From = new MailAddress(From, "PED Admin");
                foreach (var item in To.Split(","))
                {
                    message.To.Add(item);
                }
                if (CC != "")
                {
                    foreach (var item in CC.Split(","))
                    {
                        message.CC.Add(item);
                    }
                }
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = Body;
                if (Attachment != "")
                //if (!string.IsNullOrWhiteSpace(Attachment) && File.Exists(Attachment))
                {
                    Attachment fileAttachment = new Attachment(Attachment);
                    message.Attachments.Add(fileAttachment);
                }
                //if (Attachment != "")
                //{
                //    var builder = new BodyBuilder
                //    {
                //        TextBody = "Please find the attached PDF."
                //    };
                //    builder.Attachments.Add(Attachment);
                //    message.Attachments.Add(attachment);
                //    //message.Body = builder.ToMessageBody();
                //    //string[] strArrAttachement = Attachment.Split(",");
                //    //foreach (var item in strArrAttachement)
                //    //{
                //    //    if (item != "" && File.Exists(item))
                //    //    {
                //    //        var fileStream = new FileStream(item, FileMode.Open, FileAccess.Read);
                //    //        Attachment attachment = new Attachment(fileStream, Path.GetFileName(item));
                //    //        message.Attachments.Add(attachment);
                //    //    }
                //    //}

                //}
                SmtpServer.Port = SMTPPort;
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential(From, password);
                SmtpServer.EnableSsl = SMTPSSL;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Send(message);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //public static bool SendMail(string SMTPServer, int SMTPPort, bool SMTPSSL, string From, string password, string To, string Subject, string Body, string CC, string Attachment)
        //{
        //    //To = "vanithac@medicount.com";
        //    //CC = "";
        //    //To = "AKing@medicount.com,SSiebenthaler@medicount.com";
        //    //CC = "arengasamy@medicount.com,vanithac@medicount.com";
        //    MailMessage message = new MailMessage();
        //    SmtpClient smtpClient = new SmtpClient();
        //    string Password = string.Empty;
        //    string msg = string.Empty;
        //    try
        //    {
        //        SmtpClient SmtpServer = new SmtpClient(SMTPServer);
        //        message.From = new MailAddress(From, "PED Admin");
        //        foreach (var item in To.Split(","))
        //        {
        //            message.To.Add(item);
        //        }
        //        if (CC != "")
        //        {
        //            foreach (var item in CC.Split(","))
        //            {
        //                message.CC.Add(item);
        //            }
        //        }
        //        message.Subject = Subject;
        //        message.IsBodyHtml = true;
        //        message.Body = Body;

        //        if (Attachment != "")
        //        {
        //            string[] strArrAttachement = Attachment.Split(",");
        //            foreach (var item in strArrAttachement)
        //            {
        //                if (item != "" && File.Exists(item))
        //                {
        //                    var fileStream = new FileStream(item, FileMode.Open, FileAccess.Read);
        //                    Attachment attachment = new Attachment(fileStream, Path.GetFileName(item));
        //                    message.Attachments.Add(attachment);
        //                }
        //            }

        //        }
        //        SmtpServer.Port = SMTPPort;
        //        SmtpServer.UseDefaultCredentials = true;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential(From, password);
        //        SmtpServer.EnableSsl = SMTPSSL;
        //        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        SmtpServer.Send(message);
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

    }
}
