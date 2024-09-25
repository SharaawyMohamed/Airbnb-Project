﻿using Airbnb.Domain.Entities;
using Microsoft.Identity.Client;
using System.Text;

namespace Airbnb.Infrastructure.Specifications
{
    public class PropertyWithSpec : BaseSpecifications<Property, string>
    {
        public PropertyWithSpec(ProductSpecParameters param)
            : base(P =>
            (string.IsNullOrWhiteSpace(param.categoryName) || P.Categories.Any(c => c.Name == param.categoryName))
            && (!param.locationId.HasValue || P.LocationId == param.locationId)
            )
        {
            if (!string.IsNullOrWhiteSpace(param.sort.ToString()))
            {
                switch(param.sort)
                {
                    case Sort.NightPriceAsc:
                        AddOrderBy(x => x.NightPrice);
                        break;
                    case Sort.NightPriceDesc:
                        AddOrderByDescending(x => x.NightPrice);
                        break;
                    case Sort.RateAsc:
                        AddOrderBy(x => x.Rate);
                        break;
                    case Sort.RateDesc:
                        AddOrderByDescending(x => x.Rate);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;

                }
            }

            int skip = (param.pageIndex - 1) * (param.PageSize);
            skip = skip > 0 ? skip : 0;

            int take = param.PageSize;
            ApplyPagination(skip,take);
           
        }
    }
}