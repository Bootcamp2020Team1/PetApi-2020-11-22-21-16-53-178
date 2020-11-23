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
        [HttpGet]
        [Route("Pets")]
        public IEnumerable<Pet> Get()
        {
            return pets;
        }

        //[HttpGet]
        //public Pet Get()
        //{
        //    return "Hello World";
        //}

        [HttpPost]
        [Route("AddNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpDelete]
        [Route("Clear")]
        public void Clear()
        {
            pets.Clear();
        }
    }
}
