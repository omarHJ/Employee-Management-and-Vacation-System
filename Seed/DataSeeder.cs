using Employee_Management_and_Vacation_System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EmployeeManagementSystem
{
    public static class DataSeeder
    {
        public static void SeedData(ApplicationDbContext context)
        {
            // Seed Departments
            if (!context.Departments.Any())
            {
                for (int i = 1; i <= 20; i++)
                {
                    context.Departments.Add(new Department { DepartmentName = $"Department {i}" });
                }
                context.SaveChanges();
            }

            // Seed Positions
            if (!context.Positions.Any())
            {
                for (int i = 1; i <= 20; i++)
                {
                    context.Positions.Add(new Position { PositionName = $"Position {i}" });
                }
                context.SaveChanges();
            }

            // Seed VacationTypes
            if (!context.VacationTypes.Any())
            {
                context.VacationTypes.AddRange(
                    new VacationType { VacationTypeCode = "S", VacationTypeName = "Sick" },
                    new VacationType { VacationTypeCode = "U", VacationTypeName = "Unpaid" },
                    new VacationType { VacationTypeCode = "A", VacationTypeName = "Annual" },
                    new VacationType { VacationTypeCode = "O", VacationTypeName = "Day Off" },
                    new VacationType { VacationTypeCode = "B", VacationTypeName = "Business Trip" }
                );
                context.SaveChanges();
            }

            // Seed RequestStates
            if (!context.RequestStates.Any())
            {
                context.RequestStates.AddRange(
                    new RequestState { StateId = 1, StateName = "Submitted" },
                    new RequestState { StateId = 2, StateName = "Approved" },
                    new RequestState { StateId = 3, StateName = "Declined" }
                );
                context.SaveChanges();
            }

            // Seed Employees
            if (!context.Employees.Any())
            {
                for (int i = 1; i <= 10; i++)
                {
                    string empNum = $"EMP{i:D3}";
                    context.Employees.Add(new Employee
                    {
                        EmployeeNumber = empNum,
                        EmployeeName = $"Employee {i}",
                        DepartmentId = (i % 20) + 1,
                        PositionId = (i % 20) + 1,
                        GenderCode = i % 2 == 0 ? "M" : "F",
                        ReportedToEmployeeNumber = i > 1 ? $"EMP{i - 1:D3}" : null,
                        Salary = 2000 + (i * 100) + (i * 0.5m)
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
