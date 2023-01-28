namespace Payroll.Services.Models 
{
    public class PayrollItem : PersonItem
    {
        public string? BiweeklyGross { get; set; }
        public string? BiWeeklyDeductions { get; set; }
        public string? BiweeklyNet { get; set; }
    }
}