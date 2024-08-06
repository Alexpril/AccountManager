namespace AccountPhoneManager.DAL.Models
{
    public class Account
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? Status { get; set; } = "Active";
        public IList<Guid> PhoneNumberIds { get; set; } = [];
        public virtual IList<Phone> Phone { get; set; } = [];
    }
}
