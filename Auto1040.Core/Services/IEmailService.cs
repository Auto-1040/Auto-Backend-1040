using Auto1040.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Core.Services
{
    public interface IEmailService
    {
        public Task<bool> SendEmailAsync(EmailRequest request);

    }
}
