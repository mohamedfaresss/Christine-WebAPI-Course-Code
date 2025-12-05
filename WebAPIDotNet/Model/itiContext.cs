using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPIDotNet.Model
{
    public class itiContext : IdentityDbContext<ApplicationUser>
    {
     public DbSet<Department> Departmwnt { get; set;}
     public DbSet<Employee> Employee {  get; set;}
        public itiContext(DbContextOptions<itiContext> options): base(options) 
         {


        }




}
}
