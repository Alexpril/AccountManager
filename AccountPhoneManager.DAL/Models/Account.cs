namespace AccountPhoneManager.DAL.Models
{
    public class Account
    {
        /// <summary>
        /// Account id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Account name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Account status
        /// </summary>
        public string? Status { get; set; } = "Active";

        /// <summary>
        /// Assigned numbers
        /// </summary>
        public virtual IList<Phone> PhoneNumbers { get; set; } = new List<Phone>();
    }
}
