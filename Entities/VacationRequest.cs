using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_and_Vacation_System.Entities
{
    public class VacationRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public DateTime RequestSubmissionDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        [MaxLength(6)]
        public string EmployeeNumber { get; set; }
        [ForeignKey("EmployeeNumber")]
        public Employee Employee { get; set; }

        [Required]
        [MaxLength(1)]
        public string VacationTypeCode { get; set; }
        [ForeignKey("VacationTypeCode")]
        public VacationType VacationType { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Required]
        public int TotalVacationDays { get; set; }

        [Required]
        public int RequestStateId { get; set; }
        [ForeignKey("RequestStateId")]
        public RequestState RequestState { get; set; }

        [MaxLength(6)]
        public string ApprovedByEmployeeNumber { get; set; }
        [ForeignKey("ApprovedByEmployeeNumber")]
        public Employee ApprovedBy { get; set; }

        [MaxLength(6)]
        public string DeclinedByEmployeeNumber { get; set; }
        [ForeignKey("DeclinedByEmployeeNumber")]
        public Employee DeclinedBy { get; set; }
    }
}
