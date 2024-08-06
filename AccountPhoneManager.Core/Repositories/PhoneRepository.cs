using AccountPhoneManager.Core.Abstraction;
using AccountPhoneManager.DAL.Contexts;
using AccountPhoneManager.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountPhoneManager.Core.Repositories
{
    public class PhoneRepository(AccountManagerDbContexts dbContext) : IPhoneRepository
    {
        private readonly AccountManagerDbContexts _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Inserts phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void InsertPhoneNumber(string phoneNumber)
        {
            var phone = new Phone
            {
                Id = Guid.NewGuid(),
                Number = phoneNumber
            };

            _dbContext.PhoneNumbers.Add(phone);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes phone number by id
        /// </summary>
        /// <param name="phoneId"></param>
        /// <exception cref="Exception"></exception>
        public void DeletePhoneNumber(Guid phoneId)
        {
            var phone = _dbContext.PhoneNumbers
                .FirstOrDefault(p => p.Id == phoneId);

            if (phone == null)
            {
                throw new Exception("Phone number not found.");
            }

            // Find and update any account that has this phone number
            var accounts = _dbContext.Accounts
                .Include(a => a.PhoneNumbers)
                .Where(a => a.PhoneNumbers.Any(p => p.Id == phoneId))
                .ToList();

            foreach (var account in accounts)
            {
                var phoneToRemove = account.PhoneNumbers.FirstOrDefault(p => p.Id == phoneId);
                if (phoneToRemove != null)
                {
                    account.PhoneNumbers.Remove(phoneToRemove);
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
