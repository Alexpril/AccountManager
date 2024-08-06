namespace AccountPhoneManager.DAL.Models
{
    public class Phone
    {
        /// <summary>
        /// Phone Id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Phone Number
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// Foreign key for account
        /// </summary>
        public Guid? AccountId { get; set; }

        /// <summary>
        /// Assigned account
        /// </summary>
        public virtual Account? Account { get; set; }
    }
}
