using System;
using System.ComponentModel.DataAnnotations;

namespace BeerCellier.Entities
{
    public class Beer
    {
        public int ID { get; set; }

        [Required()]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required()]
        [Range(0, Int32.MaxValue)]
        public int Quantity { get; set; }

        [Required()]
        public virtual User Owner { get; set; }
    }    
}