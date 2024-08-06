using AccountPhoneManager.Core.Abstraction;
using AccountPhoneManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AccountPhoneManager.API.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountManagmentController(IAccountManagmentService accountManagmentService) : ControllerBase
    {
        private readonly IAccountManagmentService _accountManagmentService = accountManagmentService;

        [HttpPost()]
        public IActionResult CreateAccount([FromQuery] string name)
        {
            try
            {
                _accountManagmentService.CreateAccount(name);
                return Ok("Account created successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{accountId}/phones")]
        public IActionResult GetPhoneNumbersByAccountId(Guid accountId)
        {
            try
            {
                var phoneNumbers = _accountManagmentService.GetPhoneNumbersByAccountId(accountId);
                return Ok(phoneNumbers);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{accountId}/update-status")]
        public IActionResult UpdateAccount(Guid accountId, [FromQuery] AccountStatus status)
        {
            try
            {
                _accountManagmentService.UpdateAccountStatus(accountId, status);
                return Ok("Account updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{accountId}/assign-number")]
        public IActionResult UpdateAccount(Guid accountId, [FromQuery] Guid phoneId)
        {
            try
            {
                _accountManagmentService.UpdateAccountNumber(accountId, phoneId);
                return Ok("Account updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
