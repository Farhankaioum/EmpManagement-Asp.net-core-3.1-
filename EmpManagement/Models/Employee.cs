using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required!")]
        public Dept? Department { get; set; }

        public string PhotoPath { get; set; }

    }
}
