using AccountPhoneManager.Core.Abstraction;
using AccountPhoneManager.Core.Data;
using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.Core.Repositories;
using AccountPhoneManager.DAL.Models;

namespace AccountPhoneManager.Core.Services
{
    public class AccountManagmentService(IAccountRepository accountRepository) : IAccountManagmentService
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public void CreateAccount(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Account name cannot be null or empty.", nameof(name));
            }

            _accountRepository.InsertAccount(name);
        }

        public IEnumerable<Phone> GetPhoneNumbersByAccountId(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentException("Account ID cannot be empty.", nameof(accountId));
            }

            return _accountRepository.GetPhoneNumbersByAccountId(accountId);
        }

        public void UpdateAccountStatus(Guid accountId, AccountStatus status)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentException("Account ID cannot be empty.", nameof(accountId));
            }

            _accountRepository.UpdateAccount(accountId, new() { Status = status });
        }

        public void UpdateAccountNumber(Guid accountId, Guid phoneId)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentException("Account ID cannot be empty.", nameof(accountId));
            }

            _accountRepository.UpdateAccount(accountId, new() { PhoneNumberId = phoneId });
        }
    }
}
