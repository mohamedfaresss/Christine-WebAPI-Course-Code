using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDotNet.DTO;
using WebAPIDotNet.Model;

namespace WebAPIDotNet.Controllers
{
    [Route("api/[controller]")]//api/Department
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        itiContext context;
        public DepartmentController(itiContext _context)
        {
            context = _context;
        }

        [Authorize]
        [HttpGet("p")]
        public ActionResult<List<DeptWithEmpCountDto>> GetDeptDetails()
        {
                List<Department> deptlist=
                context.Departmwnt.Include(d=>d.Emps).ToList();
            List<DeptWithEmpCountDto> deptListDto = new List<DeptWithEmpCountDto>();

            foreach (Department item in deptlist)
            {
                DeptWithEmpCountDto deptDto = new DeptWithEmpCountDto();
                deptDto.ID = item.Id;
                deptDto.Name = item.Name;
                deptDto.EmpCount = item.Emps.Count();
                deptListDto.Add(deptDto);
            }
            return deptListDto;

}

        [HttpGet]//api/Department
        public IActionResult Displatdept()
        {
            List<Department> deptlist =
            context.Departmwnt.ToList();
            return Ok(deptlist);
        }


        [HttpGet("{id:int}")]
        //[Route("{id}")] // api/Department/1
        public IActionResult GetById(int id)
        {
            Department dept =
            context.Departmwnt.FirstOrDefault(d => d.Id == id);
            return Ok(dept);


        }

        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {

            Department dept =
            context.Departmwnt.FirstOrDefault(d => d.Name == name);
            return Ok(dept);

        }
        // api/Department post
        [HttpPost]
        public IActionResult AddDept(Department dept)
        {
            context.Departmwnt.Add(dept);
            context.SaveChanges();
            //  return Ok();//200
            // return Created($"http://localhost:65427/api/Department{dept.Id}",dept);//201
            return CreatedAtAction("GetById", new { id = dept.Id }, dept);
        }



        [HttpPut("{id:int}")]
        public IActionResult Updatedept(int id, Department deptFromRequest)
        {
            Department departmentFromDB =
            context.Departmwnt.FirstOrDefault(d => d.Id == id);
            if (departmentFromDB != null)
            {
                departmentFromDB.Name = deptFromRequest.Name;
                departmentFromDB.MangerName = deptFromRequest.MangerName;
                context.SaveChanges();
                return NoContent(); // NoContent ده عندي هو ال DEFAULT في ال PUT - DELETE
            }
            else
            {
                return NotFound("Department Not Found");
            }
        }

        //[HttpDelete]
        //public IActionResult Deletedept();
    }
}
