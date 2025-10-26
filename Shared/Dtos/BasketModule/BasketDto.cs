using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.BasketModule
{
    public record BasketDto
    {
        public string Id { get; set; }=string.Empty;
        public ICollection<BasketItemDto> BasketItems { get; set; } = [];
    }
}
