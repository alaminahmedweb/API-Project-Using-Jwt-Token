using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Project_With_JWT_Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCustomer()
        {
            return Ok("You Hit me") ;
        }
    }
}
