using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sgcae.Domain.Employees;

namespace Sgcae.Infrastructure.Persistence.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(employee => employee.Id);
        builder.Property(employee => employee.Id).ValueGeneratedNever();

        builder.Property(employee => employee.EmployeeCode)
            .HasMaxLength(Employee.EmployeeCodeMaxLength)
            .IsRequired();

        builder.Property(employee => employee.FirstName)
            .HasMaxLength(Employee.NameMaxLength)
            .IsRequired();

        builder.Property(employee => employee.LastName)
            .HasMaxLength(Employee.NameMaxLength)
            .IsRequired();

        builder.Property(employee => employee.Email)
            .HasConversion(email => email.Value, value => Email.Create(value).Value)
            .HasMaxLength(Email.MaxLength)
            .IsRequired();

        builder.Property(employee => employee.JobTitle)
            .HasMaxLength(Employee.JobTitleMaxLength)
            .IsRequired();

        builder.Property(employee => employee.Department)
            .HasMaxLength(Employee.DepartmentMaxLength)
            .IsRequired();

        builder.Property(employee => employee.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(employee => employee.EmployeeCode).IsUnique();
        builder.HasIndex(employee => employee.Email).IsUnique();

        builder.HasMany(employee => employee.Credentials)
            .WithOne()
            .HasForeignKey(credential => credential.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(employee => employee.Credentials)
            .HasField("_credentials")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
