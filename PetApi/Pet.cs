using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetApi
{
    public class Pet
    {
        public Pet()
        {
        }

        public Pet(string name, string type, string color, int price)
        {
            Name = name;
            Type = type;
            Color = color;
            Price = price;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals(obj as Pet);
        }

        protected bool Equals(Pet other)
        {
            return this.Color == other.Color && this.Name == other.Name && this.Price == other.Price &&
                   this.Type == other.Type;
        }
    }
}
