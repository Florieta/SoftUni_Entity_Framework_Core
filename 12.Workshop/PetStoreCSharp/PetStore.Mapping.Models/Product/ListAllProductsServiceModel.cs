﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Services.Models.Product
{
    public class ListAllProductsServiceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }

        public decimal Price { get; set; }
    }
}
