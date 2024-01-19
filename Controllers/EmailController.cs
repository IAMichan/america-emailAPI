namespace emailAPI.Controllers
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            try
            {
                string smtpServer = "smtp.mailtrap.io";
                int smtpPort = 587;
                string smtpUsername = "988ba158a9d815";
                string smtpPassword = "62f4a1ff316c7e";

                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    emailRequest.To = "america@api.test";
                    emailRequest.From = emailRequest.From;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    if (emailRequest.From != null)
                    {
                        MailMessage mailMessage = new MailMessage
                        {
                            From = new MailAddress(emailRequest.From),
                            Subject = emailRequest.Subject,
                            Body = emailRequest.Body,
                            IsBodyHtml = true
                        };

                        if (emailRequest.To != null)
                        {
                            mailMessage.To.Add(emailRequest.To);

                            await smtpClient.SendMailAsync(mailMessage);

                            return Ok("Email sent successfully");
                        }
                        else
                        {
                            return BadRequest("Recipient email address is null.");
                        }
                    }
                    else
                    {
                        return BadRequest("Sender email address is null.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }

    public class EmailRequest
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }

}
