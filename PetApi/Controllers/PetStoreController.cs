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

        [HttpGet("petname/{name}")]
        public Pet GetPetByName(string name)
        {
            var pet = pets.Where(pet => pet.Name == name).FirstOrDefault();
            return pet;
        }

        [HttpGet("pettype/{type}")]
        public List<Pet> GetPetsByType(string type)
        {
            var petList = pets.Where(pet => pet.Type == type).ToList();
            return petList;
        }

        [HttpGet("petcolor/{color}")]
        public List<Pet> GetPetsByColor(string color)
        {
            return pets.Where(pet => pet.Color == color).ToList();
        }

        [HttpGet("PriceRange")]
        public IEnumerable<Pet> GetByPriceRange(int min, int max)
        {
            return pets.Where(p => p.Price >= min && p.Price <= max);
        }

        [HttpPatch("{updateModel.name}")]
        public Pet UpdatePriceByName(UpdatePriceModel updateModel)
        {
            pets.Where(pet => pet.Name == updateModel.Name).ToList()[0].Price = updateModel.Price;
            return pets.Where(pet => pet.Name == updateModel.Name).FirstOrDefault();
        }

        [HttpPut("petprice")]
        public List<Pet> GetPetsByPriceRange(PriceRangeModel priceRange)
        {
            return pets.Where(pet => pet.Price < priceRange.MaxValue && pet.Price > priceRange.MinValue).ToList();
        }

        [HttpDelete("petname/{name}")]
        public void DeletePetsByName(string name)
        {
            pets.Remove(pets.Where(deletepet => deletepet.Name == name).FirstOrDefault());
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
