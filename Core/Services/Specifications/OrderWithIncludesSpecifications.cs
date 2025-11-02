using Domain.Entities.OrderModule;

namespace Services.Specifications
{
    internal class OrderWithIncludesSpecifications:BaseSpecifications<Order,Guid>
    {
        //Get Order By Id ==> cretria ==> id == o.Id ==> includes(deliveryMethod,orderItems)
        public OrderWithIncludesSpecifications(Guid id) : base(o => o.Id == id)
        {
            AddIncludes(o => o.DeliveryMethod);
            AddIncludes(o => o.OrderItems); 
        }
        //Get All Orders By Email ==> cretria ==> Email == o.Email ==> includes(deliveryMethod,orderItems)
        public OrderWithIncludesSpecifications(string userEmail) : base(o => o.UserEmail == userEmail)
        {
            AddIncludes(o => o.DeliveryMethod);
            AddIncludes(o => o.OrderItems);
            AddOrderBy(o => o.OrderDate);
        }
    }
}
