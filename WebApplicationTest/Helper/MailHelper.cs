using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApplicationTest.Helper
{
    public class MailHelper
    {

        public void SendMail(string recipient, string content)
        {
            //string to = "M.Fischer@rto.de";
            string to = recipient;
            string from = "DQSPAM21@gmail.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Das Zitat de Tages!";
            message.Body = content;
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("DQSPAM21@gmail.com", "Testiwort_1994");
                    client.EnableSsl = true;
                    client.Send(message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
            }

        }
    }


}
