namespace Domain.Exceptions
{
    public class OrderNotFoundException:NotFoundException
    {
        public OrderNotFoundException(Guid id):base($"Order With Id {id} NotFound")
        {
            
        }
        public OrderNotFoundException(string userEmail) : base($"Order With Email {userEmail} NotFound")
        {

        }
    }
}
