using AutoMapper;
using PetStore.Services.Models.Product;
using System;

namespace PetStore.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            this.CreateMap<ProductProfile, ListAllProductsServiceModel>();
        }
    }
}
