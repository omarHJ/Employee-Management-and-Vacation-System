using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_and_Vacation_System.Entities
{
    public class Employee
    {
        [Key]
        [MaxLength(6)]
        public string EmployeeNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string EmployeeName { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public int PositionId { get; set; }
        [ForeignKey("PositionId")]
        public Position Position { get; set; }

        [MaxLength(1)]
        public string GenderCode { get; set; } // M: Male, F: Female

        [MaxLength(6)]
        public string? ReportedToEmployeeNumber { get; set; }
        [ForeignKey("ReportedToEmployeeNumber")]
        public Employee ReportedTo { get; set; }

        public ICollection<Employee> Subordinates { get; set; }

        [Range(0, 24)]
        public int VacationDaysLeft { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        public ICollection<VacationRequest> VacationRequests { get; set; }
        public ICollection<VacationRequest> ApprovedRequests { get; set; }
        public ICollection<VacationRequest> DeclinedRequests { get; set; }

        public Employee()
        {
            VacationDaysLeft = 24;
        }
    }
}
