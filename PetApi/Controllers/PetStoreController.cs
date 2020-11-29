using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : Controller
    {
        private static IList<Pet> pets = new List<Pet>();
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

        [HttpGet]
        public IList<Pet> GetPetsByAttributes(string type, string color, int? minValue, int? maxValue)
        {
            return pets.Where(pet => (string.IsNullOrEmpty(type) || pet.Type == type) &&
                              (string.IsNullOrEmpty(color) || pet.Color == color) &&
                              (!minValue.HasValue || pet.Price >= minValue) &&
                              (!maxValue.HasValue || pet.Price <= maxValue)).ToList();
        }

        [HttpPatch("{updateModel.name}")]
        public Pet UpdatePriceByName(UpdatePriceModel updateModel)
        {
            pets.Where(pet => pet.Name == updateModel.Name).ToList()[0].Price = updateModel.Price;
            return pets.FirstOrDefault(pet => pet.Name == updateModel.Name);
        }

        [HttpPut("petprice")]
        public List<Pet> GetPetsByPriceRange(PriceRangeModel priceRange)
        {
            return pets.Where(pet => pet.Price < priceRange.MaxValue && pet.Price > priceRange.MinValue).ToList();
        }

        [HttpDelete("pets/{name}")]
        public void DeletePetsByName(string name)
        {
            pets.Remove(pets.FirstOrDefault(deletepet => deletepet.Name == name));
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
