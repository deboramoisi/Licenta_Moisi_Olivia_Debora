using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.MailService
{
    public class EmailAddress
    {
        public string DisplayName { get; set; }
        public string Address { get; set; }

        public EmailAddress()
        {
            DisplayName = "Contsal";
        }

        public static implicit operator List<object>(EmailAddress v)
        {
            throw new NotImplementedException();
        }
    }
}
