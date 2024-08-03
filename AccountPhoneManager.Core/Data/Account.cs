using AccountPhoneManager.Core.Enums;

namespace AccountPhoneManager.Core.Data
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string? Name { get; set; }
        public AccountStatus Status { get; set; } = AccountStatus.Active;
        public Guid PhoneNumberId { get; set; }
    }
}
