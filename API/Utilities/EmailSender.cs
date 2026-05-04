using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.EmailSender
{
  public class EmailSender(IConfiguration configuration) : IMessageSender
  {
    private readonly string _smtpHost = configuration["Email:SmtpHost"] ?? throw new InvalidOperationException("Email:SmtpHost is Required");
    private readonly int _smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? throw new InvalidOperationException("Email:SmtpPort is Required"));
    private readonly string _username = configuration["Email:Username"] ?? throw new InvalidOperationException("Email:Username is Required");
    private readonly string _password = configuration["Email:Password"] ?? throw new InvalidOperationException("Email:Password is Required");
    private readonly string _senderEmail = configuration["Email:SenderEmail"] ?? throw new InvalidOperationException("Email:SenderEmail is Required");
    private readonly string _senderName = configuration["Email:SenderName"] ?? throw new InvalidOperationException("Email:SenderName is Required");


    public async Task SendOtpAsync(string toEmail, string otp, int expiryMinutes = 10)
    {
      try
      {
        using var mail = new MailMessage
        {
          From = new MailAddress(_senderEmail, _senderName, Encoding.UTF8),
          Subject = "Your One-Time Password (OTP)",
          Body = BuildHtmlTemplate(otp, expiryMinutes),
          IsBodyHtml = true,
          BodyEncoding = Encoding.UTF8,
          SubjectEncoding = Encoding.UTF8
        };

        mail.To.Add(toEmail);

        using var client = new SmtpClient(_smtpHost, _smtpPort)
        {
          EnableSsl = true,
          Credentials = new NetworkCredential(_username, _password),
          Timeout = 30_000
        };

        await client.SendMailAsync(mail);

      }
      catch(Exception er)
      {
        Console.WriteLine(er.Message);
        throw;
      }

    }

    private string BuildHtmlTemplate(string otp, int expiryMinutes)
    {
      // Split OTP into individual digits for the "box" style
      string digitBoxes = "";
      foreach (char c in otp)
        digitBoxes += $@"
                    <td style=""
                        width: 48px;
                        height: 56px;
                        text-align: center;
                        vertical-align: middle;
                        font-size: 28px;
                        font-weight: 700;
                        color: #1a1a2e;
                        background: #f0f4ff;
                        border: 2px solid #c7d2fe;
                        border-radius: 10px;
                        padding: 0 4px;
                    "">{c}</td>
                    <td style=""width:8px""></td>";

      return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"" />
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
  <title>OTP Verification</title>
</head>
<body style=""margin:0; padding:0; background:#f4f6fb; font-family: 'Segoe UI', Arial, sans-serif;"">

  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:#f4f6fb; padding: 40px 16px;"">
    <tr>
      <td align=""center"">

        <!-- Card -->
        <table width=""480"" cellpadding=""0"" cellspacing=""0"" style=""
            background: #ffffff;
            border-radius: 20px;
            box-shadow: 0 4px 24px rgba(0,0,0,0.08);
            overflow: hidden;
            max-width: 100%;
        "">

          <!-- Header -->
          <tr>
            <td style=""
                background: linear-gradient(135deg, #6366f1 0%, #4f46e5 100%);
                padding: 36px 40px 28px;
                text-align: center;
            "">
              <!-- Shield icon (inline SVG) -->
              <div style=""margin-bottom:14px;"">
                <svg width=""52"" height=""52"" viewBox=""0 0 24 24"" fill=""none""
                     xmlns=""http://www.w3.org/2000/svg"">
                  <path d=""M12 2L4 6v6c0 5.25 3.5 10.15 8 11.35C16.5 22.15 20 17.25 20 12V6L12 2z""
                        fill=""rgba(255,255,255,0.25)"" stroke=""white"" stroke-width=""1.5""
                        stroke-linejoin=""round""/>
                  <path d=""M9 12l2 2 4-4"" stroke=""white"" stroke-width=""2""
                        stroke-linecap=""round"" stroke-linejoin=""round""/>
                </svg>
              </div>
              <h1 style=""margin:0; color:#ffffff; font-size:22px; font-weight:700; letter-spacing:0.3px;"">
                Verify Your Identity
              </h1>
              <p style=""margin:8px 0 0; color:rgba(255,255,255,0.80); font-size:14px;"">
                Use the code below to complete verification
              </p>
            </td>
          </tr>

          <!-- Body -->
          <tr>
            <td style=""padding: 36px 40px 28px;"">

              <p style=""margin:0 0 24px; color:#4b5563; font-size:15px; line-height:1.6;"">
                Hi there! We received a request to verify your account.
                Please use the one-time password below. Do <strong>not</strong> share it with anyone.
              </p>

              <!-- OTP digit boxes -->
              <table cellpadding=""0"" cellspacing=""0"" style=""margin: 0 auto 28px;"">
                <tr>
                  {digitBoxes}
                </tr>
              </table>

              <!-- Expiry notice -->
              <table width=""100%"" cellpadding=""0"" cellspacing=""0"">
                <tr>
                  <td style=""
                      background: #fef9c3;
                      border: 1px solid #fde047;
                      border-radius: 10px;
                      padding: 12px 16px;
                      text-align: center;
                  "">
                    <p style=""margin:0; color:#854d0e; font-size:13px;"">
                      ⏱&nbsp; This OTP expires in <strong>{expiryMinutes} minutes</strong>.
                      Do not share it with anyone.
                    </p>
                  </td>
                </tr>
              </table>

              <p style=""margin:24px 0 0; color:#9ca3af; font-size:13px; line-height:1.6;"">
                If you didn't request this, you can safely ignore this email.
                Your account is not at risk.
              </p>

            </td>
          </tr>

          <!-- Footer -->
          <tr>
            <td style=""
                background: #f9fafb;
                border-top: 1px solid #e5e7eb;
                padding: 20px 40px;
                text-align: center;
            "">
              <p style=""margin:0; color:#9ca3af; font-size:12px;"">
                &copy; {DateTime.UtcNow.Year} {_senderName}. All rights reserved.<br/>
                This is an automated email — please do not reply.
              </p>
            </td>
          </tr>

        </table>
        <!-- /Card -->

      </td>
    </tr>
  </table>

</body>
</html>";
    }
  }
}