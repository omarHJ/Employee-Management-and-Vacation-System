using Employee_Management_and_Vacation_System.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ApplicationDbContext())
            {
                // Create the database if it doesn't exist and apply migrations
                context.Database.Migrate();

                // Seed initial data
                DataSeeder.SeedData(context);

                var employeeService = new EmployeeService(context);
                var vacationRequestService = new VacationRequestService(context, employeeService);

                while (true)
                {
                    Console.WriteLine("\nEmployee Management System");
                    Console.WriteLine("1. View All Employees");
                    Console.WriteLine("2. View Employee Details");
                    Console.WriteLine("3. Update Employee Information");
                    Console.WriteLine("4. Submit Vacation Request");
                    Console.WriteLine("5. View Pending Requests to Approve");
                    Console.WriteLine("6. Approve Vacation Request");
                    Console.WriteLine("7. Decline Vacation Request");
                    Console.WriteLine("8. View Approved Vacation History");
                    Console.WriteLine("9. View Employees with Pending Requests");
                    Console.WriteLine("0. Exit");
                    Console.Write("Select an option: ");

                    string option = Console.ReadLine();
                    Console.WriteLine();

                    try
                    {
                        switch (option)
                        {
                            case "1":
                                var employees = employeeService.GetAllEmployees();
                                foreach (var emp in employees)
                                {
                                    Console.WriteLine($"{emp.EmployeeNumber} | {emp.EmployeeName} | {emp.DepartmentName} | ${emp.Salary}");
                                }
                                break;

                            case "2":
                                Console.Write("Enter Employee Number: ");
                                string empNum = Console.ReadLine();
                                var details = employeeService.GetEmployeeDetails(empNum);
                                if (details != null)
                                {
                                    Console.WriteLine($"Employee Number: {details.EmployeeNumber}");
                                    Console.WriteLine($"Name: {details.EmployeeName}");
                                    Console.WriteLine($"Department: {details.DepartmentName}");
                                    Console.WriteLine($"Position: {details.PositionName}");
                                    Console.WriteLine($"Reports To: {details.ReportedToName}");
                                    Console.WriteLine($"Vacation Days Left: {details.VacationDaysLeft}");
                                }
                                else
                                {
                                    Console.WriteLine("Employee not found.");
                                }
                                break;

                            case "3":
                                Console.Write("Enter Employee Number to update: ");
                                string updateEmpNum = Console.ReadLine();
                                Console.Write("New Name (leave blank to keep current): ");
                                string name = Console.ReadLine();

                                Console.Write("New Department ID (leave blank to keep current): ");
                                string deptIdStr = Console.ReadLine();
                                int? deptId = string.IsNullOrEmpty(deptIdStr) ? null : int.Parse(deptIdStr);

                                Console.Write("New Position ID (leave blank to keep current): ");
                                string posIdStr = Console.ReadLine();
                                int? posId = string.IsNullOrEmpty(posIdStr) ? null : int.Parse(posIdStr);

                                Console.Write("New Salary (leave blank to keep current): ");
                                string salaryStr = Console.ReadLine();
                                decimal? salary = string.IsNullOrEmpty(salaryStr) ? null : decimal.Parse(salaryStr);

                                employeeService.UpdateEmployeeInfo(
                                    updateEmpNum,
                                    string.IsNullOrEmpty(name) ? null : name,
                                    deptId,
                                    posId,
                                    salary
                                );
                                Console.WriteLine("Employee updated successfully");
                                break;

                            case "4":
                                Console.Write("Employee Number: ");
                                string reqEmpNum = Console.ReadLine();

                                Console.Write("Vacation Type (S/U/A/O/B): ");
                                string vacType = Console.ReadLine();

                                Console.Write("Start Date (yyyy-MM-dd): ");
                                DateTime startDate = DateTime.Parse(Console.ReadLine());

                                Console.Write("End Date (yyyy-MM-dd): ");
                                DateTime endDate = DateTime.Parse(Console.ReadLine());

                                Console.Write("Description: ");
                                string desc = Console.ReadLine();

                                vacationRequestService.SubmitVacationRequest(
                                    reqEmpNum, vacType, startDate, endDate, desc);
                                Console.WriteLine("Vacation request submitted successfully");
                                break;

                            case "5":
                                Console.Write("Manager Employee Number: ");
                                string mgrEmpNum = Console.ReadLine();
                                var pendingRequests = employeeService.GetPendingVacationRequests(mgrEmpNum);

                                foreach (var req in pendingRequests)
                                {
                                    Console.WriteLine($"Request: {req.Description}");
                                    Console.WriteLine($"Employee: {req.EmployeeNumber} - {req.EmployeeName}");
                                    Console.WriteLine($"Submitted: {req.SubmittedOn}");
                                    Console.WriteLine($"Duration: {req.Duration} ({req.StartDate:yyyy-MM-dd} to {req.EndDate:yyyy-MM-dd})");
                                    Console.WriteLine($"Employee Salary: ${req.EmployeeSalary}");
                                    Console.WriteLine();
                                }
                                break;

                            case "6":
                                Console.Write("Request ID to approve: ");
                                int approveReqId = int.Parse(Console.ReadLine());

                                Console.Write("Your Employee Number: ");
                                string approverEmpNum = Console.ReadLine();

                                vacationRequestService.ApproveVacationRequest(approveReqId, approverEmpNum);
                                Console.WriteLine("Vacation request approved successfully");
                                break;

                            case "7":
                                Console.Write("Request ID to decline: ");
                                int declineReqId = int.Parse(Console.ReadLine());

                                Console.Write("Your Employee Number: ");
                                string declinerEmpNum = Console.ReadLine();

                                vacationRequestService.DeclineVacationRequest(declineReqId, declinerEmpNum);
                                Console.WriteLine("Vacation request declined successfully");
                                break;

                            case "8":
                                Console.Write("Employee Number: ");
                                string historyEmpNum = Console.ReadLine();
                                var history = employeeService.GetApprovedVacationHistory(historyEmpNum);

                                foreach (var req in history)
                                {
                                    Console.WriteLine($"Type: {req.VacationType}");
                                    Console.WriteLine($"Description: {req.Description}");
                                    Console.WriteLine($"Duration: {req.Duration} (Total: {req.TotalVacationDays} days)");
                                    Console.WriteLine($"Approved By: {req.ApprovedBy}");
                                    Console.WriteLine();
                                }
                                break;

                            case "9":
                                var empsWithPending = employeeService.GetEmployeesWithPendingRequests();
                                foreach (var emp in empsWithPending)
                                {
                                    Console.WriteLine($"{emp.EmployeeNumber} - {emp.EmployeeName} | Pending Requests: {emp.PendingRequestsCount}");
                                }
                                break;

                            case "0":
                                return;

                            default:
                                Console.WriteLine("Invalid option.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
    }
}