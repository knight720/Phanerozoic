using System;
using System.Collections.Generic;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Googles
{
    /// <summary>
    /// 
    /// </summary>
    public class GmailService : IEmailService
    {
        private readonly string _userName;
        private readonly string _password;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public GmailService(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();

            _userName = configuration["Google:Gmail:Authenticate:UserName"];
            _password = configuration["Google:Gmail:Authenticate:Password"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="toList"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void Send(string from, List<string> toList, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, from));
            toList.ForEach(i => message.To.Add(new MailboxAddress(i, i)));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate(_userName, _password);

                try
                {
                    client.Send(message);

                    client.Disconnect(true);
                    Console.WriteLine("Gmail send success!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }
    }
}