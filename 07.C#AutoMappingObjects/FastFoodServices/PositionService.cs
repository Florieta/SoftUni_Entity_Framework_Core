using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFoodServices.DTO.Position;
using FastFoodServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastFoodServices
{
    public class PositionService : IPositionService

    {
        private readonly IMapper mapper;
        private readonly FastFoodContext dbContext;

        public PositionService(FastFoodContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public ICollection<EmployeeRegisterPositionsAvailable> GetPositionsAvailable()
        => dbContext
           .Positions
           .ProjectTo<EmployeeRegisterPositionsAvailable>(this.mapper.ConfigurationProvider)
           .ToList();
    }
}
