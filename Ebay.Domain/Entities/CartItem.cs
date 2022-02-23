﻿using Ebay.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
