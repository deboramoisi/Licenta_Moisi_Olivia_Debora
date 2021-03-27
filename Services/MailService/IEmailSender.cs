using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.MailService
{
    public interface IEmailSender
    {
        void SendEmail(Message message);

    }
}
