using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetApi
{
    public class UpdatePriceModel
    {
        public UpdatePriceModel()
        {
        }

        public UpdatePriceModel(string name, int price)
        {
            this.Name = name;
            this.Price = price;
        }

        public string Name { get; set; }
        public int Price { get; set; }
    }
}
