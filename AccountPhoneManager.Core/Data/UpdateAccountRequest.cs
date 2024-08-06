using AccountPhoneManager.Core.Enums;

namespace AccountPhoneManager.Core.Data
{
    public class UpdateAccountRequest
    {
        public AccountStatus? Status { get; set; }
        public Guid? PhoneNumberId { get; set; }
    }
}
