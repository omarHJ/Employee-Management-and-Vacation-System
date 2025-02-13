using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management_and_Vacation_System.Entities
{
    public class RequestState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Disables auto-increment
        public int StateId { get; set; }

        [Required]
        [MaxLength(10)]
        public string StateName { get; set; }
    }
}
