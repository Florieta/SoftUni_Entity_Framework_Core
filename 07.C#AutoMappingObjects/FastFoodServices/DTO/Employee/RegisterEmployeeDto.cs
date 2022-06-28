using System;
using System.Collections.Generic;
using System.Text;

namespace FastFoodServices.DTO.Employee
{
    public class RegisterEmployeeDto
    {
     
        public string Name { get; set; }

        public int Age { get; set; }

        public int PositionId { get; set; }

        public string Address { get; set; }
    }
}
