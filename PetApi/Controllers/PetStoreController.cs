using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : Controller
    {
        private IList<Pet> pets = new List<Pet>();
        [HttpPost("addNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }
    }
}
