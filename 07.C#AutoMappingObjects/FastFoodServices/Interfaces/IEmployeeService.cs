using FastFoodServices.DTO.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFoodServices.Interfaces
{
    public interface IEmployeeService
    {
        void Register(RegisterEmployeeDto dto);

        ICollection<ListAllEmployeesDto> All();
    }
}
