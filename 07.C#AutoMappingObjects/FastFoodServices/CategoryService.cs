using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFoodServices.DTO.Category;
using FastFoodServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastFoodServices
{
    public class CategoryService : ICategoryService
    {
        private readonly FastFoodContext dbContext;

        private readonly IMapper mapper;

        public CategoryService(FastFoodContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public ICollection<ListAllCategoriesDto> All()
        => this.dbContext
            .Categories
            .ProjectTo<ListAllCategoriesDto>(this.mapper.ConfigurationProvider).ToList();

        public void Create(CreateCategoryDto dto)
        {
            Category category = this.mapper.Map<Category>(dto);

            this.dbContext.Categories.Add(category);

            this.dbContext.SaveChanges();
        }
    }
}
