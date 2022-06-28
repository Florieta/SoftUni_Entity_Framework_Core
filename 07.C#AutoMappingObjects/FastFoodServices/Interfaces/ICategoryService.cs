using FastFoodServices.DTO.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFoodServices.Interfaces
{
    public interface ICategoryService
    {
        void Create(CreateCategoryDto dto);

        ICollection<ListAllCategoriesDto> All();
    }
}
