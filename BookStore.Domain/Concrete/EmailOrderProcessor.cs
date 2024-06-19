using System.Net;
using System.Net.Mail;
using System.Text;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "Orders@example.com";
        public string MailFromAddress = "BookStore@example.com";
        public bool UseSsl = true;
        public string Username = "SmtpUsername";
        public string Password = "SmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"D:\book_store_msg";
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings) => emailSettings = settings;
        public void ProcessOrder(Cart cart, DeliveryInfo deliveryInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                var body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("---")
                    .AppendLine("Товары:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Book.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (итого: {2:c}",
                        line.Quantity, line.Book.Name, subtotal);
                }
                body.AppendFormat("Общая стоимость: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Доставка:")
                    .AppendLine(deliveryInfo.FullName)
                    .AppendLine(deliveryInfo.Postcode.ToString())
                    .AppendLine(deliveryInfo.Country)
                    .AppendLine(deliveryInfo.City)
                    .AppendLine(deliveryInfo.Street)
                    .AppendLine(deliveryInfo.Building)
                    .AppendLine(deliveryInfo.Appartment.ToString())
                    .AppendLine("---");
                var mailMessage = new MailMessage(
                                       emailSettings.MailFromAddress,
                                       emailSettings.MailToAddress,
                                       "Новый заказ отправлен!",
                                       body.ToString());
                if (emailSettings.WriteAsFile)
                    mailMessage.BodyEncoding = Encoding.UTF8;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
