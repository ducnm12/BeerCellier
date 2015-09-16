using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeerFridge.Models
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
    }
}