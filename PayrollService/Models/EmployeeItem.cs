namespace Payroll.Services.Models 
{
    public class CreateEmployeeItem : PersonItem
    {
        public long Id { get; set; }

        public decimal BiweeklySalary { get; set; }
    }

    public partial class EmployeeItem : PayrollItem
    {

        public long Id { get; set; }
        public EmployeeItem()
        {
            Dependents = new List<DependentItem>();
        }
        public decimal BiweeklySalary { get; set; }
        public virtual ICollection<DependentItem> Dependents { get; set; }
    }
}