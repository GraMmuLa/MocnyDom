using System.Threading.Tasks;

namespace MocnyDom.Application.Email
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
        Task SendManyAsync(IEnumerable<string> to, string subject, string body);
    }
}
