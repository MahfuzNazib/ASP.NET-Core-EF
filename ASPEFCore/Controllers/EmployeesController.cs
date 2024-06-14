using ASPEFCore.Data;
using ASPEFCore.Models.DTO;
using ASPEFCore.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPEFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<EmployeesController> logger;
        public EmployeesController(ApplicationDbContext dbContext, ILogger<EmployeesController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }


        // Get All Employee
        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            try
            {
                var employees = await dbContext.Employees.ToListAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An Error Occured While Getting All Employee in EmployeeController.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error To Fetch All Employee");
            }
        }


        // Save New Employee 
        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeDTO employeeDTO)
        {
            try
            {
                var employee = new Employee()
                {
                    FullName    = employeeDTO.FullName,
                    Email       = employeeDTO.Email,
                    Phone       = employeeDTO.Phone,
                    Salary      = employeeDTO.Salary,
                    IsActive    = employeeDTO.IsActive,
                };

                await dbContext.Employees.AddAsync(employee);
                await dbContext.SaveChangesAsync();

                return Ok(employee);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Occured To Save Employee Data In DataBase");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }



        // Get Employee Details By ID
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {
                var employee = await dbContext.Employees.FindAsync(id);

                if (employee is null)
                {
                    return NotFound();
                }

                return Ok(employee);    
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Occured To Fetch Employee Information. Error Message : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Message : {ex.Message}");
            }
        }

        

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, AddEmployeeDTO employeeDTO)
        {
            var employee = dbContext.Employees.Find(id);
            
            if (employee is null)
            {
                return NotFound();
            }

            employee.FullName = employeeDTO.FullName;
            employee.Email = employeeDTO.Email;
            employee.Phone = employeeDTO.Phone;
            employee.Salary = employeeDTO.Salary;
            employee.IsActive = employeeDTO.IsActive;

            dbContext.SaveChanges();

            return Ok(employee);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id) 
        { 
            var employee = dbContext.Employees.Find(id);

            if (employee is null)
            {
                return NotFound();
            }
            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();

            return Ok(employee);    
        }
    }
}
