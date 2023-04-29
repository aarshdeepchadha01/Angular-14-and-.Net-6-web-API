using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly FullStackDbContext _fullStackDbContext;
        public EmployeesController(FullStackDbContext fullStackDbContext)
        {
            _fullStackDbContext = fullStackDbContext;
        }
        [HttpGet]
        public async  Task<IActionResult> GetAllEmployees()
        {
            var employees = await _fullStackDbContext.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody]Employee employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();
            await  _fullStackDbContext.Employees.AddAsync(employeeRequest);

            await _fullStackDbContext.SaveChangesAsync();

            return Ok(employeeRequest);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute]Guid id)
        {
            var employee = await _fullStackDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if(employee == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(employee);
            }
            
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, Employee udpateEmployeeRequest)
        {
            var employee = await _fullStackDbContext.Employees.FindAsync(id);

            if(employee == null)
            {
                return NotFound();
            }
            else
            {
                employee.Name = udpateEmployeeRequest.Name;
                employee.Email = udpateEmployeeRequest.Email;
                employee.Phone = udpateEmployeeRequest.Phone;
                employee.Salary = udpateEmployeeRequest.Salary;
                employee.Department = udpateEmployeeRequest.Department;

                await _fullStackDbContext.SaveChangesAsync();

                return Ok(employee);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await _fullStackDbContext.Employees.FindAsync(id);

            if(employee == null)
            {
                return NotFound();
            }
            else
            {
                 _fullStackDbContext.Employees.Remove(employee);
                await _fullStackDbContext.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
