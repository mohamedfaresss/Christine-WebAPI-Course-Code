namespace WebAPIDotNet.Model
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? MangerName { get; set; }

       public List<Employee>? Emps  { get; set; }
    }
}
