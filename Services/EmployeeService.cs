using EmployeeManagementSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_and_Vacation_System.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void UpdateEmployeeInfo(string employeeNumber, string name = null, int? departmentId = null,
            int? positionId = null, decimal? salary = null)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);
            if (employee == null)
                throw new Exception($"Employee with number {employeeNumber} not found");

            if (name != null)
                employee.EmployeeName = name;

            if (departmentId.HasValue)
                employee.DepartmentId = departmentId.Value;

            if (positionId.HasValue)
                employee.PositionId = positionId.Value;

            if (salary.HasValue)
                employee.Salary = salary.Value;

            _context.SaveChanges();
        }

        public void UpdateVacationDaysBalance(string employeeNumber, int daysToDeduct)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);
            if (employee == null)
                throw new Exception($"Employee with number {employeeNumber} not found");

            if (employee.VacationDaysLeft < daysToDeduct)
                throw new Exception("Employee doesn't have enough vacation days left");

            employee.VacationDaysLeft -= daysToDeduct;
            _context.SaveChanges();
        }

        public IEnumerable<dynamic> GetAllEmployees()
        {
            return _context.Employees
                .Select(e => new
                {
                    e.EmployeeNumber,
                    e.EmployeeName,
                    DepartmentName = e.Department.DepartmentName,
                    e.Salary
                })
                .ToList();
        }

        public dynamic GetEmployeeDetails(string employeeNumber)
        {
            return _context.Employees
                .Where(e => e.EmployeeNumber == employeeNumber)
                .Select(e => new
                {
                    e.EmployeeNumber,
                    e.EmployeeName,
                    DepartmentName = e.Department.DepartmentName,
                    PositionName = e.Position.PositionName,
                    ReportedToName = e.ReportedTo.EmployeeName,
                    e.VacationDaysLeft
                })
                .FirstOrDefault();
        }

        public IEnumerable<dynamic> GetEmployeesWithPendingRequests()
        {
            return _context.Employees
                .Where(e => e.VacationRequests.Any(vr => vr.RequestStateId == 1))
                .Select(e => new
                {
                    e.EmployeeNumber,
                    e.EmployeeName,
                    PendingRequestsCount = e.VacationRequests.Count(vr => vr.RequestStateId == 1)
                })
                .ToList();
        }

        public IEnumerable<dynamic> GetApprovedVacationHistory(string employeeNumber)
        {
            return _context.VacationRequests
                .Where(vr => vr.EmployeeNumber == employeeNumber && vr.RequestStateId == 2)
                .Select(vr => new
                {
                    VacationType = vr.VacationType.VacationTypeName,
                    vr.Description,
                    Duration = $"{(vr.EndDate - vr.StartDate).Days + 1} days",
                    vr.TotalVacationDays,
                    ApprovedBy = vr.ApprovedBy.EmployeeName
                })
                .ToList();
        }

        public IEnumerable<dynamic> GetPendingVacationRequests(string managerEmployeeNumber)
        {
            return _context.VacationRequests
                .Where(vr => vr.Employee.ReportedToEmployeeNumber == managerEmployeeNumber && vr.RequestStateId == 1)
                .Select(vr => new
                {
                    vr.Description,
                    vr.EmployeeNumber,
                    EmployeeName = vr.Employee.EmployeeName,
                    SubmittedOn = vr.RequestSubmissionDate,
                    Duration = (vr.EndDate - vr.StartDate).Days <= 7
                        ? $"{(vr.EndDate - vr.StartDate).Days + 1} days"
                        : $"{(vr.EndDate - vr.StartDate).Days / 7} weeks, {(vr.EndDate - vr.StartDate).Days % 7} days",
                    vr.StartDate,
                    vr.EndDate,
                    EmployeeSalary = vr.Employee.Salary
                })
                .ToList();
        }
    }

}
