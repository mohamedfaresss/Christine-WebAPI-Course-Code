using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIDotNet.Model;

namespace WebAPIDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BindingController : ControllerBase
    {
        // Route binding with constraints (alpha, int)
        // Example: GET /api/Binding/ahmed/30
        [HttpGet("{name:alpha}/{age:int} ")]
        // api/Binding/12/ahmed
        // api/Binding?agr=12&name=ahmed
        public IActionResult testPremitive(int age, string? name)
        {
            return Ok();
        }

        // Model binding: bind object from body and simple parameter
        [HttpPost]
        public IActionResult TestObj(Department dept, string name)
        {
            return Ok();
        }

        // Custom binding from route into a complex type (Department)
        [HttpGet("{Id:int}/{Name:alpha}/{Mangername:alpha}")]
        public IActionResult TestCustomBinding(
            [FromRoute] Department dept)
        {
            return Ok();
        }
    }
}
