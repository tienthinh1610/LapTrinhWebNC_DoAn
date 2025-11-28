using Microsoft.AspNetCore.Identity.UI.Services;

// Đặt trong namespace chính hoặc namespace Services của bạn
namespace SportsStore.Services
{
    // Cần phải implements IEmailSender để Identity chấp nhận
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        // Phương thức SendEmailAsync là bắt buộc
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Đây là nơi bạn sẽ gọi dịch vụ email thực (ví dụ: SendGrid, MailKit)
            // Hiện tại, chúng ta chỉ ghi log để giải quyết lỗi DI
            
            _logger.LogInformation($"--- DUMMY EMAIL SERVICE ---");
            _logger.LogInformation($"To: {email}");
            _logger.LogInformation($"Subject: {subject}");
            _logger.LogInformation($"Body (partial): {htmlMessage.Substring(0, Math.Min(htmlMessage.Length, 150))}...");
            _logger.LogInformation($"---------------------------");

            // Trả về Task hoàn thành để hệ thống biết việc gửi email "đã xong"
            return Task.CompletedTask;
        }
    }
}