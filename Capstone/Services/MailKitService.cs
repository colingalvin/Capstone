using Capstone.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Capstone.Services
{
    public class MailKitService
    {
        public void ConfirmAppointmentRequest(PendingAppointment pendingAppointment)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("wehmeiermusic@gmail.com"));
            email.To.Add(MailboxAddress.Parse($"{pendingAppointment.Email}"));
            email.Subject = "Appointment Request Received";
            email.Body = new TextPart("plain")
            {
                Text = $"Dear {pendingAppointment.FirstName},\n\n" +
                $"This email is to confirm that the piano service you requested has been received by Wehmeier Music Service and will be reviewed shortly. " +
                $"You will receive another email when your service appointmnt has been confirmed. " +
                $"The details of your service are listed below; if any changes to your appointment are needed, please contact us at wehmeiermusicservice@gmail.com.\n\n" +
                $"Appointment Details:\n" +
                $"\tRequested Time: {pendingAppointment.ServiceStart}\n" +
                $"\tServices Requested: {pendingAppointment.Services}\n" +
                $"\tEstimated Cost: ${pendingAppointment.EstimatedCost}*\n" +
                $"\t\t*repair services are billed at an hourly rate depending on severity of repair - final cost determined at time of service\n" +
                $"\tAccepted Payments: Cash, Check (made payable to Wehmeier Music Service)\n\n" +
                $"Thank you for your business!\n" +
                $"\t-Keith Wehmeier"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate($"{AuthenticationInfo.businessEmail}", $"{AuthenticationInfo.emailPassword}");
                client.Send(email);
                client.Disconnect(true);
            }
        }

    }
}
