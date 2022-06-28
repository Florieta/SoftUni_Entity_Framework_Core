using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFoodServices.DTO.Employee;
using FastFoodServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastFoodServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly FastFoodContext dbContext;

        private readonly IMapper mapper;

        public EmployeeService(FastFoodContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public ICollection<ListAllEmployeesDto> All()
        => dbContext.Employees.ProjectTo<ListAllEmployeesDto>(this.mapper.ConfigurationProvider).ToList();

        public void Register(RegisterEmployeeDto dto)
        {
            Employee employee = this.mapper.Map<Employee>(dto);

            this.dbContext.Employees.Add(employee);

            this.dbContext.SaveChanges();
        }
    }
}
