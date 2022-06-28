using FastFoodServices.DTO.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFoodServices.Interfaces
{
    public interface IPositionService
    {
        ICollection<EmployeeRegisterPositionsAvailable> GetPositionsAvailable();
    }
}
