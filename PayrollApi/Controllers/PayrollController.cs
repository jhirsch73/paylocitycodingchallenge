using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Services.Models;
using PayrollApi.Models;

namespace Payroll.Services.Controllers
{

    /// TODO: Modify Payroll Route to strictly do Payroll Functions
    [Route("api/Payroll")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly PayrollContext _context;

        public PayrollController(PayrollContext context)
        {
            _context = context;
        }

        

        // GET: api/EmployeeItems
        [HttpGet("preview")]
        public async Task<ActionResult<IEnumerable<EmployeeItem>>> GetPayrollPreview()
        {
          if (_context.EmployeeItems == null)
          {
              return NotFound();
          }
            return await _context.EmployeeItems.ToListAsync().ConfigureAwait(false);
        }

        // Post: api/preview
        [HttpPost("employee/preview")]
        public async Task<ActionResult<PayrollItem>> GetDeductionsPreview(EmployeeItem employee)
        {
          var payrollService = new PayrollService();
          return await payrollService.GetCurrentPayroll(employee).ConfigureAwait(false);
        }

        // GET: api/EmployeeItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeItem>> GetEmployeeItem(long id)
        {
          if (_context.EmployeeItems == null)
          {
              return NotFound();
          }
            var employeeItem = await _context.EmployeeItems.FindAsync(id).ConfigureAwait(false);

            if (employeeItem == null)
            {
                return NotFound();
            }

            return employeeItem;
        }

        // PUT: api/EmployeeItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/employee/add/dependent/{id}")]
        public async Task<IActionResult> AddDependantItem(long id, DependentItem dependentItem)
        {
            var employeeItem = await _context.EmployeeItems.FindAsync(id).ConfigureAwait(false);
            if (employeeItem == null)
            {
                return NotFound();
            }
            employeeItem.Dependents.Add(dependentItem);
            
            _context.Entry(employeeItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/EmployeeItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("employee/create")]
        public async Task<ActionResult<CreateEmployeeItem>> CreateEmployee(CreateEmployeeItem employeeItem)
        {
            var employee = new EmployeeItem();
            if (employeeItem.BiweeklySalary <= 0) 
            {
                employee.BiweeklySalary = 2000.00m;
            }

            employee.Id = employeeItem.Id;
            employee.FirstName = employeeItem.FirstName;
            employee.LastName = employeeItem.LastName;
            employee.Dependents = new List<DependentItem>();

            if (_context.EmployeeItems == null)
            {
                return Problem("Entity set 'PayrollContext.EmployeeItems'  is null.");
            }
        
            _context.EmployeeItems.Add(employee);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            // return CreatedAtAction("GetemployeeItem", new { id = employeeItem.Id }, employeeItem);
            return CreatedAtAction(nameof(GetEmployeeItem), new { id = employeeItem.Id }, employeeItem);
        }

        // DELETE: api/EmployeeItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeItem(long id)
        {
            if (_context.EmployeeItems == null)
            {
                return NotFound();
            }
            var employeeItem = await _context.EmployeeItems.FindAsync(id).ConfigureAwait(false);
            if (employeeItem == null)
            {
                return NotFound();
            }

            _context.EmployeeItems.Remove(employeeItem);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }

        private bool EmployeeItemExists(long id)
        {
            return (_context.EmployeeItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}