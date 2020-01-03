using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzMyeStore.Core.Models
{
    public class Basket : BaseEntity   // inherits from our base entity, because it inherits from our base entity this class will already inherit an Id
    {
        public virtual ICollection<BasketItem> BasketItems { get; set; }
        //Lazy Loading : we are setting it as a virtual ICollection , this is important for entity framework By setting it as a virtual ICollection, 
        // entity framework will know that whenever we are trying to load the basket from DB it will automatically load all the basket items as well
        //this is called Lazy Loading.
        public Basket()
        {
            this.BasketItems = new List<BasketItem>();
        }

    }
}
