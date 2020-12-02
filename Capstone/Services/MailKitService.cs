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
        public void SendAppointmentRequestEmail(PendingAppointment pendingAppointment)
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
                $"\tRequested Time: {pendingAppointment.ServiceStart:h:mm tt}\n" +
                $"\tServices Requested: {pendingAppointment.Services}\n" +
                $"\tEstimated Cost: {pendingAppointment.EstimatedCost:C}*\n" +
                $"\t\t*repair services are billed at an hourly rate depending on severity of repair - final cost determined at time of service\n" +
                $"\tAccepted Payments: Cash, Check (made payable to Wehmeier Music Service)\n\n" +
                $"Thank you for your business!\n" +
                $"\t-Keith Wehmeier, Wehmeier Music Service"
            };
            SendEmail(email);
        }

        public void SendAppointmentConfirmEmail(Appointment appointment)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("wehmeiermusic@gmail.com"));
            email.To.Add(MailboxAddress.Parse($"{appointment.Piano.Client.Email}"));
            email.Subject = "Appointment Confirmation";
            email.Body = new TextPart("plain")
            {
                Text = $"Dear {appointment.Piano.Client.FirstName},\n\n" +
                $"Your piano service request has been confirmed. Find the details of your appointment below:" +
                $"\tTime: {appointment.ServiceStart:h:mm tt} - {appointment.ServiceEnd:h:mm tt}\n" +
                $"\tServices: {appointment.Services}\n" +
                $"\tEstimated Cost: {appointment.EstimatedCost:C}*\n" +
                $"\t\t*repair services are billed at an hourly rate depending on severity of repair - final cost determined at time of service\n" +
                $"\tAccepted Payments: Cash, Check (made payable to Wehmeier Music Service), Venmo, Apple Pay\n\n" +
                $"In the event that you need to change your service details, please contact us at wehmeiermusicservice@gmail.com.\n\n" +
                $"Thank you for your business!\n" +
                $"\t-Keith Wehmeier, Wehmeier Music Service"
            };
            SendEmail(email);
        }

        public void SendServiceRemindEmail(Piano piano)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("wehmeiermusic@gmail.com"));
            email.To.Add(MailboxAddress.Parse($"{piano.Client.Email}"));
            email.Subject = "Regular Service Reminder";
            email.Body = new TextPart("plain")
            {
                Text = $"Dear {piano.Client.FirstName},\n\n" +
                $"Our records show that your {piano.Make} {piano.Configuration} was last serviced by WMS on {piano.LastService?.Date.ToString("d")}. " +
                $"In order to maximize the longevity and playability of your instrument, we recommend that pianos be serviced once a year. " +
                $"Please visit us at wehmeiermusicservice.com to schedule your regular service appointment today!\n\n" +
                $"Thank you for your business!\n" +
                $"\t-Keith Wehmeier, Wehmeier Music Service"
            };
            SendEmail(email);
        }

        public void SendUpcomingServiceEmail(Appointment appointment)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("wehmeiermusic@gmail.com"));
            email.To.Add(MailboxAddress.Parse($"{appointment.Piano.Client.Email}"));
            email.Subject = $"Appointment Reminder: {appointment.Piano.Make} {appointment.Piano.Configuration}";
            email.Body = new TextPart("plain")
            {
                Text = $"Dear {appointment.Piano.Client.FirstName},\n\n" +
                $"This is a reminder for your scheduled piano service tomorrow from {appointment.ServiceStart:h:mm tt} to {appointment.ServiceEnd:h:mm tt}. " +
                $"The expected cost of your service is {appointment.EstimatedCost:C}, though any repair services needed will be billed at an hourly rate upon completion. " +
                $"Please have an acceptable form of payment (Cash, Check, Venmo, or Apple Pay) ready at time of service. " +
                $"In the event that you need to change or cancel your service, please contact us as soon as possible at wehmeiermusicservice@gmail.com.\n\n" +
                $"Thank you for your business!\n" +
                $"\t-Keith Wehmeier, Wehmeier Music Service"
            };
            SendEmail(email);
        }

        private void SendEmail(MimeMessage email)
        {
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
