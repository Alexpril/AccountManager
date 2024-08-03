namespace AccountPhoneManager.Core.Data
{
    public class PhoneNumber
    {
        public Guid PhoneNumberId { get; set; }
        public string Number { get; set; }
        public bool IsDeleted { get; set; } = true;

    }
}
