﻿using Ebay.Infrastructure.ViewModels.Admin.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.DiscountInterfaces
{
    public interface IShowDiscount
    {
        public Task<DiscountViewDTO> GetDiscountDTO();
        public Task<IEnumerable<DiscountViewDTO>> GetDiscountsDTO();
    }
}