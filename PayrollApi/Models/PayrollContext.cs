using Microsoft.EntityFrameworkCore;
using Payroll.Services.Models;

namespace PayrollApi.Models;

public class PayrollContext : DbContext
{
    public virtual DbSet<EmployeeItem> EmployeeItems { get; set; } = null!;

    public PayrollContext(DbContextOptions<PayrollContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeItem>()
            .HasMany(o => o.Dependents);
    }

}