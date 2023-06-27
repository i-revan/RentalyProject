namespace RentalyProject.Interfaces
{
    public interface IEmailService
    {
        Task SendMail(string email, string subject, string body, bool IsHtml = false);
    }
}
