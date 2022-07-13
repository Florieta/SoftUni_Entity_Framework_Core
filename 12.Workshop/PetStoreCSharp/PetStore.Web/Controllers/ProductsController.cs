using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetStore.Services.Interface;
using PetStore.Services.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMapper mapper;

        private readonly IProductService productService;
        public ProductsController(IMapper mapper, IProductService productService)
        {
            this.mapper = mapper;
            this.productService = productService;
        }
        public IActionResult All(string order = "ascending")
        {
            ICollection<ListAllProductsServiceModel> allProducts = productService.GetAllProducts();
            return View(allProducts);
        }
    }
}
