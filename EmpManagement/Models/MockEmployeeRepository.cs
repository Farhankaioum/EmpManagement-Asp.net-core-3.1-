using System.Collections.Generic;
using System.Linq;

namespace EmpManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>
            {
                new Employee{Id = 1, Name ="Mary", Department = Dept.HR, Email = "mary@gmail.com"},
                new Employee{Id = 2, Name ="John", Department = Dept.IT, Email = "john@gmail.com"},
                new Employee{Id = 3, Name ="Sam", Department = Dept.Payroll, Email = "sam@gmail.com"}
            };
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return this._employeeList.FirstOrDefault(e => e.Id == id);
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(c => c.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }

        public Employee Delete(int Id)
        {
            var employee = _employeeList.FirstOrDefault(c => c.Id == Id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
            
        }
    }
}
