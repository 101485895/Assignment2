namespace COMP2139_ICE.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _sendGridKey;

        public EmailSender(IConfiguration configuration)
        {
            _sendGridKey = configuration["SendGrid:ApiKey"]
                ?? throw new ArgumentNullException(paramName: "SendGrid:ApiKey key missing in appsettings.json");
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var client = new SendGridClient(_sendGridKey);
                var from = new EmailAddress("matthew.zygiel@georgebrown.ca", name: "Project Inc.");
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: "This is a plain text message fallback.", htmlContent: message);
                var response = await client.SendEmailAsync(msg);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Body.ReadAsStringAsync();
                    Console.WriteLine($"Failed to send email. Status: {response.StatusCode}, Error: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending an email: {ex.Message}");
                throw;
            }
        }
    }
}
