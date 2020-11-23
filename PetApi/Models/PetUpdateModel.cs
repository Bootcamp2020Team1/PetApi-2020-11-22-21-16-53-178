using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetApi
{
    public class PetUpdateModel
    {
        public PetUpdateModel()
        {
        }

        public PetUpdateModel(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }
        public int Price { get; set; }
    }
}
