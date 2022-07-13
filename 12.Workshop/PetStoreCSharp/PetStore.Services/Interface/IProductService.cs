using PetStore.Models;
using PetStore.Services.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Services.Interface
{
    public interface IProductService
    {
        ICollection<ListAllProductsServiceModel> GetAllProducts(string order);
    }
}

