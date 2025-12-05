using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDotNet.DTO;
using WebAPIDotNet.Model;

namespace WebAPIDotNet.Controllers
{
    [Route("api/[controller]")] // api/Department
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly itiContext context;

        public DepartmentController(itiContext _context)
        {
            context = _context;
        }

        // Get departments with employee count (Requires Authorization)
        [Authorize]
        [HttpGet("p")]
        public ActionResult<List<DeptWithEmpCountDto>> GetDeptDetails()
        {
            List<Department> deptList =
                context.Departmwnt.Include(d => d.Emps).ToList();

            List<DeptWithEmpCountDto> deptListDto = new List<DeptWithEmpCountDto>();

            foreach (Department item in deptList)
            {
                DeptWithEmpCountDto deptDto = new DeptWithEmpCountDto();
                deptDto.ID = item.Id;
                deptDto.Name = item.Name;
                deptDto.EmpCount = item.Emps.Count();
                deptListDto.Add(deptDto);
            }

            return deptListDto;
        }

        // Get all departments
        [HttpGet] // api/Department
        public IActionResult Displatdept()
        {
            List<Department> deptlist = context.Departmwnt.ToList();
            return Ok(deptlist);
        }

        // Get department by ID
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Department dept =
                context.Departmwnt.FirstOrDefault(d => d.Id == id);

            return Ok(dept);
        }

        // Get department by name
        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            Department dept =
                context.Departmwnt.FirstOrDefault(d => d.Name == name);

            return Ok(dept);
        }

        // Add new department
        [HttpPost] // api/Department
        public IActionResult AddDept(Department dept)
        {
            context.Departmwnt.Add(dept);
            context.SaveChanges();

            return CreatedAtAction("GetById", new { id = dept.Id }, dept);
        }

        // Update department by ID
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

                return NoContent(); // NoContent: default response for PUT/DELETE
            }
            else
            {
                return NotFound("Department Not Found");
            }
        }

        // [HttpDelete]
        // public IActionResult Deletedept();
    }
}
