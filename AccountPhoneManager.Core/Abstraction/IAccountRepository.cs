using AccountPhoneManager.Core.Data;
using AccountPhoneManager.DAL.Models;

namespace AccountPhoneManager.Core.Abstraction
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Creates account
        /// </summary>
        /// <param name="name"></param>
        public void InsertAccount(string name);

        /// <summary>
        /// Gets all assigned numbers
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IEnumerable<Phone> GetPhoneNumbersByAccountId(Guid accountId);

        /// <summary>
        /// Updates account status or assigned numbers
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="request"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateAccount(Guid accountId, UpdateAccountRequest request);

    }
}
