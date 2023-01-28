using System;
using Payroll.Services.Models;

namespace Payroll.Services {
    public class PayrollService
    {
        public const decimal _EmployeeAnnualGrossDeduction = 1000.00m;
        public const decimal _DependentAnnualGrossDeduction = 500.00m;
        private const decimal _DeductionsDiscountForNameA = 0.1m;
        private const decimal _NumberOfPaychecks = 26m;

        // public async Task<PayrollItem> GetCurrentPayroll(List<EmployeeItem> employees)
        // {

        // }
        public async Task<PayrollItem> GetCurrentPayroll(EmployeeItem employee)
        {
            var payroll = new PayrollItem();
            var currentDeductions = GetCurrentPayrollDeduction(employee);
            var biWeeklyNet = employee.BiweeklySalary - currentDeductions;
            payroll.BiWeeklyDeductions = currentDeductions.ToString("C");
            payroll.BiweeklyGross = employee.BiweeklySalary.ToString("C");
            payroll.BiweeklyNet = biWeeklyNet.ToString("C");

            return payroll;

        }
        public decimal GetCurrentPayrollDeduction(EmployeeItem employee)
        {
            var result = 0.00m;
            var employeePaycheckDeduction = _EmployeeAnnualGrossDeduction / _NumberOfPaychecks;

            if (employee.FirstName.Substring(0, 1) == "A")
            {
                employeePaycheckDeduction = employeePaycheckDeduction * 0.9m;
            }

            if ((employee.CurrentTotalDeductions + employeePaycheckDeduction) > _EmployeeAnnualGrossDeduction) 
            {
                employeePaycheckDeduction = _EmployeeAnnualGrossDeduction - employee.CurrentTotalDeductions;
            }

            result += employeePaycheckDeduction;

            foreach(var dependent in employee.Dependents)
            {
                var dependentPaycheckDeduction = _DependentAnnualGrossDeduction / _NumberOfPaychecks;
                if (dependent.FirstName.Substring(0, 1) == "A")
                {
                    dependentPaycheckDeduction = dependentPaycheckDeduction * 0.9m;
                }

                if ((dependent.CurrentTotalDeductions + dependentPaycheckDeduction) > _DependentAnnualGrossDeduction) 
                {
                    dependentPaycheckDeduction = _DependentAnnualGrossDeduction - dependent.CurrentTotalDeductions;
                }

                result += dependentPaycheckDeduction;

            }
            return result;
        }
    }
}
