using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : ControllerBase
    {
        private static List<Pet> pets = new List<Pet>();
        [HttpPost("addNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("pets")]
        public IEnumerable<Pet> GetAllPets()
        {
            return pets;
        }

        [HttpDelete("clear")]
        public void DeletePet()
        {
            pets.Clear();
        }

        [HttpGet("petname/{name}")]
        public Pet GetPetByName(string name)
        {
            return pets.Find(pet => pet.Name == name);
        }

        [HttpGet("pettype/{type}")]
        public IEnumerable<Pet> GetPetByType(string type)
        {
            return pets.FindAll(pet => pet.Type == type);
        }

        [HttpGet("petcolor/{color}")]
        public IEnumerable<Pet> GetPetByColor(string color)
        {
            return pets.FindAll(pet => pet.Color == color);
        }

        [HttpGet("petpricerange/{pricerange}")]
        public IEnumerable<Pet> GetPetByPriceRange(string pricerange)
        {
            var prices = pricerange.Split("-");
            double min = Convert.ToDouble(prices[0]);
            double max = Convert.ToDouble(prices[1]);
            return pets.FindAll(pet => (min <= pet.Price) && (pet.Price <= max));
        }

        [HttpDelete("petdelete/{name}")]
        public void DeleteAPet(string name)
        {
            pets.RemoveAll(pet => pet.Name == name);
        }
    }
}
