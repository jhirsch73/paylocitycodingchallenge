using Xunit;
using Payroll.Services;
using Payroll.Services.Models;

namespace Payroll.UnitTests.Services
{
    public class PayrollService_Calculations
    {
        [Fact]
        public async void Calculations_SingleEmployeeNoDependents()
        {
            var employee = new EmployeeItem();
            employee.Id = 1;
            employee.FirstName = "John";
            employee.LastName = "Doe";
            employee.CurrentTotalDeductions = 0;
            employee.BiweeklySalary = 2000.00m;
            employee.Dependents = new List<DependentItem>();

            var expectedDeduction = 1000.00m / 26.0m;
            var expectedNetPayroll = 2000.00m - expectedDeduction;
            var payrollService = new PayrollService();
            
            var payrollDeduction = payrollService.GetCurrentPayrollDeduction(employee);
            var payroll = await payrollService.GetCurrentPayroll(employee).ConfigureAwait(false);
            Assert.Equal(expectedDeduction.ToString("F"), payrollDeduction.ToString("F"));
            Assert.Equal(payroll.BiweeklyGross, 2000.00m.ToString("C"));
            Assert.Equal(payroll.BiWeeklyDeductions, payrollDeduction.ToString("C"));
            Assert.Equal(payroll.BiweeklyNet, expectedNetPayroll.ToString("C"));
        }

        [Fact]
        public void Calculations_SingleEmployeeNoDependentsDiscount()
        {
            var employee = new EmployeeItem();
            employee.Id = 1;
            employee.FirstName = "Albert";
            employee.LastName = "Doe";
            employee.CurrentTotalDeductions = 0m;
            employee.Dependents = new List<DependentItem>();

            var expectedResult = (1000.00m / 26.0m) * 0.9m;
            var payrollService = new PayrollService();
            var result = payrollService.GetCurrentPayrollDeduction(employee);
            Assert.Equal(expectedResult.ToString("F"), result.ToString("F"));
        }

        [Fact]
        public void Calculations_SingleEmployeeNoDependentsExceedsAnnual()
        {
            var employee = new EmployeeItem();
            employee.Id = 1;
            employee.FirstName = "John";
            employee.LastName = "Doe";
            employee.CurrentTotalDeductions = 990.00m;
            employee.Dependents = new List<DependentItem>();


            var expectedResult = 10.00m;
            var payrollService = new PayrollService();
            var result = payrollService.GetCurrentPayrollDeduction(employee);
            Assert.Equal(expectedResult.ToString("F"), result.ToString("F"));
        }

        [Fact]
        public void Calculations_SingleEmployeeWithDependentsNoDiscounts()
        {
            var spouse = new DependentItem();
            spouse.FirstName = "Jane";
            spouse.LastName = "Doe";
            spouse.CurrentTotalDeductions = 0m;

            var employee = new EmployeeItem();
            employee.Id = 1;
            employee.FirstName = "John";
            employee.LastName = "Doe";
            employee.CurrentTotalDeductions = 0m;
            var dependents = new List<DependentItem>();
            dependents.Add(spouse);
            employee.Dependents = dependents;

            var expectedEmployeeResult = 1000.00m / 26.0m;
            var expectedDependentResult = 500.00m / 26.0m;
            var expectedResult = expectedEmployeeResult + expectedDependentResult;
            var payrollService = new PayrollService();
            var result = payrollService.GetCurrentPayrollDeduction(employee);
            Assert.Equal(expectedResult.ToString("F"), result.ToString("F"));
        }

        [Fact]
        public void Calculations_SingleEmployeeWithDependentsWithDiscounts()
        {
            var spouse = new DependentItem();
            spouse.FirstName = "Jane";
            spouse.LastName = "Doe";
            spouse.CurrentTotalDeductions = 0m;

            var child = new DependentItem();
            child.FirstName = "Andrew";
            child.LastName = "Doe";
            child.CurrentTotalDeductions = 0m;

            var employee = new EmployeeItem();
            employee.Id = 1;
            employee.FirstName = "John";
            employee.LastName = "Doe";
            employee.CurrentTotalDeductions = 0m;
            var dependents = new List<DependentItem>();
            dependents.Add(spouse);
            dependents.Add(child);
            employee.Dependents = dependents;

            var expectedEmployeeResult = 1000.00m / 26.0m;
            var expectedSpouseResult = 500.00m / 26.0m;
            var expectedChildResult = (500.00m / 26.0m) * 0.9m;
            var expectedResult = expectedEmployeeResult + expectedSpouseResult + expectedChildResult;
            var payrollService = new PayrollService();
            var result = payrollService.GetCurrentPayrollDeduction(employee);
            Assert.Equal(expectedResult.ToString("F"), result.ToString("F"));
        }

        public void Calculations_SingleEmployeeDependentsExceedsAnnual()
        {
            var spouse = new DependentItem();
            spouse.FirstName = "Jane";
            spouse.LastName = "Doe";
            spouse.CurrentTotalDeductions = 500m;

            var child = new DependentItem();
            child.FirstName = "Andrew";
            child.LastName = "Doe";
            child.CurrentTotalDeductions = 500m;

            var employee = new EmployeeItem();
            employee.Id = 1;
            employee.FirstName = "John";
            employee.LastName = "Doe";
            employee.CurrentTotalDeductions = 1000m;
            var dependents = new List<DependentItem>(); 
            dependents.Add(spouse);
            dependents.Add(child);
            employee.Dependents = dependents;

            var expectedResult = 0m;
            var payrollService = new PayrollService();
            var result = payrollService.GetCurrentPayrollDeduction(employee);
            Assert.Equal(expectedResult.ToString("F"), result.ToString("F"));
        }
    }
}