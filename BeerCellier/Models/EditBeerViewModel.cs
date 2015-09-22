using System;
using System.ComponentModel.DataAnnotations;
using BeerCellier.Entities;

namespace BeerCellier.Models
{
    public class EditBeerViewModel
    {           
        public int ID { get; set; }

        [Required()]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required()]
        [Range(0, Int32.MaxValue)]
        public int Quantity { get; set; }

        public EditBeerViewModel() { }

        public EditBeerViewModel(Beer beer)
        {
            ID = beer.ID;
            Name = beer.Name;
            Quantity = beer.Quantity;
        }
    }
}