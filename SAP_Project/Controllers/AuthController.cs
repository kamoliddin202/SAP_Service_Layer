using DataAccessLayer.Interfaces;
using DTOs.LoginDtos;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISapAuthServiceInterface _sapAuthServiceInterface;

        public AuthController(ISapAuthServiceInterface sapAuthServiceInterface)
        {
            _sapAuthServiceInterface = sapAuthServiceInterface;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(SapLoginRequest loginRequest)
        {
            try
            {
                var result = await _sapAuthServiceInterface.LoginAsync(loginRequest);
                return Ok(new { SessionCookie = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Serverda xatolik yuz berdi.", Error = ex.Message });
            }
        }
    }
}
