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

        [HttpPost("AddNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
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
