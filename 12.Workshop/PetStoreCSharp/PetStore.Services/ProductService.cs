using AutoMapper;
using AutoMapper.QueryableExtensions;
using PetStore.Data;
using PetStore.Models;
using PetStore.Services.Interface;
using PetStore.Services.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Services
{
    public class ProductService : IProductService
    {
        private readonly PetStoreDbContext dbContext;

        private readonly IMapper mapper;

        public ProductService(IMapper mapper, PetStoreDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public ICollection<ListAllProductsServiceModel> GetAllProducts(string order)
        {
            ICollection<ListAllProductsServiceModel> allProducts;
            
                if (order.ToLower() == "descending")
                {
                    allProducts = this.dbContext
                        .Products
                        .ProjectTo<ListAllProductsServiceModel>
                        (this.mapper.ConfigurationProvider)
                        .OrderByDescending(p => p.Price)
                        .ToArray();
                }
                else
                {
                    allProducts = this.dbContext
                        .Products
                        .ProjectTo<ListAllProductsServiceModel>
                        (this.mapper.ConfigurationProvider)
                        .OrderBy(p => p.Price)
                        .ToArray();
                }
                return allProducts;
            
        }
    }
}
