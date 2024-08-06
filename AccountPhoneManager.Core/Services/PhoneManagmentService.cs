using AccountPhoneManager.Core.Abstraction;
using FluentValidation;

namespace AccountPhoneManager.Core.Services
{
    public class PhoneManagmentService(IPhoneRepository phoneRepository, IValidator<string> validator): IPhoneManagmentService
    {
        private readonly IPhoneRepository _phoneRepository = phoneRepository;
        private readonly IValidator<string> _validator = validator;

        /// <summary>
        /// Used to create phone numbers
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <exception cref="ValidationException"></exception>
        public void CreatePhoneNumber(string phoneNumber)
        {
            var result = _validator.Validate(phoneNumber);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            _phoneRepository.InsertPhoneNumber(phoneNumber);
        }

        /// <summary>
        /// Used to delete phone numbers
        /// </summary>
        /// <param name="phoneId"></param>
        /// <exception cref="ArgumentException"></exception>
        public void DeletePhoneNumber(Guid phoneId)
        {
            if (phoneId == Guid.Empty)
            {
                throw new ArgumentException("Phone ID cannot be empty.", nameof(phoneId));
            }

            _phoneRepository.DeletePhoneNumber(phoneId);
        }
    }
}
