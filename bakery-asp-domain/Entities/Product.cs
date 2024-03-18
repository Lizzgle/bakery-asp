using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bakery_asp_domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public Category? Category { get; set; }

        public int CategoryId { get; set; }

        public int Calories { get; set; }

    }
}
