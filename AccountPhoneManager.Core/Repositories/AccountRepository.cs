using AccountPhoneManager.Core.Abstraction;
using AccountPhoneManager.Core.Data;
using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.DAL.Contexts;
using AccountPhoneManager.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Account = AccountPhoneManager.DAL.Models.Account;

namespace AccountPhoneManager.Core.Repositories
{
    public class AccountRepository(AccountManagerDbContexts dbContext) : IAccountRepository
    {
        private readonly AccountManagerDbContexts _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Creates account
        /// </summary>
        /// <param name="name"></param>
        public void InsertAccount(string name)
        {
            var account = new Account
            {
                Name = name,
                Status = AccountStatus.Active.ToString()
            };

            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Gets all assigned numbers
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IEnumerable<Phone> GetPhoneNumbersByAccountId(Guid accountId)
        {
            var account = _dbContext.Accounts
                .Include(a => a.PhoneNumbers)
                .FirstOrDefault(a => a.Id == accountId);

            return account?.PhoneNumbers ?? Enumerable.Empty<Phone>();
        }

        /// <summary>
        /// Updates account status or assigned numbers
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="request"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateAccount(Guid accountId, UpdateAccountRequest request)
        {
            var account = _dbContext.Accounts
                .Include(a => a.PhoneNumbers)
                .FirstOrDefault(a => a.Id == accountId);

            if (account == null)
            {
                throw new Exception("Account not found.");
            }

            if (account.Status == AccountStatus.Suspended.ToString() && request.PhoneNumberId.HasValue)
            {
                throw new Exception("Cannot update phone numbers for a suspended account.");
            }

            if (request.Status.HasValue)
            {
                account.Status = request.Status.Value.ToString();
            }

            if (request.PhoneNumberId.HasValue)
            {
                var phoneNumberId = request.PhoneNumberId.Value;

                // Check if the phone number is already assigned to another active account
                var isPhoneNumberAssigned = _dbContext.Accounts
                    .Any(a => a.Id != accountId && a.PhoneNumbers.Any(p => p.Id == phoneNumberId && a.Status == AccountStatus.Active.ToString()));

                if (isPhoneNumberAssigned)
                {
                    throw new Exception("Phone number is already assigned to another account.");
                }

                // Add the phone number to the existing list
                if (!account.PhoneNumbers.Any(p => p.Id == phoneNumberId))
                {
                    var phoneNumber = _dbContext.PhoneNumbers.FirstOrDefault(p => p.Id == phoneNumberId);
                    if (phoneNumber != null)
                    {
                        account.PhoneNumbers.Add(phoneNumber);
                    }
                    else
                    {
                        throw new Exception("Phone number not found.");
                    }
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
