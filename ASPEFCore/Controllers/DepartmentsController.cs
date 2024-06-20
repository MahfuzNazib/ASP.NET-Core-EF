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
    public class DepartmentsController : ControllerBase
    {
        public readonly ApplicationDbContext dbContext;
        public readonly ILogger<DepartmentsController> logger;

        public DepartmentsController(ApplicationDbContext dbContext, ILogger<DepartmentsController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;   
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var departments = await dbContext.Departments.ToListAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fail to fetch all departments list. Error message : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Fail tp fetch all department list. Error Message : " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewDepartment(DepartmentDto departmentDto)
        {
            try
            {
                var department = new Department()
                {
                    DepartmentName = departmentDto.DepartmentName,
                    IsActive = departmentDto.IsActive,
                };

                await dbContext.Departments.AddAsync(department);
                await dbContext.SaveChangesAsync();

                return Ok(department);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create new department. Error message : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            try
            {
                var department = await dbContext.Departments.FindAsync(id);

                if (department is null)
                {
                    return NotFound();
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Faild to fetch department details by Id.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Faild to fetch department details by ID. Error message : " + ex.Message);
            }
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateDepartment(Guid id, DepartmentDto departmentDto)
        {
            try
            {
                var department = await dbContext.Departments.FindAsync(id);

                if (department is null)
                {
                    return NotFound();
                } 

                department.DepartmentName = departmentDto.DepartmentName;
                department.IsActive = departmentDto.IsActive;
                             
                await dbContext.SaveChangesAsync();

                return Ok(department);
                    
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Faild to update department info");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Faild to update department data. Error message : {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            try
            {
                var department = await dbContext.Departments.FindAsync(id);

                if (department == null)
                {
                    return NotFound();
                }

                dbContext.Departments.Remove(department);
                await dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Delete department is not completed. Error message : {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
