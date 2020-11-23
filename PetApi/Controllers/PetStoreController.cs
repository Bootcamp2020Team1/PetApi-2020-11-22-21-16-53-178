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
        private static IList<Pet> pets = new List<Pet>();
        [HttpGet("Pets")]
        public IEnumerable<Pet> Get()
        {
            return pets;
        }

        [HttpGet("{name}")]
        public Pet GetByName(string name)
        {
            return pets.Where(p => p.Name == name).FirstOrDefault();
        }

        [HttpGet("Types/{type}")]
        public IEnumerable<Pet> GetByType(string type)
        {
            return pets.Where(p => p.Type == type);
        }

        [HttpPost("AddNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpPatch("{name}")]
        public Pet UpdatePet(string name, PetUpdateModel petUpdate)
        {
            var pet = pets.Where(p => p.Name == name).FirstOrDefault();
            pet.Price = petUpdate.Price;
            return pet;
        }

        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            pets.Remove(pets.Where(p => p.Name == name).FirstOrDefault());
        }

        [HttpDelete("Clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
