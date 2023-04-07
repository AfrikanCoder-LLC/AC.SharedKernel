using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Exceptions
{
    public class ItemNotFoundByKeyException : DomainException
    {
        public ItemNotFoundByKeyException(string id, string itemType) 
            : base($@"The matching {itemType} entity not found for key value =  [{id}]. Please verify it exists in the datastore")
        {
            
        }
    }
}
