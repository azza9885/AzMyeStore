using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzMyeStore.Core.Models
{
    public class BasketItem : BaseEntity // BasketItem inherits from BaseEntity which means it will already have the Id
    {
        public string BasketId { get; set; }
        //each basket item will be the actual products that we wanting to add to the basket
        //two ways to do this, first way to copy the product details into the Basketitem record i.e price field,description, etc..and 
       // the basket will show the information(simpler method)
        //second way is to only store the Product Id and then we would link to the product each time(more complex)
        //if first method was the down side is if any attributes related to the products added in the basket were updated , the items in the
        // Basket would have old details related to the products added in the basket.like Price,description.We will have to re-add to our product list to get the updated details
        // so using the second method if any product attributes are updated, the details related to the product are automatically updated.
        public string ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
