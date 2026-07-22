using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Ports.Emails
{

    public interface IEmailSenderPort
    {
        Task Send2FAEmailAsync(string email, string code);
        Task SendAccountConfirmationAsync(string email);
        Task SendPasswordResetAsync(string email, string resetLink);
    }
}
