using Employee_Management_and_Vacation_System.Entities;
using EmployeeManagementSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_and_Vacation_System.Services
{
    public class VacationRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeeService _employeeService;

        public VacationRequestService(ApplicationDbContext context, EmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        public void SubmitVacationRequest(string employeeNumber, string vacationTypeCode,
            DateTime startDate, DateTime endDate, string description)
        {
            if (!_context.Employees.Any(e => e.EmployeeNumber == employeeNumber))
                throw new Exception($"Employee with number {employeeNumber} not found");

            if (!_context.VacationTypes.Any(vt => vt.VacationTypeCode == vacationTypeCode))
                throw new Exception($"Invalid vacation type code: {vacationTypeCode}");

            // Calculate vacation days (excluding weekends)
            int totalDays = 0;
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    totalDays++;
            }

            // Check for overlapping vacations
            bool hasOverlap = _context.VacationRequests
                .Any(vr => vr.EmployeeNumber == employeeNumber &&
                         vr.RequestStateId != 3 && // Not declined
                         ((startDate >= vr.StartDate && startDate <= vr.EndDate) ||
                          (endDate >= vr.StartDate && endDate <= vr.EndDate) ||
                          (startDate <= vr.StartDate && endDate >= vr.EndDate)));

            if (hasOverlap)
                throw new Exception("This vacation request overlaps with an existing request");

            var request = new VacationRequest
            {
                EmployeeNumber = employeeNumber,
                VacationTypeCode = vacationTypeCode,
                StartDate = startDate,
                EndDate = endDate,
                Description = description,
                RequestSubmissionDate = DateTime.Now,
                TotalVacationDays = totalDays,
                RequestStateId = 1 // Submitted
            };

            _context.VacationRequests.Add(request);
            _context.SaveChanges();
        }

        public void ApproveVacationRequest(int requestId, string approverEmployeeNumber)
        {
            var request = _context.VacationRequests.FirstOrDefault(vr => vr.RequestId == requestId);
            if (request == null)
                throw new Exception($"Vacation request with ID {requestId} not found");

            if (request.RequestStateId != 1)
                throw new Exception("This request is not in a state that can be approved");

            var requester = _context.Employees.FirstOrDefault(e => e.EmployeeNumber == request.EmployeeNumber);
            if (requester.ReportedToEmployeeNumber != approverEmployeeNumber)
                throw new Exception("Only the direct manager can approve this request");

            request.RequestStateId = 2; // Approved
            request.ApprovedByEmployeeNumber = approverEmployeeNumber;

            _context.SaveChanges();

            // Update vacation days balance
            _employeeService.UpdateVacationDaysBalance(request.EmployeeNumber, request.TotalVacationDays);
        }

        public void DeclineVacationRequest(int requestId, string declinerEmployeeNumber)
        {
            var request = _context.VacationRequests.FirstOrDefault(vr => vr.RequestId == requestId);
            if (request == null)
                throw new Exception($"Vacation request with ID {requestId} not found");

            if (request.RequestStateId != 1)
                throw new Exception("This request is not in a state that can be declined");

            var requester = _context.Employees.FirstOrDefault(e => e.EmployeeNumber == request.EmployeeNumber);
            if (requester.ReportedToEmployeeNumber != declinerEmployeeNumber)
                throw new Exception("Only the direct manager can decline this request");

            request.RequestStateId = 3; // Declined
            request.DeclinedByEmployeeNumber = declinerEmployeeNumber;

            _context.SaveChanges();
        }
    }
}
