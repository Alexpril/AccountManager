namespace AccountPhoneManager.DAL.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public ICollection<Guid> PhoneNumberId { get; set; } = [];
        public virtual ICollection<Phone> Phone { get; set; } = [];
    }
}
