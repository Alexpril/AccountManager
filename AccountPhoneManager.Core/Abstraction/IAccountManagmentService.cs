using AccountPhoneManager.Core.Data;
using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.DAL.Models;

namespace AccountPhoneManager.Core.Abstraction
{
    public interface IAccountManagmentService
    {
        public void CreateAccount(string name);
        public IEnumerable<Phone> GetPhoneNumbersByAccountId(Guid accountId);
        public void UpdateAccountStatus(Guid accountId, AccountStatus status);
        public void UpdateAccountNumber(Guid accountId, Guid phoneId);
    }
}
