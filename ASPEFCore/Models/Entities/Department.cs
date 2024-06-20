namespace ASPEFCore.Models.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public required string DepartmentName { get; set; }
        public bool IsActive { get; set; }
    }
}
