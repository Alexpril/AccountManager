using AccountPhoneManager.Core.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AccountPhoneManager.API.Controllers
{
    [ApiController]
    [Route("phones")]
    public class PhoneManagmentController(IPhoneManagmentService phoneManagmentService) : ControllerBase
    {
        private readonly IPhoneManagmentService _phoneManagmentService = phoneManagmentService;

        [HttpPost("add")]
        public IActionResult CreatePhoneNumber([FromQuery] string phoneNumber)
        {
            try
            {
                _phoneManagmentService.CreatePhoneNumber(phoneNumber);
                return Ok("Phone number created successfully.");
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

        [HttpDelete("delete/{phoneId}")]
        public IActionResult DeletePhoneNumber(Guid phoneId)
        {
            try
            {
                _phoneManagmentService.DeletePhoneNumber(phoneId);
                return Ok("Phone number deleted successfully.");
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
