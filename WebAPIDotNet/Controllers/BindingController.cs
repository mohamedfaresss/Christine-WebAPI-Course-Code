using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIDotNet.Model;

namespace WebAPIDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BindingController : ControllerBase
    {
        [HttpGet("{name:alpha}/{age:int} ")]
        //api/Binding/12/ahmed
        //api/Binding/agr=12&name=ahmed
        public IActionResult testPremitive(int age, string? name)
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult TestObj(Department dept, string name)
        {
            return Ok();
        }

        [HttpGet("{Id:int}/{Name:alpha}/{Mangername:alpha}")]
        public IActionResult TestCustomBinding(
            [FromRoute] Department dept )
        {
            return Ok();
        }

 


    }
}
