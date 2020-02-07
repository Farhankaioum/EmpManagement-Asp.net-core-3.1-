using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManagement.Utilities
{
    public class DomainValidationAttribute : ValidationAttribute
    {
        private readonly string validateEmail;

        public DomainValidationAttribute(string validateEmail)
        {
            this.validateEmail = validateEmail;
        }
        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split("@");
            
            return strings[1].ToUpper() == validateEmail.ToUpper();
        }
    }
}
