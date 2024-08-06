using AccountPhoneManager.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountPhoneManager.Core.Data
{
    public class UpdateAccountRequest
    {
        public AccountStatus? Status { get; set; }
        public Guid? PhoneNumberId { get; set; }
    }
}
