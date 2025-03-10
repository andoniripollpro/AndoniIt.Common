﻿using System.Net.Mail;
using AndoIt.Common.Interface;
using System;
using System.Linq;

namespace AndoIt.Common.Common
{
    public class SmtpMailSender : IMailSender
    {
        public string Name { get; set; }
        public string SmtpServer { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }

        void IMailSender.Send(string subject, string text)
        {
            try
            {
                SmtpClient mySmtpClient = new SmtpClient(this.SmtpServer);

                if (string.IsNullOrWhiteSpace(this.User))
                {
                    mySmtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    mySmtpClient.UseDefaultCredentials = false;
                    System.Net.NetworkCredential basicAuthenticationInfo = new
                       System.Net.NetworkCredential(this.User, this.Password);
                    mySmtpClient.Credentials = basicAuthenticationInfo;
                }

                MailMessage myMail = new MailMessage();
                myMail.From = new MailAddress(this.From);
                var toList = this.To.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                toList.ToList().ForEach(x => { myMail.To.Add(new MailAddress(x.Trim())); });
                myMail.Subject = subject ?? this.Subject;
                myMail.Body = text ?? this.Text;

                var replyToList = this.ReplyTo.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                replyToList.ToList().ForEach(x => { myMail.ReplyToList.Add(new MailAddress(x.Trim())); });
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                myMail.IsBodyHtml = true; //??

                mySmtpClient.Send(myMail);
            }

            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
