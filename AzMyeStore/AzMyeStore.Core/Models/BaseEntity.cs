using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzMyeStore.Core.Models
{
    public abstract class BaseEntity  //creating it as an abstract class , by setting it as an abstract class we will never allow "Base Entity" to create 
                                         //an instance of its own, it will only allow to create a class that implements it
    {
        public string Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; } //not required but useful

        public BaseEntity()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }
    }
}
