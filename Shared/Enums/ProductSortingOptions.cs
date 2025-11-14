using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums
{
    public enum ProductSortingOptions
    {
        [Display(Name = "NameAsc")]
        NameAsc=1,
        [Display(Name ="NameDesc")]
        NameDesc=2,
        [Display(Name = "PriceAsc")]
        PriceAsc=3, 
        [Display(Name ="PriceDesc")]
        PriceDesc=4
    }
}
