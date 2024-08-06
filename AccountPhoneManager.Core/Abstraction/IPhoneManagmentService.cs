namespace AccountPhoneManager.Core.Abstraction
{
    public interface IPhoneManagmentService
    {
        /// <summary>
        /// Used to create phone numbers
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void CreatePhoneNumber(string phoneNumber);

        /// <summary>
        /// Used to delete phone numbers
        /// </summary>
        /// <param name="phoneId"></param>
        public void DeletePhoneNumber(Guid phoneId);
    }
}
