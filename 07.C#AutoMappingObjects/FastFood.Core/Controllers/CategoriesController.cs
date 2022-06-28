namespace FastFood.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Models;
    using FastFoodServices.DTO.Category;
    using FastFoodServices.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Categories;

    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;


        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            if(!ModelState.IsValid)
            {
                return this.RedirectToAction("Create");

                
            }

            CreateCategoryDto categoryDto = this.mapper.Map<CreateCategoryDto>(model);


            this.categoryService.Create(categoryDto);

            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            ICollection<ListAllCategoriesDto> categoriesDto = this.categoryService.All();
            List<CategoryAllViewModel> categoryViewModels = this.mapper
                .Map<ICollection<ListAllCategoriesDto>, ICollection<CategoryAllViewModel>>(categoriesDto).ToList();

            return this.View("All", categoryViewModels);
        }
    }
}
