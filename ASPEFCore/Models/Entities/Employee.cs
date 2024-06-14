namespace ASPEFCore.Models.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }

        public required string FullName { get; set; }

        public required string Email { get; set; }

        public string? Phone { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

    }
}
