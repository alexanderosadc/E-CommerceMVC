using Ebay.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Entities
{
    public class Photo : BaseEntity
    {
        public string Name { get; set; }
        public byte[] BinaryData { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
