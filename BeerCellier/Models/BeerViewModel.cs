using BeerCellier.Entities;
using System.Collections.Generic;

namespace BeerCellier.Models
{
    public class BeerViewModel
    {
        public int ID { get; set; }       
        public string Name { get; set; }
        public int Quantity { get; set; }

        public BeerViewModel() { }

        public BeerViewModel(Beer beer)
        {
            ID = beer.ID;
            Name = beer.Name;
            Quantity = beer.Quantity;
        }
    }

    public static class BeerViewModelExtension
    {
        public static IEnumerable<BeerViewModel> ToViewModels(this IEnumerable<Beer> beers)
        {
            foreach (var beer in beers)
            {
                yield return new BeerViewModel(beer);
            }
        }
    }
}