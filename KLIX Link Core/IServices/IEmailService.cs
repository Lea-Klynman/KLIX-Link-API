using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.IServices
{
    public interface IEmailService
    {
        public Task<bool> SendEmailAsync(EmailRequest request);

    }
}
