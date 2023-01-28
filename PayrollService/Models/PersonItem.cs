namespace Payroll.Services.Models 
{
    public abstract class PersonItem 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal CurrentTotalDeductions { get; set;}
    }
}