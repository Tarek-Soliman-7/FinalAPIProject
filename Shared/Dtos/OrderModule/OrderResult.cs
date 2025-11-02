namespace Shared.Dtos.OrderModule
{
    public record OrderResult
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public AddressDto ShippingAddress { get; init; }
        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
        public string PaymentStatus { get; init; } = string.Empty ;
        public string DeliveryMethod { get; init; }= string.Empty ;
        public int? DeliveryMethodId { get; init; }
        public decimal SubTotal { get; init; } // OrderItem1.Quantity * OrderItem1.Price + OrderItem2.Quantity * OrderItem2.Price + .... + OrderItem$.Quantity * OrderItem$.Price
        public decimal Total { get; init; }    // Total=SubTotal+DeliveryMethod.Price 
        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.UtcNow;
        public string PaymentIntentId { get; init; } = string.Empty;
    }
}
