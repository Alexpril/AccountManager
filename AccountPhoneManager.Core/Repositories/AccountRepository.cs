using AccountPhoneManager.Core.Data;
using AccountPhoneManager.Core.Enums;
using AccountPhoneManager.DAL.Contexts;
using AccountPhoneManager.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountPhoneManager.Core.Repositories
{
    public class AccountRepository(AccountManagerDbContexts dbContext)
    {
        private readonly AccountManagerDbContexts _dbContext = dbContext;

        public void CreateAccount(string name)
        {
            _dbContext.Accounts.Add(new() { Name = name });
            _dbContext.SaveChanges();
        }

        public void UpdateAccountStatus(Guid accountId, UpdateAccountRequest request)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                throw new Exception();
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
                // Check if the phone number is already assigned to another account
                if (_dbContext.Accounts.Any(a => a.Id != accountId && a.PhoneNumberIds.Contains(request.PhoneNumberId.Value)))
                {
                    throw new Exception("Phone number is already assigned to another account.");
                }

                // Add the phone number to the existing list
                if (!account.PhoneNumberIds.Contains(request.PhoneNumberId.Value))
                {
                    account.PhoneNumberIds.Add(request.PhoneNumberId.Value);
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
