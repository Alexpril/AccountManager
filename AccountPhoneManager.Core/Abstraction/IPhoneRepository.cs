using AccountPhoneManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountPhoneManager.Core.Abstraction
{
    public interface IPhoneRepository
    {
        /// <summary>
        /// Inserts phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void InsertPhoneNumber(string phoneNumber);

        /// <summary>
        /// Deletes phone number by id
        /// </summary>
        /// <param name="phoneId"></param>
        /// <exception cref="Exception"></exception>
        public void DeletePhoneNumber(Guid phoneId);
    }
}
