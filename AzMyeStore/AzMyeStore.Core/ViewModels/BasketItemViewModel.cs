using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzMyeStore.Core.ViewModels
{
    public class BasketItemViewModel
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

       //with viewmodel in place switch back to basketservice and create the method
       //GetBasketItems
    }

    

}
