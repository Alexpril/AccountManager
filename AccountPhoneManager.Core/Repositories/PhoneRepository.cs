using AccountPhoneManager.Core.Data;
using AccountPhoneManager.DAL.Contexts;
using AccountPhoneManager.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountPhoneManager.Core.Repositories
{
    public class PhoneRepository(AccountManagerDbContexts dbContext)
    {
        private readonly AccountManagerDbContexts _dbContext = dbContext;

        public void CreatePhoneNumber(string phoneNumber)
        {
            var phone = new Phone
            {
                Id = Guid.NewGuid(),
                Number = phoneNumber,
                IsDeleted = false
            };

            _dbContext.PhoneNumbers.Add(phone);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Phone> ReadPhoneNumbersByAccountId(Guid accountId)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);

            return account?.Phone ?? Enumerable.Empty<Phone>();
        }

        public void DeletePhoneNumber(Guid phoneId)
        {
            var phone = _dbContext.PhoneNumbers.FirstOrDefault(p => p.Id == phoneId);
            if (phone != null)
            {
                phone.IsDeleted = true;

                var account = _dbContext.Accounts.FirstOrDefault(a => a.PhoneNumberIds.Contains(phoneId));
                account?.PhoneNumberIds.Remove(phoneId);

                _dbContext.SaveChanges();
            }
        }
    }
}
